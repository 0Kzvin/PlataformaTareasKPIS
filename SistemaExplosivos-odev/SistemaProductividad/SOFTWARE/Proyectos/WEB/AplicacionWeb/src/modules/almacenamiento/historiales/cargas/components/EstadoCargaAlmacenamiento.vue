<template>
  <div class="row bg-fondo3 q-pa-md" style="border-radius: 12px">
    <div class="col-12">
      <div class="row">
        <div class="col-6">
          <span class="text-size-18 text-weight-medium">{{
            esFinal ? traducir('EstadoFinal') : traducir('EstadoInicial')
          }}</span>
        </div>
        <div class="col-6">
          <div class="row justify-end">
            <span>{{ traducir('Activo') }}</span>
          </div>
        </div>
      </div>
      <div class="row q-pt-md">
        <div class="col-5">
          <div style="max-width: 104px">
            <TanqueNivel
              ref="tanqueRef"
              :tanque="TanqueExplosivos"
              :nivel="nivelInicial"
              :opacidad="0.5"
              :offset-inicio-pct="10"
              :color-nivel="valorColor"
            />
          </div>
        </div>
        <div class="col-7">
          <div class="row">
            <div class="col-6">
              <span class="text-bold">{{ traducir('Nivel') + ':' }} </span>
            </div>
            <div class="col-6">
              <span>{{ valorNivel }} mm</span>
            </div>
          </div>
          <div class="row q-pt-sm">
            <div class="col-6">
              <span class="text-bold">{{ traducir('Peso') + ':' }} </span>
            </div>
            <div class="col-6">
              <span>{{ valorVolumen }} Kg</span>
            </div>
          </div>
          <div class="row q-pt-sm">
            <div class="col-6">
              <span class="text-bold">{{ traducir('Porcentaje') + ':' }} </span>
            </div>
            <div class="col-6">
              <span>{{ valorPorcentaje }} %</span>
            </div>
          </div>
          <q-separator class="q-mt-md"></q-separator>
          <div class="row q-pt-md">
            <div class="col-12">
              <div class="row text-center justify-center">
                <span class="text-bold">{{ traducir('Fecha') }}</span>
              </div>
              <div class="row text-center justify-center">
                <span>{{ traducir(valorFecha) }}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { nextTick, onMounted, ref, toRefs, inject } from 'vue'
import TanqueExplosivos from 'src/assets/tanque.svg'
import TanqueNivel from 'src/modules/almacenamiento/analisis/monitoreo/components/depositos/components/TanqueNivel.vue'

const traducir = inject('traducir', (key) => key)

const tanqueRef = ref(null)

const props = defineProps({
  esFinal: {
    type: Boolean,
    default: false,
  },
  valorNivel: {
    type: Number,
    default: 0,
  },
  valorVolumen: {
    type: Number,
    default: 0,
  },
  valorPorcentaje: {
    type: Number,
    default: 0,
  },
  valorFecha: {
    type: String,
    default: 'SinFecha',
  },
  valorColor: {
    type: String,
    default: '#66BB6A',
  },
})

const { esFinal, valorNivel, valorVolumen, valorPorcentaje, valorFecha, valorColor } = toRefs(props)

const nivelInicial = ref(0)

onMounted(async () => {
  setTimeout(() => {
    nivelInicial.value = valorPorcentaje.value
  }, 450)

  await nextTick()

  setTimeout(() => {
    if (tanqueRef.value && tanqueRef.value.forzarMedicion) {
      tanqueRef.value.forzarMedicion()
    }
  }, 400) // 200ms es un buen valor para capturar el final de la animación del diálogo.
})
</script>

<style lang="scss" scoped></style>
