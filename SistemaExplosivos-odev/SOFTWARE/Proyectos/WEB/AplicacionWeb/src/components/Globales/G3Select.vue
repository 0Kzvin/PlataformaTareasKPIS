<template>
  <div v-if="existe" :class="clase">
    <q-select
      dense
      filled
      :use-input="!opcionSeleccionada"
      clearable
      map-options
      :label-slot="true"
      class="cursor-pointer"
      bg-color="primarylight"
      :disable="deshabilitar"
      input-class="text-textsecondary"
      label-color="textsecondary"
      v-model="opcionSeleccionada"
      :options="opcionesFiltradas"
      :option-label="nombrePropiedadMostrada"
      :option-value="nombrePropiedadEmitida"
      :emit-value="nombrePropiedadEmitida != ''"
      :loading="cargando"
      @filter="filtrarOpciones"
      :rules="[(val) => (llenadoObligatorio ? !!val : true) || traducir('CompletarCampo')]"
    >
      <template v-slot:prepend>
        <q-icon :name="icono" class="q-mr-sm text-textsecondary" size="xs" />
      </template>
      <template v-slot:label>
        {{ traducir(tituloSelect) }} {{ llenadoObligatorio ? '*' : '' }}
      </template>
      <template v-if="mostrarPropiedadCustomSeleccionada" v-slot:selected-item="{ opt }">
        <slot :name="'selectedOption'" :props="opt"></slot>
      </template>
      <template v-slot:option="scope">
        <q-item v-bind="scope.itemProps">
          <slot :name="'firstSeccion'" :props="scope.opt"></slot>

          <q-item-section>
            <span v-if="nombrePropiedadMostrada != '' && nombrePropiedadSubtitulo == ''">
              {{ scope.opt[nombrePropiedadMostrada] }}
            </span>
            <div v-if="nombrePropiedadMostrada != '' && nombrePropiedadSubtitulo != ''">
              <q-item-label>
                {{
                  (nombreTituloOpcion != '' ? traducir(nombreTituloOpcion) + ': ' : '') +
                  scope.opt[nombrePropiedadMostrada]
                }}</q-item-label
              >
              <q-item-label caption>
                {{
                  (nombreSubtituloOpcion != '' ? traducir(nombreSubtituloOpcion) + ': ' : '') +
                  scope.opt[nombrePropiedadSubtitulo]
                }}</q-item-label
              >
            </div>
            <slot v-else :name="'middleSeccion'" :props="scope.opt"></slot>
          </q-item-section>
          <slot :name="'lastSeccion'" :props="scope.opt"></slot>
        </q-item>
      </template>
      <template v-slot:no-option>
        <q-item>
          <q-item-section class="text-grey"> {{ traducir('SinOpciones') }}</q-item-section>
        </q-item>
      </template>
      <template v-slot:item="props">
        <slot name="item" :props="props"></slot>
      </template>
      <template v-slot:after>
        <q-btn
          v-if="dialogoCrear != undefined"
          style="width: 30px; height: 30px"
          round
          size="sm"
          color="positive"
          icon="add"
          @click="abrirDialogoCrear"
        >
          <q-tooltip v-if="tituloToolTip != ''" class="text-size-14">
            {{ traducir(tituloToolTip) }}
          </q-tooltip>
        </q-btn>
      </template>
    </q-select>
  </div>
</template>

<script setup>
import { ref, toRefs, watch, onMounted, inject } from 'vue'
import CustomFormDialog from 'src/components/Globales/Dialogs/CustomFormDialog.vue'
import { useQuasar } from 'quasar'
const traducir = inject('traducir')

const emit = defineEmits(['update:modelValue'])
const $q = useQuasar()

// EN EL ARCHIVO .VUE DONDE SE USA ESTE COMPONENTE, DEBE IMPORTARSE EL ARCHIVO .JS QUE CONTIENE LA FUNCION DE LISTAR.

