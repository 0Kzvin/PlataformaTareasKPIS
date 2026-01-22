<!-- eslint-disable no-useless-escape -->
<template>
  <div>
    <slot name="on-top">
      <template v-if="selectorFechasRango">
        <div class="q-mb-lg row justify-start items-end" style="display: flex">
          <q-input filled v-model="rangoFechasLabel" readonly style="width: 250px" :label-slot="true"
            class="cursor-pointer bg-fondo2 text-textprimary" input-class="cursor-pointer text-textprimary">
            <q-popup-proxy ref="qDateProxy" transition-show="scale" transition-hide="scale" :offset="[-40, -20, 0, 0]">
              <div>
                <q-date v-model="rangoFechas" range class="text-primary-contrast">
                  <div class="row items-center justify-end">
                    <q-btn v-close-popup color="primary" flat rounded class="text-primary-contrast bg-primary">{{
                      traducir('Cerrar') }}</q-btn>
                  </div>
                </q-date>
              </div>
            </q-popup-proxy>
            <template v-slot:label>
              <span class="text-size-14 cursor-pointer text-textprimary">{{
                traducir('SeleccionarFechas')
              }}</span>
            </template>
            <template v-slot:prepend>
              <q-icon name="event" class="cursor-pointer text-textprimary"></q-icon>
            </template>
          </q-input>
          <slot name="on-top-right"></slot>
        </div>
      </template>

      <slot name="on-top-under"></slot>
    </slot>
    <q-table :grid="grid" :rows="filas" :dense="dense" :title="titulo" :filter="filtros" :loading="cargando"
      :columns="columnas" style="border-radius: 16px" flat :sort-method="customSort" :separator="tipoSeparador"
      :selection="modoSeleccion" :hide-header="ocultarHeader" :hide-bottom="ocultarFooter" :filter-method="metodoFiltro"
      card-class="bg-fondo3 text-textprimary" table-class="text-textprimary"
      table-header-class="bg-primary text-primary-contrast" :wrap-cells="limitarAnchoCeldas"
      v-model:selected="modeloSeleccion" :visible-columns="columnasVisibles" :no-data-label="traducir('TablaSinDatos')"
      :row-key="(row) => row[llaveFilaUnica]" :pagination="{ rowsPerPage: filasPorPagina }"
      :no-results-label="traducir('TablaSinResultadosFiltro')" @update:selected="(seleccion) => onSeleccion(seleccion)"
      :rows-per-page-options="[5, 7, 10, 15, 20, 25, 30, 50, 100]">
      <template v-slot:header="props">
        <q-tr :props="props">
          <q-th v-if="modoSeleccion != 'none'" :style="!cargando ? 'position: sticky' : ''"> </q-th>
          <q-th v-for="col in props.cols" :key="col.name" :props="props" :style="limitarAnchoCeldas
            ? 'max-width: 250px !important; min-width: 200px !important;'
            : '' + !cargando
              ? 'position: sticky'
              : ''
            ">
            <span class="text-size-12 q-pa-md">
              {{ col.label }}
            </span>
          </q-th>
        </q-tr>
      </template>

      <template v-slot:top-row="cols" v-if="propsFiltros.length !== 0">
        <q-tr class="fila-filtros">
          <q-th v-if="modoSeleccion != 'none'" :style="!cargando ? 'position: sticky' : ''"> </q-th>
          <q-th v-for="(col, index) in cols.cols" :key="index">
            <div v-for="(filtro, index) in propsFiltros" :key="index"
              :class="typeof filtros[filtro] === 'object' ? 'justify-center' : ''"
              :auto-width="typeof filtros[filtro] === 'object' ? true : false">
              <div v-if="col.name === filtro">
                <div v-if="typeof filtros[filtro] === 'string'">
                  <q-input dense filled color="grey-3" label-color="textsecondary" class="q-mb-sm" debounce="200"
                    :label="traducir('Filtrar')" v-model="filtros[filtro]">
                    <template #append>
                      <q-btn v-if="filtros[filtro].length > 0" class="cursor-pointer" @click="filtros[filtro] = ''"
                        size="xs" flat round>
                        <i class="pi pi-times-circle text-size-18"></i>
                      </q-btn>
                    </template>
                  </q-input>
                </div>
                <div v-if="typeof filtros[filtro] === 'object'">
                  <q-select dense filled style="min-width: 160px; margin-top: -8px" class="full-width"
                    label-color="textsecondary" input-class="text-textprimary" :label="traducir('Filtrar')"
                    dropdown-icon="expand_more" popup-content-class="bg-fondo3 text-textprimary"
                    v-model="filtros[filtro]" :options="opcionesFiltrosSelects[filtro]"
                    @update:model-value="(value) => onChangedSelector(value, filtro)">
                    <template #append>
                      <q-btn v-if="filtros[filtro]" class="cursor-pointer" @click="filtros[filtro] = null" size="xs"
                        flat round>
                        <i class="pi pi-times-circle text-size-18"></i>
                      </q-btn>
                    </template>
                  </q-select>
                </div>
                <div class="fit row wrap justify-center items-start content-start"
                  v-if="typeof filtros[filtro] === 'number'">
                  <q-input dense style="width: 125px" @clear="filtros[filtro] = 0" class="q-mb-sm text-textsecondary"
                    type="number" debounce="200" label-color="text-textsecondary" color="primary"
                    :label="traducir('Filtrar')" v-model.number="filtros[filtro]"
                    @update:model-value="(value) => prevenirBorrarInputNumber(value, filtro)">
                    <template v-slot:after>
                      <q-select dense class="q-mb-sm q-mt-sm" v-model="seleccionadoresFiltroNumeros[filtro]"
                        :options="filtroCantidades" />
                    </template>
                  </q-input>
                </div>
              </div>
            </div>
          </q-th>
        </q-tr>
      </template>

      <template v-slot:bottom-row="cols" v-if="propsUltimaFila.length !== 0">
        <q-tr>
          <q-th v-for="(col, index) in cols.cols" :key="index">
            <div v-for="(columna, index) in propsUltimaFila" :key="index"
              :class="typeof ultimaFila[columna] === 'string' ? 'justify-center' : ''"
              :auto-width="typeof ultimaFila[columna] === 'string' ? true : false">
              <div v-if="col.name === columna">
                <div v-if="typeof ultimaFila[columna] === 'string'">
                  <span class="text-textsecondary text-weight-bold">
                    {{ ultimaFila[columna] }}
                  </span>
                </div>
              </div>
            </div>
          </q-th>
        </q-tr>
      </template>

      <template v-slot:item="props">
        <slot name="item" :props="props"></slot>
      </template>

      <template v-slot:body="props">
        <q-tr :props="props">
          <q-td v-if="modoSeleccion != 'none'">
            <G3CheckBox v-model="props.selected" />
          </q-td>
          <q-td :props="props" v-for="col in props.cols" :key="col.name" class="text-center"
            :style="col.style ? col.style + (' ' + estiloColumnas) : estiloColumnas">
            <slot :name="`col-start-${col.name}`" :props="props"></slot>
            <span class="text-size-12" v-if="!col.seOculta">{{ col.value }}</span>
            <slot :name="`col-end-${col.name}`" :props="props"></slot>
          </q-td>
        </q-tr>
        <q-tr v-show="props.row.expandir" :props="props">
          <q-td colspan="100%">
            <slot name="expand-row-slot" :props="props" />
          </q-td>
        </q-tr>
      </template>

      <template v-slot:loading>
        <q-inner-loading showing class="q-pt-xl">
          <template v-slot:default>
            <BuscandoDataApi class="q-pt-xl" />
          </template>
        </q-inner-loading>
      </template>

      <template v-slot:no-data>
        <template v-if="!cargando">
          <no-data-api />
        </template>
        <template v-else>
          <div style="width: 200px; height: 300px"></div>
        </template>
      </template>
    </q-table>
  </div>
