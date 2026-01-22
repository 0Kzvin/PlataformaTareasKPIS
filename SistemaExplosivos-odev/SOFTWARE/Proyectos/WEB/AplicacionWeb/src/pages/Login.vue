<!-- eslint-disable vue/multi-word-component-names -->
<template>
  <q-page class="row window-height window-width overflow-hidden bg-fondo3">
    <div class="col-md-6 gt-sm" style="height: 100dvh">
      <div class="row justify-center" style="height: 100%; width: 100%; padding: 32px; box-sizing: border-box">
        <q-img :src="fotoLogin" fit="cover" style="height: 100%; width: 100%; border-radius: 16px" loading="eager" />
      </div>
    </div>

    <div class="col-12 col-md-6 column bg-fondo3 hide-scrollbar" style="height: 100dvh; overflow-y: auto">
      <div class="col-grow column justify-center items-center q-px-md"
        style="min-height: min-content; padding-top: 20px; padding-bottom: 20px">
        <div class="column items-center justify-center full-width">
          <G3Cliente class="q-mb-md" />

          <GBlastLogo :width="$q.screen.lt.md ? 280 : 480" :height="$q.screen.lt.md ? 68 : 120" class="q-mb-md" />

          <div class="text-h6 q-mb-md text-weight-bold text-textprimary" v-if="olvideMiContrasenia">
            {{ traducir('RecuperacionCredenciales') }}
          </div>

          <div class="row justify-center q-mt-md" style="max-width: 493px; width: 100%">
            <form style="width: 100%"
              @submit.prevent="olvideMiContrasenia ? onSubmitOlvideMiContrasenia() : onSubmit()">
              <div class="q-mb-md">
                <q-input v-model="usuarioOCorreo" type="text" rounded outlined borderless :label="olvideMiContrasenia
                  ? traducir('IngresaUsuarioCorreo')
                  : traducir('UsuarioCorreo')
                  " input-class="text-textprimary" label-color="textsecondary" class="bg-fondo2 text-size-16"
                  style="border-radius: 28px">
                  <template v-slot:prepend>
                    <q-icon name="person" class="text-textsecondary" />
                  </template>
                  <template v-slot:append>
                    <div></div>
                  </template>
                </q-input>
              </div>

              <div v-if="!olvideMiContrasenia">
                <div class="q-mb-md">
                  <q-input v-model="password" :type="isPwdHidden ? 'password' : 'text'" rounded outlined
                    :label="traducir('Contrasena')" input-class="text-textprimary" label-color="textsecondary"
                    class="bg-fondo2 text-size-16" style="border-radius: 28px">
                    <template v-slot:prepend>
                      <q-icon name="lock" class="text-textsecondary" />
                    </template>

                    <template v-slot:append>
                      <q-icon :key="isPwdHidden" :name="isPwdHidden ? 'visibility' : 'visibility_off'"
                        class="cursor-pointer text-textsecondary" @click="togglePasswordVisibility">
                        <q-tooltip>
                          {{ isPwdHidden ? traducir('MostrarPWD') : traducir('OcultarPWD') }} {{ traducir('Contrasena')
                          }}
                        </q-tooltip>
                      </q-icon>
                    </template>
                  </q-input>
                </div>

                <div class="row justify-center items-center q-my-md">
                  <G3CheckBox v-model="deseaMantenerSesion" :label="traducir('MantenerSesion')" class="text-size-14" />
                </div>

                <div class="row q-col-gutter-md">
                  <div class="col-12 col-sm-6">
                    <q-btn outline no-caps class="text-weight-medium full-width text-textprimary"
                      style="min-height: 53px; border: 1px solid var(--q-text-primary)" @click="onReestablecer">
                      <i class="q-mr-md pi pi-refresh text-textprimary text-size-18" />
                      <span class="text-weight-regular text-textprimary text-size-18">{{
                        traducir('Limpiar')
                      }}</span>
                    </q-btn>
                  </div>
                  <div class="col-12 col-sm-6">
                    <q-btn outline color="primary" no-caps class="full-width bg-primary" style="min-height: 53px"
                      type="submit">
                      <i class="q-mr-md pi pi-sign-in text-primary-contrast text-size-18" />
                      <span class="text-weight-regular text-primary-contrast text-size-18">
                        {{ traducir('IniciarSesion') }}
                      </span>
                    </q-btn>
                  </div>
                </div>

                <div class="row justify-center q-pt-lg">
                  <q-btn flat color="primary" no-caps class="full-width" style="min-height: 53px"
                    @click="toggleMode(true)">
                    <span class="text-weight-regular text-primary text-weight-bold text-size-18">
                      {{ traducir('OlvidasteContrasena') }}
                    </span>
                  </q-btn>
                </div>
              </div>

              <div v-else>
                <div class="text-caption text-center q-mb-lg text-textsecondary">
                  {{ traducir('InstruccionesRecuperacion') }}
                </div>

                <div class="row q-col-gutter-md">
                  <div class="col-12 col-sm-6 order-sm-first">
                    <q-btn outline no-caps class="text-weight-medium full-width text-textprimary"
                      style="min-height: 53px; border: 1px solid var(--q-text-primary)" @click="toggleMode(false)">
                      <i class="q-mr-md pi pi-arrow-left text-textprimary text-size-18" />
                      <span class="text-weight-regular text-textprimary text-size-18">{{
                        traducir('Volver')
                      }}</span>
                    </q-btn>
                  </div>
                  <div class="col-12 col-sm-6">
                    <q-btn outline color="primary" no-caps class="full-width bg-primary" style="min-height: 53px"
                      @click="onSubmitOlvideMiContrasenia">
                      <i class="q-mr-md pi pi-send text-primary-contrast text-size-18" />
                      <span class="text-weight-regular text-primary-contrast text-size-18">
                        {{ traducir('Recuperar') }}
                      </span>
                    </q-btn>
                  </div>
                </div>
              </div>
            </form>
          </div>
        </div>


        <div class="q-mb-xl text-center q-mt-lg">
          <!-- Botón PWA Prominente -->
          <div v-if="pwaStore.canInstall" class="q-mb-lg full-width">
            <q-btn outline color="primary" no-caps class="full-width bg-primary" style="min-height: 53px"
              @click="pwaStore.installApp">
              <i class="q-mr-md pi pi-download text-primary-contrast text-size-18" />
              <span class="text-weight-regular text-primary-contrast text-size-18">
                {{ traducir('InstalarAplicacion') }}
              </span>
            </q-btn>
          </div>

          <G3Logo :width="250" :height="100" />
          <div class="text-caption text-textsecondary q-mt-md">
            {{ traducir('DerechosReservados') }}
          </div>
        </div>
      </div>
    </div>
  </q-page>
