<template>
  <q-layout view="Lhh LpR fff">
    <G3Drawer v-model="leftDrawerOpen" />

    <G3Header @toggleDrawer="toggleDrawer" />

    <div v-if="$q.screen.lt.md && !leftDrawerOpen"
      class="fixed-left bg-primary shadow-5 cursor-pointer row items-center justify-center pestana-drawer"
      @click="leftDrawerOpen = true">
      <i class="pi pi-angle-right text-primary-contrast text-size-16 text-weight-bold"></i>
      <div class="absolute-full rounded-borders pulse-effect"></div>
    </div>

    <q-page-container :class="['bg-fondo2', $q.screen.gt.sm ? 'q-ml-md q-px-md' : 'q-px-md']">
      <router-view v-slot="{ Component, route }">
        <transition name="fade-slide" mode="out-in">
          <component :is="Component" :key="route.path" />
        </transition>
      </router-view>
    </q-page-container>
  </q-layout>
</template>

<script setup>
import { ref, watch, onMounted } from 'vue' // added onMounted
import { useQuasar } from 'quasar'
import G3Drawer from 'src/components/Disenio/DrawerLayout/G3Drawer.vue'
import G3Header from 'src/components/Disenio/HeaderLayout/G3Header.vue'
import { iniciarSignalR, detenerSignalR } from 'src/services/SignalRService.js'
import { useSesionStore } from 'src/stores/sesion'

const $q = useQuasar()
const storeSesion = useSesionStore()

// Activate SignalR
onMounted(() => {
  if (storeSesion.estaAutentificado) {
    iniciarSignalR()
  }
})

watch(
  () => storeSesion.estaAutentificado,
  (estaAutentificado) => {
    if (estaAutentificado) {
      iniciarSignalR()
    } else {
      detenerSignalR()
    }
  },
)

// Estado Inicial
const leftDrawerOpen = ref($q.screen.gt.sm)

const toggleDrawer = () => {
  leftDrawerOpen.value = !leftDrawerOpen.value
}

// --- SOLUCIÓN DEL BUG ---
watch(
  () => $q.screen.gt.sm,
  (esEscritorio) => {
    if (esEscritorio) {
      // Pasó a PC -> Forzar ABRIR (Fijo)
      leftDrawerOpen.value = false

      setTimeout(() => {
        // Pasó a PC -> Forzar ABRIR (Fijo)
        leftDrawerOpen.value = true
      }, 100)
    } else {
      setTimeout(() => {
        leftDrawerOpen.value = false
      }, 100)
    }
  },
)
</script>

<style scoped>
/* Tus estilos se mantienen igual */
.pestana-drawer {
  width: 30px;
  height: 60px;
  top: 50%;
  transform: translateY(-50%);
  border-top-right-radius: 50px;
  border-bottom-right-radius: 50px;
  z-index: 6000;
  opacity: 0.8;
  transition: all 0.3s ease;
}

.pulse-effect {
  animation: pulse-shadow 2s infinite;
  border-top-right-radius: 50px;
  border-bottom-right-radius: 50px;
}

@keyframes pulse-shadow {
  0% {
    box-shadow: 0 0 0 0 rgba(25, 118, 210, 0.7);
  }

  70% {
    box-shadow: 0 0 0 10px rgba(25, 118, 210, 0);
  }

  100% {
    box-shadow: 0 0 0 0 rgba(25, 118, 210, 0);
  }
}

/* Transiciones suaves entre páginas/módulos */
.fade-slide-enter-active,
.fade-slide-leave-active {
  transition: all 0.25s ease-out;
}

.fade-slide-enter-from {
  opacity: 0;
  transform: translateX(20px);
}

.fade-slide-leave-to {
  opacity: 0;
  transform: translateX(-20px);
}
</style>