</template>

<script setup>
import { computed, reactive, ref, watch, inject } from 'vue'
import { date } from 'quasar'
import NoDataApi from './NoDataApi.vue'
import BuscandoDataApi from './BuscandoDataApi.vue'

const traducir = inject('traducir')

const props = defineProps({
  // * Establece el titulo de la table en la parte izquierda superior
  titulo: {
    type: String,
    default: '',
  },
  // * Array de objetos de columnas por parte de quasar framework
  columnas: {
    type: Array,
    default: () => [],
  },
  // * Se seleccionan todas las columnas visibles
  // * Por defecto muestra todas las columnas
  columnasVisibles: {
    type: Array,
    default: () => [],
  },
  // * Array de las filas por llenar.
  // * Las propiedades deben de tener el mismo nombre que la propiedad name de las columnas
  filas: {
    type: Array,
    default: () => [],
  },
  // * Objeto: Busca a partir de propiedades en los objetos ligadosa un v-model
  // ! Parametros necesarios: !
  // ? String -> buscador normal
  // ? Number -> buscador de cantidades
  // ? Objeto -> Seleccionador con dropdown
  // ?! Objeto requiere propiedades necesarias mencionadas abajo ?!
  /**
   * * seleccion: null <--- item seleccionado por defecto en el dropdown
   * * opciones: [ ] <--- array con las opciones por seleccionar
   */
  filtros: {
    type: Object,
    default: () => ({}),
  },
  ultimaFila: {
    type: Object,
    default: () => ({}),
  },
  // * Variable que determina si la tabla se encuentra cargando datos asincronos o no
  cargando: {
    type: Boolean,
    default: false,
  },
  // * Activa el selector de fechas por rango
  selectorFechasRango: {
    type: Boolean,
    default: false,
  },
  // * Decide si se muestra o no el boton para limpiar filtros
  // ! Elimina el div row de este boton por lo que tambien limpia espacio de arriba de la tabla
  // mostrarBotonLimpiarFiltros: {
  //   type: Boolean,
  //   default: true,
  // },
  tipoSeparador: {
    type: String,
    default: 'none',
  },
  // * Activa el modo grid de la tabla en lugar de tenerlo con filas normales
  grid: {
    type: Boolean,
    default: false,
  },
  // * Establece el identificador unico de una tabla, debe de ser completamente unico
  llaveFilaUnica: {
    type: String,
    default: 'id',
  },
  // * Habilita o deshabilita el modo de seleccion justo como la documentacion de Quasar
  // ! Es necesario establecer una llave de fila unica para utilizar esta opcion correctamente
  modoSeleccion: {
    type: String,
    default: 'none',
  },
  // * v-model que invoca el cambio de seleccion cuando este se habilita
  // ! Funciona igual que en quasar, regresando un array como en la documentacion de quasar sin importar single o multiple
  modeloSeleccion: {
    type: Array,
    default: () => [],
  },
  dense: {
    type: Boolean,
    default: false,
  },
  ocultarHeader: {
    type: Boolean,
    default: false,
  },
  ocultarFooter: {
    type: Boolean,
    default: false,
  },
  filasPorPagina: {
    type: Number,
    default: 5,
  },
  estiloTabla: {},
  estiloColumnas: {
    type: String,
    default: '',
  },
  limitarAnchoCeldas: {
    type: Boolean,
    default: false,
  },
})

