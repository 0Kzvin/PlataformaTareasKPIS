<template>
  <q-card class="bg-fondo3 text-textprimary q-pa-sm" style="max-width: 880px; max-height: 640px; border-radius: 16px">
    <q-card-section class="row items-center">
      <div class="text-weight-medium col text-size-32">
        {{ traducir('InformacionDelSilo') }}
      </div>

      <q-btn :outline="!hover" :flat="hover" no-caps :color="hover ? 'primary' : 'black'" @mouseover="hover = true"
        @mouseleave="hover = false" @click="cancelarForm" class="btn-negativo">
        <i class="pi pi-times q-mr-sm" />
        {{ traducir('Cerrar') }}
      </q-btn>
    </q-card-section>

    <q-separator class="q-mx-md"></q-separator>

    <q-card-section class="row text-size-18 q-col-gutter-md">
      <div class="col-4">
        <strong>{{ traducir('Nombre') }}:</strong> {{ modelo.deposito }}
      </div>
      <div class="col-4">
        <strong>{{ traducir('Producto') }}:</strong> {{ modelo.producto }}
      </div>
      <div class="col-4">
        <strong>{{ traducir('Ubicacion') }}:</strong> {{ modelo.ubicacion }}
      </div>
    </q-card-section>

    <q-card-section class="q-pt-none">
      <q-scroll-area class="bg-fondo" style="height: 448px">
        <div class="row q-col-gutter-md q-pa-md">
          <div class="col-6">
            <EstadoCargaAlmacenamiento :valor-color="modelo.colorProducto" :valor-nivel="modelo.nivelInicial"
              :valor-volumen="modelo.volumenInicial" :valor-fecha="modelo.fechaHoraInicial"
              :valor-porcentaje="modelo.porcentajeInicial" />
          </div>
          <div class="col-6">
            <EstadoCargaAlmacenamiento :es-final="true" :valor-color="modelo.colorProducto"
              :valor-nivel="modelo.nivelFinal" :valor-volumen="modelo.volumenFinal" :valor-fecha="modelo.fechaHoraFinal"
              :valor-porcentaje="modelo.porcentajeFinal" />
          </div>
        </div>

        <div class="q-mx-md q-my-sm q-pa-md bg-fondo3" style="border-radius: 12px">
          <div class="text-weight-medium text-size-18 q-mb-sm">
            {{ traducir('ResumenDeCambios') }}
          </div>
          <div class="row q-col-gutter-md">
            <div class="col-4">
              <DetalleEstadistica class="bg-fondo" :tamanio-icono="24" :tamanio-titulo="14" :tamanio-valor="20"
                icono-estadistica="pi pi-database" :titulo-estadistica="traducir('DiferenciaDeNivel')"
                :valor-estadistica="diffNivel" unidad-estadistica="mm" es-decimal />
            </div>

            <div class="col-4">
              <DetalleEstadistica class="bg-fondo" :tamanio-icono="24" :tamanio-titulo="14" :tamanio-valor="20"
                icono-estadistica="pi pi-sliders-h" :titulo-estadistica="traducir('DiferenciaDePeso')"
                :valor-estadistica="diffPeso" unidad-estadistica="Kg" es-decimal />
            </div>

            <div class="col-4">
              <DetalleEstadistica class="bg-fondo" :tamanio-icono="24" :tamanio-titulo="14" :tamanio-valor="20"
                icono-estadistica="pi pi-chart-line" :titulo-estadistica="traducir('DiferenciaDePorcentaje')"
                :valor-estadistica="diffPorcentaje" :es-positiva="diffPorcentaje > 0" unidad-estadistica="%"
                es-decimal />
            </div>
          </div>
        </div>
      </q-scroll-area>
    </q-card-section>
  </q-card>
</template>

<script setup>
import { onMounted, ref, inject } from 'vue'
import DetalleEstadistica from '../../supersacos/components/DetalleEstadistica.vue'
import EstadoCargaAlmacenamiento from './EstadoCargaAlmacenamiento.vue'

const traducir = inject('traducir', (key) => key)

const emits = defineEmits(['onDialogCancel', 'onDialogHide', 'onDialogOK'])

const props = defineProps({
  propsCustomForm: {
    type: Object,
    default: () => {
      return {
        modelo: {},
      }
    },
  },
})

const {
  propsCustomForm: { modelo },
} = props

const cancelarForm = () => {
  emits('onDialogCancel')
}

const diffNivel = ref(0)
const diffPeso = ref(0)
const diffPorcentaje = ref(0)

onMounted(async () => {
  setTimeout(() => {
    diffNivel.value = Number((modelo.nivelFinal - modelo.nivelInicial).toFixed(3))

    diffPeso.value = Number(modelo.volumenCargado.toFixed(2))

    diffPorcentaje.value = Number((modelo.porcentajeFinal - modelo.porcentajeInicial).toFixed(2))
  }, 450)
})

const hover = ref(false)
</script>

<style scoped>
.btn-negativo {
  width: 144px;
  height: 40px;
  transition: all 0.6s ease;
}
</style>
