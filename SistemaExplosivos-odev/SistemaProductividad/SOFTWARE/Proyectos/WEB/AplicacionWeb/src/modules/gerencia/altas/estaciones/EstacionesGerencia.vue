<template>
  <q-page>
    <G3ModuloHeader :modulo="traducir('Estaciones')" requiere-boton-crear :boton-crear-texto="traducir('CrearEstacion')"
      boton-crear-icono="pi pi-plus-circle" :metodo-refrescar="refrescarDatos" :metodo-crear="mostrarCrearEditarDialog"
      @limpiar-filtros="onLimpiarFiltros" />

    <div class="row q-mt-md">
      <div class="col-12 text-textprimary">
        <CustomTable ref="tablaRef" :columnas-visibles="columnasVisibles" :columnas="columnas" :filas="datosTabla"
          :filas-por-pagina="5" :cargando="cargandoTabla" :filtros="filtroQuery">
          <template v-slot:col-start-opciones="{ props }">
            <BotonOpciones :items="llenarArrayOpciones(props)" />
          </template>
          <template v-slot:col-end-fechaRegistro="{ props }">
            <G3DetalleTabla :etiqueta="traducir('Registrado')" :valor="`${props.row.fechaRegistro}`" vertical />
            <G3DetalleTabla :etiqueta="traducir('Modificado')" :valor="`${props.row.fechaModificacion}`" vertical />
          </template>
          <template v-slot:col-end-estado="{ props }">
            <q-chip dense square :color="props.row.estado ? 'green-1' : 'red-1'"
              :text-color="props.row.estado ? 'green-9' : 'red-8'"
              class="text-weight-bold q-px-md q-py-xs rounded-badge">
              {{ props.row.estado ? traducir('Activado') : traducir('Desactivado') }}
            </q-chip>
          </template>
        </CustomTable>
      </div>
    </div>
  </q-page>
</template>

<script setup>
import { useQuasar } from 'quasar'
import { onMounted, reactive, ref, inject } from 'vue'
import G3ModuloHeader from 'src/components/Genericos/G3ModuloHeader.vue'
import CustomTable from 'src/components/Globales/G3CustomTable.vue'
import { estacionesGerencia } from 'src/api/moduloGerencia'
import CustomFormDialog from 'src/components/Globales/Dialogs/CustomFormDialog.vue'
import CrearEditarEstaciones from './components/CrearEditarEstaciones.vue'
import BotonOpciones from 'src/components/Paginas/General/OpcionesPage/BotonOpciones.vue'
import G3DetalleTabla from 'src/components/Globales/Tabla/components/G3DetalleTabla.vue'
import * as quasarUtils from 'src/utils/quasar-utils.js'

const traducir = inject('traducir', (key) => key)

const $q = useQuasar()

const cargandoTabla = ref(false)
const datosTabla = ref([])

onMounted(async () => {
  await refrescarDatos()
})

const tablaRef = ref(null)

function onLimpiarFiltros() {
  tablaRef.value?.limpiarFiltros()
}

const refrescarDatos = async () => {
  cargandoTabla.value = true
  const resp = await estacionesGerencia.listar()
  cargandoTabla.value = false

  if (!resp.exito) {
    datosTabla.value = []
    return
  }

  datosTabla.value = resp.payload.respuesta
}

const mostrarCrearEditarDialog = async ({ editar = false, modelo = {} }) => {
  $q.dialog({
    component: CustomFormDialog,
    componentProps: {
      formularioComponent: CrearEditarEstaciones,
      noBackdropDismiss: false,
      formularioComponentProps: {
        refrescarDatos,
        editar,
        modelo,
      },
    },
  })
}

const cambiarEstadoModelo = async (id) => {
  cargandoTabla.value = true
  const resp = await estacionesGerencia.cambiarEstado(id)
  cargandoTabla.value = false

  if (!resp.exito) {
    await quasarUtils.aviso({
      error: true,
      mensaje: resp.payload.mensaje,
    })

    return
  }

  await refrescarDatos()

  await quasarUtils.aviso({
    exito: true,
    mensaje: resp.payload.mensaje,
  })
}

const borrarModelo = async (id) => {
  cargandoTabla.value = true
  const resp = await estacionesGerencia.borrar(id)
  cargandoTabla.value = false

  if (!resp.exito) {
    await quasarUtils.aviso({
      error: true,
      mensaje: resp.payload.mensaje,
    })

    return
  }

  await refrescarDatos()

  await quasarUtils.aviso({
    exito: true,
    mensaje: resp.payload.mensaje,
  })
}

const columnasVisibles = ['opciones', 'nombre', 'ip', 'fechaRegistro', 'estado']

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
    name: 'nombre',
    label: traducir('Nombre'),
    align: 'center',
    field: (modelo) => modelo.nombre,
  },
  {
    name: 'ip',
    label: traducir('Ip'),
    align: 'center',
    field: (modelo) => modelo.ip,
  },
  {
    name: 'fechaRegistro',
    label: traducir('Fechas'),
    align: 'center',
  },
  {
    name: 'estado',
    label: traducir('Estado'),
    align: 'center',
    sortable: true,
  },
]

const filtroQuery = reactive({
  nombre: '',
  ip: '',
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
  estado: {
    seleccion: null,
    opciones: [
      {
        label: traducir('Activado'),
        value: true,
      },
      {
        label: traducir('Desactivado'),
        value: false,
      },
    ],
  },
})

const llenarArrayOpciones = (props) => {
  const arrayOpciones = []

  arrayOpciones.push({
    titulo: traducir('Editar'),
    descripcion: traducir('EditarInformacion'),
    icono: 'fa fa-pen',
    color: 'warning',
    accion: () => mostrarCrearEditarDialog({ editar: true, modelo: props.row }),
  })

  arrayOpciones.push({
    titulo: traducir('Estado'),
    descripcion: props.row.estado ? traducir('DesactivarRegistro') : traducir('ActivarRegistro'),
    icono: props.row.estado ? 'fa fa-ban' : 'fa fa-check',
    color: props.row.estado ? 'negative' : 'positive',
    accion: () => cambiarEstadoModelo(props.row.id),
  })

  arrayOpciones.push({
    titulo: traducir('Borrar'),
    descripcion: traducir('EliminarRegistro'),
    icono: 'fa fa-trash',
    color: 'negative',
    accion: () => borrarModelo(props.row.id),
  })

  return arrayOpciones
}
</script>

<style lang="scss" scoped></style>