const props = defineProps({
  //NECESARIAS
  valorInicial: {
    //El valor inicial que mostrará cuando se use en un diálogo de editar
    type: String,
    default: '',
  },
  nombrePropiedadMostrada: {
    type: String,
    default: '', //Propiedad cuyo valor se mostrará en el select
  },
  obtenerInfoApi: {
    // Es la funcion que hace la peticion a la api para obtener la lista de opciones
    type: Function,
  },

  //NECESARIAS SI SE QUIERE USAR EL BTN DE CREAR
  dialogoCrear: {
    type: Function,
  },

  tituloSelect: {
    type: String,
    default: 'Título Select',
  },

  //OPCIONALES
  clase: {
    type: String, // Tooltip del boton de crear
    default: 'col-6',
  },
  tituloToolTip: {
    type: String, // Tooltip del boton de crear
    default: '',
  },
  nombrePropiedadEmitida: {
    type: String,
    default: '', //Propiedad cuyo valor devolverá el componente. Si no es especificada, se mandará el objeto entero de vuelta.
  },
  llenadoObligatorio: {
    type: Boolean,
    default: false,
  },
  deshabilitar: {
    type: Boolean,
    default: false, //Esta propiedad es para des/habilitar el select en caso de ser requerido
  },
  filtrosFuncion: {
    type: Object,
    // eslint-disable-next-line vue/require-valid-default-prop
    default: {}, //Por aqui se reciben todos los filtros que se van a aplicar en el llamado a la api
  },
  existe: {
    // Este booleano se usar para quitar el select cuando el cliente no necesite llenar esa información
    type: Boolean,
    default: true,
  },
  nombrePropiedadSubtitulo: {
    //Propiedad que se muestra como subtitulo en cada opción.
    type: String,
    default: '',
  },
  nombreSubtituloOpcion: {
    // Aclara al usuario cuál propiedad se le está mostrando en el subtitulo de cada opcióon del select
    type: String,
    default: '',
  },
  nombreTituloOpcion: {
    // Aclara al usuario cuál propiedad se le está mostrando en el titulo de cada opcióon del select
    type: String,
    default: '',
  },
  propiedadesAFiltrar: {
    // Aclara al usuario cuál propiedad se le está mostrando en el titulo de cada opcióon del select
    type: Array,
    // eslint-disable-next-line vue/require-valid-default-prop
    default: [],
  },
  mostrarPropiedadCustomSeleccionada: {
    // Esta propiedad cambia el valor mostrado selecciodo y en su lugar plasma el componente deseado por medio de un V-Slot
    type: Boolean,
    default: false,
  },
  opcionesSeleccionadasFiltrar: {
    // Si el array no viene vacio, se retiraran esas opciones del array original para que no se puedan seleccionar
    type: Array,
    // eslint-disable-next-line vue/require-valid-default-prop
    default: [],
  },
  icono: {
    // Si el array no viene vacio, se retiraran esas opciones del array original para que no se puedan seleccionar
    type: String,
    default: '',
  },
  // modelValue: {
  //   type: String,
  //   default: "",
  // },
})

const {
  tituloSelect,
  tituloToolTip,
  nombrePropiedadMostrada,
  nombrePropiedadEmitida,
  dialogoCrear,
  valorInicial,
  llenadoObligatorio,
  obtenerInfoApi,
  deshabilitar,
  filtrosFuncion,
  existe,
  nombrePropiedadSubtitulo,
  nombreSubtituloOpcion,
  nombreTituloOpcion,
  clase,
  propiedadesAFiltrar,
  mostrarPropiedadCustomSeleccionada,
  opcionesSeleccionadasFiltrar,
  icono,
  // modelValue,
} = toRefs(props)

// console.log('API: ', obtenerInfoApi);

const arrayOpciones = ref([])
const cargando = ref(true)
var filtrosAnteriores = filtrosFuncion.value
var opcionSeleccionadoAnterior = valorInicial.value
// if(!obtenerInfoApi.value){
//   console.log("No has importado el archivo .js donde se encuentra la funci[on de listar.");
// }
// console.log(opcionesSeleccionadasFiltrar.value);
// const opcionSeleccionada = ref(valorInicial.value );

if (!existe.value) emit('update:modelValue', '')
const opcionSeleccionada = ref(valorInicial.value)
const opcionesFiltradas = ref(arrayOpciones.value)
const objetoEntero = ref({})
var nombreP = nombrePropiedadMostrada.value
const opcionesQuitar = ref(opcionesSeleccionadasFiltrar.value)
const quitarOpciones = ref(false)

