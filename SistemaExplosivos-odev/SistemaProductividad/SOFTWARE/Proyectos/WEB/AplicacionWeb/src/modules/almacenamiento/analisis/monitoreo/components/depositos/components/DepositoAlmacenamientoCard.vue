<template>
  <div class="row q-pa-md bg-fondo3 text-textprimary" style="border-radius: 8px; cursor: grab">
    <div class="col-12">
      <div class="row items-center">
        <div class="col-9">
          <div class="row text-size-21">
            <span class="text-bold"> {{ deposito.producto }} / </span>
            <span class="q-pl-xs">{{ deposito.nombre }}</span>
          </div>
        </div>
        <div class="col-3">
          <div class="row justify-end">
            <i class="pi pi-info-circle cursor-pointer q-mr-sm text-size-24">
              <q-tooltip class="text-size-14"> {{ traducir('VerMasInformacion') }}</q-tooltip>
            </i>
            <i class="pi pi-bolt cursor-pointer text-size-24" v-if="tieneConexion">
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
      <div class="row items-center q-mt-md">
        <div class="col-xs-12 col-sm-5 col-md-6 col-lg-4">
          <div class="row justify-center">
            <TanqueNivel style="max-width: 128px" :tanque="TanqueExplosivos" :nivel="estadoDeposito.porcentajeNivel"
              :opacidad="0.5" :offset-inicio-pct="10" :color-nivel="deposito.colorProducto" />
          </div>
        </div>
        <div class="col-xs-12 col-sm-7 col-md-6 col-lg-8">
          <div class="row q-py-sm justify-center" style="max-width: 400px">
            <div class="col-12">
              <div class="row justify-center items-center">
                <span class="text-weight-bold"> {{ traducir('Capacidad') }} </span>
              </div>
              <div class="row justify-center items-center">
                <span class="text-weight-bold">{{ traducir('MaximaOperativa') }}</span>
              </div>
              <div class="row text-textsecondary">
                <div class="col-6">
                  <div class="row justify-center">
                    <span>{{ stringNumerosConCommas(deposito.capacidadMaxima, 1) }} Kg </span>
                  </div>
                </div>
                <div class="col-6">
                  <div class="row justify-center">
                    <span>{{ stringNumerosConCommas(deposito.capacidadOperativa, 1) }} Kg</span>
                  </div>
                </div>
              </div>

              <div class="row justify-center q-mt-md">
                <span class="text-weight-bold"> {{ traducir('Volumen') }} </span>
              </div>
              <div class="row justify-center">
                <span class="text-textsecondary">
                  {{ stringNumerosConCommas(estadoDeposito.volumen, 1) }} Kg
                </span>
              </div>

              <div class="row justify-center q-mt-md">
                <span class="text-weight-bold"> {{ traducir('UltimaActualizacion') }}</span>
              </div>
              <div class="row text-center justify-center">
                <span class="text-textsecondary text-weight-light">
                  {{
                    date.formatDate(estadoDeposito.fechaHora, 'DD/MM/YYYY hh: mm: ss A') ??
                    traducir('NoHayDatosRecientes')
                  }}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, toRefs, inject } from 'vue'
import TanqueExplosivos from 'src/assets/tanque.svg'
import { date } from 'quasar'
import { stringNumerosConCommas } from 'src/utils/utils'
import TanqueNivel from './TanqueNivel.vue'

const traducir = inject('traducir', (key) => key)

const props = defineProps({
  deposito: {
    type: Object,
    default: null,
  },
  datoRemoto: {
    type: Object,
    default: null,
  },
  tieneConexion: {
    type: Boolean,
    default: false,
  },
})

const { deposito, datoRemoto, tieneConexion } = toRefs(props)

const estadoDeposito = computed(() => {
  if (tieneConexion.value && datoRemoto.value?.estatusDeposito) {
    return datoRemoto.value.estatusDeposito
  }

  return deposito.value?.estatusDTO ?? null
})
</script>

<style scoped></style>
