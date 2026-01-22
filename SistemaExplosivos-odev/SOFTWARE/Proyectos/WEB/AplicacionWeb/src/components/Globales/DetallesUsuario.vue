<template>
  <q-card
    style="width: 90vw; max-width: 500px; height: auto; border-radius: 16px; overflow: hidden"
    class="bg-fondo3 shadow-3"
  >
    <div class="bg-primary relative-position" style="height: 110px">
      <q-btn
        dense
        flat
        icon="close"
        color="white"
        class="absolute-top-right q-ma-sm"
        v-close-popup
        style="z-index: 10"
      />
    </div>

    <div class="q-px-lg q-pb-lg relative-position">
      <div style="margin-top: -55px; margin-bottom: 12px" class="row justify-between items-end">
        <div class="relative-position" style="width: 110px; height: 110px">
          <q-avatar
            size="110px"
            class="bg-fondo3 shadow-2 absolute"
            style="padding: 4px; top: 0; left: 0"
          >
            <q-img
              :src="usuarioData.foto || noUsuarioFoto"
              style="border-radius: 50%; height: 100%; width: 100%"
              fit="cover"
              position="center"
              loading="eager"
            />
          </q-avatar>
          <q-btn
            round
            color="secondary"
            icon="camera_alt"
            size="sm"
            class="absolute-bottom-right shadow-2"
            style="bottom: 5px; right: 5px; z-index: 20"
            @click="mostrarCambiarFotoPerfilDialog"
          />
        </div>

        <q-btn
          outline
          rounded
          color="primary"
          icon="edit"
          :label="traducir('Editar')"
          class="q-mb-xs"
          @click="mostrarEditarUsuarioDialog"
        />
      </div>

      <div class="q-mb-md">
        <div class="text-h6 text-weight-bold text-textprimary ellipsis">
          {{ usuarioData.nombreCompleto }}
        </div>
        <div class="text-subtitle2 text-textsecondary">
          {{ usuarioData.rol || traducir('RolNoAsignado') }}
        </div>
      </div>

      <q-separator class="q-mb-md" />

      <q-list class="text-textprimary">
        <q-item class="q-px-none">
          <q-item-section avatar style="min-width: 40px">
            <q-icon name="alternate_email" color="primary" size="22px" />
          </q-item-section>
          <q-item-section>
            <q-item-label caption>{{ traducir('CorreoElectronico') }}</q-item-label>
            <q-item-label class="text-body2 text-textprimary text-weight-medium ellipsis">{{
              usuarioData.correo
            }}</q-item-label>
          </q-item-section>
        </q-item>

        <q-item class="q-px-none">
          <q-item-section avatar style="min-width: 40px">
            <q-icon name="badge" color="primary" size="22px" />
          </q-item-section>
          <q-item-section>
            <q-item-label caption>{{ traducir('Usuario') }}</q-item-label>
            <q-item-label class="text-body2 text-textprimary text-weight-medium">{{
              usuarioData.nombreUsuario
            }}</q-item-label>
          </q-item-section>
        </q-item>

        <q-item class="q-px-none">
          <q-item-section avatar style="min-width: 40px">
            <q-icon name="phone" color="primary" size="22px" />
          </q-item-section>
          <q-item-section>
            <q-item-label caption>{{ traducir('Telefono') }}</q-item-label>
            <q-item-label class="text-body2 text-textprimary text-weight-medium">{{
              usuarioData.numeroTelefonico || '---'
            }}</q-item-label>
          </q-item-section>
        </q-item>

        <q-item class="q-px-none">
          <q-item-section avatar style="min-width: 40px">
            <q-icon name="event" color="primary" size="22px" />
          </q-item-section>
          <q-item-section>
            <q-item-label caption>{{ traducir('FechaDeRegistro') }}</q-item-label>
            <q-item-label class="text-body2 text-textprimary text-weight-medium">
              {{
                usuarioData.fechaRegistro
                  ? date.formatDate(usuarioData.fechaRegistro, 'DD MMMM YYYY')
                  : '---'
              }}
            </q-item-label>
          </q-item-section>
        </q-item>
      </q-list>
    </div>
  </q-card>
</template>

<script setup>
import { inject, computed } from 'vue'
import { useQuasar, date } from 'quasar'
import CustomFormDialog from 'src/components/Globales/Dialogs/CustomFormDialog.vue'
import editarUsuarioForm from './editarUsuarioForm.vue'
import cambiarFotoPerfilForm from './CambiarFotoPerfil.vue'
import G3LogoLightMini from 'src/assets/G3LogoLightMini.svg'
import G3LogoDarkMini from 'src/assets/G3LogoDarkMini.svg'

const traducir = inject('traducir')
const $q = useQuasar()

const noUsuarioFoto = computed(() => ($q.dark.isActive ? G3LogoDarkMini : G3LogoLightMini))

// Props: Recibimos el usuario directamente o dentro de propsCustomForm (por si acaso)
const props = defineProps({
  usuario: { type: Object, default: () => null },
  propsCustomForm: { type: Object, default: () => ({}) },
})

// Extracción segura del usuario
const usuarioData = computed(() => {
  // Prioridad 1: Prop directo 'usuario'
  if (props.usuario) return props.usuario
  // Prioridad 2: Prop 'propsCustomForm.usuario' (como lo mandamos ahora)
  if (props.propsCustomForm && props.propsCustomForm.usuario) return props.propsCustomForm.usuario
  // Fallback
  return {}
})

const mostrarEditarUsuarioDialog = () => {
  $q.dialog({
    component: CustomFormDialog,
    componentProps: {
      formularioComponent: editarUsuarioForm,
      // Pasamos usuario explícitamente
      formularioComponentProps: { usuario: usuarioData.value },
    },
  })
}

const mostrarCambiarFotoPerfilDialog = () => {
  $q.dialog({
    component: CustomFormDialog,
    componentProps: {
      formularioComponent: cambiarFotoPerfilForm,
      formularioComponentProps: { usuario: usuarioData.value },
    },
  })
}
</script>
