<template>
  <div v-if="vertical">
    <div class="row justify-center">
      <div class="col-12">
        <span class="text-weight-bold q-mr-xs">{{ etiqueta }}</span>
      </div>
    </div>
    <div class="row justify-center">
      <div class="col-12">
        <span>{{ valor }}</span>
      </div>
    </div>
  </div>
  <div class="row justify-center" v-else>
    <div class="col-12">
      <span class="text-weight-bold q-mr-xs">{{ etiqueta }}: </span>
      <span>{{ valor }}</span>
    </div>
  </div>
</template>

<script setup>
import { toRefs } from 'vue'

const props = defineProps({
  vertical: {
    type: Boolean,
    default: false,
  },
  etiqueta: {
    type: String,
    default: '',
  },
  valor: {
    type: String,
    default: '',
  },
})

const { etiqueta, valor, vertical } = toRefs(props)
</script>
