<template>
  <q-checkbox
    v-bind="$attrs"
    :model-value="modelValue"
    @update:model-value="$emit('update:modelValue', $event)"
    class="g3-checkbox-safe"
    :class="{ 'g3-checkbox-safe--resaltado': debeResaltar }"
  >
    <!-- Pass through all slots -->
    <template v-for="(_, slot) in $slots" v-slot:[slot]="scope">
      <slot :name="slot" v-bind="scope || {}" />
    </template>
  </q-checkbox>
</template>

<script setup>
import { computed, watch, ref, onMounted, onUnmounted } from 'vue'
import { colors, useQuasar } from 'quasar'

const { getPaletteColor, luminosity } = colors
const $q = useQuasar()

const props = defineProps({
  modelValue: {
    required: true,
  },
  resaltar: {
    type: Boolean,
    default: false,
  },
})

defineEmits(['update:modelValue'])

const autoResaltar = ref(false)
let observer = null

// Function to calculate if contrast is needed based on Primary brightness vs Theme Mode
const calcularAutoResaltar = () => {
  // Get current Primary color
  const primary = getPaletteColor('primary')
  if (!primary) return false

  // Calculate luminosity (0 to 1)
  // Higher = Lighter, Lower = Darker
  // We use luminosity > 0.5 to match ThemeService logic
  const isPrimaryLight = luminosity(primary) > 0.5
  const isDarkMode = $q.dark.isActive

  // Logic:
  // If Dark Mode (Dark BG) && Primary is Dark -> Need Contrast (Resaltar)
  // If Light Mode (Light BG) && Primary is Light -> Need Contrast (Resaltar)
  if (isDarkMode && !isPrimaryLight) return true
  if (!isDarkMode && isPrimaryLight) return true

  return false
}

// Watch for Dark Mode changes
watch(
  () => $q.dark.isActive,
  () => {
    autoResaltar.value = calcularAutoResaltar()
  },
)

onMounted(() => {
  autoResaltar.value = calcularAutoResaltar()

  // Observe style changes on :root (html) and body where Quasar sets CSS vars
  // This allows reacting to dynamic color changes without reload
  const targetNode = document.documentElement
  const config = { attributes: true, attributeFilter: ['style', 'class'] }

  observer = new MutationObserver(() => {
    autoResaltar.value = calcularAutoResaltar()
  })

  observer.observe(targetNode, config)
})

onUnmounted(() => {
  if (observer) {
    observer.disconnect()
  }
})

const debeResaltar = computed(() => {
  return props.resaltar || autoResaltar.value
})
</script>

<style lang="scss">
.g3-checkbox-safe {
  // === DEFAULT BEHAVIOR (Primary Color) ===

  // 1. Unchecked State: Force Border to Primary
  .q-checkbox__inner .q-checkbox__bg {
    border-color: var(--q-primary) !important;
  }

  // 2. Checked State: Transparent BG, Border Primary, Tick Primary
  // Target the INNER element having the truthy/indet class
  .q-checkbox__inner--truthy,
  .q-checkbox__inner--indet {
    .q-checkbox__bg {
      background-color: transparent !important;
      border-color: var(--q-primary) !important;
    }
    .q-checkbox__svg {
      color: var(--q-primary) !important;
    }
  }

  // === RESALTADO BEHAVIOR (Contrast Color) ===
  &--resaltado {
    // 1. Unchecked State: Force Border to Contrast
    .q-checkbox__inner .q-checkbox__bg {
      border-color: var(--q-primary-contrast) !important;
    }

    // 2. Checked State: Transparent BG, Border Contrast, Tick Contrast
    .q-checkbox__inner--truthy,
    .q-checkbox__inner--indet {
      .q-checkbox__bg {
        background-color: transparent !important;
        border-color: var(--q-primary-contrast) !important;
      }
      .q-checkbox__svg {
        color: var(--q-primary-contrast) !important;
        path {
          stroke: var(--q-primary-contrast) !important;
          stroke-width: 3px;
        }
      }
    }
  }
}
</style>
