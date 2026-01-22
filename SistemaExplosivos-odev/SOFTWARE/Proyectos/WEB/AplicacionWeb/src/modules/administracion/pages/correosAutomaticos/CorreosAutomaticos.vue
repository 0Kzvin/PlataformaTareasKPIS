<template>
  <q-page>
    <G3ModuloHeader :modulo="traducir('CorreosAutomaticos')" />
    <div class="row q-mt-md">
      <div class="col-12">
        <G3CustomTable :columnasVisibles="[
          'opciones',
          'nombre',
          'activo',
          'nombreModulo',
          'descripcion',
          'cronHumanizado',
        ]" :columnas="[
            {
              name: 'id',
              label: '',
              align: 'center',
              field: (correoAutomatico) => correoAutomatico.id,
            },
            {
              name: 'identificadorUnico',
              label: '',
              align: 'center',
              field: (correoAutomatico) => correoAutomatico.idUnico,
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
              field: (correoAutomatico) => correoAutomatico.nombre,
            },
            {
              name: 'descripcion',
              label: traducir('Descripcion'),
              align: 'center',
              field: (correoAutomatico) => correoAutomatico.descripcion,
            },
            {
              name: 'cronHumanizado',
              label: traducir('Configuracion'),
              align: 'center',
              field: (correoAutomatico) => correoAutomatico.expresionCronHumanizada,
            },
            {
              name: 'nombreModulo',
              label: traducir('Modulo'),
              align: 'center',
              field: (correoAutomatico) => correoAutomatico.nombreModulo,
            },
            {
              name: 'activo',
              label: traducir('Estado'),
              align: 'center',
              sortable: true,
            },
          ]" :filas="correoAutomaticos" :cargando="cargandoTablaCorreos" :filtros="filtrosCorreosQuery">
          <template v-slot:col-end-activo="{ props }">
            <q-chip dense square :color="props.row.activo ? 'positive' : 'negative'" text-color="white"
              class="text-weight-bolder">{{ props.row.activo ? traducir('Activado') : traducir('Desactivado')
              }}</q-chip>
          </template>
          <template v-slot:col-start-opciones="{ props }">
            <boton-opciones :items="llenarArrayOpciones(props)" />
          </template>
        </G3CustomTable>
      </div>
    </div>
  </q-page>
</template>

<script setup>
import { useQuasar } from 'quasar'
import { onMounted, ref, reactive, inject } from 'vue'
import { correosAutomaticosAdmin } from 'src/api/moduloAdministracion'
import G3CustomTable from 'src/components/Globales/G3CustomTable.vue'
import { pCorreosAutomaticosAD } from 'src/core/permisos/moduloAdministracion'
import { estaPermisoOtorgado } from 'src/core/permisos/check.js'
import * as quasarUtils from 'src/utils/quasar-utils.js'
import BotonOpciones from 'src/components/Paginas/General/OpcionesPage/BotonOpciones.vue'
import CustomFormDialog from 'src/components/Globales/Dialogs/CustomFormDialog.vue'
import CrearEditarCorreosAutomaticosForm from 'src/modules/administracion/pages/correosAutomaticos/components/CrearEditarCorreosAutomaticosForm.vue'
import { refreshSource } from 'src/utils/utils'
import G3ModuloHeader from 'src/components/Genericos/G3ModuloHeader.vue'

const traducir = inject('traducir', (key) => key)

const $q = useQuasar()
const correoAutomaticos = ref([])
const cargandoTablaCorreos = ref(false)

