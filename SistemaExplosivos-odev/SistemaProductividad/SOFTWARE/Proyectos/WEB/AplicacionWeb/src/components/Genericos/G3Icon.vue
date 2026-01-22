<template>
  <div
    class="inline-block"
    :style="{
      width: size + (isNaN(size) ? '' : 'px'),
      height: size + (isNaN(size) ? '' : 'px'),
      color: color,
    }"
    v-html="processedSvg"
  />
</template>

<script setup>
import { ref, watchEffect } from 'vue'

const props = defineProps({
  icon: { type: String, required: true }, // puede ser un import o un string de svg
  size: { type: [String, Number], default: 24 },
  color: { type: String, default: 'currentColor' },
})

const processedSvg = ref('')

// procesa el contenido del svg y reemplaza fill/stroke
watchEffect(() => {
  let svg = props.icon

  // si el SVG viene de un archivo importado
  if (svg.endsWith?.('.svg')) {
    // import dinámico
    fetch(svg)
      .then((r) => r.text())
      .then((text) => {
        processedSvg.value = text.replace(/fill="[^"]*"/g, `fill="currentColor"`)
      })
  } else {
    // si ya es un string
    processedSvg.value = svg.replace(/fill="[^"]*"/g, `fill="currentColor"`)
  }
})
</script>

<style scoped>
/* Para que herede el color */
:deep(svg) {
  width: 100%;
  height: 100%;
  fill: currentColor;
}
</style>
