<template>
  <q-page>
    <G3ModuloHeader :modulo="traducir('Supersacos')"
      :requiere-boton-crear="sesionStore.estaPermisoOtorgado(PermisoCrearMovimiento)" :requiere-boton-fechas="true"
      :metodo-crear="mostrarCrearEditarDialog" boton-crear-icono="pi pi-pen-to-square"
      :boton-crear-texto="traducir('RegistrarMovimiento')" @update:fechas="fechas = $event"
      @update:hora="horaCorte = $event" :metodo-refrescar="refrescarDatos" @limpiar-filtros="onLimpiarFiltros" />

    <div class="row q-mt-md text-textprimary">
      <div class="col-3 q-pr-sm">
        <DetalleEstadistica icono-estadistica="pi pi-database" :titulo-estadistica="traducir('InventarioInicial')"
          :valor-estadistica="respuesta.inventarioInicial ?? 0" unidad-estadistica="Kg" />
      </div>
      <div class="col-3 q-px-sm">
        <DetalleEstadistica icono-estadistica="pi pi-arrow-up" :titulo-estadistica="traducir('Entradas')"
          :valor-estadistica="respuesta.entradas ?? 0" unidad-estadistica="Kg" />
      </div>
      <div class="col-3 q-px-sm">
        <DetalleEstadistica icono-estadistica="pi pi-arrow-down" :titulo-estadistica="traducir('Salidas')"
          :valor-estadistica="respuesta.salidas ?? 0" unidad-estadistica="Kg" />
      </div>
      <div class="col-3 q-pl-sm">
        <DetalleEstadistica icono-estadistica="pi pi-chart-line" :titulo-estadistica="traducir('InventarioFinal')"
          :es-negativa="respuesta.inventarioFinal < respuesta.inventarioInicial"
          :es-positiva="respuesta.inventarioFinal > respuesta.inventarioInicial"
          :valor-estadistica="respuesta.inventarioFinal ?? 0" unidad-estadistica="Kg" />
      </div>
    </div>

    <div class="row q-mt-md">
      <div class="col-12 text-textprimary">
        <G3CustomTable ref="tablaRef" :columnas-visibles="columnasVisibles" :columnas="columnas" :filas="datosTabla"
          :filas-por-pagina="5" :cargando="cargandoTabla" :filtros="filtroQuery">
          <template v-slot:col-start-opciones="{ props }">
            <boton-opciones :items="llenarArrayOpciones(props)" />
          </template>
          <template v-slot:col-end-producto="{ props }">
            <div class="row justify-center items-center">
              <q-icon v-if="props.row.colorProducto" name="fa fa-square" :style="{ color: props.row.colorProducto }"
                class="q-pr-sm"></q-icon>
              <span>{{ props.row.producto }}</span>
            </div>
          </template>
          <template v-slot:col-end-cantidadInicial="{ props }">
            <G3DetalleTabla etiqueta="Inicial" :valor="`${props.row.cantidadInicial} Kg`" />
            <G3DetalleTabla etiqueta="Final" :valor="`${props.row.cantidadFinal} Kg`" />
          </template>
          <template v-slot:col-end-observaciones="{ props }">
            <span v-if="props.row.observaciones"> {{ props.row.observaciones }}</span>
            <span v-else>---</span>
          </template>
          <template v-slot:col-end-cantidadMovimiento="{ props }">
            <div class="row justify-center items-center">
              <i v-if="props.row.cantidadMovimiento > 0" class="pi pi-arrow-up q-pr-xs text-positive text-size-12"></i>
              <i v-if="props.row.cantidadMovimiento < 0"
                class="pi pi-arrow-down q-pr-xs text-negative text-size-12"></i>
              <span> {{ props.row.cantidadMovimiento }} Kg </span>
            </div>
          </template>
          <template v-slot:col-end-fechaRegistro="{ props }">
            <G3DetalleTabla etiqueta="Registrado" :valor="`${props.row.fechaRegistro}`" vertical />
            <G3DetalleTabla etiqueta="Modificado" :valor="`${props.row.fechaModificacion}`" vertical />
          </template>
        </G3CustomTable>
      </div>
    </div>
  </q-page>
</template>

<script setup>
import G3ModuloHeader from 'src/components/Genericos/G3ModuloHeader.vue'
import G3CustomTable from 'src/components/Globales/G3CustomTable.vue'
import BotonOpciones from 'src/components/Paginas/General/OpcionesPage/BotonOpciones.vue'
import G3DetalleTabla from 'src/components/Globales/Tabla/components/G3DetalleTabla.vue'
import { onMounted, reactive, ref, watch, inject } from 'vue'
import { supersacosAlmacenamiento } from 'src/api/moduloAlmacenamiento'
import { debounce, useQuasar } from 'quasar'
import { refreshSource, source } from 'src/utils/utils'
import DetalleEstadistica from './components/DetalleEstadistica.vue'
import CustomFormDialog from 'src/components/Globales/Dialogs/CustomFormDialog.vue'
import CrearEditarMovimientoSuperSaco from './components/CrearEditarMovimientoSuperSaco.vue'
import * as quasarUtils from 'src/utils/quasar-utils.js'
import { useSesionStore } from 'src/stores/sesion'
import {
  PermisoCrearMovimiento,
  PermisoEditarMovimiento,
  PermisoEliminarMovimiento,
} from 'src/core/permisos/moduloAlmacenamiento/movimientos.js'

