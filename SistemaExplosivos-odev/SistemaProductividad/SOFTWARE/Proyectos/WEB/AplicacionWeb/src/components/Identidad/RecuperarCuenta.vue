<template>
  <q-card class="q-pa-md shadow-10 relative-position" flat bordered style="max-width: 500px; max-height: 525px">
    <q-card-section class="column items-center q-pb-none q-pt-sm">
      <q-avatar size="60px" class="q-mb-sm text-size-32" color="red-1" text-color="primary">
        <q-icon name="security" />
      </q-avatar>

      <div class="text-h6 text-weight-bold text-textprimary text-center" style="line-height: 1.2">
        {{ !codigoCorrecto ? traducir('Recuperacion') : traducir('CambiarContrasenia') }}
      </div>
      <div class="text-caption text-textsecondary text-center q-mt-xs">
        {{
          !codigoCorrecto
            ? traducir('IngresaElCodigoEnviadoATuCorreo')
            : traducir('DefineTusNuevasCredenciales')
        }}
      </div>
    </q-card-section>

    <q-card-section class="q-pt-md">
      <div v-if="!codigoCorrecto" class="column q-gutter-y-md">
        <div class="bg-blue-1 text-primary q-pa-sm rounded-borders text-center text-caption text-size-12"
          style="line-height: 1.3">
          <span class="text-weight-bold">{{ traducir('EsperarCorreo') }}</span>
          <div class="q-mt-xs text-textsecondary text-size-10">
            {{ traducir('RevisaTuBandejaDeEntradaOSpam') }}
          </div>
        </div>

        <div>
          <q-input outlined v-model="codigoDeRecuperacion" placeholder="000000" mask="######"
            input-class="text-center text-h5 text-weight-bold letter-spacing-custom" class="input-code-style" autofocus
            dense />
        </div>

        <div class="column items-center">
          <div v-if="!solicitarNuevoCodigo"
            class="row items-center text-positive text-weight-medium text-caption q-mb-xs text-bold">
            <q-icon name="check_circle" size="sm" class="q-mr-xs" />
            {{ traducir('ListoParaReenviar') }}
          </div>

          <div class="text-center text-md">
            <span class="text-textsecondary">{{ traducir('NoLlego') }} </span>

            <q-btn v-if="!solicitarNuevoCodigo" flat dense no-caps color="primary"
              class="text-weight-bold text-underline q-pa-sm q-ml-xs" size="md" :label="traducir('ReenviarCodigo')"
              @click="reenviarCodigo()" />

            <span v-else class="text-red text-weight-bold q-ml-sm">
              {{ traducir('Espere') }} {{ (timeRemaining / 1000).toFixed(0) }}s
            </span>
          </div>
        </div>

        <div class="column q-gutter-y-sm q-mt-sm">
          <q-btn color="primary" class="full-width shadow-2 text-size-16" style="height: 48px; border-radius: 12px"
            no-caps unelevated :loading="cargandoRevisionCodigo" @click="verificarCodigo">
            <span class="text-weight-bold">{{ traducir('RevisarCodigo') }}</span>
          </q-btn>

          <q-btn outline color="textsecondary" no-caps class="full-width text-size-16" style="
              height: 48px;
              border-radius: 12px;
              border: 1px solid var(--q-text-secondary);
            " @click="cancelarForm()">
            {{ traducir('Cancelar') }}
          </q-btn>
        </div>
      </div>

      <div v-else class="column q-gutter-y-md">
        <div>
          <label class="text-weight-bold text-textsecondary q-mb-xs block text-caption">{{
            traducir('ContraNueva')
          }}</label>
          <q-input outlined v-model="nuevoPassword" :type="passwordOculto ? 'password' : 'text'" placeholder="********"
            class="input-code-style" dense>
            <template v-slot:append>
              <q-icon :name="passwordOculto ? 'visibility_off' : 'visibility'" class="cursor-pointer text-textsecondary"
                size="xs" @click="passwordOculto = !passwordOculto" />
            </template>
          </q-input>
        </div>

        <div>
          <label class="text-weight-bold text-textsecondary q-mb-xs block text-caption">{{
            traducir('ConfirmarContraNueva')
          }}</label>
          <q-input outlined v-model="confirmarNuevoPassword" :type="confirmarPasswordOculto ? 'password' : 'text'"
            placeholder="********" class="input-code-style" dense>
            <template v-slot:append>
              <q-icon :name="confirmarPasswordOculto ? 'visibility_off' : 'visibility'"
                class="cursor-pointer text-textsecondary" size="xs"
                @click="confirmarPasswordOculto = !confirmarPasswordOculto" />
            </template>
          </q-input>
        </div>

        <div class="column q-gutter-y-sm q-mt-sm">
          <q-btn color="primary" class="full-width shadow-2 text-size-16" style="height: 48px; border-radius: 12px"
            no-caps unelevated :loading="cambiandoPassword" @click="cambiarContrasenia">
            <span class="text-weight-bold">{{ traducir('CambiarContrasenia') }}</span>
          </q-btn>

          <q-btn outline color="textsecondary" no-caps class="full-width text-size-16" style="height: 48px; border-radius: 12px; border: 1px
            solid var(--q-text-secondary);" @click="cancelarForm()">
            {{ traducir('Cancelar') }}
          </q-btn>
        </div>
      </div>
    </q-card-section>
  </q-card>
