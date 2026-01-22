<template>
  <q-card class="bg-fondo3" :style="estiloCard">
    <q-card-section class="bg-primary text-primary-contrast row items-center q-py-xs shrink">
      <q-icon name="edit" size="20px" class="q-mr-sm" />
      <div class="text-subtitle1 text-weight-bold">{{ traducir('EditarUsuario') }}</div>
      <q-space />
      <q-btn dense flat icon="close" v-close-popup />
    </q-card-section>

    <q-card-section class="scroll full-width" style="padding: 0">
      <q-form @submit="enviarFormulario" class="q-pa-md">
        <div class="row q-col-gutter-sm">
          <div class="col-12 col-sm-6">
            <q-input v-model="nombres" filled :label="traducir('Nombre') + ' *'" :dense="$q.screen.lt.sm"
              :class="claseInput" :rules="[(val) => !!val || traducir('CompletarCampo')]">
              <template v-slot:prepend><q-icon name="person" size="xs" /></template>
            </q-input>
          </div>

          <div class="col-12 col-sm-6">
            <q-input v-model="apellidos" filled :label="traducir('Apellido') + ' *'" :dense="$q.screen.lt.sm"
              :class="claseInput" :rules="[(val) => !!val || traducir('CompletarCampo')]">
              <template v-slot:prepend><q-icon name="person_outline" size="xs" /></template>
            </q-input>
          </div>

          <div class="col-12 col-sm-6">
            <q-input v-model="nombreUsuario" filled :label="traducir('Usuario') + ' *'" :dense="$q.screen.lt.sm"
              :class="claseInput" :rules="[(val) => !!val || traducir('CompletarCampo')]">
              <template v-slot:prepend><q-icon name="badge" size="xs" /></template>
            </q-input>
          </div>

          <div class="col-12 col-sm-6">
            <q-input v-model="correo" filled type="email" :label="traducir('Correo')" :dense="$q.screen.lt.sm"
              :class="claseInput" bottom-slots>
              <template v-slot:prepend><q-icon name="email" size="xs" /></template>
            </q-input>
          </div>

          <div class="col-12 col-sm-6">
            <q-input v-model="numeroTelefonico" filled mask="###-###-####" unmasked-value :label="traducir('Telefono')"
              :dense="$q.screen.lt.sm" :class="claseInput" bottom-slots>
              <template v-slot:prepend><q-icon name="phone" size="xs" /></template>
            </q-input>
          </div>

          <div class="col-12 col-sm-6">
            <q-input v-model="password" filled type="password" :label="traducir('ContrasenaOpcional')"
              :hint="traducir('VacioParaMantenerActual')" :dense="$q.screen.lt.sm" :class="claseInput">
              <template v-slot:prepend><q-icon name="lock" size="xs" /></template>
            </q-input>
          </div>
        </div>

        <div class="row justify-end q-mt-md q-gutter-sm">
          <q-btn flat :label="traducir('Cancelar')" color="negative" @click="cancelarForm" :dense="$q.screen.lt.sm" />
          <q-btn unelevated :label="traducir('EnviarCambios')" color="primary" type="submit" icon="save"
            :dense="$q.screen.lt.sm" />
        </div>
      </q-form>
    </q-card-section>
  </q-card>
</template>

<script setup>
import { ref, inject, computed } from 'vue'
import { useSesionStore } from 'src/stores/sesion'
import { identidadGFuelAdmin } from 'src/api/moduloAdministracion'
import * as quasarUtils from 'src/utils/quasar-utils.js'
import { useQuasar } from 'quasar'

const $q = useQuasar()

// === ESTILOS RESPONSIVOS ===
const estiloCard = computed(() => ({
  width: '100%',
  maxWidth: '800px',
  display: 'flex',
  flexDirection: 'column',
  height: 'auto',
  maxHeight: '90dvh', // Evita que se salga de la pantalla verticalmente
  borderRadius: '12px',
  // En móvil aseguramos que no toque los bordes laterales
  margin: $q.screen.lt.sm ? '0 5px' : '0',
}))

// Clase para reducir fuente en móvil
const claseInput = computed(() => {
  return $q.screen.lt.sm ? 'text-caption' : ''
})

// === PROPS ===
const props = defineProps({
  usuario: { type: Object, default: null },
  propsCustomForm: { type: Object, default: null },
})

const emit = defineEmits(['onDialogCancel', 'onDialogHide', 'onDialogOK'])
const traducir = inject('traducir')
const store = useSesionStore()

// === LÓGICA DE DATOS ===
const usuarioData = props.usuario || props.propsCustomForm?.usuario || {}

const nombres = ref(usuarioData.nombre || '')
const apellidos = ref(usuarioData.apellidos || '')
const nombreUsuario = ref(usuarioData.nombreUsuario || '')
const correo = ref(usuarioData.correo || '')
const numeroTelefonico = ref(usuarioData.numeroTelefonico || '')
const password = ref('')

const cancelarForm = () => {
  emit('onDialogCancel')
}

const enviarFormulario = async () => {
  const nombresTrim = nombres.value?.trim()
  const apellidosTrim = apellidos.value?.trim()
  const nombreUsuarioTrim = nombreUsuario.value?.trim()

  if (!nombresTrim || !apellidosTrim || !nombreUsuarioTrim) {
    quasarUtils.aviso({ error: true, mensaje: traducir('FormularioInvalido') })
    return
  }

  const decision = await quasarUtils.decision({ mensaje: traducir('EditarUsuarioPregunta') })

  if (decision) {
    const model = {
      IdUsuario: usuarioData.idUsuario,
      Username: nombreUsuarioTrim,
      Email: correo.value?.trim(),
      NombreCompleto: `${nombresTrim} ${apellidosTrim}`,
      Nombre: nombresTrim,
      Apellidos: apellidosTrim,
      PhoneNumber: numeroTelefonico.value?.trim(),
      ...(password.value ? { Password: password.value } : {}),
    }

    quasarUtils.cargandoSimple({})
    const resp = await identidadGFuelAdmin.editarMiUsuario(model)
    quasarUtils.ocultarCargandoSimple()

    if (!resp.exito) {
      quasarUtils.aviso({ error: true, mensaje: resp.payload.errores[0] })
      return
    }

    await quasarUtils.aviso({ exito: true, mensaje: resp.payload.mensaje })
    await store.refrescarDatosUsuarios({ infoUsuario: model })
    emit('onDialogOK')
  }
}
</script>

<style scoped>
/* Ajuste fino para inputs en móvil si 'dense' no es suficiente */
</style>
