<template>
  <q-card flat bordered class="q-pa-lg" style="width: 1200px; height: 650px"
    :class="$q.dark.isActive ? 'bg-dark text-white' : 'bg-white text-black'">
    <div class="row justify-between items-center q-mb-md">
      <span class="text-weight-medium text-size-32">
        <template v-if="editar">{{ traducir('EditarRol') }}</template>
        <template v-else>{{ traducir('CrearRol') }}</template>
      </span>

      <div class="row q-gutter-md">
        <q-btn flat :color="$q.dark.isActive ? 'white' : 'grey-8'" class="text-weight-light"
          :class="$q.dark.isActive ? 'bg-grey-9' : 'bg-grey-2'" no-caps style="width: 224px" @click="cancelarForm">
          <i class="pi pi-times-circle q-mr-md text-size-20"></i>
          <span class="text-size-18"> {{ traducir('Cancelar') }}</span>
        </q-btn>

        <q-btn color="primary" no-caps style="width: 224px" class="text-primary-contrast" @click="enviarFormulario">
          <i class="pi pi-check-circle q-mr-md text-size-20"></i>
          <span class="text-size-18">
            {{ editar ? traducir('GuardarCambios') : traducir('CrearRol') }}
          </span>
        </q-btn>
      </div>
    </div>

    <q-separator color="grey-2" />

    <div class="row" style="height: calc(100% - 68px)">
      <div class="col-4 column">
        <div class="q-my-md">
          <div class="text-subtitle1 q-mb-xs">{{ traducir('NombreDelRol') }}:</div>
          <q-input filled dense type="text" v-model="nombreRol"
            :rules="[(val) => (val && val.length > 0) || traducir('CampoRequerido')]" />
        </div>

        <div class="q-mb-md">
          <div class="text-subtitle1 q-mb-xs">{{ traducir('Descripcion') }}:</div>
          <q-input filled dense type="textarea" v-model="descripcion" />
        </div>

        <div class="text-subtitle1 q-mb-sm">{{ traducir('ModulosHabilitados') }}:</div>
        <div class="text-caption text-grey-6 q-mb-sm">
          <i class="pi pi-info-circle q-mr-xs"></i>
          {{ traducir('MarqueParaHabilitar') }}
        </div>

        <div class="col" style="height: 100%; position: relative">
          <div v-if="cargandoModulos" class="full-width full-height flex flex-center">
            <q-spinner size="60px" color="primary" />
          </div>

          <q-scroll-area v-else class="fit">
            <div v-for="mod in opcionesModulos" :key="mod.id" class="q-pa-sm q-mb-sm cursor-pointer" :style="$q.dark.isActive
                ? 'border: 1px solid #555; border-radius: 4px'
                : 'border: 1px solid #f5f5f5; border-radius: 4px'
              " @click="mod.activo = !mod.activo">
              <div class="row justify-between items-center">
                <div>
                  <div class="text-body1">
                    {{ traducir(mod.nombreNormalizado, mod.nombre) }}
                  </div>
                  <div class="text-caption text-grey-7">
                    {{ contarPermisosFiltrados(mod) }} {{ traducir('PermisosDisponibles') }}
                  </div>
                </div>
                <!-- Usamos v-model para activar/desactivar módulos -->
                <G3CheckBox v-model="mod.activo" @click.stop />
              </div>
              <q-tooltip class="bg-grey-9 text-white" :delay="300">
                {{ traducir(mod.descripcion, traducir('ModuloOperativo')) }}
              </q-tooltip>
            </div>
          </q-scroll-area>
        </div>
      </div>
      <div class="col-8 flex flex-center" v-if="modulosSeleccionados.length < 1">
        <div class="text-center">
          <i class="pi pi-arrow-left text-grey-5 text-h3"></i>
          <div class="text-grey-6 text-subtitle1 q-mt-md">{{ traducir('SeleccioneUnModulo') }}</div>
          <div class="text-caption text-grey-5 q-mt-sm">
            {{ traducir('SeleccioneModulosParaMostrarPermisos') }}
          </div>
        </div>
      </div>
      <div class="col-8 q-pl-lg" v-else>
        <div class="row q-py-md items-center">
          <div class="col-6">
            <span>{{ traducir('PermisosOtorgados') }}: {{ permisosOtorgados.length }}</span>
          </div>

          <div class="col-6">
            <div class="row justify-end">
              <q-input dense outlined v-model="busquedaPermisos" :label="traducir('Buscar')" style="width: 365px"
                debounce="300">
                <template v-slot:prepend>
                  <i class="pi pi-search q-mr-sm" />
                </template>
              </q-input>
            </div>
          </div>
        </div>

        <div v-if="cargandoPermisos" class="flex flex-center" style="height: calc(100% - 80px)">
          <q-spinner size="40px" color="primary" />
        </div>

        <q-scroll-area v-else class="row" style="height: calc(100% - 80px)">
          <q-list>
            <q-expansion-item v-for="(modulo, index) in modulosSeleccionados" :key="index"
              class="q-mb-sm overflow-hidden" default-opened header-class="bg-primary text-primary-contrast"
              expand-icon-class="text-primary-contrast" style="border-radius: 8px">
              <template #header>
                <q-item-section>
                  <div class="row items-center full-width">
                    <div class="col">
                      <q-item-label class="text-size-21 text-weight-medium">
                        {{ traducir(modulo.nombreNormalizado, modulo.nombre) }}
                      </q-item-label>
                      <q-item-label class="text-primary-contrast text-weight-light" style="opacity: 0.85">
                        {{ traducir(modulo.value.descripcion, traducir('ModuloOperativo')) }}
                      </q-item-label>
                    </div>
                    <!-- SELECT ALL MODULE CHECKBOX -->
                    <div @click.stop class="q-mr-sm">
                      <G3CheckBox :model-value="!!modulo.todosSeleccionados" resaltar @update:model-value="
                        (val) => toggleSeleccionarTodosModulo(modulo, val === true)
                      " dense :label="traducir('SeleccionarTodos')" />
                      <q-tooltip class="bg-grey-9 text-white">
                        {{ traducir('SeleccionarTodosModuloTooltip') }}
                      </q-tooltip>
                    </div>
                  </div>
                </q-item-section>
                <q-tooltip class="bg-grey-9 text-white" :delay="500">
                  {{ traducir('ClickParaDesplegarPermisos') }}
                </q-tooltip>
              </template>

              <div class="q-pa-sm">
                <!-- Iteramos sobre los permisos filtrados del módulo -->
                <div v-if="!filtrarPermisos(modulo) || filtrarPermisos(modulo).length === 0"
                  class="q-pa-md text-grey-7">
                  {{ traducir('NoSeEncontraronPermisos') }}
                </div>

                <template v-for="(grupoPermisosModulo, grpIndex) in filtrarPermisos(modulo)" :key="grpIndex">
                  <q-expansion-item class="q-mb-sm" default-opened
                    :header-class="$q.dark.isActive ? 'text-white' : 'text-grey-9'" dense>
                    <template #header>
                      <q-item-section>
                        <div class="row items-center full-width">
                          <div class="col">
                            <span class="text-size-18 text-weight-medium">
                              {{
                                traducir(
                                  grupoPermisosModulo.grupoNombreNormalizado,
                                  grupoPermisosModulo.grupoNombre,
                                )
                              }}
                            </span>
                          </div>
                          <!-- SELECT ALL GROUP CHECKBOX -->
                          <div @click.stop class="q-mr-sm">
                            <G3CheckBox :model-value="grupoPermisosModulo.todosSeleccionados" @update:model-value="
                              (val) =>
                                toggleSeleccionarTodosGrupo(grupoPermisosModulo, val === true)
                            " dense :label="traducir('SeleccionarTodos')"></G3CheckBox>
                            <q-tooltip class="bg-grey-9 text-white">
                              {{ traducir('SeleccionarTodosGrupoTooltip') }}
                            </q-tooltip>
                          </div>
                        </div>
                      </q-item-section>
                      <q-tooltip class="bg-grey-9 text-white" :delay="500">
                        {{ traducir('ClickParaDesplegarPermisos') }}
                      </q-tooltip>
                    </template>

                    <div class="q-pa-sm">
                      <div class="row">
                        <div v-for="(permiso, pIndex) in grupoPermisosModulo.permisos" :key="pIndex" class="col-6">
                          <div class="row items-center q-my-xs cursor-pointer">
                            <div class="col-10 q-pl-sm ellipsis">
                              {{ traducir(permiso.nombre, permiso.nombre) }}
                            </div>
                            <div class="col-2">
                              <!-- INDIVIDUAL PERMISSION CHECKBOX -->
                              <G3CheckBox :model-value="permiso.seleccionado" @update:model-value="
                                (val) => togglePermiso(grupoPermisosModulo, permiso, val)
                              " size="sm"></G3CheckBox>
                            </div>
                            <q-tooltip class="bg-grey-9 text-white" anchor="top middle" self="bottom middle">
                              {{ traducir(permiso.descripcion, permiso.descripcion) }}
                            </q-tooltip>
                          </div>
                        </div>
                      </div>
                    </div>
                  </q-expansion-item>
                  <q-separator v-if="grpIndex + 1 !== filtrarPermisos(modulo).length" class="q-my-sm" />
                </template>
              </div>
            </q-expansion-item>
          </q-list>
        </q-scroll-area>
      </div>
    </div>
  </q-card>
