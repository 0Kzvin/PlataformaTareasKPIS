<template>
  <q-card class="bg-fondo3" :style="estiloCard">
    <q-card-section class="bg-primary text-primary-contrast row items-center q-py-sm shrink">
      <q-icon name="camera_alt" size="20px" class="q-mr-sm" />
      <div class="text-subtitle1 text-weight-bold">{{ traducir('CambiarFotoDePerfil') }}</div>
      <q-space />
      <q-btn dense flat icon="close" v-close-popup />
    </q-card-section>

    <q-card-section class="scroll full-width q-pa-md text-center">
      <div class="column items-center justify-center q-mb-lg">
        <q-avatar size="150px" class="shadow-3 q-mb-md bg-grey-2 relative-position">
          <q-img
            :src="previewUrl || usuario.foto || noUsuarioFoto"
            style="height: 100%; width: 100%"
            fit="cover"
            loading="eager"
          />

          <div
            v-if="previewUrl"
            class="absolute-bottom bg-black text-primary-contrast text-caption q-py-xs"
            style="opacity: 0.7"
          >
            {{ traducir('Nueva') }}
          </div>
        </q-avatar>

        <div class="text-subtitle2 text-grey-7 q-mb-sm">
          {{ previewUrl ? traducir('VistaPreviaDeLaNuevaImagen') : traducir('ImagenActual') }}
        </div>

        <q-file
          v-model="fotoUsuario"
          outlined
          dense
          :label="traducir('SeleccionarImagen')"
          accept=".jpg, .jpeg, .png, image/*"
          class="full-width"
          style="max-width: 300px"
          @update:model-value="generarPreview"
        >
          <template v-slot:prepend>
            <q-icon name="attach_file" />
          </template>
          <template v-slot:append>
            <q-icon
              v-if="fotoUsuario"
              name="close"
              @click.stop.prevent="limpiarSeleccion"
              class="cursor-pointer"
            />
          </template>
        </q-file>

        <div v-if="errorSeleccion" class="text-negative text-caption q-mt-sm">
          {{ traducir('PorFavorSeleccioneUnaImagenValida') }}
        </div>
      </div>

      <div class="row justify-end q-mt-md q-gutter-sm">
        <q-btn
          flat
          :label="traducir('Cancelar')"
          color="negative"
          @click="cancelarForm"
          :dense="$q.screen.lt.sm"
        />
        <q-btn
          unelevated
          :label="traducir('GuardarFoto')"
          color="primary"
          @click="enviarFormulario"
          icon="save"
          :disable="!fotoUsuario"
          :dense="$q.screen.lt.sm"
        />
      </div>
    </q-card-section>
  </q-card>
</template>

<script setup>
import { ref, inject, computed, onUnmounted } from 'vue'
import { identidadGFuelAdmin } from 'src/api/moduloAdministracion'
import { useSesionStore } from 'src/stores/sesion'
import * as quasarUtils from 'src/utils/quasar-utils.js'
import { useQuasar } from 'quasar'
import G3LogoLightMini from 'src/assets/G3LogoLightMini.svg'
import G3LogoDarkMini from 'src/assets/G3LogoDarkMini.svg'

const $q = useQuasar()
const traducir = inject('traducir')
const emit = defineEmits(['onDialogCancel', 'onDialogHide', 'onDialogOK'])
const store = useSesionStore()

const noUsuarioFoto = computed(() => ($q.dark.isActive ? G3LogoDarkMini : G3LogoLightMini))

// Obtenemos usuario actual para mostrar la foto vieja
const usuario = store.ObtenerUsuario

// Refs
const fotoUsuario = ref(null) // Archivo File
const previewUrl = ref(null) // URL blob para mostrar
const errorSeleccion = ref(false)

// === ESTILOS RESPONSIVOS ===
const estiloCard = computed(() => ({
  width: '100%',
  maxWidth: '500px', // Tarjeta pequeña y centrada
  height: 'auto',
  maxHeight: '90vh',
  borderRadius: '12px',
  display: 'flex',
  flexDirection: 'column',
  margin: $q.screen.lt.sm ? '0 10px' : '0',
}))

// === MÉTODOS ===

// Generar URL temporal para previsualizar sin subir
const generarPreview = (file) => {
  if (file) {
    previewUrl.value = URL.createObjectURL(file)
    errorSeleccion.value = false
  } else {
    previewUrl.value = null
  }
}

const limpiarSeleccion = () => {
  fotoUsuario.value = null
  previewUrl.value = null
}

const cancelarForm = () => {
  emit('onDialogCancel')
}

const enviarFormulario = async () => {
  if (!fotoUsuario.value) {
    errorSeleccion.value = true
    return
  }

  const decision = await quasarUtils.decision({ mensaje: traducir('AsignarImagenPregunta') })

  if (decision) {
    const fotoModel = {
      IdUsuario: usuario.idUsuario,
      Foto: fotoUsuario.value,
    }

    quasarUtils.cargandoSimple()
    const resp = await identidadGFuelAdmin.cambiarFotoPerfil(fotoModel)
    quasarUtils.ocultarCargandoSimple()

    if (!resp.exito) {
      quasarUtils.aviso({
        error: true,
        mensaje: resp.payload.errors ? resp.payload.errors[0] : traducir('ErrorAlSubirImagen'),
      })
      return
    }

    // Actualizar Store
    await store.refrescarFotoUsuarios({ foto: resp.payload.foto })

    await quasarUtils.aviso({ exito: true, mensaje: resp.payload.mensaje })
    emit('onDialogOK')
  }
}

// Limpieza de memoria al destruir componente
onUnmounted(() => {
  if (previewUrl.value) {
    URL.revokeObjectURL(previewUrl.value)
  }
})
</script>