onMounted(async () => {
  await refrescarArray(filtrosFuncion.value)
})

const refrescarArray = async (filtros) => {
  // console.log('obtenerInfoApi: ', obtenerInfoApi.value);
  if (!obtenerInfoApi.value) return
  // console.log('llego aqui');
  cargando.value = true
  var resp = await obtenerInfoApi.value(filtros)
  // console.log('Resp: ', resp);
  if (!resp.exito) {
    arrayOpciones.value = []
    return
  }
  arrayOpciones.value = resp.payload
  cargando.value = false
  // console.log('Api: ',arrayOpciones.value);
}

watch(filtrosFuncion, async (filtros) => {
  if (JSON.stringify(filtrosAnteriores) == JSON.stringify(filtros)) return
  await refrescarArray(filtros)
  filtrosAnteriores = filtros
})

watch(props, (nuevoValor) => {
  if (nuevoValor.valorInicial == opcionSeleccionadoAnterior) return
  opcionSeleccionada.value = nuevoValor.valorInicial
  opcionSeleccionadoAnterior = nuevoValor.valorInicial
})

watch(arrayOpciones, (valor) => {
  if (valor.length <= 0) opcionSeleccionada.value = null
  if (deshabilitar.value) opcionSeleccionada.value = null
})

watch(opcionesQuitar.value, (val) => {
  if (val.length > 0) {
    quitarOpciones.value = true
    filtrarOpciones(val)
    quitarOpciones.value = false
  }
})

const abrirDialogoCrear = async () => {
  let editar = false
  let fila = []
  $q.dialog({
    component: CustomFormDialog,
    componentProps: {
      formularioComponent: dialogoCrear,
      formularioComponentProps: {
        fila,
        editar,
        //Al final del dialogo crearEditar, se debe escribir un if para que esta funcion se ejecute.
        enviarNuevoValor: seCreoRegistro,
      },
    },
  })
}

const filtrarOpciones = (val, update) => {
  if (opcionesQuitar.value.length > 0 && quitarOpciones.value) {
    opcionesFiltradas.value = arrayOpciones.value.filter(
      (item2) => !opcionesQuitar.value.some((item1) => item1[nombreP] === item2[nombreP]),
    )
    return
  }
  if (val === '' && opcionesQuitar.value.length <= 0) {
    update(() => {
      opcionesFiltradas.value = arrayOpciones.value
    })
    return
  }

  update(() => {
    const needle = val.toLowerCase()
    opcionesFiltradas.value = arrayOpciones.value.filter((option) => {
      if (propiedadesAFiltrar.value.length > 0) {
        var resp = false
        for (const key in propiedadesAFiltrar.value) {
          var xd = propiedadesAFiltrar.value[key]
          var result = option[xd].toLowerCase().indexOf(needle) > -1
          if (result) {
            resp = result
            break
          }
        }
        return resp
      } else return option[nombreP].toLowerCase().indexOf(needle) > -1
    })
    if (opcionesQuitar.value.length > 0) {
      opcionesFiltradas.value = arrayOpciones.value.filter(
        (item2) => !opcionesQuitar.value.some((item1) => item1[nombreP] === item2[nombreP]),
      )
    }
  })
}

watch(opcionSeleccionada, () => {
  if (Object.keys(objetoEntero.value).length !== 0) {
    let valorDevuelto =
      nombrePropiedadEmitida.value != ''
        ? opcionSeleccionada.value[nombrePropiedadEmitida.value]
        : opcionSeleccionada.value
    // console.log('IF', valorDevuelto);
    emit('update:modelValue', valorDevuelto)
    objetoEntero.value = {}
  } else {
    emit('update:modelValue', opcionSeleccionada.value)
  }
  // console.log('Opciones: ', opcionesQuitar.value);
})

// Vacía el array de opciones antes de llenarlo con la lista actualizada de opciones.
//Luego, como valor seleccionado, toma el último registro creado.
const seCreoRegistro = async () => {
  arrayOpciones.value = []
  await refrescarArray()
  objetoEntero.value = arrayOpciones.value.slice(-1)
  opcionSeleccionada.value = objetoEntero.value[0]
}
</script>