</template>

<script setup>
import { computed, onMounted, ref, toRefs, inject } from 'vue'
import { identidadGFuelAdmin, modulosGFuelAdmin } from 'src/api/moduloAdministracion'
import * as quasarUtils from 'src/utils/quasar-utils.js'
import { useQuasar } from 'quasar'

const $q = useQuasar()
const traducir = inject('traducir', (key) => key)

const props = defineProps({
  propsCustomForm: {
    type: Object,
    default: () => {
      return {
        editar: false,
      }
    },
  },
})

const emits = defineEmits(['onDialogCancel', 'onDialogHide', 'onDialogOK'])

const { propsCustomForm } = toRefs(props)

const { editar, rol, refrescarRoles } = propsCustomForm.value

// Estado del formulario
const nombreRol = ref(editar ? rol.nombreRol : null)
const descripcion = ref(editar ? rol.descripcion : null)
const busquedaPermisos = ref('')

// Estado de Módulos
const cargandoModulos = ref(false)
const modulos = ref([])
const opcionesModulos = ref([])

const modulosSeleccionados = computed(() => opcionesModulos.value.filter((m) => m.activo))

// Estado de Permisos
const cargandoPermisos = ref(false)
const permisosOtorgados = ref([]) // Array de IDs de permisos seleccionados

