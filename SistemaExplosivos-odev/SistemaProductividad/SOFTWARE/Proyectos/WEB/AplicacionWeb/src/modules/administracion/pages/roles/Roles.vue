<!-- eslint-disable vue/multi-word-component-names -->
<template>
  <q-page>
    <G3ModuloHeader :modulo="traducir('Roles')" :requiere-boton-crear="true" :boton-crear-texto="traducir('CrearRol')"
      boton-crear-icono="pi pi-plus-circle" :metodo-crear="mostrarCrearRolDialog" />

    <div class="row q-mt-md">
      <div class="col-12">
        <G3CustomTable :columnasVisibles="['opciones', 'nombreRol', 'descripcion', 'modulos', 'estado']" :columnas="[
          {
            name: 'id',
            label: '',
            field: (roles) => roles.id,
          },
          {
            name: 'opciones',
            label: traducir('Opciones'),
            align: 'center',
          },
          {
            name: 'nombreRol',
            label: traducir('Rol'),
            align: 'center',
            field: (roles) => roles.nombreRol,
          },
          {
            name: 'descripcion',
            label: traducir('Descripcion'),
            align: 'center',
            field: (roles) => (roles.descripcion ? roles.descripcion : '-'),
          },
          {
            name: 'modulos',
            label: traducir('Modulos'),
            align: 'center',
          },
          {
            name: 'estado',
            label: traducir('Estado'),
            align: 'center',
          },
        ]" :filas="roles" :cargando="cargandoTablaRoles" :filasPorPagina="10" :filtros="{
          nombreRol: '',
          descripcion: '',
          estado: {
            seleccion: null,
            opciones: [
              {
                label: traducir('Activado'),
                value: true,
              },
              {
                label: traducir('Desactivado'),
                value: false,
              },
            ],
          },
        }">
          <template v-slot:col-end-estado="{ props }">
            <q-chip dense square :color="props.row.estado ? 'green-1' : 'red-1'"
              :text-color="props.row.estado ? 'green-9' : 'red-8'"
              class="text-weight-bold q-px-md q-py-xs rounded-badge text-size-12">
              {{ props.row.estado ? traducir('Activado') : traducir('Desactivado') }}
            </q-chip>
          </template>
          <template v-slot:col-end-modulos="{ props }">
            <div v-for="(modulo, index) in props.row.modulosActivados" :key="index" class="row justify-center">
              <q-chip dense square :color="props.row.estado ? 'green-1' : 'red-1'"
                :text-color="props.row.estado ? 'green-9' : 'red-8'"
                class="text-weight-bold q-px-md q-py-xs rounded-badge text-size-12">
                {{ traducir(modulo.nombreNormalizado, modulo.nombre) }}
              </q-chip>
            </div>
          </template>
          <template v-slot:col-start-opciones="{ props }">
            <boton-opciones :items="llenarArrayOpciones(props)" />
          </template>
        </G3CustomTable>
      </div>
    </div>
  </q-page>
</template>

<script setup>
import { useQuasar } from 'quasar'
import { onMounted, ref, inject } from 'vue'
import { refreshSource } from 'src/utils/utils'
import { useSesionStore } from 'src/stores/sesion'
import * as quasarUtils from 'src/utils/quasar-utils.js'
import G3CustomTable from 'src/components/Globales/G3CustomTable.vue'
import { identidadGFuelAdmin } from 'src/api/moduloAdministracion'
import { pIdentidad } from 'src/core/permisos/moduloAdministracion'
import { estaPermisoOtorgado } from 'src/core/permisos/check.js'
import G3ModuloHeader from 'src/components/Genericos/G3ModuloHeader.vue'
import CustomFormDialog from 'src/components/Globales/Dialogs/CustomFormDialog.vue'
import BotonOpciones from 'src/components/Paginas/General/OpcionesPage/BotonOpciones.vue'
import CrearEditarRolForm from './components/CrearEditarRolForm.vue'

const traducir = inject('traducir')

const $q = useQuasar()
const roles = ref([])
const cargandoTablaRoles = ref(false)

const storeSesion = useSesionStore()
const usuario = storeSesion.ObtenerUsuario

const refrescarRoles = async () => {
  cargandoTablaRoles.value = true
  const resp = await identidadGFuelAdmin.listarRoles()
  cargandoTablaRoles.value = false
  if (!resp.exito) {
    roles.value = []
    return
  }
  roles.value = resp.payload
}

const mostrarCrearRolDialog = async (args = {}) => {
  $q.dialog({
    component: CustomFormDialog,
    componentProps: {
      formularioComponent: CrearEditarRolForm,
      noBackdropDismiss: false,
      formularioComponentProps: {
        refrescarRoles,
        ...args,
      },
    },
  })
}

const cambiarEstadoRol = async (idRol, idUsuario, estado) => {
  quasarUtils.cargandoSimple()
  const resp = await identidadGFuelAdmin.cambiarEstadoRol({
    idRol,
    idUsuario,
  })
  quasarUtils.ocultarCargandoSimple()
  if (!resp.exito) {
    quasarUtils.aviso({
      error: true,
      mensaje: resp.payload.errores[0],
    })
    return
  }
  refrescarRoles()
  await quasarUtils.aviso({
    exito: true,
    mensaje: estado
      ? traducir('RegistroDesactivadoConExitoMensaje')
      : traducir('RegistroActivadoConExitoMensaje'),
  })
}

const borrarRol = async (id) => {
  quasarUtils.cargandoSimple()
  const resp = await identidadGFuelAdmin.borrarRol(id)
  quasarUtils.ocultarCargandoSimple()
  if (!resp.exito) {
    quasarUtils.aviso({
      error: true,
      mensaje: resp.payload.errores[0],
    })
    return
  }
  refrescarRoles()
  await quasarUtils.aviso({
    exito: true,
    mensaje: traducir('RegistroBorradoConExitoMensaje'),
  })
}

onMounted(async () => {
  refreshSource()
  refrescarRoles()
})

const llenarArrayOpciones = (props) => {
  const arrayOpciones = []
  if (estaPermisoOtorgado(pIdentidad.PermisoEditarRol)) {
    arrayOpciones.push({
      titulo: traducir('Editar'),
      descripcion: traducir('EditarInformacion'),
      icono: 'fa fa-pen',
      color: 'warning',
      accion: () => mostrarCrearRolDialog({ editar: true, rol: props.row }),
    })
  }

  if (estaPermisoOtorgado(pIdentidad.PermisoCambiarEstadoRol)) {
    arrayOpciones.push({
      titulo: traducir('Estado'),
      descripcion: props.row.estado ? traducir('DesactivarRegistro') : traducir('ActivarRegistro'),
      icono: props.row.estado ? 'fa fa-ban' : 'fa fa-check',
      color: props.row.estado ? 'warning' : 'positive',
      accion: () =>
        cambiarEstadoRol(props.row.id, usuario.idUsuario, props.row.estado, props.row.nombreRol),
    })
  }

  if (estaPermisoOtorgado(pIdentidad.PermisoBorrarRol)) {
    arrayOpciones.push({
      titulo: traducir('Borrar'),
      descripcion: traducir('EliminarRegistro'),
      icono: 'fa fa-trash',
      color: 'negative',
      accion: () => borrarRol(props.row.id, props.row.nombreRol),
    })
  }

  return arrayOpciones
}
</script>

<style scoped>
.rounded-badge {
  height: 26px;
  border-radius: 6px;
}
</style>
