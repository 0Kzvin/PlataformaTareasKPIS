<template>
  <q-page>
    <G3ModuloHeader :modulo="traducir('Operadores')" requiere-boton-crear :boton-crear-texto="traducir('CrearOperador')"
      boton-crear-icono="pi pi-plus-circle" :metodo-crear="mostrarCrearEditarDialog" />

    <div class="row q-mt-md">
      <div class="col-12 text-textprimary">
        <CustomTable :columnas-visibles="columnasVisibles" :columnas="columnas" :filas="datosTabla"
          :filas-por-pagina="5" :cargando="cargandoTabla" :filtros="filtroQuery">
          <template v-slot:col-start-opciones="{ props }">
            <BotonOpciones :items="llenarArrayOpciones(props)" />
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
import { operadoresGerencia } from 'src/api/moduloGerencia'
import G3ModuloHeader from 'src/components/Genericos/G3ModuloHeader.vue'
import CustomFormDialog from 'src/components/Globales/Dialogs/CustomFormDialog.vue'
import { onMounted, reactive, ref, inject } from 'vue'
import CustomTable from 'src/components/Globales/G3CustomTable.vue'
import BotonOpciones from 'src/components/Paginas/General/OpcionesPage/BotonOpciones.vue'
import CrearEditarOperadores from './components/CrearEditarOperadores.vue'

const traducir = inject('traducir', (key) => key)

const $q = useQuasar()

const cargandoTabla = ref(false)
const datosTabla = ref([])

onMounted(async () => {
  await refrescarDatos()
})

const refrescarDatos = async () => {
  cargandoTabla.value = true
  const resp = await operadoresGerencia.listar()
  cargandoTabla.value = false

  if (!resp.exito) {
    datosTabla.value = []
    return
  }

  datosTabla.value = resp.payload.respuesta
}

const mostrarCrearEditarDialog = async (editar = false, modelo = {}) => {
  $q.dialog({
    component: CustomFormDialog,
    componentProps: {
      formularioComponent: CrearEditarOperadores,
      noBackdropDismiss: false,
      formularioComponentProps: {
        refrescarDatos,
        editar,
        modelo,
      },
    },
  })
}

const columnasVisibles = ['opciones', 'nombre', 'codigoAplicacion', 'fechaRegistro', 'estado']

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
    name: 'codigoAplicacion',
    label: traducir('CodigoAplicacion'),
    align: 'center',
    field: (modelo) => modelo.codigoAplicacion,
  },
  {
    name: 'fechaRegistro',
    label: traducir('Fechas'),
    align: 'center',
    field: (modelo) => modelo.fechaRegistro,
  },
  {
    name: 'estado',
    label: traducir('Estado'),
    align: 'center',
    sortable: true,
  },
]

//TODO : Revisar si se verán los códigos de aplicación, tal vez se podrían ocultar si la persona no tiene los permisos
const filtroQuery = reactive({
  nombre: '',
  codigoAplicacion: '',
  fechaRegistro: '',
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
    accion: () => { },
  })

  arrayOpciones.push({
    titulo: traducir('Estado'),
    descripcion: props.row.estado ? traducir('DesactivarRegistro') : traducir('ActivarRegistro'),
    icono: props.row.estado ? 'fa fa-ban' : 'fa fa-check',
    color: props.row.estado ? 'negative' : 'positive',
    accion: () => { },
  })

  arrayOpciones.push({
    titulo: traducir('Borrar'),
    descripcion: traducir('EliminarRegistro'),
    icono: 'fa fa-trash',
    color: 'negative',
    accion: () => { },
  })

  return arrayOpciones
}
</script>

<style lang="scss" scoped></style>