// Inicialización
onMounted(async () => {
  await listarModulos()
  await cargarTodosPermisos()
})

// === LOGICA DE MODULOS ===
const listarModulos = async () => {
  cargandoModulos.value = true
  const resp = await modulosGFuelAdmin.listar()
  cargandoModulos.value = false

  if (!resp.exito) {
    modulos.value = []
    return
  }

  modulos.value = resp.payload

  opcionesModulos.value = modulos.value.map((e) => {
    return {
      id: e.id,
      nombre: traducir(e.nombre),
      activo: false,
      value: e,
      permmisosAgrupados: [], // Lo guardamos en el wrapper
    }
  })

  // Pre-selección si es edición (activar modulos)
  if (editar && rol.modulosActivados) {
    opcionesModulos.value.forEach((e) => {
      if (rol.modulosActivados.some((m) => m.id === e.id)) {
        e.activo = true
      }
    })
  }
}

// === LOGICA DE PERMISOS (CARGA COMPLETA) ===
const cargarTodosPermisos = async () => {
  cargandoPermisos.value = true

  // Creamos un array de promesas para cargar permisos de cada módulo INDIVIDUALMENTE
  // Esto evita el error de la API con IDs múltiples y cumple el requerimiento del usuario
  const promesas = opcionesModulos.value.map(async (modulo) => {
    const resp = await identidadGFuelAdmin.listarPermisos(true, [modulo.id], false)
    if (resp.exito) {
      // Asignamos directamente al wrapper del modulo
      const grupos = resp.payload
      modulo.permmisosAgrupados = grupos

      // Procesamos el estado inicial de check (seleccionado) si estamos editando
      grupos.forEach((grupo) => {
        let cantSeleccionados = 0
        grupo.permisos.forEach((permiso) => {
          if (editar) {
            // Verificar si el rol tiene este permiso asignado
            const asignado = permiso.idRolesAsignados?.includes(rol.id)
            if (asignado) {
              permiso.seleccionado = true
              if (!permisosOtorgados.value.includes(permiso.id)) {
                permisosOtorgados.value.push(permiso.id)
              }
              cantSeleccionados++
            } else {
              permiso.seleccionado = false
            }
          } else {
            permiso.seleccionado = false
          }
        })
        grupo.todosSeleccionados =
          grupo.numeroPermisos > 0 && cantSeleccionados === grupo.numeroPermisos
      })

      // Inicializar el estado de "todosSeleccionados" del modulo
      // Si todos los grupos estan completos, el modulo tambien.
      // Importante: Inicializar en false si no cumple, para evitar undefined/null (estado indeterminado)
      const todosGrupos = grupos.every((g) => g.todosSeleccionados)
      modulo.todosSeleccionados = todosGrupos
    } else {
      modulo.permmisosAgrupados = []
      modulo.todosSeleccionados = false
    }
  })

  await Promise.all(promesas)
  cargandoPermisos.value = false
}

