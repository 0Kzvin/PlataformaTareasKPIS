<template>
  <q-btn class="text-textprimary" outline no-caps style="width: 232px; min-height: 60px" @click="menuAbierto = true">
    <q-tooltip class="text-size-16">
      {{ traducir('TooltipSeleccionarRango') }}
    </q-tooltip>

    <div v-if="etiquetaRangoFechas" class="full-width column justify-start items-start q-pt-xs">
      <span class="text-bold text-left"> {{ traducir('EtiquetaFecha') }} </span>

      <div class="row items-center no-wrap justify-between full-width">
        <span class="text-size-14 cursor-pointer ellipsis">
          {{ etiquetaRangoFechas }}
        </span>

        <i class="pi pi-calendar q-ml-sm" />
      </div>
    </div>

    <q-menu v-model="menuAbierto" @before-show="abriendoMenu = true" @show="abriendoMenu = false"
      @before-hide="abriendoMenu = false" :persistent="mouseEnBoton" transition-show="jump-down"
      transition-hide="jump-up" no-parent-event class="bg-fondo2">
      <q-date color="primary" class="bg-fondo2 text-textprimary" v-model="rangoFechas" :range="rangoActivado"
        :options="opcionesFechas" today-btn>
        <div class="row items-center">
          <div class="col-6">
            <div class="row justify-center">
              <q-btn v-close-popup class="q-pr-sm" flat color="textsecondary">
                <div class="row items-center">
                  <i class="pi pi-times q-mr-sm" />
                  {{ traducir('Cancelar') }}
                </div>
              </q-btn>
            </div>
          </div>
          <div class="col-6">
            <div class="row justify-center">
              <q-btn v-if="rangoActivado" v-close-popup no-caps flat color="primary"
                @click="rangoFechas = obtenerRangoMesActual()">
                <div class="row items-center">
                  <i class="pi pi-calendar q-pr-sm" />
                  <span> {{ traducir('EsteMes') }} </span>
                  <q-tooltip>
                    <span class="text-size-12 text-cammelcase">{{ camelCase(mesAnioActual) }}</span>
                  </q-tooltip>
                </div>
              </q-btn>
            </div>
          </div>
        </div>
      </q-date>
    </q-menu>
  </q-btn>
</template>

<script setup>
import { ref, watch, inject, toRefs, onBeforeMount, computed } from 'vue'
import { usePreferenciaStore } from 'src/stores/preferencias'
const traducir = inject('traducir')

function camelCase(texto) {
  if (!texto) return ''
  return texto.charAt(0).toUpperCase() + texto.slice(1)
}
/**
 * Props del componente.
 * @property {string} fecha1 - Fecha inicial del rango.
 * @property {string} fecha2 - Fecha final del rango.
 * @property {boolean} rango - Indica si el selector de fechas debe permitir un rango.
 * @property {boolean} formatoHumanizado - Indica si el formato de las fechas debe ser humanizado.
 */
const props = defineProps({
  fecha1: {
    type: String,
    default: null,
  },
  fecha2: {
    type: String,
    default: null,
  },
  rango: {
    type: Boolean,
    default: true,
  },
  opcionesFechas: {
    type: Function,
    default: () => {
      return true
    },
  },
  formatoHumanizado: {
    type: Boolean,
    default: false,
  },
})

/**
 * Emits del componente.
 * @event cambioFechasSelector - Se emite cuando se selecciona un nuevo rango de fechas.
 */
const emit = defineEmits(['update:fechas'])

const { opcionesFechas } = toRefs(props)

// Referencias reactivas.
const rangoActivado = ref(props.rango)
const rangoFechas = ref({
  from: '',
  to: '',
})
const etiquetaRangoFechas = ref('')
const menuAbierto = ref(false)
const mouseEnBoton = ref(false)
const abriendoMenu = ref(false)

const preferenciaStore = usePreferenciaStore()

const mesAnioActual = computed(() => {
  return new Date().toLocaleString(preferenciaStore.lenguaje, {
    month: 'long',
    year: 'numeric',
  })
})

onBeforeMount(() => {
  if (props.rango && !props.fecha1 && !props.fecha2) {
    rangoFechas.value = obtenerRangoMesActual()
  }
})

/**
 * Inicializa la etiqueta del rango de fechas si se proporcionan fechas iniciales.
 */
if (props.fecha1 != null) {
  let etiquetaTemporal = props.fecha1
  if (props.fecha2 != null) {
    etiquetaTemporal += ' ~ ' + props.fecha2
  }
  etiquetaRangoFechas.value = etiquetaTemporal
}

/**
 * Formatea una fecha en un formato humanizado.
 * @param {string} fecha - Fecha en formato YYYY/MM/DD.
 * @returns {string} Fecha formateada.
 */
const formatearFechaHumanizada = (fecha) => {
  const opciones = { year: 'numeric', month: 'long', day: 'numeric' }
  return new Date(fecha).toLocaleDateString(usePreferenciaStore().lenguaje, opciones)
}

/**
 * Observa los cambios en el rango de fechas y actualiza la etiqueta y emite el evento correspondiente.
 * @param {Object|string} nuevoRango - Nuevo rango de fechas seleccionado.
 */
watch(rangoFechas, (nuevoRango) => {
  if (!nuevoRango) return

  if (typeof nuevoRango === 'object') {
    const { from, to } = nuevoRango

    // Solo actualizar si al menos una fecha tiene valor.
    if (from || to) {
      if (props.formatoHumanizado) {
        const fromFormatted = formatearFechaHumanizada(from)
        const toFormatted = to ? formatearFechaHumanizada(to) : ''
        etiquetaRangoFechas.value = toFormatted
          ? `${fromFormatted} - ${toFormatted}`
          : fromFormatted
      } else {
        etiquetaRangoFechas.value = to ? `${from}  ~  ${to}` : from
      }

      emit('update:fechas', nuevoRango)
    } else {
      etiquetaRangoFechas.value = ''
      emit('update:fechas', nuevoRango)
    }
  } else if (typeof nuevoRango === 'string') {
    if (props.formatoHumanizado) {
      etiquetaRangoFechas.value = formatearFechaHumanizada(nuevoRango)
    } else {
      etiquetaRangoFechas.value = nuevoRango
    }

    emit('update:fechas', { from: nuevoRango, to: nuevoRango })
  }
})

const obtenerRangoMesActual = () => {
  const hoy = new Date()

  const primerDia = new Date(hoy.getFullYear(), hoy.getMonth(), 1)
  const ultimoDia = new Date(hoy.getFullYear(), hoy.getMonth() + 1, 0)

  const formatear = (fecha) => {
    const y = fecha.getFullYear()
    const m = String(fecha.getMonth() + 1).padStart(2, '0')
    const d = String(fecha.getDate()).padStart(2, '0')
    return `${y}/${m}/${d}`
  }

  return {
    from: formatear(primerDia),
    to: formatear(ultimoDia),
  }
}
</script>
