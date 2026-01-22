<!-- eslint-disable vue/multi-word-component-names -->
<template>
  <q-page>
    <G3ModuloHeader :modulo="traducir('Auditoria')" />
    <!-- <selector-fechas class="q-mb-md" @cambioFechasSelector="cambioFechasSelector" /> -->
    <div class="row q-mt-md">
      <div class="col-12 text-textprimary">
        <G3CustomTable :columnasVisibles="[
          'opciones',
          'mensaje',
          'nivel',
          'excepcion',
          'usuario',
          'accion',
          'fechaHora',
        ]" :columnas="[
            {
              name: 'id',
              label: '',
              field: (logs) => logs.id,
            },
            {
              name: 'opciones',
              label: traducir('Opciones'),
              align: 'center',
            },
            {
              name: 'mensaje',
              label: traducir('Mensaje'),
              align: 'center',
              field: (logs) => logs.mensaje,
            },
            {
              name: 'nivel',
              label: traducir('Nivel'),
              align: 'center',
              field: (logs) => logs.nivel,
            },
            {
              name: 'excepcion',
              label: traducir('Excepcion'),
              align: 'center',
              field: (logs) => (logs.excepcion ? logs.excepcion : '-'),
            },
            {
              name: 'usuario',
              label: traducir('Usuario'),
              align: 'center',
              field: (logs) => logs.usuario,
            },
            {
              name: 'accion',
              label: traducir('Accion'),
              align: 'center',
              field: (logs) => logs.accion,
            },
            {
              name: 'fechaHora',
              label: traducir('FechaHora'),
              align: 'center',
              field: (logs) => logs.fechaHora,
            },
          ]" :filas="logs" :cargando="cargandoTablaLogs" :filasPorPagina="10" :filtros="{
            mensaje: '',
            excepcion: '',
            usuario: '',
            fechaHora: '',
            nivel: {
              seleccion: null,
              opciones: [
                {
                  label: traducir('Information'),
                  value: 'Information',
                },
                {
                  label: traducir('Warning'),
                  value: 'Warning',
                },
              ],
            },
            accion: '',
          }">
          <template v-slot:col-start-opciones="{ props }">
            <q-btn class="q-mr-sm text-size-12" color="accent" round dense icon="priority_high"
              @click="mostrarDetallesDialog(props.row)"></q-btn>
          </template>
        </G3CustomTable>
      </div>
    </div>
  </q-page>
</template>

<script setup>
import { onMounted, ref, inject } from 'vue'
import { logsAdmin } from 'src/api/moduloAdministracion/index.js'
import { useQuasar } from 'quasar'
import CustomFormDialog from 'src/components/Globales/Dialogs/CustomFormDialog.vue'
import G3CustomTable from 'src/components/Globales/G3CustomTable.vue'
import DetallesLog from 'src/modules/administracion/pages/auditoria/components/DetallesLog.vue'
// import SelectorFechas from 'src/components/Globales/SelectorFechas.vue'
import { refreshSource } from 'src/utils/utils'
import G3ModuloHeader from 'src/components/Genericos/G3ModuloHeader.vue'

const traducir = inject('traducir', (key) => key)

const $q = useQuasar()
const logs = ref([])
const cargandoTablaLogs = ref(false)

const refrescarLogs = async (fecha1 = null, fecha2 = null) => {
  cargandoTablaLogs.value = true
  const resp = await logsAdmin.listar(fecha1, fecha2)
  cargandoTablaLogs.value = false
  if (!resp.exito) {
    logs.value = []
    return
  }
  logs.value = resp.payload
}

onMounted(async () => {
  refreshSource()
  refrescarLogs()
})

// const cambioFechasSelector = (value) => {
//   if (!value) return
//   refrescarLogs(value.from, value.to)
// }

const mostrarDetallesDialog = (log) => {
  $q.dialog({
    component: CustomFormDialog,
    componentProps: {
      formularioComponent: DetallesLog,
      noBackdropDismiss: false,
      formularioComponentProps: {
        log,
      },
    },
  })
}
</script>