</template>

<script setup>
import { ref, inject } from 'vue'
import { useRouter } from 'vue-router'
import { useQuasar } from 'quasar'
import { useSesionStore } from 'src/stores/sesion'
import { usePwaStore } from 'src/stores/pwa'

// Recursos
import fotoLogin from 'src/assets/fondo_pipa.jpeg'
import { G3Logo, GBlastLogo } from 'src/components/Logos'

// Componentes y Utils
import TransparentDialog from 'src/components/Globales/Dialogs/TransparentLoadingDialog.vue'
import CustomFormDialog from 'src/components/Globales/Dialogs/CustomFormDialog.vue'
import RecuperarCuentaVue from 'src/components/Identidad/RecuperarCuenta.vue'
import G3Cliente from 'src/components/Disenio/HeaderLayout/G3Cliente.vue'

import { identidadGFuelAdmin } from 'src/api/moduloAdministracion'
import * as quasarUtils from 'src/utils/quasar-utils.js'
import { idModuloAdmin, idModuloIds } from 'src/core/modulos'
import { useNavigation } from 'src/composables/useNavigation.js'

const storeSesion = useSesionStore()
const $q = useQuasar()
const router = useRouter()
const pwaStore = usePwaStore()
const traducir = inject('traducir', (key) => key)
const { obtenerMenuPorModulo } = useNavigation()

// -- Variables Reactivas --
// Nota: Usamos 'usuarioOCorreo' para ambos formularios (Login y Recuperación)
const usuarioOCorreo = ref(null)
const password = ref(null)
const deseaMantenerSesion = ref(true)
const isPwdHidden = ref(true)
const errores = ref([])

// Controla qué formulario se ve
const olvideMiContrasenia = ref(false)
const ejecutandoOnSubmit = ref(false)

// -- Métodos Generales --

// Alternar entre Login y Olvidé Contraseña
const toggleMode = (val) => {
  olvideMiContrasenia.value = val
  errores.value = []
  // No limpiamos usuarioOCorreo para que el usuario no tenga que reescribirlo
  if (val) {
    password.value = null // Limpiamos password por seguridad al cambiar de vista
  }
}

// Limpiar campos (Botón Reset en Login)
const onReestablecer = () => {
  usuarioOCorreo.value = null
  password.value = null
  errores.value = []
}

