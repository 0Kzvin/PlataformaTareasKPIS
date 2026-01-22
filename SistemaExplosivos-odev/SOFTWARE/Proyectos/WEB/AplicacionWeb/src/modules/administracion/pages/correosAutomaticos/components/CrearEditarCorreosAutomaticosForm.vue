<!-- eslint-disable vue/no-v-text-v-html-on-component -->
<template>
  <q-card class="q-pa-md" style="max-width: 800px; max-height: 850px">
    <q-card-section>
      <span class="text-size-21 text-weight-bold">
        <template v-if="editar">{{ traducir('EditarCorreoAutomatico') }}</template>
        <template v-else>{{ traducir('CrearCorreoAutomatico') }}</template>
      </span>
    </q-card-section>

    <q-form @submit="enviarFormulario">
      <q-card-section class="row q-col-gutter-x-xl q-col-gutter-y-lg">
        <div class="col-6">
          <q-input
            v-model="nombre"
            dense
            clearable
            class="text-size-18"
            debounce="200"
            color="primary"
            :label="traducir('Nombre') + ' *'"
            :rules="[
              (val) => ((val?.trim() ?? false) && val.length > 0) || traducir('CompletarCampo'),
            ]"
          ></q-input>
        </div>
        <div class="col-6">
          <q-input
            v-model="descripcion"
            dense
            clearable
            class="text-size-18"
            debounce="200"
            color="primary"
            :label="traducir('Descripcion') + ' *'"
            :rules="[
              (val) => ((val?.trim() ?? false) && val.length > 0) || traducir('CompletarCampo'),
            ]"
          ></q-input>
        </div>
        <div class="col-6">
          <q-select
            filled
            class="q-mt-sm"
            input-debounce="0"
            bottom-slots
            clearable
            readonly
            v-model="nombreModulo"
            :options="opcionesModulos"
            option-value="nombre"
            option-label="nombreNormalizado"
            :label="traducir('Modulo')"
          >
            <template v-slot:no-option>
              <q-item>
                <q-item-section class="text-italic text-grey">
                  {{ traducir('SinOpciones') }}
                </q-item-section>
              </q-item>
            </template>
          </q-select>
        </div>
        <div class="col-6">
          <q-select
            filled
            class="q-mt-sm"
            input-debounce="0"
            bottom-slots
            clearable
            multiple
            v-model="destinatarios"
            :options="opcionesDestinatarios"
            use-chips
            option-value="correo"
            option-label="correo"
            :label="traducir('Destinatarios')"
          >
            <template v-slot:no-option>
              <q-item>
                <q-item-section class="text-italic text-grey">
                  {{ traducir('SinOpciones') }}
                </q-item-section>
              </q-item>
            </template>
          </q-select>
        </div>
        <div class="col-12">
          <span> {{ traducir('Frecuencia') }} - {{ traducir('Tiempos') }}</span>
        </div>

        <div class="row full-width justify-around">
          <div class="col-4 text-right q-pr-lg">
            {{ traducir('EspecifiqueLasHoras') }}
          </div>
          <div class="col-4">
            <q-select
              filled
              v-model="horasSeleccionada"
              :options="opcionesHoras"
              :label="traducir('Horas')"
              multiple
              emit-value
              map-options
            >
              <template v-slot:option="{ itemProps, opt, selected, toggleOption }">
                <q-item v-bind="itemProps">
                  <q-item-section>
                    <q-item-label v-html="opt.label" />
                  </q-item-section>
                  <q-item-section side>
                    <q-checkbox :model-value="selected" @update:model-value="toggleOption(opt)" />
                  </q-item-section>
                </q-item>
              </template>
            </q-select>
          </div>
          <div class="col-4 q-pl-lg">
            <q-option-group
              :options="opcionesHoraFrecuencia"
              type="radio"
              v-model="horaFrecuenciaSeleccionada"
            />
          </div>
        </div>
        <div class="row full-width justify-around">
          <div class="col-4 text-right q-pr-lg">
            {{ traducir('EspecifiqueLosDias') }}
          </div>
          <div class="col-4">
            <q-select
              :disable="diasFrecuenciaSeleccionada == 1"
              filled
              v-model="diasSeleccionados"
              :options="opcionesDias"
              :label="traducir('DiasSeleccionados')"
              multiple
              emit-value
              map-options
              use-chips
            >
              <template v-slot:option="{ itemProps, opt, selected, toggleOption }">
                <q-item v-bind="itemProps">
                  <q-item-section>
                    <q-item-label v-html="opt.label" />
                  </q-item-section>
                  <q-item-section side>
                    <q-checkbox :model-value="selected" @update:model-value="toggleOption(opt)" />
                  </q-item-section>
                </q-item>
              </template>
            </q-select>
          </div>
          <div class="col-4 q-pl-lg">
            <q-option-group
              :options="opcionesDiasFrecuencia"
              type="radio"
              v-model="diasFrecuenciaSeleccionada"
            />
          </div>
        </div>
      </q-card-section>
      <q-card-actions class="justify-end q-pt-lg">
        <q-btn flat color="primary" @click="cancelarForm()">{{ traducir('Cancelar') }}</q-btn>
        <q-btn flat color="primary" class="q-ml-md" type="submit">{{
          traducir('EnviarCambios')
        }}</q-btn>
      </q-card-actions>
    </q-form>
  </q-card>
