<template>
  <div class="bg-fondo3 row items-center full-width q-mt-xs q-pa-lg" style="border-radius: 16px">
    <div class="row full-width text-textprimary text-size-32">
      <div class="col-12 col-md-6">
        {{ modulo }}
      </div>
      <div class="col-12 col-md-6">
        <div
          class="row text-weight-medium text-textsecondary justify-start justify-md-end q-mt-sm q-mt-md-none text-size-20">
          {{ titleCase(fechasActuales) }}
        </div>
      </div>
    </div>

    <div class="row full-width items-center q-mt-md">
      <div class="col-12 col-md-6 q-mb-md q-mb-md-none">
        <div class="row items-center wrap q-gutter-y-sm">
          <G3SelectorFechas v-if="requiereBotonFechas" class="q-mr-md" @update:fechas="actualizarFechas" />

          <G3HoraCorte v-if="requiereBotonHoraCorte" @update:hora="actualizarHora" class="q-pr-md" />

          <div v-if="requiereBotonCrear" class="col-12 col-md-auto">
            <q-btn outline no-caps class="text-weight-medium text-textprimary full-width"
              style="width: 100%; max-width: 248px" @click="metodoCrear">
              <i v-if="botonCrearIcono.startsWith('pi')" class="q-mr-md text-size-18" :class="botonCrearIcono" />
              <q-icon v-else :name="botonCrearIcono" class="text-size-18" />
              <span class="text-weight-regular text-size-18">{{ botonCrearTexto }}</span>
            </q-btn>
          </div>
        </div>
      </div>
      <div class="col-12 col-md-6">
        <div class="row justify-start justify-md-end q-gutter-md">
          <q-btn outline no-caps class="text-textprimary" @click="limpiarFiltros" v-if="requiereBotonFiltro">
            <i class="pi pi-filter-slash q-mr-md text-size-18" />
            <span class="text-weight-regular text-size-18">{{
              traducir('LimpiarFiltros')
            }}</span>
          </q-btn>

          <q-btn unelevated color="primary" no-caps class="text-primary-contrast" @click="metodoRefrescar()"
            v-if="requiereBotonActualizar">
            <i class="pi pi-refresh q-mr-md text-size-18" />
            <span class="text-weight-regular text-size-18">
              {{ traducir('ActualizarDatosTablero') }}
            </span>
          </q-btn>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, toRefs, inject } from 'vue'
import G3SelectorFechas from './G3SelectorFechas.vue'
import G3HoraCorte from './G3HoraCorte.vue'
import { usePreferenciaStore } from 'src/stores/preferencias'

const traducir = inject('traducir', (key) => key)

const props = defineProps({
  modulo: {
    type: String,
    default: 'No se encontró el módulo',
  },
  requiereBotonCrear: {
    type: Boolean,
    default: false,
  },
  botonCrearIcono: {
    type: String,
    default: '',
  },
  botonCrearTexto: {
    type: String,
    default: 'Sin texto definido',
  },
  metodoCrear: {
    type: Function,
    default: () => { },
  },
  requiereBotonActualizar: {
    type: Boolean,
    default: true,
  },
  requiereBotonFiltro: {
    type: Boolean,
    default: true,
  },
  requiereBotonFechas: {
    type: Boolean,
    default: false,
  },
  requiereBotonHoraCorte: {
    type: Boolean,
    default: false,
  },
  metodoRefrescar: {
    type: Function,
    default: () => { },
  },
})

const { modulo, metodoCrear, metodoRefrescar } = toRefs(props)

const emit = defineEmits(['limpiar-filtros', 'update:fechas', 'update:hora'])

const fechasActuales = ref('')

function limpiarFiltros() {
  emit('limpiar-filtros')
}

function actualizarFechas(fechas) {
  fechasActuales.value = fechas
  const fromFormatted = formatearFechaHumanizada(fechas.from)
  const toFormatted = fechas.to ? formatearFechaHumanizada(fechas.to) : ''

  if (fromFormatted == toFormatted) {
    fechasActuales.value = fromFormatted
  } else {
    fechasActuales.value = toFormatted ? `${fromFormatted} - ${toFormatted}` : fromFormatted
  }
  emit('update:fechas', fechas)
}

function actualizarHora(hora) {
  emit('update:hora', hora)
}

const formatearFechaHumanizada = (fecha) => {
  const opciones = { year: 'numeric', month: 'long', day: 'numeric' }
  return new Date(fecha).toLocaleDateString(usePreferenciaStore().lenguaje, opciones)
}

function titleCase(texto) {
  if (!texto) return ''

  const excepciones = [
    'de',
    'del',
    'la',
    'las',
    'el',
    'los',
    'y',
    'o',
    'u',
    'en',
    'a',
    'por',
    'para',
    'con',
    'sin',
    'sobre',
    'al',
  ]

  return texto
    .toLowerCase()
    .split(' ')
    .map((palabra, index) => {
      if (index !== 0 && excepciones.includes(palabra)) {
        return palabra
      }

      return palabra.charAt(0).toUpperCase() + palabra.slice(1)
    })
    .join(' ')
}
</script>

<style scoped></style>
