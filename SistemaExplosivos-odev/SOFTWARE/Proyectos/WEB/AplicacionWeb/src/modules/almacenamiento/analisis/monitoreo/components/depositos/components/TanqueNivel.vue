<template>
  <div ref="containerRef" class="tanque-container">
    <div v-if="!ready" class="tanque-loading absolute-full column flex-center">
      <q-spinner color="secondary" size="3em" />
    </div>

    <div :class="{ invisible: !ready }">
      <q-img ref="imgRef" :src="tanque" class="tanque-img" fit="contain" @load.once="medir" loading="eager" />

      <div v-if="ready" class="nivel" :style="nivelStyle" />

      <div v-if="ready" class="nivel-texto text-size-18" :style="textoNivelStyle">{{ Math.round(nivel) }}%</div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onBeforeUnmount } from 'vue'

const props = defineProps({
  tanque: { type: String, required: true },
  nivel: { type: Number, default: 0 },

  colorNivel: { type: String, default: '#4CAF50' },
  opacidad: { type: Number, default: 0.45 },

  cilindroTopPct: { type: Number, default: 10 },
  cilindroBottomPct: { type: Number, default: 78 },
  cilindroWidthPct: { type: Number, default: 64 },
  cilindroLeftPct: { type: Number, default: 18 },

  offsetInicioPct: {
    type: Number,
    default: 2,
  },
})

const imgRef = ref(null)
const containerRef = ref(null)
const imgHeight = ref(0)
const imgWidth = ref(0)
const ready = ref(false)

let resizeObserver = null

/**
 * Mide las dimensiones del contenedor de la imagen y activa la visualización.
 */
const medir = () => {
  const qImgEl = imgRef.value?.$el
  if (!qImgEl) return

  // Obtener las dimensiones del contenedor de q-img
  const rect = qImgEl.getBoundingClientRect()

  // Validar dimensiones, lo crucial para el problema en el QDialog
  if (!rect.height || !rect.width || rect.height === 0 || rect.width === 0) {
    return
  }

  imgHeight.value = rect.height
  imgWidth.value = rect.width
  ready.value = true
}

const forzarMedicion = () => {
  medir()
}

defineExpose({
  forzarMedicion,
})

onMounted(() => {
  resizeObserver = new ResizeObserver(() => {
    medir()
  })

  if (containerRef.value) {
    resizeObserver.observe(containerRef.value)
  }
})

onBeforeUnmount(() => {
  resizeObserver?.disconnect()
})

const textoNivelStyle = computed(() => {
  if (!ready.value) return {}

  const cilindroTop = (props.cilindroTopPct / 100) * imgHeight.value
  const cilindroBottom = (props.cilindroBottomPct / 100) * imgHeight.value
  const cilindroAltura = cilindroBottom - cilindroTop

  return {
    top: `${cilindroTop + cilindroAltura / 2}px`,
    left: '50%',
    transform: 'translate(-50%, -50%)',
  }
})

const nivelStyle = computed(() => {
  if (!ready.value) return {}

  const cilindroTop = (props.cilindroTopPct / 100) * imgHeight.value
  const cilindroBottom = (props.cilindroBottomPct / 100) * imgHeight.value
  const cilindroAltura = cilindroBottom - cilindroTop
  const offsetInicioPx = (props.offsetInicioPct / 100) * cilindroAltura
  const nivelNormalizado = Math.min(Math.max(props.nivel, 0), 100) / 100
  const alturaUtil = cilindroAltura - offsetInicioPx
  const nivelAltura = nivelNormalizado * alturaUtil

  return {
    height: `${nivelAltura}px`,
    bottom: `${imgHeight.value - cilindroBottom + offsetInicioPx}px`,
    left: `${(props.cilindroLeftPct / 100) * imgWidth.value - 16}px`,
    width: `${(props.cilindroWidthPct / 100) * imgWidth.value + 32}px`,
    backgroundColor: props.colorNivel,
    opacity: props.opacidad,
  }
})
</script>

<style scoped>
.tanque-container {
  position: relative;
  width: 100%;
}

.tanque-img {
  position: relative;
  z-index: 1;
}

.nivel {
  position: absolute;
  z-index: 10;
  border-radius: 4px;
  transition: height 0.6s ease;
  pointer-events: none;
}

.nivel-texto {
  position: absolute;
  z-index: 20;
  /* encima del nivel y del tanque */
  font-weight: 600;

  color: var(--q-text-primary);
  pointer-events: none;

  /* mejora legibilidad */
  background: rgba(255, 255, 255, 0.75);
  padding: 2px 8px;
  border-radius: 8px;
}
</style>