// === FILTRADO ===
const filtrarPermisos = (mod) => {
  // Usamos mod.permmisosAgrupados (wrapper) en lugar de mod.value...
  const grupos = mod.permmisosAgrupados || []
  if (!busquedaPermisos.value) return grupos

  const query = busquedaPermisos.value.toLowerCase()
  return grupos
    .map((grupo) => {
      // Usamos 'grupoNombre' si existe, o 'grupoNombreNormalizado' como fallback
      // El usuario indicó usar nombres más simples si están disponibles
      const nombreGrupo = grupo.grupoNombre || grupo.grupoNombreNormalizado || ''

      const grupoCoincide = nombreGrupo.toLowerCase().includes(query)

      const permisosCoinciden = grupo.permisos.filter((p) => {
        const nombrePermiso = p.nombre || p.nombreNormalizado || ''
        return nombrePermiso.toLowerCase().includes(query)
      })

      if (grupoCoincide) return grupo
      if (permisosCoinciden.length > 0) {
        return { ...grupo, permisos: permisosCoinciden }
      }
      return null
    })
    .filter((g) => g !== null)
}

const contarPermisosFiltrados = (mod) => {
  const grupos = filtrarPermisos(mod) || []
  return grupos.reduce((acc, g) => acc + g.permisos.length, 0)
}

// === INTERACCION CHECKBOXES ===
const toggleSeleccionarTodosModulo = (modulo, valor) => {
  modulo.todosSeleccionados = valor
  if (modulo.permmisosAgrupados) {
    modulo.permmisosAgrupados.forEach((grupo) => {
      toggleSeleccionarTodosGrupo(grupo, valor)
    })
  }
}

const toggleSeleccionarTodosGrupo = (grupo, valor) => {
  grupo.todosSeleccionados = valor
  grupo.permisos.forEach((p) => {
    p.seleccionado = valor
    actualizarPermisoOtorgado(p.id, valor)
  })
  // Actualizar estado del padre (modulo) si es necesario
  // Por ahora simple unidireccional o verificamos estado padre luego?
  // Para mantener simpleza y rendimiento no recalculamos hacia arriba cada vez
  // Pero si el usuario desmarca un grupo, el "todos" del modulo deberia desmarcarse.
  // Lo haremos en el watcher o computado si fuera reactivo estricto, o hookeamos:
  actualizarEstadoTodosModulo(obtenerModuloDeGrupo(grupo))
}

const obtenerModuloDeGrupo = (grupo) => {
  // Busqueda inversa poco eficiente pero necesaria sin back-reference
  return opcionesModulos.value.find((m) => m.permmisosAgrupados?.includes(grupo))
}

const actualizarEstadoTodosModulo = (modulo) => {
  if (!modulo || !modulo.permmisosAgrupados) return
  // Si todos los grupos tienen todosSeleccionados = true, entonces modulo tambien
  const todos = modulo.permmisosAgrupados.every((g) => g.todosSeleccionados)
  modulo.todosSeleccionados = todos
}