</template>

<script setup>
import { onMounted, ref, reactive, toRefs, inject } from 'vue'
import { correosAutomaticosAdmin, modulosGFuelAdmin } from 'src/api/moduloAdministracion'
import * as quasarUtils from 'src/utils/quasar-utils.js'

const traducir = inject('traducir', (key) => key)

const props = defineProps({
  propsCustomForm: {
    type: Object,
    default: () => {
      return {
        editar: false,
      }
    },
  },
})

const { propsCustomForm } = toRefs(props)

const { refrescarCorreosAutomaticos, editar, correoAutomatico } = propsCustomForm.value

const emits = defineEmits(['onDialogCancel', 'onDialogHide', 'onDialogOK'])

const opcionesModulos = ref([])
const cargandoModulos = ref(false)

const cargandoDestinatarios = ref(false)
const opcionesDestinatarios = ref([])

const opcionesHoras = ref([])
const horasSeleccionada = ref(null)

const opcionesHoraFrecuencia = reactive([
  {
    label: traducir('HoraFija'),
    value: 1,
  },
  {
    label: traducir('CadaCuantasHoras'),
    value: 2,
  },
])

const horaFrecuenciaSeleccionada = ref(1)

const opcionesDias = ref([
  { label: traducir('Lunes'), value: 'MON' },
  { label: traducir('Martes'), value: 'TUE' },
  { label: traducir('Miercoles'), value: 'WED' },
  { label: traducir('Jueves'), value: 'THU' },
  { label: traducir('Viernes'), value: 'FRI' },
  { label: traducir('Sabado'), value: 'SAT' },
  { label: traducir('Domingo'), value: 'SUN' },
])
const diasSeleccionados = ref([])

const opcionesDiasFrecuencia = reactive([
  {
    label: traducir('TodosLosDias'),
    value: 1,
  },
  {
    label: traducir('ElegirDias'),
    value: 2,
  },
])
const diasFrecuenciaSeleccionada = ref(1)

const nombre = editar ? ref(correoAutomatico.nombre) : ref('')
const nombreModulo = editar ? ref(correoAutomatico.nombreModulo) : ref('')
const descripcion = editar ? ref(correoAutomatico.descripcion) : ref('')
const destinatarios = editar
  ? ref(correoAutomatico.listaDestinatarios ? correoAutomatico.listaDestinatarios : [])
  : ref([])

const refrescarModulos = async () => {
  cargandoModulos.value = true
  const resp = await modulosGFuelAdmin.listar()
  cargandoModulos.value = false
  if (!resp.exito) {
    opcionesModulos.value = []
    return
  }
  opcionesModulos.value = resp.payload
}

const refrescarDestinatarios = async () => {
  cargandoDestinatarios.value = true
  const resp = await correosAutomaticosAdmin.listarDestinatarios()
  cargandoDestinatarios.value = false

  if (!resp.exito) {
    opcionesDestinatarios.value = []
    return
  }

  var destinatariosFormateo = []
  resp.payload.forEach((e) => {
    if (e.email) {
      destinatariosFormateo.push(e.email)
    }
  })

  opcionesDestinatarios.value = destinatariosFormateo
}

