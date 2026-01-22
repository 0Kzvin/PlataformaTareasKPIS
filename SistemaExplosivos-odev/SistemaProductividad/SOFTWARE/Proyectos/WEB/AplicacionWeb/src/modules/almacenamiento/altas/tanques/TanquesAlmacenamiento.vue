<template>
  <q-page>
    <G3ModuloHeader :modulo="traducir('Tanques')"
      :requiere-boton-crear="sesionStore.estaPermisoOtorgado(PermisoCrearDeposito)"
      :boton-crear-texto="traducir('CrearTanque')" boton-crear-icono="pi pi-plus-circle"
      :metodo-crear="mostrarCrearEditarTanqueDialog" :metodo-refrescar="refrescarDatos"
      @limpiar-filtros="onLimpiarFiltros" />

    <div class="row q-mt-md">
      <div class="col-12 text-textprimary">
        <G3CustomTable ref="tablaRef" :columnas-visibles="columnasVisibles" :columnas="columnas" :filas="datosTabla"
          :filas-por-pagina="5" :cargando="cargandoTabla" :filtros="filtroQuery">
          <template v-slot:col-start-opciones="{ props }">
            <BotonOpciones :items="llenarArrayOpciones(props)" />
          </template>
          <template v-slot:col-end-nombre="{ props }">
            <G3DetalleTabla :etiqueta="traducir('Nombre')" :valor="props.row.nombre" />
            <G3DetalleTabla :etiqueta="traducir('Apodo')" :valor="props.row.apodo" />
          </template>
          <template v-slot:col-end-producto="{ props }">
            <div class="row justify-center items-center">
              <q-icon name="fa fa-square" :style="{ color: props.row.colorProducto }" class="q-pr-sm"></q-icon>
              <span>{{ props.row.producto }}</span>
            </div>
          </template>
          <template v-slot:col-end-capacidadMaxima="{ props }">
            <G3DetalleTabla :etiqueta="traducir('Operativa')" :valor="`${props.row.capacidadOperativa} Kg`" />
            <G3DetalleTabla :etiqueta="traducir('Maxima')" :valor="`${props.row.capacidadMaxima} Kg`" />
          </template>
          <template v-slot:col-end-alturaMaxima="{ props }">
            <G3DetalleTabla :etiqueta="traducir('Operativa')" :valor="`${props.row.alturaOperativa} Kg`" />
            <G3DetalleTabla :etiqueta="traducir('Maxima')" :valor="`${props.row.alturaMaxima} Kg`" />
          </template>
          <template v-slot:col-end-limiteAlto="{ props }">
            <G3DetalleTabla :etiqueta="traducir('Maximo')" :valor="`${props.row.limiteMaximo} mm`" />
            <G3DetalleTabla :etiqueta="traducir('Alto')" :valor="`${props.row.limiteAlto} mm`" />
            <G3DetalleTabla :etiqueta="traducir('Bajo')" :valor="`${props.row.limiteBajo} mm`" />
            <G3DetalleTabla :etiqueta="traducir('Minimo')" :valor="`${props.row.limiteMinimo} mm`" />
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
        </G3CustomTable>
      </div>
    </div>
  </q-page>
</template>

<script setup>
import { useQuasar } from 'quasar'
import { onMounted, reactive, ref, inject } from 'vue'
import { tanquesAlmacenamiento } from 'src/api/moduloAlmacenamiento'
import G3ModuloHeader from 'src/components/Genericos/G3ModuloHeader.vue'
import G3CustomTable from 'src/components/Globales/G3CustomTable.vue'
import BotonOpciones from 'src/components/Paginas/General/OpcionesPage/BotonOpciones.vue'
import CustomFormDialog from 'src/components/Globales/Dialogs/CustomFormDialog.vue'
import CrearEditarTanquesForm from './components/CrearEditarTanquesForm.vue'
import G3DetalleTabla from 'src/components/Globales/Tabla/components/G3DetalleTabla.vue'
import * as quasarUtils from 'src/utils/quasar-utils.js'
import { useSesionStore } from 'src/stores/sesion'
import {
  PermisoCrearDeposito,
  PermisoEditarDeposito,
  PermisoEliminarDeposito,
} from 'src/core/permisos/moduloAlmacenamiento/tanques.js'

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
  const resp = await tanquesAlmacenamiento.listar(filtro)
  cargandoTabla.value = false

  if (!resp.exito) {
    datosTabla.value = []
    return
  }

  datosTabla.value = resp.payload.respuesta
}

const mostrarCrearEditarTanqueDialog = async ({ editar = false, modelo = {} }) => {
  $q.dialog({
    component: CustomFormDialog,
    componentProps: {
      formularioComponent: CrearEditarTanquesForm,
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
  const resp = await tanquesAlmacenamiento.cambiarEstado(id)
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
  const resp = await tanquesAlmacenamiento.borrar(id)
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
  'nombre',
  'producto',
  'capacidadMaxima',
  'alturaMaxima',
  'limiteAlto',
  'ubicacion',
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
    name: 'nombre',
    label: traducir('Nombre'),
    align: 'center',
  },
  {
    name: 'producto',
    label: traducir('Producto'),
    align: 'center',
  },
  {
    name: 'capacidadMaxima',
    label: traducir('Capacidades') + ' (Kg)',
    align: 'center',
  },
  {
    name: 'alturaMaxima',
    label: traducir('Alturas') + ' (mm)',
    align: 'center',
  },
  {
    name: 'limiteAlto',
    label: traducir('Limites') + ' (mm)',
    align: 'center',
  },
  {
    name: 'ubicacion',
    label: traducir('Ubicacion'),
    align: 'center',
    field: (modelo) => modelo.ubicacion,
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
  nombre: [
    {
      propiedad: 'nombre',
      valor: '',
    },
    {
      propiedad: 'apodo',
      valor: '',
    },
  ],
  producto: '',
  capacidadMaxima: [
    {
      propiedad: 'capacidadMaxima',
      valor: '',
    },
    {
      propiedad: 'capacidadOperativa',
      valor: '',
    },
  ],
  alturaMaxima: [
    {
      propiedad: 'alturaMaxima',
      valor: '',
    },
    {
      propiedad: 'alturaOperativa',
      valor: '',
    },
  ],
  limiteAlto: [
    {
      propiedad: 'limiteMaximo',
      valor: '',
    },
    {
      propiedad: 'limiteAlto',
      valor: '',
    },
    {
      propiedad: 'limiteBajo',
      valor: '',
    },
    {
      propiedad: 'limiteMinimo',
      valor: '',
    },
  ],
  ubicacion: '',
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

  if (sesionStore.estaPermisoOtorgado(PermisoEditarDeposito)) {
    arrayOpciones.push({
      titulo: traducir('Editar'),
      descripcion: traducir('EditarInformacion'),
      icono: 'pi pi-pencil',
      color: 'warning',
      accion: () => mostrarCrearEditarTanqueDialog({ editar: true, modelo: props.row }),
    })

    arrayOpciones.push({
      titulo: traducir('Estado'),
      descripcion: props.row.estado ? traducir('DesactivarRegistro') : traducir('ActivarRegistro'),
      icono: props.row.estado ? 'pi pi-ban' : 'pi pi-check',
      color: props.row.estado ? 'negative' : 'positive',
      accion: () => cambiarEstadoModelo(props.row.id),
    })
  }

  if (sesionStore.estaPermisoOtorgado(PermisoEliminarDeposito)) {
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

<style scoped></style>
