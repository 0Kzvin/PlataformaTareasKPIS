<template>
  <div class="row q-pa-md bg-fondo3 text-textprimary" style="border-radius: 8px; cursor: grab">
    <div class="col-12">
      <div class="row items-center q-pb-md">
        <div class="col-9">
          <div class="row text-size-21">
            <span class="text-bold">{{ equipo.producto }} / </span>
            <span class="q-pl-xs"> {{ equipo.numeroEconomico }}</span>
          </div>
        </div>
        <div class="col-3">
          <div class="row justify-end">
            <i class="pi pi-info-circle cursor-pointer q-mr-sm text-size-24">
              <q-tooltip class="text-size-14"> {{ traducir('VerMasInformacion') }}</q-tooltip>
            </i>
            <i class="pi pi-bolt cursor-pointer text-size-24" v-if="actualizarContinuamente">
              <q-tooltip class="text-size-14">
                {{ traducir('MonitoreoTiempoRealActivo') }}
              </q-tooltip>
            </i>
            <i class="pi pi-database cursor-pointer text-size-24" v-else>
              <q-tooltip class="text-size-14"> {{ traducir('UltimoGuardadoBD') }} </q-tooltip>
            </i>
          </div>
        </div>
      </div>
      <div class="row justify-center q-pb-md">
        <q-img :src="CamionMezclador" style="max-width: 320px" class="q-mx-lg" loading="eager" />
      </div>
      <div class="row q-pb-md">
        <div class="col-6">
          <div class="row justify-center">
            <span class="text-weight-bold">{{ traducir('Capacidad') }}</span>
          </div>
          <div class="row justify-center">
            <span class="text-textsecondary">
              {{ stringNumerosConCommas(equipo.capacidad, 1) }} Kg
            </span>
          </div>
        </div>
        <div class="col-6">
          <div class="row justify-center">
            <span class="text-weight-bold">{{ traducir('Volumen') }}</span>
          </div>
          <div class="row justify-center">
            <span class="text-textsecondary">
              {{ stringNumerosConCommas(equipo.cantidadActual, 1) }} Kg
            </span>
          </div>
        </div>
      </div>
      <div class="row">
        <div class="col-6">
          <div class="row justify-center">
            <span class="text-weight-bold">{{ traducir('UltimoCambio') }}</span>
          </div>
          <div class="row justify-center">
            <span class="text-textsecondary">16/05/2025 04:40 PM</span>
          </div>
        </div>
        <div class="col-6">
          <div class="row justify-center">
            <span class="text-weight-bold">{{ traducir('UltimaActualizacion') }}</span>
          </div>
          <div class="row justify-center">
            <span class="text-textsecondary">30/12/2025 04:55 PM</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, toRefs, inject } from 'vue'
import CamionMezclador from 'src/assets/camionMezclador.svg'
import { stringNumerosConCommas } from 'src/utils/utils'

const traducir = inject('traducir', (key) => key)

const props = defineProps({
  equipo: {
    type: Object,
    default: null,
  },
  datoRemoto: {
    type: Object,
    default: null,
  },
})

const { equipo } = toRefs(props)

const actualizarContinuamente = ref(true)
</script>

<style lang="scss" scoped></style>