// Toggle Password Visibility
const togglePasswordVisibility = () => {
  isPwdHidden.value = !isPwdHidden.value
}

// -- LÓGICA DE LOGIN --
const onSubmit = async () => {
  if (!usuarioOCorreo.value) return
  if (!password.value) return

  const dialog = $q.dialog({
    component: TransparentDialog,
  })

  try {
    const resp = await identidadGFuelAdmin.login(usuarioOCorreo.value, password.value)
    if (!resp.exito) {
      errores.value = resp.payload.errores
      dialog.hide()
      quasarUtils.mostrarSnackbar({ mensaje: resp.payload.errores[0] })
      return
    }

    // Pinia
    await storeSesion.guardarTokenBotonLogin({
      tokenInfo: resp.payload,
      mantenerSesionF: deseaMantenerSesion.value,
      estaAutentificado: true,
    })

    // Router Logic
    const modulosOtorgados = storeSesion.obtenerModulosOtorgados
    let moduloHome = modulosOtorgados.find((x) => x.id != idModuloAdmin)
    if (!moduloHome) {
      moduloHome = modulosOtorgados.find((x) => x.id)
    }

    if (!moduloHome) {
      quasarUtils.mostrarSnackbar({ mensaje: traducir('NoModulosUsuario') })
      dialog.hide()
      return
    }

    await enviarAModulo(moduloHome.id)
    dialog.hide()
  } catch (error) {
    dialog.hide()
    console.error('Error in login flow:', error)
    quasarUtils.mostrarSnackbar({ mensaje: traducir('ErrorInesperado') || 'Error inesperado' })
  }
}

// -- LÓGICA DE RECUPERAR CONTRASEÑA --
const onSubmitOlvideMiContrasenia = async () => {
  if (ejecutandoOnSubmit.value) return
  ejecutandoOnSubmit.value = true

  // Usamos la misma variable 'usuarioOCorreo'
  if (!usuarioOCorreo.value) {
    quasarUtils.mostrarSnackbar({ mensaje: traducir('IngresaTuUsuario') })
    ejecutandoOnSubmit.value = false
    return
  }

  const dialog = $q.dialog({
    component: TransparentDialog,
  })

  const resp = await identidadGFuelAdmin.solicitarRecuperacionCuenta(usuarioOCorreo.value)

  dialog.hide()

  if (!resp.exito) {
    quasarUtils.mostrarSnackbar({ mensaje: resp.payload.errores[0] })
    ejecutandoOnSubmit.value = false
    return
  }

  // Éxito: Abrimos el diálogo de confirmación/OTP
  const usuario = usuarioOCorreo.value

  $q.dialog({
    component: CustomFormDialog,
    componentProps: {
      formularioComponent: RecuperarCuentaVue,
      formularioComponentProps: {
        usuario,
      },
    },
  })

  // Reseteamos la vista al login
  olvideMiContrasenia.value = false
  ejecutandoOnSubmit.value = false
}

// -- Helpers de Ruteo --
const enviarAModulo = async (idModulo = 0) => {
  // Ahora usamos el composable para obtener las rutas del módulo
  // obtenerMenuPorModulo ya filtra por permisos internamente
  const listaRutasModulo = obtenerMenuPorModulo(idModulo)

  // Buscamos la primera ruta disponible en los grupos devueltos
  // listaRutasModulo es un Array de Grupos (objetos con propiedad 'items')

  // Aplanamos items de todos los grupos
  const todosLosItems = listaRutasModulo.flatMap((grupo) => grupo.items || [])

  const primeraRutaHabilitada = todosLosItems.find((item) => !item.paginaOculta)?.ruta

  // Fallback a buscar en items directos si la estructura fuera distinta (solo por seguridad)
  // Pero useNavigation retorna grupos [{items:[]}]

  const modulosValidos = Object.values(idModuloIds)

  try {
    if (modulosValidos.includes(idModulo) && primeraRutaHabilitada) {
      await router.push({ path: primeraRutaHabilitada })
    } else {
      await router.push({ name: 'Error404' })
    }
  } catch (error) {
    console.error('Error navigating/sending to module:', error)
    // Optional: show a snackbar or handle specific router errors
  }
}
</script>

<style scoped lang="scss">
/* Scrollbar invisible pero funcional */
.hide-scrollbar::-webkit-scrollbar {
  display: none;
}

.hide-scrollbar {
  -ms-overflow-style: none;
  scrollbar-width: none;
}
</style>
