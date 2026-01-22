<template>
  <q-drawer v-model="drawerModel" :mini="esMini" :width="realDrawerWidth" :mini-width="80" :breakpoint="1023"
    side="left" class="bg-fondo3 dynamic-width-drawer">
    <div class="absolute-top bg-fondo3" style="height: 150px; z-index: 50">
      <div class="row items-center full-height q-px-sm" :class="esMini ? 'justify-center column' : 'justify-center'">
        <!-- Logo -->
        <div class="flex flex-center col" v-if="!miniState">
          <div class="column flex-center">
            <GBlastLogo v-show="!esMini" class="q-mr-sm" :width="200" :height="60" />
            <q-btn v-if="pwaStore.canInstall" flat color="textprimary" size="sm" no-caps @click="pwaStore.installApp"
              class="q-mt-xs">
              <q-icon name="download" class="q-mr-xs" />
              {{ traducir('InstalarAplicacion') }}
            </q-btn>
          </div>
        </div>

        <!-- Botón -->
        <div v-if="!$q.screen.lt.md">
          <div class="row q-mb-md" v-if="miniState">
            <GBlastIcon v-show="esMini" :size="48" />
          </div>
          <div class="row justify-center">
            <q-btn dense round flat color="textprimary" @click="toggleMiniState">
              <i :class="[miniState ? 'pi pi-window-maximize' : 'pi pi-window-minimize', 'text-size-20']" />
              <q-tooltip>
                {{ miniState ? traducir('ExpandirMenu') : traducir('ContraerMenu') }}
              </q-tooltip>
            </q-btn>
          </div>

          <!-- Boton PWA en Mini (Ahora debajo del toggle) -->
          <div class="row justify-center q-mt-sm" v-if="pwaStore.canInstall && miniState">
            <q-btn dense round flat color="textprimary" @click="pwaStore.installApp">
              <q-icon name="download" class="text-size-20" />
              <q-tooltip>
                {{ traducir('InstalarAplicacion') }}
              </q-tooltip>
            </q-btn>
          </div>
        </div>
      </div>
    </div>

    <ListaApp :listado="listadoRutas" :permisos="permisosOtorgados" :mini-state="esMini" />
  </q-drawer>
</template>

<script setup>
import { ref, watch, computed, inject, onMounted, onUnmounted } from 'vue'
import { useSesionStore } from 'src/stores/sesion'
import { usePwaStore } from 'src/stores/pwa'
import { useRoute } from 'vue-router'
import { useQuasar } from 'quasar'
import { modulosItems as moduloItems } from 'src/core/modulos'
import ListaApp from 'src/components/Disenio/ListasLayouts/ListaApp.vue'
import { useNavigation } from 'src/composables/useNavigation.js'
import { GBlastLogo, GBlastIcon } from 'src/components/Logos'

const traducir = inject('traducir', (key) => key)

const $q = useQuasar()
const store = useSesionStore()
const pwaStore = usePwaStore()
const route = useRoute()
const { obtenerMenuPorModulo } = useNavigation()

// --- COMUNICACIÓN CON EL PADRE (v-model) ---
const props = defineProps({
  modelValue: { type: Boolean, default: true },
})
const emit = defineEmits(['update:modelValue'])

// Sincronización del estado Abierto/Cerrado
const drawerModel = computed({
  get: () => props.modelValue,
  set: (val) => emit('update:modelValue', val),
})

// --- LÓGICA DE TAMAÑO DE TEXTO DINÁMICO ---
// Sincronización inversa: CSS define el ancho (fit-content), pero debemos
const realDrawerWidth = ref(260)
let resizeObserver = null

onMounted(() => {
  // Usamos setTimeout para asegurar que el DOM de Quasar se ha renderizado completamente
  setTimeout(() => {
    const drawerEl = document.querySelector('.dynamic-width-drawer')

    if (drawerEl) {
      const updateWidth = () => {
        if (drawerEl.offsetWidth > 50) {
          realDrawerWidth.value = drawerEl.offsetWidth
        }
      }

      resizeObserver = new ResizeObserver((entries) => {
        for (const entry of entries) {
          if (entry.contentBoxSize) {
            updateWidth()
          }
        }
      })
      resizeObserver.observe(drawerEl)

      // Force update on route change to handle transitions
      watch(
        () => route.path,
        () => {
          setTimeout(updateWidth, 400) // 300ms transition + 100ms buffer
        }
      )
    }
  }, 200)
})

onUnmounted(() => {
  if (resizeObserver) {
    resizeObserver.disconnect()
  }
})

// --- LÓGICA DE ESTADO MINI ---
const STORAGE_KEY = 'drawer-preference'
const savedState = localStorage.getItem(STORAGE_KEY)
// Preferencia del usuario (true = quiere mini, false = quiere expandido)
const miniState = ref(savedState === 'true')

// PROPIEDAD COMPUTADA CRÍTICA:
// Controla visualmente si el drawer se renderiza como Mini o Normal.
const esMini = computed(() => {
  // REGLA 1: Si es Móvil (pantalla < 1024px), FORZAR MODO EXPANDIDO (false).
  if ($q.screen.lt.md) {
    return false
  }
  // REGLA 2: Si es PC, respetar la preferencia del usuario.
  return miniState.value
})

const toggleMiniState = () => {
  miniState.value = !miniState.value
  localStorage.setItem(STORAGE_KEY, miniState.value)
}

// --- DATOS DE RUTAS ---
const modulosOtorgados = computed(() => {
  const modulos = store.modulosOtorgados
  return Array.isArray(modulos) ? modulos : []
})
const modulosOtorgadosItems = ref(moduloItems)
const permisosOtorgados = computed(() => {
  const permisos = store.permisosOtorgados
  return Array.isArray(permisos) ? permisos : []
})

const listadoRutas = computed(() => {
  const moduloEncontrado = modulosOtorgadosItems.value.find(
    (item) =>
      modulosOtorgados.value.find((x) => x.id === item.idModulo) &&
      item.ruta.split('/')[1] === route.path.split('/')[1],
  )

  if (!moduloEncontrado) return []

  // USAR EL COMPOSABLE NUEVO
  const menuDesdeRouter = obtenerMenuPorModulo(moduloEncontrado.idModulo)

  if (menuDesdeRouter && menuDesdeRouter.length > 0) {
    return menuDesdeRouter
  }

  return []
})
</script>

<style lang="scss" scoped>
/* Lógica para ancho dinámico */
:deep(.q-drawer) {
  /* Por defecto (expandido) intenta ajustarse al contenido */
  width: fit-content !important;
  min-width: 260px;
  max-width: 80vw;
  /* Seguridad para no ocupar toda la pantalla */

  /* Transiciones suaves */
  transition: width 0.3s ease-in-out;
}

:deep(.q-drawer--mini) {
  /* En modo mini, forzar ancho fijo estándar */
  width: 80px !important;
  min-width: 80px !important;
}

/* El contenido interno también debe permitir expansión */
:deep(.q-drawer__content) {
  width: fit-content !important;
  min-width: 100%;
}
</style>