const llenarArrayHoras = () => {
  for (let index = 0; index < 24; index++) {
    opcionesHoras.value.push({
      label: `${index + 1}`,
      value: index + 1,
    })
  }
}

const formatearInputCron = () => {
  //Formatear cron dependiendo que tipo sea
  var cronFormateado = ''
  var horaFormateada = ''
  var diasFormateados = ''
  if (horaFrecuenciaSeleccionada.value == 2) {
    horaFormateada = `0/${horasSeleccionada.value ? horasSeleccionada.value.join(',') : '12'}`
  } else if (horaFrecuenciaSeleccionada.value == 1) {
    horaFormateada = `${horasSeleccionada.value ? horasSeleccionada.value.join(',') : '12'}`
  }

  if (diasFrecuenciaSeleccionada.value == 1) {
    diasFormateados = '*'
  } else if (diasFrecuenciaSeleccionada.value == 2) {
    var diasConcatenados = ''

    for (const item in diasSeleccionados.value) {
      if (diasConcatenados == '') {
        diasConcatenados = diasSeleccionados.value[item]
      } else {
        diasConcatenados = diasConcatenados + ',' + diasSeleccionados.value[item]
      }
    }
    diasFormateados = `${diasConcatenados ? diasConcatenados : '1'}`
  }

  cronFormateado = `0 0 ${horaFormateada} ? * ${diasFormateados} *`
  return cronFormateado
}

onMounted(() => {
  refrescarModulos()
  refrescarDestinatarios()
  llenarArrayHoras()
})

const cancelarForm = () => {
  emits('onDialogCancel')
}

const enviarFormulario = async () => {
  var nuevoCron = ''
  try {
    nuevoCron = formatearInputCron()
  } catch {
    quasarUtils.aviso({
      error: true,
      mensaje: traducir('ErrorFormatearFrecuenciaCorreo'),
    })
    return
  }
  if (!horasSeleccionada.value) {
    quasarUtils.aviso({
      error: true,
      mensaje: traducir('SeleccionarHoraAccionAutomatica'),
    })
    return
  }
  if (diasSeleccionados.value.length == 0 && diasFrecuenciaSeleccionada.value == 2) {
    quasarUtils.aviso({
      error: true,
      mensaje: traducir('SeleccionarDiasAccionAutomatica'),
    })
    return
  }
  const nombreTrim = nombre.value.trim()
  if (!nombreTrim) {
    quasarUtils.aviso({
      error: true,
      mensaje: traducir('NombreAccionAutomatica'),
    })
    return
  }
  const decision = await quasarUtils.decision({
    mensaje: editar ? traducir('EditarOperadorPregunta') : traducir('CrearOperadorPregunta'),
  })
  if (decision) {
    const registrarEditarModel = {
      Id: correoAutomatico.id,
      IdModulo: correoAutomatico.idModulo,
      Nombre: nombreTrim,
      NombreModulo: nombreModulo.value,
      NombreClave: correoAutomatico.nombreClave,
      Descripcion: descripcion.value,
      ExpresionCron: nuevoCron,
      ListaDestinatarios: destinatarios.value,
    }
    var resp
    quasarUtils.cargandoSimple()
    if (editar) {
      resp = await correosAutomaticosAdmin.actualizar(registrarEditarModel)
    } else {
      resp = await correosAutomaticosAdmin.crear(registrarEditarModel)
    }
    // console.log(resp);
    quasarUtils.ocultarCargandoSimple()
    if (!resp.exito) {
      quasarUtils.aviso({
        error: true,
        mensaje: resp.payload.errores[0],
      })
      return
    }
    refrescarCorreosAutomaticos()
    await quasarUtils.aviso({
      exito: true,
      mensaje: editar
        ? traducir('RegistroEditadoConExitoMensaje')
        : traducir('RegistroCreadoConExitoMensaje'),
    })
    emits('onDialogCancel')
  }
}
</script>

<style></style>