const $q = useQuasar()
const sesionStore = useSesionStore()

const cargandoTabla = ref(false)
const datosTabla = ref([])
const respuesta = ref({})

const fechas = ref(null)
const horaCorte = ref(null)

const traducir = inject('traducir', (key) => key)

onMounted(() => {
  refrescarDatos()
})

const tablaRef = ref(null)

function onLimpiarFiltros() {
  tablaRef.value?.limpiarFiltros()
}

const refrescarDatos = debounce(async () => {
  respuesta.value = []

  const filtro = {
    fechaInicial: fechas.value.from,
    fechaFinal: fechas.value.to,
  }

  cargandoTabla.value = true
  const resp = await supersacosAlmacenamiento.listar(filtro)
  cargandoTabla.value = false

  if (!resp.exito) {
    datosTabla.value = []
    return
  }

  respuesta.value = resp.payload.respuesta
  datosTabla.value = resp.payload.respuesta.movimientos
}, 300)

const mostrarCrearEditarDialog = async ({ editar = false, modelo = {} }) => {
  $q.dialog({
    component: CustomFormDialog,
    componentProps: {
      formularioComponent: CrearEditarMovimientoSuperSaco,
      noBackdropDismiss: false,
      formularioComponentProps: {
        refrescarDatos,
        editar,
        cantidadInicialModelo: respuesta.value.inventarioFinal,
        modelo,
      },
    },
  })
}

const borrarModelo = async (id) => {
  cargandoTabla.value = true
  const resp = await supersacosAlmacenamiento.borrar(id)
  cargandoTabla.value = false

  if (!resp.exito) {
    await quasarUtils.aviso({
      error: true,
      mensaje: resp.payload.mensaje,
    })

    return
  }

  refrescarDatos()

  await quasarUtils.aviso({
    exito: true,
    mensaje: resp.payload.mensaje,
  })
}

const refrescarToken = () => {
  if (source) {
    source.cancel()
  }

  refreshSource()
}

const columnasVisibles = [
  'opciones',
  'producto',
  'ubicacion',
  'observaciones',
  'cantidadInicial',
  'cantidadMovimiento',
  'fechaHora',
  'fechaRegistro',
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
    name: 'observaciones',
    label: traducir('Observaciones'),
    align: 'center',
    field: (modelo) => modelo.observaciones,
  },
  {
    name: 'cantidadInicial',
    label: traducir('Cantidades'),
    align: 'center',
  },
  {
    name: 'cantidadMovimiento',
    label: traducir('Movimiento'),
    align: 'center',
  },
  {
    name: 'fechaHora',
    label: traducir('Fecha'),
    align: 'center',
    field: (modelo) => modelo.fechaHora,
  },
  {
    name: 'fechaRegistro',
    label: traducir('Fechas'),
    align: 'center',
  },
]

const filtroQuery = reactive({
  producto: '',
  ubicacion: '',
  observaciones: '',
  cantidadInicial: '',
  cantidadMovimiento: '',
  fechaHora: '',
  fechaRegistro: [
    {
      propiedad: 'fechaRegistro',
      valor: '',
    },
    {
      propiedad: 'fechaModificacion',
      valor: '',
    },
  ],
})

const llenarArrayOpciones = (props) => {
  const arrayOpciones = []

  // arrayOpciones.push({
  //   titulo: traducir('VerDetalles'),
  //   descripcion: traducir('VeaAMayorDetalle'),
  //   icono: 'pi pi-eye',
  //   color: 'secondary',
  //   accion: () => {
  //     props.row
  //   },
  // })

  if (sesionStore.estaPermisoOtorgado(PermisoEditarMovimiento)) {
    arrayOpciones.push({
      titulo: traducir('Editar'),
      descripcion: traducir('ModifiqueLaInformacion'),
      icono: 'pi pi-pencil',
      color: 'warning',
      accion: () => mostrarCrearEditarDialog({ editar: true, modelo: props.row }),
    })
  }

  if (sesionStore.estaPermisoOtorgado(PermisoEliminarMovimiento)) {
    arrayOpciones.push({
      titulo: traducir('Borrar'),
      descripcion: traducir('ElimineElRegistro'),
      icono: 'pi pi-trash',
      color: 'negative',
      accion: () => borrarModelo(props.row.id),
    })
  }

  return arrayOpciones
}

watch([fechas, horaCorte], () => {
  refrescarToken()
  refrescarDatos()
})
</script>

<style lang="scss" scoped></style>
