<template>
  <q-page>
    <G3ModuloHeader :modulo="traducir('Cargas')" :requiere-boton-fechas="true" :metodo-refrescar="refrescarDatosTabla"
      @update:fechas="fechas = $event" @update:hora="horaCorte = $event" @limpiar-filtros="onLimpiarFiltros" />
    <div class="row q-mt-md">
      <div class="col-12 text-textprimary">
        <G3CustomTable ref="tablaRef" :columnas-visibles="columnasVisibles" :columnas="columnas" :filas="datosTabla"
          :filas-por-pagina="5" :cargando="cargandoTabla" :filtros="filtroQuery">
          <template v-slot:col-start-opciones="{ props }">
            <BotonOpciones :items="llenarArrayOpciones(props)" />
          </template>
          <template v-slot:col-end-producto="{ props }">
            <div class="row justify-center items-center">
              <q-icon v-if="props.row.colorProducto" name="fa fa-square" :style="{ color: props.row.colorProducto }"
                class="q-pr-sm"></q-icon>
              <span>{{ props.row.producto }}</span>
            </div>
          </template>
          <template v-slot:col-end-nivelInicial="{ props }">
            <G3DetalleTabla etiqueta="Inicial" :valor="`${props.row.nivelInicial} mm`" />
            <G3DetalleTabla etiqueta="Final" :valor="`${props.row.nivelFinal} mm`" />
          </template>
          <template v-slot:col-end-volumenInicial="{ props }">
            <G3DetalleTabla etiqueta="Inicial" :valor="`${props.row.volumenInicial} Kg`" />
            <G3DetalleTabla etiqueta="Final" :valor="`${props.row.volumenFinal} Kg`" />
          </template>
          <template v-slot:col-end-volumenCargado="{ props }">
            <G3DetalleTabla etiqueta="Total" :valor="`${props.row.volumenCargado} Kg`" vertical />
          </template>
          <template v-slot:col-end-fechaHoraInicial="{ props }">
            <div style="width: 200px">
              <G3DetalleTabla etiqueta="Inicio" :valor="`${props.row.fechaHoraInicial}`" />
              <G3DetalleTabla etiqueta="Fin" :valor="`${props.row.fechaHoraFinal}`" />
            </div>
          </template>
        </G3CustomTable>
      </div>
    </div>
  </q-page>
</template>

<script setup>
import { onMounted, reactive, ref, watch, inject } from 'vue'
import { cargasAlmacenamiento } from 'src/api/moduloAlmacenamiento'
import G3ModuloHeader from 'src/components/Genericos/G3ModuloHeader.vue'
import G3CustomTable from 'src/components/Globales/G3CustomTable.vue'
import BotonOpciones from 'src/components/Paginas/General/OpcionesPage/BotonOpciones.vue'
import G3DetalleTabla from 'src/components/Globales/Tabla/components/G3DetalleTabla.vue'
import { refreshSource, source } from 'src/utils/utils'
import { debounce, useQuasar } from 'quasar'
import CustomFormDialog from 'src/components/Globales/Dialogs/CustomFormDialog.vue'
import DetalleCargaAlmacenamiento from './components/DetalleCargaAlmacenamiento.vue'

const $q = useQuasar()

const cargandoTabla = ref(false)
const datosTabla = ref([])

const fechas = ref(null)
const horaCorte = ref(null)

const traducir = inject('traducir', (key) => key)

onMounted(() => {
  refrescarDatosTabla()
})

const refrescarDatosTabla = debounce(async () => {
  const filtro = {
    traerCargas: true,
    fechaInicial: fechas.value.from,
    fechaFinal: fechas.value.to,
  }

  cargandoTabla.value = true
  const resp = await cargasAlmacenamiento.listar(filtro)
  cargandoTabla.value = false

  if (!resp.exito) {
    datosTabla.value = []
    return
  }

  datosTabla.value = resp.payload.respuesta
}, 300)

const verDetalleDialog = async ({ modelo }) => {
  $q.dialog({
    component: CustomFormDialog,
    componentProps: {
      formularioComponent: DetalleCargaAlmacenamiento,
      noBackdropDismiss: false,
      formularioComponentProps: {
        modelo,
      },
    },
  })
}

const refrescarToken = () => {
  if (source) {
    source.cancel()
  }

  refreshSource()
}

const tablaRef = ref(null)

function onLimpiarFiltros() {
  tablaRef.value?.limpiarFiltros()
}

const columnasVisibles = [
  'opciones',
  'deposito',
  'producto',
  'ubicacion',
  'nivelInicial',
  'volumenInicial',
  'volumenCargado',
  'fechaHoraInicial',
]

const columnas = [
  {
    name: 'id',
    label: '',
    field: (modelo) => modelo.id,
  },
  {
    name: 'opciones',
    label: traducir('Opciones'),
    align: 'center',
  },
  {
    name: 'deposito',
    label: traducir('Deposito'),
    align: 'center',
    field: (modelo) => modelo.deposito,
  },
  {
    name: 'producto',
    label: traducir('Producto'),
    align: 'center',
  },
  {
    name: 'ubicacion',
    label: traducir('Ubicacion'),
    align: 'center',
    field: (modelo) => modelo.ubicacion,
  },
  {
    name: 'nivelInicial',
    label: traducir('Niveles'),
    align: 'center',
  },
  {
    name: 'volumenInicial',
    label: traducir('Volumenes'),
    align: 'center',
  },
  {
    name: 'volumenCargado',
    label: traducir('Carga'),
    align: 'center',
  },
  {
    name: 'fechaHoraInicial',
    label: traducir('Fechas'),
    align: 'center',
  },
]

const filtroQuery = reactive({
  deposito: '',
  producto: '',
  ubicacion: '',
  nivelInicial: '',
  volumenInicial: '',
  volumenCargado: '',
  fechaHoraInicial: '',
})

const llenarArrayOpciones = (props) => {
  const arrayOpciones = []

  arrayOpciones.push({
    titulo: traducir('VerDetalles'),
    descripcion: traducir('VeaAMayorDetalle'),
    icono: 'pi pi-eye',
    color: 'info',
    accion: () => verDetalleDialog({ modelo: props.row }),
  })

  return arrayOpciones
}

watch([fechas, horaCorte], () => {
  refrescarToken()
  refrescarDatosTabla()
})
</script>

<style lang="scss" scoped></style>