const togglePermiso = (grupo, permiso, valor) => {
  permiso.seleccionado = valor
  actualizarPermisoOtorgado(permiso.id, valor)
  // Verificar si todos estan seleccionados
  const todos = grupo.permisos.every((p) => p.seleccionado)
  grupo.todosSeleccionados = todos

  // Propagar hacia arriba al modulo
  actualizarEstadoTodosModulo(obtenerModuloDeGrupo(grupo))
}

const actualizarPermisoOtorgado = (idPermiso, agregar) => {
  const index = permisosOtorgados.value.indexOf(idPermiso)
  if (agregar) {
    if (index === -1) permisosOtorgados.value.push(idPermiso)
  } else {
    if (index !== -1) permisosOtorgados.value.splice(index, 1)
  }
}

// === GUARDAR / CANCELAR ===
const cancelarForm = () => {
  emits('onDialogCancel')
}

const enviarFormulario = async () => {
  const nombreParams = nombreRol.value?.trim()
  if (!nombreParams) {
    quasarUtils.aviso({
      error: true,
      mensaje: traducir('NombreRolAvisoEspecificar'),
    })
    return
  }

  // Validamos que haya permisos seleccionados
  // OJO: Tambien debemos validar que el modulo seleccionado este "activo"
  // Pero permisosOtorgados solo se llena si se clickean checkboxes.
  // Si desactivo un modulo, visualmente desaparece, pero mis permisos en 'permisosOtorgados' siguen ahi?
  // Deberiamos filtrar los permisosOtorgados para asegurarnos que pertenecen a modulos activos?
  // Por simplicidad, asumimos que el usuario gestiona esto correctamente,
  // o limpiamos permisos al desactivar modulo (opcional, pero mas complejo)
  // El usuario pidio persistencia local, asi que quizas mantenerlos seleccionados aunque ocultos es deseado.

  if (permisosOtorgados.value.length === 0) {
    quasarUtils.aviso({
      error: true,
      mensaje: traducir('PermisosAvisoEspecificar'),
    })
    return
  }

  const decision = await quasarUtils.decision({
    mensaje: editar ? traducir('EditarRolPregunta') : traducir('CrearRolPregunta'),
  })

  if (decision) {
    // Filtramos los permisos para enviar SOLO los que pertenecen a modulos ACTIVOS en el form
    // Esto es importante para coherencia: si desmarco el modulo, no deberia mandar sus permisos

    // Recopilamos IDs validos iterando los modulos activos
    let permisosFinales = []
    modulosSeleccionados.value.forEach((mod) => {
      if (mod.permmisosAgrupados) {
        mod.permmisosAgrupados.forEach((g) => {
          g.permisos.forEach((p) => {
            if (p.seleccionado) permisosFinales.push(p.id)
          })
        })
      }
    })

    if (permisosFinales.length === 0) {
      quasarUtils.aviso({
        error: true,
        mensaje: traducir('PermisosAvisoEspecificar'),
      })
      return
    }

    const modelo = {
      nombreRol: nombreParams,
      descripcion: descripcion.value?.trim() ?? '',
      permisosOtorgados: permisosFinales,
    }

    quasarUtils.cargandoSimple()
    let resp
    if (editar) {
      modelo.idRol = rol.id
      resp = await identidadGFuelAdmin.editarRol(modelo)
    } else {
      resp = await identidadGFuelAdmin.crearRol(modelo)
    }
    quasarUtils.ocultarCargandoSimple()

    if (!resp.exito) {
      quasarUtils.aviso({
        error: true,
        mensaje: resp.payload.errores ? resp.payload.errores[0] : 'Error',
      })
      return
    }

    if (refrescarRoles) refrescarRoles()
    await quasarUtils.aviso({
      exito: true,
      mensaje: editar
        ? traducir('RegistroEditadoConExitoMensaje')
        : traducir('RegistroCreadoConExitoMensaje'),
    })
    emits('onDialogOK')
  }
}
</script>

<style scoped></style>
```