const filtrosCorreosQuery = reactive({
  nombre: '',
  nombreModulo: '',
  descripcion: '',
  cronHumanizado: '',
  activo: {
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

const refrescarCorreos = async () => {
  cargandoTablaCorreos.value = true
  const resp = await correosAutomaticosAdmin.listar()
  cargandoTablaCorreos.value = false
  if (!resp.exito) {
    correoAutomaticos.value = []
    return
  }
  correoAutomaticos.value = resp.payload
  correoAutomaticos.value.forEach((e) => {
    e.cronHumanizado = humanizarExpresionCron(e.expresionCron)
  })
}

const mostrarCrearEditarCorreoAutomatico = async ({ editar = false, correoAutomatico = {} }) => {
  if (!editar) correoAutomatico = {}
  $q.dialog({
    component: CustomFormDialog,
    componentProps: {
      formularioComponent: CrearEditarCorreosAutomaticosForm,
      formularioComponentProps: {
        refrescarCorreosAutomaticos: refrescarCorreos,
        editar,
        correoAutomatico,
      },
    },
  })
}

const cambiarEstadoCorreoAutomatico = async (id, estado, nombreCorreoAutomatico) => {
  const decision = await quasarUtils.decision({
    titulo: estado ? traducir('DesactivarRegistroPregunta') : traducir('ActivarRegistroPregunta'),
    mensaje: `${traducir('CorreoAutomatico')}: ${nombreCorreoAutomatico}`,
    icono: 'secondary',
    iconoColor: estado ? 'secondary' : 'positive',
  })
  if (decision) {
    quasarUtils.cargandoSimple()
    const resp = await correosAutomaticosAdmin.cambiarEstado(id)
    quasarUtils.ocultarCargandoSimple()
    if (!resp.exito) {
      quasarUtils.aviso({
        error: true,
        mensaje: resp.payload.errores[0],
      })
      return
    }
    refrescarCorreos()
    await quasarUtils.aviso({
      exito: true,
      mensaje: traducir('EstadoCorreoExito'),
    })
  }
}

const humanizarExpresionCron = (cronExpression) => {
  var expresionHoras = ''
  var expresionDias = ''
  var splitCron = cronExpression.split(' ')
  if (splitCron[2].split('/').length == 1) {
    expresionHoras = `${traducir('ALas')} ${splitCron[2].split('/')}:00`
    if (splitCron[2].split('/') > 12) {
      expresionHoras = expresionHoras + ' ' + traducir('PM')
    } else {
      expresionHoras = expresionHoras + ' ' + traducir('AM')
    }
  } else if (splitCron[2].split('/').length == 2) {
    if (splitCron[2].split('/')[1] == '1') {
      expresionHoras = traducir('CadaHoras')
    } else {
      expresionHoras = `${traducir('Cada')} ${splitCron[2].split('/')[1]} ${traducir('Horas')}`
    }
  }
  if (splitCron[5].split(',').length == 1) {
    expresionDias = traducir('TodosLosDias')
  } else if (splitCron[5].split(',').length > 1) {
    var arrayDias = splitCron[5].split(',')
    var diasEnEspañol = ''
    var i = 0
    arrayDias.forEach((x) => {
      let diaTraducido = ''
      if (x == 'SUN') diaTraducido = traducir('Domingo')
      else if (x == 'MON') diaTraducido = traducir('Lunes')
      else if (x == 'TUE') diaTraducido = traducir('Martes')
      else if (x == 'WED') diaTraducido = traducir('Miercoles')
      else if (x == 'THU') diaTraducido = traducir('Jueves')
      else if (x == 'FRI') diaTraducido = traducir('Viernes')
      else if (x == 'SAT') diaTraducido = traducir('Sabado')

      if (diasEnEspañol == '') {
        diasEnEspañol = diaTraducido
      } else {
        if (arrayDias[i + 1] == undefined) {
          diasEnEspañol = diasEnEspañol + ' ' + traducir('y') + ' ' + diaTraducido
        } else {
          diasEnEspañol = diasEnEspañol + ', ' + diaTraducido
        }
      }
      i++
    })
    expresionDias = `${traducir('TodosLos')} ${diasEnEspañol}`
  }
  return expresionHoras + ' , ' + expresionDias
}

onMounted(async () => {
  refreshSource()
  refrescarCorreos()
})

const llenarArrayOpciones = (props) => {
  const arrayOpciones = []
  if (estaPermisoOtorgado(pCorreosAutomaticosAD.PermisoEditar)) {
    arrayOpciones.push({
      titulo: traducir('Editar'),
      descripcion: traducir('EditarInformacion'),
      icono: 'edit',
      color: 'warning',
      accion: () =>
        mostrarCrearEditarCorreoAutomatico({
          editar: true,
          correoAutomatico: props.row,
        }),
    })
  }
  if (estaPermisoOtorgado(pCorreosAutomaticosAD.PermisoCambiarEstado)) {
    arrayOpciones.push({
      titulo: traducir('Estado'),
      descripcion: props.row.activo ? traducir('DesactivarRegistro') : traducir('ActivarRegistro'),
      icono: props.row.activo ? 'block' : 'check',
      color: props.row.activo ? 'negative' : 'positive',
      accion: () => cambiarEstadoCorreoAutomatico(props.row.id, props.row.activo, props.row.nombre),
    })
  }
  return arrayOpciones
}
</script>
