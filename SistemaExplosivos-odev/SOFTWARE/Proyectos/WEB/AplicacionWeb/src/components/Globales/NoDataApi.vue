<template>
  <div class="full-width row justify-center q-my-md bg-fondo3">
    <div class="col-12 self-center text-center">
      <span class="text-textsecondary text-weight-bold text-size-21">
        {{ traducir(titulo) }}
      </span>
    </div>
    <q-img src="../../assets/no_data_nobg.gif" style="max-width: 200px" loading="eager" />
    <div v-if="mensaje != ''" class="col-12 self-center text-center text-textsecondary">
      <span class="text-weight-medium text-size-16"> {{ traducir('Mensaje') }}: </span>
      <span class="text-weight-light text-size-16"> {{ traducir(mensaje) }}. </span>
    </div>
  </div>
</template>

<script setup>
import { toRefs, inject } from 'vue'
const traducir = inject('traducir')

const props = defineProps({
  titulo: {
    type: String,
    default: 'ConsulaNoEncontroInformacion',
  },
  mensaje: {
    type: String,
    default: '',
  },
})

const { mensaje, titulo } = toRefs(props)
</script>
