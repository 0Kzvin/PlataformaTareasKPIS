<template>
  <q-page>
    <G3ModuloHeader :modulo="traducir('Equipos')"
      :requiere-boton-crear="sesionStore.estaPermisoOtorgado(PermisoCrearEquipo)"
      :boton-crear-texto="traducir('CrearEquipo')" boton-crear-icono="pi pi-plus-circle"
      :metodo-crear="mostrarCrearEditarEquipoDialog" :metodo-refrescar="refrescarDatos"
      @limpiar-filtros="onLimpiarFiltros" />

    <div class="row q-mt-md">
      <div class="col-12 text-textprimary">
        <G3CustomTable ref="tablaRef" :columnas-visibles="columnasVisibles" :columnas="columnas" :filas="datosTabla"
          :filas-por-pagina="5" :cargando="cargandoTabla" :filtros="filtroQuery">
          <template v-slot:col-start-opciones="{ props }">
            <boton-opciones :items="llenarArrayOpciones(props)" />
          </template>
          <template v-slot:col-start-numeroEconomico="{ props }">
            <G3DetalleTabla :etiqueta="traducir('NoEco')" :valor="props.row.numeroEconomico" />
            <G3DetalleTabla :etiqueta="traducir('Apodo')" :valor="props.row.apodo" />
          </template>

          <template v-slot:col-end-producto="{ props }">
            <div class="row justify-center items-center">
              <q-icon name="fa fa-square" :style="{ color: props.row.colorProducto }" class="q-pr-sm"></q-icon>
              <span>{{ props.row.producto }}</span>
            </div>
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
          <template v-slot:col-end-esExterno="{ props }">
            <q-chip dense square :color="props.row.esExterno ? 'orange-1' : 'blue-1'"
              :text-color="props.row.esExterno ? 'orange-8' : 'blue-9'"
              class="text-weight-bold q-px-md q-py-xs rounded-badge">
              {{ props.row.esExterno ? traducir('Externo') : traducir('Local') }}
            </q-chip>
          </template>
        </G3CustomTable>
      </div>
    </div>
  </q-page>
</template>

<script setup>
import { useQuasar } from 'quasar'
import { onMounted, reactive, ref, inject } from 'vue'
import G3CustomTable from 'src/components/Globales/G3CustomTable.vue'
import { equiposAlmacenamiento } from 'src/api/moduloAlmacenamiento'
import G3ModuloHeader from 'src/components/Genericos/G3ModuloHeader.vue'
import CrearEditarEquiposForm from './components/CrearEditarEquiposForm.vue'
import CustomFormDialog from 'src/components/Globales/Dialogs/CustomFormDialog.vue'
import BotonOpciones from 'src/components/Paginas/General/OpcionesPage/BotonOpciones.vue'
import G3DetalleTabla from 'src/components/Globales/Tabla/components/G3DetalleTabla.vue'
import * as quasarUtils from 'src/utils/quasar-utils.js'
import { useSesionStore } from 'src/stores/sesion'
import {
  PermisoCrearEquipo,
  PermisoEditarEquipo,
  PermisoEliminarEquipo,
} from 'src/core/permisos/moduloAlmacenamiento/equipos.js'

const traducir = inject('traducir')

const $q = useQuasar()
const sesionStore = useSesionStore()

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
  const filtro = {
    borrado: false,
  }

  cargandoTabla.value = true
  const resp = await equiposAlmacenamiento.listar(filtro)
  cargandoTabla.value = false

  if (!resp.exito) {
    datosTabla.value = []
    return
  }

  datosTabla.value = resp.payload.respuesta
}

const mostrarCrearEditarEquipoDialog = async ({ editar = false, modelo = {} }) => {
  $q.dialog({
    component: CustomFormDialog,
    componentProps: {
      formularioComponent: CrearEditarEquiposForm,
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
  const resp = await equiposAlmacenamiento.cambiarEstado(id)
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
  const resp = await equiposAlmacenamiento.borrar(id)
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

const columnasVisibles = [
  'opciones',
  'numeroEconomico',
  'producto',
  'cantidadActual',
  'capacidad',
  'esExterno',
  'fechaRegistro',
  'estado',
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
    name: 'numeroEconomico',
    label: traducir('NumeroEconomico'),
    align: 'center',
  },
  {
    name: 'producto',
    label: traducir('Producto'),
    align: 'center',
  },
  {
    name: 'cantidadActual',
    label: traducir('CantidadActual'),
    align: 'center',
    field: (modelo) => modelo.cantidadActual,
  },
  {
    name: 'capacidad',
    label: traducir('Capacidad'),
    align: 'center',
    field: (modelo) => modelo.capacidad,
  },
  {
    name: 'esExterno',
    label: traducir('EsExterno'),
    align: 'center',
    sortable: true,
  },
  {
    name: 'fechaRegistro',
    label: traducir('FechaDeCreacion'),
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
  numeroEconomico: [
    {
      propiedad: 'numeroEconomico',
      valor: '',
    },
    {
      propiedad: 'apodo',
      valor: '',
    },
  ],
  producto: '',
  cantidadActual: '',
  capacidad: '',
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
  esExterno: {
    seleccion: null,
    opciones: [
      {
        label: traducir('Externo'),
        value: true,
      },
      {
        label: traducir('Local'),
        value: false,
      },
    ],
  },
})

const llenarArrayOpciones = (props) => {
  const arrayOpciones = []

  if (sesionStore.estaPermisoOtorgado(PermisoEditarEquipo)) {
    arrayOpciones.push({
      titulo: traducir('Editar'),
      descripcion: traducir('EditarInformacion'),
      icono: 'pi pi-pencil',
      color: 'warning',
      accion: () => mostrarCrearEditarEquipoDialog({ editar: true, modelo: props.row }),
    })

    arrayOpciones.push({
      titulo: traducir('Estado'),
      descripcion: props.row.estado ? traducir('DesactivarRegistro') : traducir('ActivarRegistro'),
      icono: props.row.estado ? 'pi pi-ban' : 'pi pi-check',
      color: props.row.estado ? 'negative' : 'positive',
      accion: () => cambiarEstadoModelo(props.row.id),
    })
  }

  if (sesionStore.estaPermisoOtorgado(PermisoEliminarEquipo)) {
    arrayOpciones.push({
      titulo: traducir('Borrar'),
      descripcion: traducir('EliminarRegistro'),
      icono: 'pi pi-trash',
      color: 'negative',
      accion: () => borrarModelo(props.row.id),
    })
  }

  return arrayOpciones
}
</script>

<style lang="scss" scoped></style>
