<template>
  <div class="row text-black items-stretch">
    <!-- ENTRADAS -->
    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-4 q-pr-md column">
      <q-card flat class="full-height column">
        <q-card-section>
          <div class="text-h6">{{ traducir('Entradas') }}</div>
          <div class="text-caption">{{ traducir('Nitrato') }}</div>
        </q-card-section>

        <div class="col column">
          <G3CustomTable class="col" :columnas-visibles="columnasVisibles" :columnas="columnas" :filas="datos.entradas"
            filas-por-pagina="7" :cargando="cargandoDatos" :filtros="filtroQuery">
            <template v-slot:col-end-fechaHora="{ props }">
              <span class="text-size-12">{{ props.row.fechaHora }}</span>
            </template>
          </G3CustomTable>
        </div>
      </q-card>
    </div>

    <!-- SALIDAS -->
    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-4 q-pr-md column">
      <q-card flat class="full-height column">
        <q-card-section>
          <div class="text-h6">{{ traducir('Salidas') }}</div>
          <div class="text-caption">{{ traducir('Nitrato') }}</div>
        </q-card-section>

        <div class="col column">
          <G3CustomTable class="col" :columnas-visibles="columnasVisibles" :columnas="columnas" :filas="datos.salidas"
            filas-por-pagina="7" :cargando="cargandoDatos" :filtros="filtroQuery">
            <template v-slot:col-end-fechaHora="{ props }">
              <span class="text-size-12">{{ props.row.fechaHora }}</span>
            </template>
          </G3CustomTable>
        </div>
      </q-card>
    </div>

    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-4 column">
      <q-card flat class="column q-mb-md full-width">
        <q-card-section>
          <div class="text-h6">{{ traducir('Total') }}</div>
          <div class="text-caption">{{ traducir('Nitrato') }}</div>
        </q-card-section>

        <div class="full-width q-pr-xl q-pb-md">
          <GraficaPisoAlmacenamiento v-if="!cargandoDatos"
            :datos-grafico="datos?.periodoActual?.movimientosPeriodo ?? []"
            :color="datos?.periodoAnterior?.colorProducto ?? ''" />
        </div>
      </q-card>

      <q-card flat class="column full-width col">
        <q-card-section>
          <div class="text-h6">{{ traducir('TotalSemanaAnterior') }}</div>
          <div class="text-caption">{{ traducir('Nitrato') }}</div>
        </q-card-section>

        <div class="full-width q-pr-xl q-pb-md">
          <GraficaPisoAlmacenamiento v-if="!cargandoDatos"
            :datos-grafico="datos?.periodoAnterior?.movimientosPeriodo ?? []"
            :color="datos?.periodoAnterior?.colorProducto ?? ''" />
        </div>
      </q-card>
    </div>
  </div>
</template>

<script setup>
import { onMounted, reactive, ref, watch } from 'vue'
import { inject } from 'vue'
import { date } from 'quasar'
import {
  obtenerFechaDeSemana,
  obtenerNumeroSemanasEnAnio,
  obtenerSemanaISO,
  refreshSource,
  source,
} from 'src/utils/utils'
import { estatusAlmacenamiento } from 'src/api/moduloAlmacenamiento'
import G3CustomTable from 'src/components/Globales/G3CustomTable.vue'
import GraficaPisoAlmacenamiento from './components/GraficaPisoAlmacenamiento.vue'

const traducir = inject('traducir', (key) => key)

const cargandoDatos = ref(false)
const datos = ref([])

const inicioSemanaDate = ref(null)
const terminoSemanaDate = ref(null)
const semanasDelAnio = ref(null)
const semanaActual = ref(null)
const mesActual = ref(null)
const anioActual = ref(null)
const semanaSolicitud = ref(null)
const mesSolicitud = ref(null)
const anioSolicitudMes = ref(null)
const anioSolicitudSemana = ref(null)

const obtenerFechas = async () => {
  const ahora = new Date()
  semanaActual.value = await obtenerSemanaISO()
  mesActual.value = ahora.getMonth()
  anioActual.value = ahora.getFullYear()
  semanaSolicitud.value = await obtenerSemanaISO()
  mesSolicitud.value = ahora.getMonth()
  anioSolicitudMes.value = ahora.getFullYear()
  anioSolicitudSemana.value = ahora.getFullYear()
  semanasDelAnio.value = obtenerNumeroSemanasEnAnio(ahora.getFullYear())

  inicioSemanaDate.value = new Date(ahora.setDate(ahora.getDate() - ahora.getDay() + 1))

  terminoSemanaDate.value = new Date(ahora.setDate(ahora.getDate() - ahora.getDay() + 7))
}

onMounted(async () => {
  await obtenerFechas()
})

const refrescarDatos = async () => {
  cargandoDatos.value = true
  const filtro = {
    fechaInicio: date.formatDate(inicioSemanaDate.value, 'YYYY-MM-DDTHH:mm:ss'),
    fechaFinal: date.formatDate(terminoSemanaDate.value, 'YYYY-MM-DDTHH:mm:ss'),
  }

  const resp = await estatusAlmacenamiento.listarSuperSacosEstatus(filtro)
  cargandoDatos.value = false

  if (!resp.exito) {
    datos.value = []
    return
  }

  datos.value = resp.payload.data
}

const cancelarSolicitud = () => {
  //Cada que cambie el rango de fechas, se cancelaran todas las peticiones anteriores existentes.
  if (source) {
    source.cancel()
  }

  //Despues se refrescara el token para las nuevas peticiones
  refreshSource()
}

watch(semanaSolicitud, async (valorNuevo) => {
  cancelarSolicitud()

  if (valorNuevo > semanasDelAnio.value + 1 || valorNuevo < 0 || !semanasDelAnio.value) return

  if (valorNuevo > semanasDelAnio.value) {
    anioSolicitudSemana.value = anioSolicitudSemana.value + 1
    semanaSolicitud.value = 1
    semanasDelAnio.value = obtenerNumeroSemanasEnAnio(anioSolicitudSemana.value)
  } else if (valorNuevo < 1) {
    anioSolicitudSemana.value = anioSolicitudSemana.value - 1
    semanasDelAnio.value = obtenerNumeroSemanasEnAnio(anioSolicitudSemana.value)
    semanaSolicitud.value = semanasDelAnio.value
  }

  if (valorNuevo > semanasDelAnio.value || valorNuevo < 1) return

  let fechaEnviada = obtenerFechaDeSemana(semanaSolicitud.value, anioSolicitudSemana.value)

  inicioSemanaDate.value = new Date(
    fechaEnviada.setDate(fechaEnviada.getDate() - fechaEnviada.getDay() + 1),
  )

  terminoSemanaDate.value = new Date(
    fechaEnviada.setDate(fechaEnviada.getDate() - fechaEnviada.getDay() + 7),
  )

  await refrescarDatos()
})

const columnasVisibles = ['cantidadMovimiento', 'ubicacion', 'fechaHora']

const columnas = [
  {
    name: 'id',
    label: '',
    field: (modelo) => modelo.id,
  },
  {
    name: 'cantidadMovimiento',
    label: traducir('Cantidad') + ' (Kg)',
    align: 'center',
    field: (modelo) => modelo.cantidadMovimiento,
  },
  {
    name: 'ubicacion',
    label: traducir('Ubicacion'),
    align: 'center',
    field: (modelo) => modelo.ubicacion,
  },
  {
    name: 'fechaHora',
    label: traducir('FechaHora'),
    align: 'center',
  },
]

const filtroQuery = reactive({
  cantidadMovimiento: '',
  ubicacion: '',
  fechaHora: '',
})
</script>

<style scoped></style>