const emit = defineEmits(['cambioFechas', 'update:modeloSeleccion'])

const rangoFechas = ref({
  from: '',
  to: '',
})
const rangoFechasLabel = ref('')
const filtroCantidades = [
  {
    label: '=',
    value: '=',
  },
  {
    label: '>=',
    value: '>=',
  },
  {
    label: '<=',
    value: '<=',
  },
]

// * Busca los filtros y rellena un objeto reactivo con propiedades de los filtros
// eslint-disable-next-line vue/no-dupe-keys
const filtros = reactive({})

const seleccionadoresFiltroNumeros = reactive({})

const opcionesFiltrosSelects = reactive({})

for (let filtro in props.filtros) {
  if (typeof props.filtros[filtro] === 'number') {
    seleccionadoresFiltroNumeros[filtro] = { label: '=', value: '=' }
  }
  if (typeof props.filtros[filtro] === 'object') {
    if (Array.isArray(props.filtros[filtro])) {
      filtros[filtro] = ''
    } else {
      filtros[filtro] = props.filtros[filtro].seleccion
      opcionesFiltrosSelects[filtro] = props.filtros[filtro].opciones
    }
  } else {
    filtros[filtro] = props.filtros[filtro]
  }
}

// eslint-disable-next-line vue/no-dupe-keys
const ultimaFila = reactive({})

for (const key in props.ultimaFila) {
  ultimaFila[key] = props.ultimaFila[key]
}

const filas = computed(() => {
  return props.filas
})

const tipoSeparador = computed(() => {
  return props.tipoSeparador
})

const grid = computed(() => {
  return props.grid
})

