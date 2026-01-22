<template>
  <q-btn
    :flat="flat"
    :no-caps="noCaps"
    :unelevated="unelevated"
    :outline="outline"
    :color="isHover ? colorHover : colorBase"
    @mouseover="isHover = true"
    @mouseleave="isHover = false"
    v-bind="$attrs"
  >
    <q-icon v-if="icon.includes('fa-')" :name="icon"></q-icon>
    <i v-else :class="`${icon} q-mr-sm`"></i>
    <span>
      {{ label }}
    </span>
  </q-btn>
</template>

<script setup>
import { ref, toRefs } from 'vue'

const props = defineProps({
  label: {
    type: String,
    default: 'ActualizarTablero',
  },
  icon: {
    type: String,
    default: 'pi pi-refresh',
  },
  noCaps: {
    type: Boolean,
    default: false,
  },
  colorBase: {
    type: String,
    default: 'black',
  }, // gris por defecto
  colorHover: {
    type: String,
    default: 'primary',
  }, // rojo por defecto
  textColorBase: {
    type: String,
    default: 'black',
  },
  textColorHover: {
    type: String,
    default: 'primary',
  },
  outline: { type: Boolean, default: true },
  flat: { type: Boolean, default: false },
  unelevated: { type: Boolean, default: false },
})

const { label, icon, colorHover, colorBase, outline, flat, unelevated, noCaps } = toRefs(props)

const isHover = ref(false)
</script>

<style scoped></style>
