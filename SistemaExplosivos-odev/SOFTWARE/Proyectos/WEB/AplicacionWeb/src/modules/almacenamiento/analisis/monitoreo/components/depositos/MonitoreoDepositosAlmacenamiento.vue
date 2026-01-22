<template>
  <div class="row full-height">
    <!-- LOADING -->
    <div v-if="cargandoEstaciones" class="col-12 flex flex-center">
      <BuscandoDataApi />
    </div>

    <!-- CONTENIDO -->
    <div v-else class="col-12">
      <div class="row q-mb-md justify-end">
        <q-input dense outlined :label="traducir('BuscarEstacion')" style="width: 320px">
          <template #prepend>
            <i class="pi pi-search q-pr-sm" />
          </template>
        </q-input>
      </div>

      <div v-for="(estacion, index) in estaciones" :key="index" class="row">
        <div class="col-12">
          <EstacionDepositoAlmacenamiento :estacion="estacion" />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref, inject } from 'vue'
import { estacionesGerencia } from 'src/api/moduloGerencia'
import EstacionDepositoAlmacenamiento from './components/EstacionDepositoAlmacenamiento.vue'
import BuscandoDataApi from 'src/components/Globales/BuscandoDataApi.vue'

const traducir = inject('traducir', (key) => key)

const estaciones = ref([])
const opcionesEstaciones = ref([])
const cargandoEstaciones = ref(false)

onMounted(async () => {
  await refrescarEstaciones()
})

const refrescarEstaciones = async () => {
  cargandoEstaciones.value = true
  const resp = await estacionesGerencia.listar()
  cargandoEstaciones.value = false

  if (!resp.exito) {
    estaciones.value = []
    opcionesEstaciones.value = []
    return
  }

  estaciones.value = resp.payload.respuesta
}
</script>

<style lang="scss" scoped></style>