const columnas = computed(() => {
  props.columnas.forEach((e) => {
    // ? e.format formatea el objeto columna, el cual nos trae el valor, y toda la fila, se setea a partir del elemento esperado,
    e.format = (val, row) => {
      Object.keys(row).forEach((prop) => {
        if (prop.includes('_unformatted')) return
        switch (typeof row[prop]) {
          case 'string':
            if (row[prop].includes('T') && !row[prop].includes(' ')) {
              const dateRegexp =
                // eslint-disable-next-line no-useless-escape
                /[0-9]{4}-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}:[0-9]{2}(\.[0-9]+)?([zZ]|([\+-])([01]\d|2[0-3]):?([0-5]\d)?)?/
              const matches = row[prop].match(dateRegexp)
              if (matches != null) {
                row[`${prop}_unformatted`] = row[prop]
                row[prop] = date.formatDate(row[prop], 'DD/MM/YY hh:mm:ss A')
              }
            }
            break
          case 'number':
            break
          default:
            break
        }
      })

      return val
    }

    //Para filtrar el objeto columna
    e.sortable = true
  })
  return props.columnas
})

const cargando = computed(() => {
  return props.cargando
})

const ocultarHeader = computed(() => {
  return props.ocultarHeader
})

const ocultarFooter = computed(() => {
  return props.ocultarFooter
})

const llaveFilaUnica = computed(() => {
  return props.llaveFilaUnica
})

const modoSeleccion = computed(() => {
  return props.modoSeleccion
})

const estiloColumnas = computed(() => {
  return props.estiloColumnas
})

const limitarAnchoCeldas = computed(() => {
  return props.limitarAnchoCeldas
})

const dense = computed(() => {
  return props.dense
})

const modeloSeleccion = props.modeloSeleccion

const propsFiltros = computed(() => {
  return Object.keys(props.filtros)
})

const propsUltimaFila = computed(() => {
  return Object.keys(props.ultimaFila)
})

const selectorFechasRango = computed(() => {
  return props.selectorFechasRango
})

const columnasVisibles = computed(() => {
  if (props.columnasVisibles.length === 0) {
    const nombresColumnasVisibles = new Array(props.columnas.length)
    for (var i = 0; i < nombresColumnasVisibles.length; i++) {
      nombresColumnasVisibles[i] = props.columnas[i].name
    }
    return nombresColumnasVisibles
  }
  return props.columnasVisibles
})
const filasPorPagina = computed(() => {
  return props.filasPorPagina
})

const titulo = computed(() => {
  return props.titulo
})

// ? Emite los cambios realizados por el selector de rango por fechas directo hacia el template
watch(rangoFechas, (value) => {
  if (!value) return

  if (typeof value === 'object') {
    if (!value.from) {
      rangoFechasLabel.value = ''
      return
    }
    if (!value.to) {
      rangoFechasLabel.value = ''
      return
    }
    emit('cambioFechas', value)
    rangoFechasLabel.value = `${value.from} ~ ${value.to}`
  } else if (typeof value === 'string') {
    rangoFechasLabel.value = value
    const rangoNuevo = {
      from: value,
      to: value,
    }
    emit('cambioFechas', rangoNuevo)
  }
})

