<template>
  <router-view />
  <SinInternet v-if="!isOnline" />
</template>


<script setup>
import { onMounted } from 'vue'
import { usePreferenciaStore } from 'src/stores/preferencias'
import { usePwaStore } from 'src/stores/pwa'
import { useOnline } from '@vueuse/core'
import { useQuasar } from 'quasar'
import { traducir } from 'src/services/TranslationService.js'
import SinInternet from 'src/pages/SinInternet.vue'

const isOnline = useOnline()
const preferencias = usePreferenciaStore()
const pwaStore = usePwaStore()
const $q = useQuasar()

onMounted(() => {
  preferencias.cargarConfiguraciones()

  // Escuchar evento de actualización del Service Worker
  document.addEventListener('swUpdated', updateAvailable, { once: true })

  // Escuchar evento de instalación y pasarlo al Store
  window.addEventListener('beforeinstallprompt', (e) => {
    pwaStore.captureEvent(e)
  })
})

function updateAvailable(event) {
  const registration = event.detail
  $q.notify({
    message: traducir('NuevaVersionDisponible'),
    icon: 'system_update',
    color: 'primary',
    position: 'top',
    timeout: 0,
    actions: [
      {
        label: traducir('ActualizarAhora'),
        color: 'white',
        handler: () => {
          if (registration && registration.waiting) {
            registration.waiting.postMessage({ type: 'SKIP_WAITING' })
            // Recargar la página cuando el nuevo SW tome el control
            navigator.serviceWorker.addEventListener('controllerchange', () => {
              window.location.reload()
            })
          }
        },
      },
    ],
  })
}
</script>