</template>

<script setup>
import { computed, ref, toRefs, onBeforeUnmount, onMounted, inject } from 'vue'
import { identidadGFuelAdmin } from 'src/api/moduloAdministracion'
import * as quasarUtils from 'src/utils/quasar-utils.js'

// Inyecciones
const traducir = inject('traducir')
const emit = defineEmits(['onDialogCancel', 'onDialogHide', 'onDialogOK'])

// Props
const props = defineProps({
  propsCustomForm: {
    type: Object,
    default() {
      return { usuario: '' }
    },
  },
})

const { propsCustomForm } = toRefs(props)
const { usuario } = toRefs(propsCustomForm.value)

// Estados
const codigoDeRecuperacion = ref('')
const cargandoRevisionCodigo = ref(false)
const codigoCorrecto = ref(false)

const nuevoPassword = ref('')
const passwordOculto = ref(true)
const confirmarPasswordOculto = ref(true)
const confirmarNuevoPassword = ref('')
const cambiandoPassword = ref(false)

// Timer
const solicitarNuevoCodigo = ref(false)
const timeoutId = ref(null)
const endTime = ref(0)
const defaultTime = 30000
const timeRemaining = ref(defaultTime)

const startTimer = () => {
  const timeoutDuration = defaultTime
  endTime.value = Date.now() + timeRemaining.value
  solicitarNuevoCodigo.value = true
  timeoutId.value = setInterval(() => {
    timeRemaining.value = endTime.value - Date.now()
    if (timeRemaining.value <= 0) {
      clearInterval(timeoutId.value)
      solicitarNuevoCodigo.value = false
      timeRemaining.value = timeoutDuration
    }
  }, 100)
}

// Acciones API
const reenviarCodigo = async () => {
  if (!usuario.value) return
  const resp = await identidadGFuelAdmin.solicitarRecuperacionCuenta(usuario.value)

  await quasarUtils.aviso({
    error: !resp.exito,
    exito: resp.exito,
    mensaje: resp.exito ? resp.payload.mensaje : resp.payload.errores[0],
  })

  if (resp.exito) {
    timeRemaining.value = defaultTime
    startTimer()
  }
}

const verificarCodigo = async () => {
  if (!codigoDeRecuperacion.value) return
  cargandoRevisionCodigo.value = true

  const resp = await identidadGFuelAdmin.verificarCodigoRecuperacion(codigoDeRecuperacion.value)

  if (!resp.exito) {
    await quasarUtils.aviso({ error: true, exito: false, mensaje: resp.payload.mensaje })
    codigoCorrecto.value = false
    codigoDeRecuperacion.value = ''
    cargandoRevisionCodigo.value = false
    return
  }

  codigoCorrecto.value = true
  cargandoRevisionCodigo.value = false
}

const cambiarContrasenia = async () => {
  if (!longitudContras.value) {
    await quasarUtils.aviso({
      error: true,
      mensaje: traducir('LasContrasenasDebenTenerAlMenos5Caracteres'),
    })
    return
  }
  if (!coincidenContras.value) {
    await quasarUtils.aviso({ error: true, mensaje: traducir('LasContrasenasNoCoinciden') })
    return
  }

  cambiandoPassword.value = true
  const data = {
    password: nuevoPassword.value,
    confirmarPassword: confirmarNuevoPassword.value,
    codigo: codigoDeRecuperacion.value,
  }

  const resp = await identidadGFuelAdmin.recuperarCuenta(data)

  if (!resp.exito) {
    await quasarUtils.aviso({ error: true, mensaje: resp.payload.errores[0] })
    cambiandoPassword.value = false
    return
  }

  await quasarUtils.aviso({ exito: true, mensaje: resp.payload.mensaje })
  cambiandoPassword.value = false
  cancelarForm()
}

const cancelarForm = () => emit('onDialogCancel')

// Computadas
const coincidenContras = computed(() => nuevoPassword.value == confirmarNuevoPassword.value)
const longitudContras = computed(
  () => nuevoPassword.value.length >= 5 && confirmarNuevoPassword.value.length >= 5,
)

// Lifecycle
onMounted(() => startTimer())
onBeforeUnmount(() => {
  if (timeoutId.value) clearInterval(timeoutId.value)
})
</script>

<style scoped lang="scss">
/* Espaciado para los números del código */
.letter-spacing-custom {
  letter-spacing: 5px;
}

/* INPUT PERSONALIZADO (Compacto pero moderno) */
:deep(.input-code-style .q-field__control) {
  background: var(--q-fondo2) !important;
  border-radius: 12px !important;
  height: 52px;
  /* Reduje de 60px a 52px para que no sea tan alto */
  font-size: 16px;
}

/* Bordes */
:deep(.input-code-style .q-field__control:before) {
  border: 1.5px solid var(--q-text-secondary) !important;
}

:deep(.input-code-style.q-field--focused .q-field__control:after) {
  border: 2px solid var(--q-primary) !important;
  border-radius: 12px !important;
}

/* Sin sombra */
:deep(.input-code-style .q-field__shadow) {
  display: none;
}
</style>
