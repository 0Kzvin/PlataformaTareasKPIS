<template>
  <q-card flat style="height: 80px; border-radius: 8px">
    <div class="row items-center full-height">
      <div class="col-3">
        <div class="row justify-center items-center">
          <i :class="`${iconoEstadistica}`" :style="{ fontSize: `${tamanioIcono}px` }" />
        </div>
      </div>

      <div class="col-9">
        <div class="column justify-center">
          <span class="text-weight-medium" :style="{ fontSize: `${tamanioTitulo}px` }">{{
            tituloEstadistica
          }}</span>
          <div class="row text-negative" v-if="esNegativa">
            <span class="text-weight-bold" :style="{ fontSize: `${tamanioValor}px` }">
              <i class="pi pi-arrow-down" />
              {{ esDecimal ? valorAnimado.toFixed(2) : (Math.round(valorAnimado) ?? 0) }}
              {{ unidadEstadistica }}
            </span>
          </div>
          <div class="row text-positive" v-else-if="esPositiva">
            <span class="text-weight-bold" :style="{ fontSize: `${tamanioValor}px` }">
              <i class="pi pi-arrow-up" />
              {{ esDecimal ? valorAnimado.toFixed(2) : (Math.round(valorAnimado) ?? 0) }}
              {{ unidadEstadistica }}
            </span>
          </div>
          <div v-else>
            <span class="text-weight-bold" :style="{ fontSize: `${tamanioValor}px` }">
              {{ esDecimal ? valorAnimado.toFixed(2) : (Math.round(valorAnimado) ?? 0) }}
              {{ unidadEstadistica }}
            </span>
          </div>
        </div>
      </div>
    </div>
  </q-card>
</template>

<script setup>
import { toRefs } from 'vue'
import { TransitionPresets, useTransition } from '@vueuse/core'

const props = defineProps({
  tituloEstadistica: {
    type: String,
    default: 'Sin título',
  },
  iconoEstadistica: {
    type: String,
    default: 'pi pi-user',
  },
  valorEstadistica: {
    type: Number,
    default: 0,
  },
  tamanioTitulo: {
    type: Number,
    default: 18,
  },
  tamanioValor: {
    type: Number,
    default: 24,
  },
  tamanioIcono: {
    type: Number,
    default: 28,
  },
  unidadEstadistica: {
    type: String,
    default: '',
  },
  esNegativa: {
    type: Boolean,
    default: false,
  },
  esPositiva: {
    type: Boolean,
    default: false,
  },
  esDecimal: {
    type: Boolean,
    default: false,
  },
})

const {
  iconoEstadistica,
  tituloEstadistica,
  valorEstadistica,
  unidadEstadistica,
  esNegativa,
  esPositiva,
} = toRefs(props)

const valorAnimado = useTransition(valorEstadistica, {
  duration: 800,
  transition: TransitionPresets.easeOutExpo,
})
</script>

<style lang="scss" scoped></style>