const metodoFiltro = (rows, terms) => {
  if (cargando.value) return rows

  let filasEncontradas = rows

  const propiedadesFiltros = Object.keys(terms)

  for (const filtroPropiedad of propiedadesFiltros) {
    const terminoBuscado = terms[filtroPropiedad]

    if (terminoBuscado && typeof terminoBuscado === 'string') {
      const termino = terminoBuscado.toUpperCase()

      filasEncontradas = filasEncontradas.filter((e) => {
        const filtro = props.filtros[filtroPropiedad]

        if (Array.isArray(filtro)) {
          return filtro.some(({ propiedad }) => {
            const valor = e[propiedad]
            return (
              valor !== undefined &&
              valor !== null &&
              valor.toString().toUpperCase().includes(termino)
            )
          })
        }

        const valor = e[filtroPropiedad]
        return (
          valor !== undefined && valor !== null && valor.toString().toUpperCase().includes(termino)
        )
      })
    }
    if (terminoBuscado && typeof terminoBuscado === 'number') {
      const opcionNumero = seleccionadoresFiltroNumeros[filtroPropiedad].value
      if (opcionNumero == '=') {
        filasEncontradas = filasEncontradas.filter((e) => e[filtroPropiedad] == terminoBuscado)
      } else if (opcionNumero == '>=') {
        filasEncontradas = filasEncontradas.filter((e) => e[filtroPropiedad] >= terminoBuscado)
      } else if (opcionNumero == '<=') {
        filasEncontradas = filasEncontradas.filter((e) => e[filtroPropiedad] <= terminoBuscado)
      }
    }
    if (terminoBuscado && typeof terminoBuscado === 'object' && terminoBuscado !== null) {
      filasEncontradas = filasEncontradas.filter((e) => e[filtroPropiedad] === terminoBuscado.value)
    }
  }
  return filasEncontradas
}
const customSort = (rows, sortBy, descending) => {
  const data = [...rows]
  if (sortBy) {
    data.sort((a, b) => {
      const x = descending ? b : a
      const y = descending ? a : b
      if (x[`${sortBy}_unformatted`]) {
        if (
          typeof x[`${sortBy}_unformatted`] !== 'boolean' &&
          typeof y[`${sortBy}_unformatted`] !== 'boolean'
        ) {
          if (
            typeof x[`${sortBy}_unformatted`] === 'string' &&
            typeof y[`${sortBy}_unformatted`] === 'string'
          ) {
            if (
              x[`${sortBy}_unformatted`].includes('T') &&
              !x[`${sortBy}_unformatted`].includes(' ')
            ) {
              const dateRegexp =
                // eslint-disable-next-line no-useless-escape
                /[0-9]{4}-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}:[0-9]{2}(\.[0-9]+)?([zZ]|([\+-])([01]\d|2[0-3]):?([0-5]\d)?)?/
              let matches = x[`${sortBy}_unformatted`].match(dateRegexp)
              if (matches) {
                return descending
                  ? -1 * new Date(x[`${sortBy}_unformatted`]) -
                  new Date(y[[`${sortBy}_unformatted`]])
                  : 1 * new Date(x[`${sortBy}_unformatted`]) -
                  new Date(y[[`${sortBy}_unformatted`]])
              }
            }
          }
        }
      }

      if (typeof x[sortBy] === 'string' && typeof y[sortBy] === 'string') {
        // string sort
        return x[sortBy] > y[sortBy] ? 1 : x[sortBy] < y[sortBy] ? -1 : 0
      } else if (typeof x[sortBy] === 'boolean' && typeof y[sortBy] === 'boolean') {
        return x[sortBy] - y[sortBy]
      } else {
        // numeric sort
        return parseFloat(x[sortBy]) - parseFloat(y[sortBy])
      }
    })
  }

  return data
}
const prevenirBorrarInputNumber = (value, nombreFiltro) => {
  if (!value) {
    filtros[nombreFiltro] = 0
  }
}

const limpiarFiltros = () => {
  for (let filtro in props.filtros) {
    if (typeof props.filtros[filtro] === 'number') {
      filtros[filtro] = 0
    } else if (typeof props.filtros[filtro] === 'string') {
      filtros[filtro] = ''
    } else if (typeof props.filtros[filtro] === 'object') {
      if (Array.isArray(props.filtros[filtro])) {
        filtros[filtro] = ''
      } else {
        filtros[filtro] = null
      }
    }
  }
}

defineExpose({ limpiarFiltros })

const onChangedSelector = (value, nombreFiltro) => {
  emit('onChangedQSelect', { nombreFiltro, value })
}

const onSeleccion = (seleccion) => {
  emit('update:modeloSeleccion', seleccion)
}
</script>

<style scoped>
/* ===============================
   TABLA CON HEADERS STICKY
   =============================== */

.sticky-header-table {
  max-height: 700px;
  min-height: 400px;
}

.sticky-header-table .q-table__top,
.sticky-header-table .q-table__bottom,
.sticky-header-table thead tr th {
  z-index: 1;
}

.sticky-header-table thead tr:first-child th {
  top: 0;
}

.sticky-header-table.q-table--loading thead tr:last-child th {
  top: 48px;
}

:deep(.q-table__middle) {
  overflow-x: auto;

  scrollbar-width: thin;
  scrollbar-color: var(--q-primary, #e41321) var(--q-fondo2, #f4f4f4);
}
</style>
