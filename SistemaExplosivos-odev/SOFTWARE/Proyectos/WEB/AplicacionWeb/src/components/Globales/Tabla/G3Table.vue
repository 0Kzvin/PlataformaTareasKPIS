<template>
  <q-table
    v-if="primeraConfiguracionRealizada"
    ref="G3AdvancedTable"
    class="g3TableStyle"
    :class="pantallaCompleta ? '' : 'g3TableSizes'"
    :title="tituloTabla"
    :columns="columnas"
    :rows="datosTabla"
    dense
    :filter="filtros"
    :filter-method="metodoParaFiltrados"
    :visible-columns="columnasMapeadas.columnasVisibles"
    :separator="separadorTabla"
    :selection="modoSeleccion"
    :selected="informacionSeleccionada"
    :row-key="(row) => row[llaveFilaUnica]"
    @update:selected="(seleccion) => actualizarSeleccion(seleccion)"
    v-model:expanded="arrayExpandido"
    virtual-scroll
    virtual-scroll-slice-ratio-before="2"
    virtual-scroll-slice-ratio-after="2"
    :pagination="paginacionTabla"
    :rows-per-page-options="filasPorPaginas"
    :loading="cargando"
    :table-style="!existenValoresTablaReal ? 'max-height: 10px !important' : ''"
  >
    <template #top="props">
      <div
        class="full-width column no-wrap justify-center content-center q-gutter-xs q-mb-xs"
        v-if="existenValoresTablaReal"
      >
        <div
          class="full-width row wrap items-center content-center q-gutter-xs"
          :class="`justify-${alineacionTitulo}`"
          v-if="!ocultarTituloTabla"
        >
          <span class="text-h5">{{ traducir(tituloTabla) }}</span>
          <q-btn flat round class="cursor-pointer" @click="establecerPantallaCompleta(props)">
            <template #default>
              <q-icon
                :name="props.inFullscreen ? 'fa-solid fa-compress' : 'fa-solid fa-expand'"
                size="xs"
                class="text-textprimary"
              />
              <q-tooltip>
                <span class="text-size-12">{{
                  props.inFullscreen
                    ? traducir('QuitarPantallaCompleta')
                    : traducir('ColocarPantallaCompleta')
                }}</span>
              </q-tooltip>
            </template>
          </q-btn>
        </div>
        <div class="full-width row wrap justify-start items-center content-center q-gutter-xs">
          <q-btn align="around" outline color="secondary" v-if="mostrarBotonFiltros">
            <template #default>
              <q-tooltip>
                <span class="text-size-12">{{ traducir('LimpiarFiltrosDescripcion') }}</span>
              </q-tooltip>
              <q-icon left name="filter_list_off" />
              <span>{{ traducir('LimpiarFiltros') }}</span>
              <q-menu>
                <div class="column q-pa-sm q-gutter-sm">
                  <span class="text-size-12 text-center text-secondary">{{
                    traducir('OpcionesMenuFiltros')
                  }}</span>
                  <q-btn
                    align="between"
                    outline
                    color="secondary"
                    class="justify-center"
                    @click="limpiarFiltros"
                    v-if="mostrarFilaFiltros"
                  >
                    <q-tooltip>
                      <span class="text-size-12">{{
                        traducir('ReestablecerBusquedasDescripcion')
                      }}</span>
                    </q-tooltip>
                    <q-icon left name="search_off" />
                    <span>{{ traducir('ReestablecerBusquedas') }}</span>
                  </q-btn>
                  <q-btn
                    align="between"
                    outline
                    color="secondary"
                    class="justify-center"
                    @click="reestablecerFiltros"
                    v-if="mostrarFilaFiltros"
                  >
                    <q-tooltip>
                      <span class="text-size-12">{{
                        traducir('ReestablecerOpcionesFiltradoDescripcion')
                      }}</span>
                    </q-tooltip>
                    <q-icon left name="filter_alt_off" />
                    <span>{{ traducir('ReestablecerOpcionesFiltrado') }}</span>
                  </q-btn>
                  <q-btn
                    align="between"
                    outline
                    color="secondary"
                    class="justify-center"
                    @click="reestablecerTotales"
                    v-if="mostrarFilaTotales"
                  >
                    <q-tooltip>
                      <span class="text-size-12">{{
                        traducir('ReestablecerOpcionesTotalesDescripcion')
                      }}</span>
                    </q-tooltip>
                    <q-icon left name="fa fa-filter-circle-xmark" size="xs" />
                    <span>{{ traducir('ReestablecerOpcionesTotales') }}</span>
                  </q-btn>
                </div>
              </q-menu>
            </template>
          </q-btn>

          <q-btn align="around" outline color="secondary" v-if="!ocultarBotonExportarDatos">
            <template #default>
              <q-tooltip>
                <span class="text-size-12">{{ traducir('ExportarTablaDescripcion') }}</span>
              </q-tooltip>
              <q-icon left name="fa fa-file-export" size="xs" />
              <span>{{ traducir('ExportarTabla') }}</span>
              <q-menu>
                <div class="column q-pa-sm q-gutter-sm">
                  <span class="text-size-12 text-center text-secondary">{{
                    traducir('OpcionesMenuExportacion')
                  }}</span>
                  <q-btn align="between" outline color="secondary" class="justify-center">
                    <!-- <q-tooltip>
                        <span class="text-size-12">{{
                          traducir("ReestablecerBusquedasDescripcion")
                        }}</span>
                      </q-tooltip> -->
                    <q-icon left name="fa-solid fa-file-csv" size="xs" />
                    <span>{{ traducir('CSV') }}</span>
                  </q-btn>
                  <q-btn
                    align="between"
                    outline
                    color="secondary"
                    class="justify-center"
                    @click="reestablecerTotales"
                  >
                    <!-- <q-tooltip>
                        <span class="text-size-12">{{
                          traducir("ReestablecerOpcionesTotalesDescripcion")
                        }}</span>
                      </q-tooltip> -->
                    <q-icon left name="fa-solid fa-file-excel" size="xs" />
                    <span>{{ traducir('EXCEL') }}</span>
                  </q-btn>
                  <q-btn
                    align="between"
                    outline
                    color="secondary"
                    class="justify-center"
                    @click="reestablecerTotales"
                  >
                    <!-- <q-tooltip>
                        <span class="text-size-12">{{
                          traducir("ReestablecerOpcionesTotalesDescripcion")
                        }}</span>
                      </q-tooltip> -->
                    <q-icon left name="fa-solid fa-file-pdf" size="xs" />
                    <span>{{ traducir('PDF') }}</span>
                  </q-btn>
                </div>
              </q-menu>
            </template>
          </q-btn>

          <q-btn outline round color="secondary" icon="settings" size="md">
            <q-tooltip>
              <span class="text-size-12">{{ traducir('OpcionesTablaDescripcion') }}</span>
            </q-tooltip>
            <q-menu>
              <div class="column q-pa-sm q-gutter-sm justify-center items-center">
                <span class="text-size-12 text-center text-secondary">{{
                  traducir('OpcionesMenuDeTabla')
                }}</span>
                <q-checkbox
                  dense
                  v-model="guardarConfiguracionesTabla"
                  v-if="identificadorTabla"
                  color="secondary"
                >
                  <template #default>
                    <span class="text-size-12 text-center text-secondary">{{
                      traducir('GuardarConfiguracionesTabla')
                    }}</span>
                  </template>
                </q-checkbox>
                <q-checkbox
                  dense
                  v-model="guardarConfiguracionesFiltros"
                  color="secondary"
                  v-if="mostrarFilaFiltros && identificadorTabla"
                >
                  <template #default>
                    <span class="text-size-12 text-center text-secondary">{{
                      traducir('GuardarFiltrosDeTabla')
                    }}</span>
                  </template> </q-checkbox
                ><q-checkbox
                  dense
                  v-model="guardarConfiguracionesOrdenDeColumnas"
                  color="secondary"
                  v-if="mostrarFilaFiltros && identificadorTabla"
                >
                  <template #default>
                    <span class="text-size-12 text-center text-secondary">{{
                      traducir('GuardarOrdenDeTabla')
                    }}</span>
                  </template>
                </q-checkbox>
                <!-- existenDatosLS -->
                <q-select
                  v-model="columnasUsuario"
                  multiple
                  outlined
                  dense
                  :display-value="`${
                    columnasUsuario ? columnasUsuario.length : 0
                  } ${traducir('Seleccionadas')}`"
                  :label="traducir('ColumnasOcultas')"
                  clearable
                  emit-value
                  map-options
                  :options="columnasMapeadas.columnas"
                  option-value="nombre"
                  :option-label="
                    (opt) =>
                      opt.llaveTraduccion ? traducir(opt.llaveTraduccion) : opt.nombreFormateado
                  "
                  options-cover
                  style="min-width: 200px"
                >
                </q-select>
                <q-select
                  v-model="separadorTabla"
                  outlined
                  dense
                  :label="traducir(`SeparadorCeldas`)"
                  emit-value
                  map-options
                  :options="opcionesSeparador"
                  option-value="valor"
                  :option-label="(opt) => (opt ? traducir(opt.nombre) : '')"
                  options-cover
                  style="min-width: 200px"
                >
                </q-select>
              </div>
            </q-menu>
          </q-btn>

          <q-btn
            outline
            color="secondary"
            round
            @click="establecerPantallaCompleta(props)"
            v-if="ocultarTituloTabla"
          >
            <template #default>
              <q-icon
                :name="props.inFullscreen ? 'fa-solid fa-compress' : 'fa-solid fa-expand'"
                size="xs"
                class="text-textprimary"
              />
              <q-tooltip>
                <span class="text-size-12">{{
                  props.inFullscreen
                    ? traducir('QuitarPantallaCompleta')
                    : traducir('ColocarPantallaCompleta')
                }}</span>
              </q-tooltip>
            </template>
          </q-btn>
        </div>
      </div>
    </template>

    <template #header="props">
      <q-tr :props="props" v-if="!cargando.value && existenValoresTablaReal">
        <q-th v-if="mostrarColumnasDeOpciones" class="g3ColumnaEstatica">
          <q-icon name="fa-solid fa-lock" class="q-mr-xs" size="14px">
            <q-tooltip>
              <span class="text-size-12">{{ traducir('ColumnaBloqueada') }}</span>
            </q-tooltip>
          </q-icon>
          <span class="text-size-12">{{ traducir('Opciones') }}</span>
        </q-th>

        <template v-for="col in props.cols" :key="col.name">
          <q-th
            :props="props"
            style="text-align: center; z-index: 3 !important"
            :draggable="col.sePuedeMover ? true : false"
            @dragstart="onDragStart(col.index)"
            @dragover="onDragOver"
            @drop="onDrop(col.index)"
            v-show="col.mostrarColumna"
          >
            <q-icon
              name="fa-solid fa-arrows-left-right-to-line"
              class="q-mr-xs"
              size="14px"
              v-if="col.sePuedeMover"
            >
              <q-tooltip>
                <span class="text-size-12">{{ traducir('TooltipMoverColumna') }}</span>
              </q-tooltip>
            </q-icon>
            <q-icon name="fa-solid fa-lock" class="q-mr-xs" size="14px" v-else>
              <q-tooltip>
                <span class="text-size-12">{{ traducir('ColumnaBloqueada') }}</span>
              </q-tooltip>
            </q-icon>
            <span class="text-size-12"
              >{{ col.llaveTraduccion ? traducir(col.llaveTraduccion) : col.label }}
              {{ col.unidades ? `(${col.unidades})` : '' }}</span
            >
          </q-th>
        </template>
      </q-tr>
    </template>

    <template #top-row="cols">
      <q-tr class="fila-filtros" v-if="mostrarFilaFiltros && existenValoresTablaReal">
        <q-td
          v-if="mostrarColumnasDeOpciones"
          class="g3ColumnaEstatica g3BackgroundTable"
          style="z-index: 5"
        >
          <div class="column no-wrap justify-start items-center content-center">
            <q-checkbox
              v-show="modoSeleccion == 'multiple'"
              v-if="tipoSelector == 'checkbox'"
              @click="seleccionarTodos"
              v-model="todaInformacionSeleccionada"
              size="50px"
            >
              <q-tooltip>
                <span class="text-size-12"
                  >{{
                    todaInformacionSeleccionada
                      ? traducir('DeseleccionarTodaTabla')
                      : traducir('SeleccionarTodaTabla')
                  }}
                </span>
              </q-tooltip>
            </q-checkbox>
            <q-toggle
              v-show="modoSeleccion == 'multiple'"
              v-else-if="tipoSelector == 'toggle'"
              @click="seleccionarTodos"
              v-model="todaInformacionSeleccionada"
              checked-icon="fa-solid fa-check-double"
              size="50px"
            >
              <q-tooltip>
                <span class="text-size-12"
                  >{{
                    todaInformacionSeleccionada
                      ? traducir('DeseleccionarTodaTabla')
                      : traducir('SeleccionarTodaTabla')
                  }}
                </span>
              </q-tooltip>
            </q-toggle>

            <q-toggle
              v-show="expandirFilas"
              v-model="todasColumnasExpandidas"
              checked-icon="fa-solid fa-angles-down"
              @click="expandirTodos"
              unchecked-icon="fa-solid fa-angles-up"
              size="50px"
            >
              <q-tooltip>
                <span class="text-size-12"
                  >{{
                    todasColumnasExpandidas
                      ? traducir('ContraerTodaTabla')
                      : traducir('ExpandirTodaTabla')
                  }}
                </span>
              </q-tooltip>
            </q-toggle>
          </div>
        </q-td>
        <q-td v-for="(col, index) in cols.cols" :key="index" v-show="col.mostrarColumna">
          <div
            class="fit column no-wrap text-center q-gutter-sm"
            v-if="col.tipoVariable === 'string'"
          >
            <q-select
              v-model="filtros[col.name].opcionSeleccionada"
              outlined
              dense
              hide-bottom-space
              label-slot
              input-debounce="1000"
              :options="filtros[col.name].opciones"
              option-value="valor"
              :option-label="(opt) => (opt ? traducir(opt.nombre) : '')"
              popup-content-class="text-size-12"
              options-cover
              style="min-width: 150px"
            >
              <q-tooltip>
                <span class="text-size-12">{{ traducir('OpcionFiltradoDescripcion') }}</span>
              </q-tooltip>
              <template #label>
                <span class="text-size-12">{{ traducir('OpcionFiltrado') }}</span>
              </template>
              <template #selected-item="scope">
                <span class="text-size-12">{{ traducir(scope.opt.nombre) }}</span>
              </template>
            </q-select>
            <q-select
              v-if="filtros[col.name].filtroSelector"
              v-model="filtros[col.name].valor"
              outlined
              dense
              multiple
              emit-value
              map-options
              input-debounce="1000"
              label-slot
              clearable
              @clear="filtros[col.name].valor = []"
              :options="filtros[col.name].opcionesValor"
              option-value="valor"
              option-label="nombre"
              use-chips
              style="min-width: 150px"
            >
              <q-tooltip>
                <span class="text-size-12">{{ traducir('FiltroSelectorDescripcion') }}</span>
              </q-tooltip>
              <template #label>
                <span class="text-size-12">{{ traducir('Filtro') }}</span>
              </template>
              <template #selected-item="scope">
                <q-chip
                  removable
                  dense
                  @remove="scope.removeAtIndex(scope.index)"
                  :tabindex="scope.tabindex"
                >
                  <span class="text-size-12">{{ scope.opt.nombre }}</span>
                </q-chip>
              </template>
            </q-select>
            <q-input
              v-else
              clearable
              outlined
              dense
              @clear="filtros[col.name].valor = ''"
              class="q-mb-sm"
              debounce="500"
              color="primary"
              label-slot
              v-model="filtros[col.name].valor"
              style="min-width: 150px"
              input-class="text-size-12"
            >
              <q-tooltip>
                <span class="text-size-12">{{ traducir('FiltroDescripcion') }}</span>
              </q-tooltip>
              <template #label>
                <span class="text-size-12">{{ traducir('Filtro') }}</span>
              </template>
            </q-input>
          </div>
          <div
            class="fit column no-wrap text-center q-gutter-sm"
            v-else-if="
              col.tipoVariable === 'number' ||
              col.tipoVariable === 'dinero' ||
              col.tipoVariable === 'porcentaje'
            "
          >
            <q-select
              v-model="filtros[col.name].opcionSeleccionada"
              outlined
              dense
              label-slot
              :options="filtros[col.name].opciones"
              option-value="valor"
              :option-label="(opt) => (opt ? traducir(opt.nombre) : '')"
              options-cover
              style="min-width: 150px"
            >
              <q-tooltip>
                <span class="text-size-12">{{ traducir('OpcionFiltradoDescripcion') }}</span>
              </q-tooltip>
              <template #label>
                <span class="text-size-12">{{ traducir('OpcionFiltrado') }}</span>
              </template>
              <template #selected-item="scope">
                <span class="text-size-12">{{ traducir(scope.opt.nombre) }}</span>
              </template>
            </q-select>
            <q-input
              clearable
              outlined
              dense
              @clear="filtros[col.name].valor = null"
              class="q-mb-sm"
              debounce="200"
              color="primary"
              label-slot
              v-model="filtros[col.name].valor"
              style="min-width: 150px"
              type="number"
              input-class="text-size-12"
            >
              <q-tooltip>
                <span class="text-size-12">{{ traducir('FiltroDescripcion') }}</span>
              </q-tooltip>
              <template #label>
                <span class="text-size-12">{{ traducir('Filtro') }}</span>
              </template>
            </q-input>
          </div>
          <div
            class="fit column no-wrap text-center q-gutter-sm"
            v-else-if="col.tipoVariable === 'boolean'"
          >
            <q-select
              v-model="filtros[col.name].opcionSeleccionada"
              outlined
              dense
              label-slot
              :options="filtros[col.name].opciones"
              option-value="valor"
              :option-label="(opt) => (opt ? traducir(opt.nombre) : '')"
              options-cover
              style="min-width: 150px"
            >
              <q-tooltip>
                <span class="text-size-12">{{ traducir('OpcionFiltradoDescripcion') }}</span>
              </q-tooltip>
              <template #label>
                <span class="text-size-12">{{ traducir('OpcionFiltrado') }}</span>
              </template>
              <template #selected-item="scope">
                <span class="text-size-12">{{ traducir(scope.opt.nombre) }}</span>
              </template>
            </q-select>
            <q-select
              v-model="filtros[col.name].valor"
              outlined
              dense
              multiple
              emit-value
              map-options
              label-slot
              clearable
              @clear="filtros[col.name].valor = []"
              :options="filtros[col.name].opcionesValor"
              option-value="valor"
              option-label="nombre"
              options-cover
              style="min-width: 150px"
            >
              <q-tooltip>
                <span class="text-size-12">{{ traducir('FiltroSelectorDescripcion') }}</span>
              </q-tooltip>
              <template #label>
                <span class="text-size-12">{{ traducir('Filtro') }}</span>
              </template>
              <template #selected-item="scope">
                <q-chip
                  removable
                  dense
                  @remove="scope.removeAtIndex(scope.index)"
                  :tabindex="scope.tabindex"
                >
                  <span class="text-size-12">{{ scope.opt.nombre }}</span>
                </q-chip>
              </template>
            </q-select>
          </div>
          <div
            class="fit column no-wrap text-center q-gutter-sm"
            v-else-if="col.tipoVariable === 'Date'"
          >
            <q-select
              v-model="filtros[col.name].opcionSeleccionada"
              outlined
              dense
              label-slot
              :options="filtros[col.name].opciones"
              option-value="valor"
              :option-label="(opt) => (opt ? traducir(opt.nombre) : '')"
              options-cover
              style="min-width: 150px"
            >
              <q-tooltip>
                <span class="text-size-12">{{ traducir('OpcionFiltradoDescripcion') }}</span>
              </q-tooltip>
              <template #label>
                <span class="text-size-12">{{ traducir('OpcionFiltrado') }}</span>
              </template>
              <template #selected-item="scope">
                <span class="text-size-12">{{ traducir(scope.opt.nombre) }}</span>
              </template>
            </q-select>
            <q-input
              clearable
              outlined
              dense
              @clear="filtros[col.name].valor = null"
              class="q-mb-sm"
              debounce="200"
              color="primary"
              label-slot
              autogrow
              v-model="filtros[col.name].valor"
              style="min-width: 150px"
              input-class="text-size-12"
            >
              <q-tooltip>
                <span class="text-size-12">{{ traducir('FiltroDescripcion') }}</span>
              </q-tooltip>
              <template #label>
                <span class="text-size-12">{{ traducir('Filtro') }}</span>
              </template>
              <template #append>
                <!-- TODO: REALIZAR FILTRADO POR HORA Y FECHA Y OPCIONES DE FILTRADO ENTRE -->
                <!-- <q-btn flat round color="secondary" icon="event" size="md">
                    <q-menu>
                      <div class="column q-pa-md q-gutter-sm">
                        <q-input dense v-model="filtros[col.name].valorTimeOnly" filled type="time" input-class="text-size-12" clearable style="min-width: 150px">
                          <q-tooltip>
                            <span class="text-size-12">{{
                              traducir("FiltroTimeDescripcion")
                            }}</span>
                          </q-tooltip>
                        </q-input>
                        <q-input dense v-model="filtros[col.name].valorDateOnly" filled type="date" input-class="text-size-12" clearable style="min-width: 150px">
                          <q-tooltip>
                            <span class="text-size-12">{{
                              traducir("FiltroDateDescripcion")
                            }}</span>
                          </q-tooltip>
                        </q-input>
                      </div>
                    </q-menu>
                  </q-btn> -->
                <q-icon name="event" class="cursor-pointer" cr>
                  <q-popup-proxy cover transition-show="scale" transition-hide="scale">
                    <q-date
                      v-model="filtros[col.name].valor"
                      color="primary"
                      :mask="lenguaje == 'es-MX' ? 'DD/MM/YYYY' : 'MM/DD/YYYY'"
                    >
                      <q-space />
                      <q-btn
                        v-close-popup
                        flat
                        color="primary"
                        rounded
                        @click="
                          filtros[col.name].valor = date.formatDate(
                            new Date(),
                            lenguaje == 'es-MX' ? 'DD/MM/YYYY' : 'MM/DD/YYYY',
                          )
                        "
                        >{{ traducir('Hoy') }}</q-btn
                      >
                      <q-btn v-close-popup color="primary" flat rounded>{{
                        traducir('Cerrar')
                      }}</q-btn>
                    </q-date>
                  </q-popup-proxy>
                </q-icon>
              </template>
            </q-input>
          </div>
        </q-td>
      </q-tr>
    </template>

    <template #body="props">
      <q-tr
        :props="props"
        :class="props.rowIndex % 2 !== 0 ? 'bg-filaParCT' : 'bg-fondo2'"
        v-if="existenValoresTablaReal"
      >
        <q-td
          v-if="mostrarColumnasDeOpciones"
          class="g3ColumnaEstatica g3BackgroundTable"
          style="width: 100px"
        >
          <div class="row no-wrap justify-center items-center content-center">
            <div class="column no-wrap justify-center items-center content-center">
              <q-checkbox
                v-show="modoSeleccion != 'none'"
                v-if="tipoSelector == 'checkbox'"
                v-model="props.selected"
              >
                <q-tooltip>
                  <span class="text-size-16"
                    >{{
                      props.selected
                        ? traducir('DeseleccionarRegistroTabla')
                        : traducir('SeleccionarRegistroTabla')
                    }}
                  </span>
                </q-tooltip>
              </q-checkbox>
              <q-toggle
                v-show="modoSeleccion != 'none'"
                v-else-if="tipoSelector == 'toggle'"
                v-model="props.selected"
                checked-icon="fa-solid fa-check"
              >
                <q-tooltip>
                  <span class="text-size-16"
                    >{{
                      props.selected
                        ? traducir('DeseleccionarRegistroTabla')
                        : traducir('SeleccionarRegistroTabla')
                    }}
                  </span>
                </q-tooltip>
              </q-toggle>

              <q-toggle
                v-show="expandirFilas"
                v-model="props.expand"
                checked-icon="fa-solid fa-angle-down"
                unchecked-icon="fa-solid fa-angle-up"
              >
                <q-tooltip>
                  <span class="text-size-16"
                    >{{
                      props.expand
                        ? traducir('ContraerRegistroTabla')
                        : traducir('ExpandirRegistroTabla')
                    }}
                  </span>
                </q-tooltip>
              </q-toggle>
            </div>
            <div class="column no-wrap justify-center items-center content-center">
              <slot name="acceso-directo" :props="props" />
            </div>
          </div>
        </q-td>
        <q-td :props="props" v-for="col in props.cols" :key="col.name" v-show="col.mostrarColumna">
          <slot :name="`celda-${col.name}`" :props="props">
            <div
              v-if="
                col.tipoVariable === 'string' ||
                col.tipoVariable === 'number' ||
                col.tipoVariable === 'Date' ||
                col.tipoVariable === 'porcentaje' ||
                col.tipoVariable === 'dinero'
              "
            >
              <strong class="text-size-12 text-center" v-if="col.columnasEnColumna">{{
                col.llaveTraduccion ? `${traducir(col.llaveTraduccion)}: ` : `${col.label}: `
              }}</strong>
              <span class="text-size-12 text-center" v-if="!col.parametroDivision">
                {{ col.value }}</span
              >
              <div
                v-else-if="col.parametroDivision"
                class="column justify-center no-wrap items-center content-center"
              >
                <q-chip
                  v-for="(parametro, ind) in col.value.split(col.parametroDivision)"
                  :key="ind"
                  dense
                  square
                  color="positive"
                  text-color="white"
                  class="text-weight-bolder justify-center no-wrap text-center items-center content-center"
                >
                  <template #default>
                    <span class="text-size-12 text-center">{{ parametro }}</span>
                  </template>
                </q-chip>
              </div>
              <template v-for="(columna, ind) in col.columnasEnColumna" :key="ind">
                <br />
                <strong class="text-size-12 text-center">{{
                  props.cols.find((c) => c.name === columna).llaveTraduccion
                    ? `${traducir(props.cols.find((c) => c.name === columna).llaveTraduccion)}: `
                    : `${props.cols.find((c) => c.name === columna).label}: `
                }}</strong>
                <span
                  class="text-size-12 text-center"
                  v-if="props.cols.find((c) => c.name === columna).columnaOtrosValores"
                  >{{
                    props.cols.find((c) => c.name === columna).value == true
                      ? props.cols.find((c) => c.name === columna).columnaOtrosValores
                          .valorVerdadero
                      : props.cols.find((c) => c.name === columna).columnaOtrosValores.valorFalso
                  }}</span
                >
                <span class="text-size-12 text-center" v-else>
                  {{ props.cols.find((c) => c.name === columna).value }}</span
                >
              </template>
              <template v-for="(columna, ind) in col.columnasBooleanasEnColumna" :key="ind">
                <br />
                <q-chip
                  dense
                  square
                  :color="
                    props.cols.find((c) => c.name === columna).value == true
                      ? 'positive'
                      : 'negative'
                  "
                  text-color="white"
                  class="text-weight-bolder"
                >
                  <q-tooltip v-if="props.cols.find((c) => c.name === columna).columnaOtrosValores">
                    <span class="text-size-12 text-center">{{
                      props.cols.find((c) => c.name === columna).value == true
                        ? props.cols.find((c) => c.name === columna).columnaOtrosValores
                            .valorVerdadero
                        : props.cols.find((c) => c.name === columna).columnaOtrosValores.valorFalso
                    }}</span>
                  </q-tooltip>
                  <span class="text-size-12 text-center">{{
                    props.cols.find((c) => c.name === columna).llaveTraduccion
                      ? `${traducir(props.cols.find((c) => c.name === columna).llaveTraduccion)}`
                      : `${props.cols.find((c) => c.name === columna).label}`
                  }}</span>
                </q-chip>
              </template>
            </div>

            <div v-else-if="col.tipoVariable === 'color'">
              <q-avatar
                text-color="white"
                size="lg"
                :style="{
                  background: col.value,
                  color: col.value,
                }"
              ></q-avatar>
            </div>

            <div class="text-size-12 text-center" v-else-if="col.tipoVariable === 'boolean'">
              <q-chip
                dense
                square
                :color="col.value == 'true' ? 'positive' : 'negative'"
                text-color="white"
                class="text-weight-bolder"
              >
                <q-tooltip>
                  <span class="text-size-16">{{
                    col.value == 'true'
                      ? col.columnaOtrosValores.valorVerdadero
                      : col.columnaOtrosValores.valorFalso
                  }}</span>
                </q-tooltip>
                <strong class="text-size-12 text-center" v-if="col.columnasEnColumna">{{
                  col.llaveTraduccion ? `${traducir(col.llaveTraduccion)}: ` : `${col.label}: `
                }}</strong>
                <span class="text-size-12 text-center" v-if="col.columnaOtrosValores">{{
                  col.value == 'true'
                    ? col.columnaOtrosValores.valorVerdadero
                    : col.columnaOtrosValores.valorFalso
                }}</span>
                <span class="text-size-12 text-center" v-else> {{ col.value }}</span>
              </q-chip>

              <template v-for="(columna, ind) in col.columnasEnColumna" :key="ind">
                <br />
                <strong class="text-size-12 text-center">{{
                  props.cols.find((c) => c.name === columna).llaveTraduccion
                    ? `${traducir(props.cols.find((c) => c.name === columna).llaveTraduccion)}: `
                    : `${props.cols.find((c) => c.name === columna).label}: `
                }}</strong>
                <span class="text-size-12 text-center">{{
                  props.cols.find((c) => c.name === columna).value
                }}</span>
              </template>

              <template v-for="(columna, ind) in col.columnasBooleanasEnColumna" :key="ind">
                <br />
                <q-chip
                  dense
                  square
                  :color="
                    props.cols.find((c) => c.name === columna).value == 'true'
                      ? 'positive'
                      : 'negative'
                  "
                  text-color="white"
                  class="text-weight-bolder"
                >
                  <q-tooltip v-if="props.cols.find((c) => c.name === columna).columnaOtrosValores">
                    <span class="text-size-12 text-center">{{
                      props.cols.find((c) => c.name === columna).value == 'true'
                        ? props.cols.find((c) => c.name === columna).columnaOtrosValores
                            .valorVerdadero
                        : props.cols.find((c) => c.name === columna).columnaOtrosValores.valorFalso
                    }}</span>
                  </q-tooltip>
                  <span class="text-size-12 text-center">{{
                    props.cols.find((c) => c.name === columna).llaveTraduccion
                      ? `${traducir(props.cols.find((c) => c.name === columna).llaveTraduccion)}`
                      : `${props.cols.find((c) => c.name === columna).label}`
                  }}</span>
                </q-chip>
              </template>
            </div>
            <div class="text-size-12 text-center" v-else-if="col.tipoVariable === 'Array'">
              <!-- //MOSTRARARRAY -->
              <template v-if="col.mostrarDatosArray">
                <span v-for="(objetoArray, index) in col.value" :key="index">
                  {{ objetoArray }}
                  <br />
                </span>
              </template>
              <template v-else>
                {{ col.value.length }}
              </template>
            </div>
            <div class="text-size-12 text-center" v-else-if="col.tipoVariable === 'Object'">
              <template v-if="typeof col.value === 'string'">
                <template v-for="(valor, propiedad, index) in JSON.parse(col.value)" :key="index">
                  <template
                    v-if="col.objetoPropiedadesAMostrar.find((x) => x == propiedad) != undefined"
                  >
                    <strong>{{ startCase(propiedad) }}:</strong>
                    {{ valor }}
                    <br />
                  </template>
                </template>
              </template>
              <template v-else>
                <!-- Si `col.value` ya es un objeto, mostrarlo directamente -->
                <template v-for="(valor, propiedad, index) in col.value" :key="index">
                  <strong class="text-size-12 text-center">{{
                    col.llaveTraduccion ? `${traducir(col.llaveTraduccion)}: ` : `${col.label}: `
                  }}</strong>
                  {{ valor }}
                  <br />
                  <br />
                </template>
              </template>
            </div>
            <q-img
              v-else-if="col.tipoVariable === 'imagen'"
              :src="col.value.length > 0 ? col.value : './../../assets/no-image.gif'"
              :alt="col.name"
              style="max-width: 100px; border-radius: 10px"
              class="text-center"
              loading="eager"
            >
              <template #loading>
                <q-spinner-gears color="white" />
              </template>
              <template #error> </template>
            </q-img>
          </slot>
        </q-td>
      </q-tr>
      <q-tr
        v-show="props.expand"
        :props="props"
        :class="props.rowIndex % 2 !== 0 ? 'bg-filaParCT' : 'bg-fondo2'"
      >
        <q-td colspan="100%">
          <slot :name="`celda-expandida`" :props="props"> </slot>
        </q-td>
      </q-tr>
    </template>

    <template #bottom-row="cols">
      <q-tr
        v-if="mostrarFilaTotales && existenValoresTablaReal"
        class="g3pruebasestilos g3FondoTabla"
      >
        <q-td
          v-if="mostrarColumnasDeOpciones"
          class="g3ColumnaYFilaEstatica g3FondoTabla"
          style="padding-top: 10px !important; padding-bottom: 15px !important"
        >
          <span class="text-size-16 text-bold">{{ traducir('Totales') }}</span>
        </q-td>
        <q-td
          v-for="(col, index) in cols.cols"
          :key="index"
          v-show="col.mostrarColumna"
          style="padding-top: 10px !important; padding-bottom: 15px !important"
        >
          <div
            class="fit column no-wrap text-center q-gutter-sm"
            v-if="col.tipoVariable === 'number' || col.tipoVariable === 'dinero'"
          >
            <q-select
              v-model="filtros[col.name].opcionTotalesSeleccionada"
              outlined
              dense
              :label="traducir('OpcionTotales')"
              :options="filtros[col.name].opcionesTotales"
              option-value="valor"
              :option-label="(opt) => (opt ? traducir(opt.nombre) : '')"
              options-cover
              style="min-width: 150px"
            >
              <q-tooltip>
                <span class="text-size-12">{{ traducir('OpcionTotalesDescripcion') }}</span>
              </q-tooltip>
              <template #label>
                <span class="text-size-12">{{ traducir('OpcionTotales') }}</span>
              </template>
              <template #selected-item="scope">
                <span class="text-size-12">{{ traducir(scope.opt.nombre) }}</span>
              </template>
            </q-select>
            <div v-if="filtros[col.name].opcionTotalesSeleccionada.valor == 'Σ'">
              <span class="text-size-14" v-if="col.tipoVariable === 'number'">
                {{
                  G3TableUtils.formatearNumero(
                    G3TableUtils.obtenerSumaTotales(listaValoresTotales(col.name)),
                    2,
                  )
                }}
              </span>
              <span class="text-size-14" v-else-if="col.tipoVariable === 'dinero'">
                {{
                  G3TableUtils.formatearDinero(
                    G3TableUtils.obtenerSumaTotales(listaValoresTotales(col.name)),
                    2,
                  )
                }}
              </span>
            </div>

            <div v-if="filtros[col.name].opcionTotalesSeleccionada.valor == 'x̄'">
              <span class="text-size-14" v-if="col.tipoVariable === 'number'">
                {{
                  G3TableUtils.formatearNumero(
                    G3TableUtils.obtenerPromedio(listaValoresTotales(col.name)),
                    2,
                  )
                }}
              </span>
              <span class="text-size-14" v-else-if="col.tipoVariable === 'dinero'">
                {{
                  G3TableUtils.formatearDinero(
                    G3TableUtils.obtenerPromedio(listaValoresTotales(col.name)),
                    2,
                  )
                }}
              </span>
            </div>

            <div v-if="filtros[col.name].opcionTotalesSeleccionada.valor == 'max'">
              <span class="text-size-14" v-if="col.tipoVariable === 'number'">
                {{
                  G3TableUtils.formatearNumero(
                    G3TableUtils.obtenerNumeroMayor(listaValoresTotales(col.name)),
                    2,
                  )
                }}
              </span>
              <span class="text-size-14" v-else-if="col.tipoVariable === 'dinero'">
                {{
                  G3TableUtils.formatearDinero(
                    G3TableUtils.obtenerNumeroMayor(listaValoresTotales(col.name)),
                    2,
                  )
                }}
              </span>
            </div>

            <div v-if="filtros[col.name].opcionTotalesSeleccionada.valor == 'min'">
              <span class="text-size-14" v-if="col.tipoVariable === 'number'">
                {{
                  G3TableUtils.formatearNumero(
                    G3TableUtils.obtenerNumeroMenor(listaValoresTotales(col.name)),
                    2,
                  )
                }}
              </span>
              <span class="text-size-14" v-else-if="col.tipoVariable === 'dinero'">
                {{
                  G3TableUtils.formatearDinero(
                    G3TableUtils.obtenerNumeroMenor(listaValoresTotales(col.name)),
                    2,
                  )
                }}
              </span>
            </div>
            <div v-if="filtros[col.name].opcionTotalesSeleccionada.valor == 'M'">
              <span class="text-size-14" v-if="col.tipoVariable === 'number'">
                {{
                  G3TableUtils.formatearNumero(
                    G3TableUtils.obtenerMediana(listaValoresTotales(col.name)),
                    2,
                  )
                }}
              </span>
              <span class="text-size-14" v-else-if="col.tipoVariable === 'dinero'">
                {{
                  G3TableUtils.formatearDinero(
                    G3TableUtils.obtenerMediana(listaValoresTotales(col.name)),
                    2,
                  )
                }}
              </span>
            </div>

            <div v-if="filtros[col.name].opcionTotalesSeleccionada.valor == 'Mo'">
              <span class="text-size-14" v-if="col.tipoVariable === 'number'">
                {{
                  G3TableUtils.formatearNumero(
                    G3TableUtils.obtenerModa(listaValoresTotales(col.name)).join(' - '),
                    2,
                  )
                }}
              </span>
              <span class="text-size-14" v-else-if="col.tipoVariable === 'dinero'">
                {{
                  G3TableUtils.formatearDinero(
                    G3TableUtils.obtenerModa(listaValoresTotales(col.name)).join(' - '),
                    2,
                  )
                }}
              </span>
            </div>

            <div v-if="filtros[col.name].opcionTotalesSeleccionada.valor == '∆'">
              <span class="text-size-14" v-if="col.tipoVariable === 'number'">
                {{
                  G3TableUtils.formatearNumero(
                    G3TableUtils.obtenerRango(listaValoresTotales(col.name)),
                    2,
                  )
                }}
              </span>
              <span class="text-size-14" v-else-if="col.tipoVariable === 'dinero'">
                {{
                  G3TableUtils.formatearDinero(
                    G3TableUtils.obtenerRango(listaValoresTotales(col.name)),
                    2,
                  )
                }}
              </span>
            </div>

            <div v-if="filtros[col.name].opcionTotalesSeleccionada.valor == 'σ²'">
              <span class="text-size-14">
                {{
                  G3TableUtils.formatearNumero(
                    G3TableUtils.obtenerVarianza(listaValoresTotales(col.name)),
                    2,
                  )
                }}
              </span>
            </div>

            <div v-if="filtros[col.name].opcionTotalesSeleccionada.valor == 'σ'">
              <span class="text-size-14">
                {{
                  G3TableUtils.formatearNumero(
                    G3TableUtils.obtenerDesviacionEstandar(listaValoresTotales(col.name)),
                    2,
                  )
                }}
              </span>
            </div>

            <div v-if="filtros[col.name].opcionTotalesSeleccionada.valor == 'Π'">
              <span class="text-size-14" v-if="col.tipoVariable === 'number'">
                {{
                  G3TableUtils.formatearNumero(
                    G3TableUtils.obtenerProductoria(listaValoresTotales(col.name)),
                    2,
                  )
                }}
              </span>
              <span class="text-size-14" v-else-if="col.tipoVariable === 'dinero'">
                {{
                  G3TableUtils.formatearDinero(
                    G3TableUtils.obtenerProductoria(listaValoresTotales(col.name)),
                    2,
                  )
                }}
              </span>
            </div>

            <div v-if="filtros[col.name].opcionTotalesSeleccionada.valor == 'Σ²'">
              <span class="text-size-14" v-if="col.tipoVariable === 'number'">
                {{
                  G3TableUtils.formatearNumero(
                    G3TableUtils.obtenerSumaCuadrados(listaValoresTotales(col.name)),
                    2,
                  )
                }}
              </span>
              <span class="text-size-14" v-else-if="col.tipoVariable === 'dinero'">
                {{
                  G3TableUtils.formatearDinero(
                    G3TableUtils.obtenerSumaCuadrados(listaValoresTotales(col.name)),
                    2,
                  )
                }}
              </span>
            </div>
          </div>
        </q-td>
      </q-tr>
    </template>

    <template #bottom="scope">
      <div class="full-width row justify-center items-center content-center q-my-md">
        <div class="row justify-center items-center content-center">
          <q-select
            v-model="paginacionSeleccionada"
            outlined
            dense
            label-slot
            :options="filasPorPaginas"
            options-cover
            option-value="nombre"
            map-options
            :option-label="(opt) => (opt ? opt.nombre : '')"
            @update:model-value="
              (opcionSeleccionada) => (scope.pagination.rowsPerPage = opcionSeleccionada.valor)
            "
            style="min-width: 125px"
          >
            <q-tooltip>
              <span class="text-size-12">{{ traducir('FilasPorPaginaDescripcion') }}</span>
            </q-tooltip>
            <template #label>
              <span class="text-size-12">{{ traducir('FilasPorPagina') }}</span>
            </template>
            <template #selected-item="selectScope">
              <span class="text-size-12">{{ selectScope.opt.nombre }}</span>
            </template>
          </q-select>
        </div>

        <q-space />

        <div>
          <q-pagination
            v-model="scope.pagination.page"
            icon-first="first_page"
            icon-prev="chevron_left"
            icon-next="chevron_right"
            icon-last="last_page"
            color="secondary"
            active-color="primary"
            ellipses="false"
            direction-links
            boundary-numbers
            boundary-links
            :max="scope.pagesNumber"
            max-pages="7"
          />
        </div>

        <q-space />

        <div class="justify-center items-center">
          <span class="text-size-12">{{ `${traducir('VisualizacionP1')} ` }}</span>
          <strong class="text-size-12"
            >{{ `${1 + (scope.pagination.page - 1) * scope.pagination.rowsPerPage}` }}
          </strong>
          <span class="text-size-12">{{ ` ${traducir('Al')} ` }}</span>
          <strong class="text-size-12">{{
            `${
              scope.pagination.rowsPerPage > 0
                ? scope.pagination.page * scope.pagination.rowsPerPage
                : filasFiltradas
                  ? filasFiltradas.length
                  : traducir('TodasLasFilas')
            }`
          }}</strong>
          <span class="text-size-12">{{ ` ${traducir('De')} ` }}</span>
          <strong class="text-size-12">{{
            `${filasFiltradas ? filasFiltradas.length : ''}`
          }}</strong>
          <span class="text-size-12">{{ ` ${traducir('VisualizacionP2')}` }}</span>
        </div>
      </div>
    </template>

    <template #loading>
      <q-inner-loading showing>
        <template v-slot:default>
          <BuscandoDataApi v-if="existenValoresTablaReal" />
        </template>
      </q-inner-loading>
    </template>

    <template #no-data>
      <template v-if="!cargando">
        <NoDataApi />
      </template>
      <template v-else-if="cargando && !existenValoresTablaReal">
        <q-inner-loading showing>
          <template v-slot:default>
            <BuscandoDataApi />
          </template>
        </q-inner-loading>
      </template>
    </template>
  </q-table>
  <div v-else>
    <NoDataApi />
    <!-- primeraConfiguracionRealizada: {{ primeraConfiguracionRealizada }}
    XD -->
  </div>
</template>

<script setup>
import {
  toRefs,
  computed,
  ref,
  onMounted,
  watch,
  watchEffect,
  useSlots,
  onUpdated,
  inject,
} from 'vue'
import NoDataApi from '../NoDataApi.vue'
import BuscandoDataApi from '../BuscandoDataApi.vue'
import { startCase } from 'es-toolkit/string'
import { sortBy, isEmpty } from 'es-toolkit/compat'
import MiniSearch from 'minisearch'
import { useQuasar, date } from 'quasar'

import { usePreferenciaStore } from 'src/stores/preferencias'
const traducir = inject('traducir')

import * as G3TableUtils from './g3tableutils.js'

const G3AdvancedTable = ref(null)
const versionTabla = ref(1.1)

const $q = useQuasar()
const storePreferencias = usePreferenciaStore()

const lenguaje = computed(() => (storePreferencias.lenguaje.includes('es') ? 'es-MX' : 'en-US'))

const slots = useSlots()

const seUsaSlotsOpciones = computed(() => {
  return !!slots['acceso-directo']
})

const props = defineProps({
  identificadorTabla: {
    type: String,
    required: true,
    default: '',
  },
  //Datos que vienen de la API para mostrarse
  datosTabla: {
    type: Array,
    default() {
      return []
    },
  },
  configuracionesColumnas: {
    type: Array,
    default: () => [
      // {
      //   name: "id",
      //   sinFormato: true,
      //   esInvisible: true,
      //   sinMovimiento: true,
      //   sinOrdenamiento: true,
      //   align: "center",
      //   esSelector: false,
      //   columnas: ["tonelTanque", "transportista", "conductor", "origen"],
      //   columnasBooleanas: ["asignada"],
      //   type: "porcentaje",
      //   unidad: "%",
      //   llaveTraduccion: "PorcentajeMermaPermisible",
      //   valorVerdadero: "Verdadero",
      //   valorFalso: "Falso",
      // }
    ],
  },
  llaveFilaUnica: {
    type: String,
    default: 'idUnico',
  },
  //none, single or multiple
  modoSeleccion: {
    type: String,
    default: 'none',
  },
  //checkbox or toggle
  tipoSelector: {
    type: String,
    default: 'checkbox',
  },
  filasSeleccionadas: {
    type: Array,
    default: () => [],
  },
  expandirFilas: {
    type: Boolean,
    default: false,
  },
  expandirPorSeleccion: {
    type: Boolean,
    default: false,
  },
  filasExpandidas: {
    type: Array,
    default: () => [],
  },
  cargando: {
    type: Boolean,
    default: false,
  },
  tituloTabla: {
    type: String,
    default: 'G3AdvancedTable',
  },
  ocultarTituloTabla: {
    type: Boolean,
    default: true,
  },
  //center, start, end
  alineacionTitulo: {
    type: String,
    default: 'center',
  },
  opcionesPaginacion: {
    type: Object,
    default: () => {
      return {
        sortBy: null, //ORDENAR POR COLUMNA, MANDAR STRING
        descending: false, //ORDENAR POR ORDEN DESCENDENTE
        page: 1, //PAGINA INICIAL
        rowsPerPage: 15, //FILAS POR PAGINA
      }
    },
  },
  opcionFilasPorPaginas: {
    type: Number,
    default: -1,
  },
  ocultarFilaTotales: {
    type: Boolean,
    default: false,
  },
  ocultarFilaFiltros: {
    type: Boolean,
    default: false,
  },
  ocultarBotonExportarDatos: {
    type: Boolean,
    default: true,
  },
  validacionSeleccionRegistro: {
    type: Function,
    default: null,
  },
})

const {
  datosTabla,
  configuracionesColumnas,
  modoSeleccion,
  tipoSelector,
  cargando,
  tituloTabla,
  expandirFilas,
  expandirPorSeleccion,
  llaveFilaUnica,
  opcionesPaginacion,
  opcionFilasPorPaginas,
  ocultarFilaTotales,
  ocultarFilaFiltros,
  identificadorTabla,
  ocultarTituloTabla,
  ocultarBotonExportarDatos,
  filasExpandidas,
  filasSeleccionadas,
  validacionSeleccionRegistro,
} = toRefs(props)

const primeraConfiguracionRealizada = ref(false)
const guardarConfiguracionesTabla = ref(true)
const guardarConfiguracionesFiltros = ref(true)
const guardarConfiguracionesOrdenDeColumnas = ref(true)
const existenDatosLS = ref(false)

const columnasUsuario = ref([])
const propiedades = ref([])
const mostrarColumnasDeOpciones = ref(false)
const columnasMapeadas = ref([])
const columnas = ref([])
const indiceColumnaArrastrada = ref(null)
const filtros = ref({})
const seConfiguraronLosFiltros = ref(false)
const filasPorPaginas = ref(0)
const paginacionSeleccionada = ref(null)
const paginacionTabla = ref(null)

watch(datosTabla, (nuevosDatos, viejosDatos) => {
  // console.log("Datos", nuevosDatos, viejosDatos);
  if (nuevosDatos == viejosDatos) return

  if (
    nuevosDatos.length > 0 &&
    !primeraConfiguracionRealizada.value &&
    !realizandoConfiguracion.value
  ) {
    realizandoConfiguracion.value = true
    configurarPropiedades()
    configurarMostrarFilasDeOpciones()
    configurarColumnasMapeadas()
    configurarColumnasTabla()
    configurarFiltros()
    configurarFilasIniciales()
    configurarFilasPorPaginas()
    configurarPaginacionTabla()

    primeraConfiguracionRealizada.value = true
    configuracionInicialLocalStorage()
    realizandoConfiguracion.value = false
  }
})

watch(columnasUsuario, (nuevosDatos, viejosDatos) => {
  if (typeof nuevosDatos != 'object') return

  if (nuevosDatos != null && nuevosDatos.length >= columnasMapeadas.value.columnas.length) {
    columnasUsuario.value = viejosDatos
    return
  }
  if (viejosDatos != null && viejosDatos.length == columnasMapeadas.value.columnas.length) return

  configurarColumnasMapeadas()
})

const configurarPropiedades = () => {
  if (propiedades.value.length == 0) {
    const nombrePropiedades = Object.keys(datosTabla.value[0])
    const valorPropiedades = Object.values(datosTabla.value[0])
    const indexPropiedades = Object.keys(datosTabla.value[0]).map((nombre, index) => index)

    for (let i = 0; i < nombrePropiedades.length; i++) {
      const tipoVariable = G3TableUtils.obtenerTipoVariable(valorPropiedades[i])
      let valorPropiedad = valorPropiedades[i]

      if (tipoVariable == 'Array') {
        valorPropiedad = []
      }

      if (tipoVariable == 'Object') {
        valorPropiedad = {}
      }

      const propiedad = {
        nombrePropiedad: nombrePropiedades[i],
        valorPropiedad: valorPropiedad,
        indexPropiedad: indexPropiedades[i],
      }

      propiedades.value.push(propiedad)
    }
  }
}

const configurarMostrarFilasDeOpciones = () => {
  if (modoSeleccion.value != 'none' || expandirFilas.value || seUsaSlotsOpciones.value) {
    mostrarColumnasDeOpciones.value = true
  }
}

const configurarColumnasMapeadas = () => {
  const columnas = []
  const columnasVisibles = []
  const columnasNoVisibles = []
  const columnasNoVisiblesNativas = []

  const columnasInvisiblesNativas = configuracionesColumnas.value.filter((x) => {
    if (Object.prototype.hasOwnProperty.call(x, 'esInvisible')) {
      if (x['esInvisible']) {
        return x
      }
    }
  })

  const columnasSobreOtras = configuracionesColumnas.value.filter((x) =>
    Object.prototype.hasOwnProperty.call(x, 'columnas'),
  )

  const columnasBooleanasSobreOtras = configuracionesColumnas.value.filter((x) =>
    Object.prototype.hasOwnProperty.call(x, 'columnasBooleanas'),
  )

  const columnasTraducciones = configuracionesColumnas.value.filter((x) =>
    Object.prototype.hasOwnProperty.call(x, 'llaveTraduccion'),
  )

  propiedades.value.forEach((propiedad) => {
    const noExistePropiedad = !columnasInvisiblesNativas.find(
      (x) => x.name == propiedad.nombrePropiedad,
    )

    if (noExistePropiedad) {
      columnasVisibles.push(propiedad.nombrePropiedad)

      if (columnasSobreOtras) {
        const existeStack = columnaExisteEnOtraColumna(propiedad.nombrePropiedad)
        if (existeStack) {
          return
        }
      }

      if (columnasBooleanasSobreOtras) {
        const existeStack = columnaBooleanaExisteEnOtraColumna(propiedad.nombrePropiedad)
        if (existeStack) {
          return
        }
      }

      const traduccionPropiedad = columnasTraducciones.find(
        (unidad) => unidad.name === propiedad.nombrePropiedad,
      )

      columnas.push({
        nombre: propiedad.nombrePropiedad,
        nombreFormateado: startCase(propiedad.nombrePropiedad),
        llaveTraduccion: traduccionPropiedad?.llaveTraduccion || null,
      })
    } else {
      columnasNoVisiblesNativas.push(propiedad.nombrePropiedad)
    }
  })

  if (columnasUsuario.value) {
    columnasUsuario.value.forEach((columna) => {
      columnasNoVisibles.includes(columna)
        ? columnasNoVisibles.splice(columnasNoVisibles.indexOf(columna), 1)
        : columnasNoVisibles.push(columna)
      columnasVisibles.includes(columna)
        ? columnasVisibles.splice(columnasVisibles.indexOf(columna), 1)
        : columnasVisibles.push(columna)
    })
  }

  //borrar filtros de las que oculte el usuario
  if (seConfiguraronLosFiltros.value) {
    columnasNoVisibles.forEach((columna) => {
      filtros.value[columna].valor = filtros.value[columna].valorDefault
      filtros.value[columna].opcionTotalesSeleccionada = filtros.value[columna].opcionesTotales[0]
      filtros.value[columna].opcionSeleccionada = filtros.value[columna].opciones[0]
    })
  }

  columnasMapeadas.value = {
    columnasVisibles,
    columnasInvisibles: [...columnasNoVisiblesNativas, ...columnasNoVisibles],
    columnas,
  }
}

const configurarColumnasTabla = () => {
  columnas.value = propiedades.value.map((propiedad) => {
    const configuracionesColumna = configuracionesColumnas.value.find(
      (configuracion) => configuracion.name === propiedad.nombrePropiedad,
    )

    let tieneSinFormato = false
    let tieneAlineacion = false
    let tieneFormatoForzado = false
    let tienePropiedadOrdenamiento = false
    let permiteOrdenamiento = true
    let tieneSinMovimiento = false
    let tieneFiltroSelector = false
    let tieneUnidades = false
    let tieneTraduccion = false
    let tieneOtrasColumnas = false
    let tieneOtrasColumnasBooleanas = false
    let tieneDivision = false
    let tieneObjetoPropiedadesAMostrar = false
    let tieneInvisibilidad = columnasMapeadas.value.columnasVisibles.includes(
      propiedad.nombrePropiedad,
    )
    let tieneOtrosValores = false
    let mostrarColumna = true
    let mostrarDatosArray = false

    let tipoVariable = G3TableUtils.obtenerTipoVariable(propiedad.valorPropiedad)

    let esColumnaSobreOtra = columnaExisteEnOtraColumna(propiedad.nombrePropiedad)
    let esColumnaBooleanaSobreOtra = columnaBooleanaExisteEnOtraColumna(propiedad.nombrePropiedad)

    if (esColumnaSobreOtra) {
      mostrarColumna = false
    }

    if (esColumnaBooleanaSobreOtra) {
      mostrarColumna = false
    }

    if (configuracionesColumna) {
      tieneSinFormato = G3TableUtils.tieneLaPropiedadYValor(configuracionesColumna, 'sinFormato')
      tieneFormatoForzado = Object.prototype.hasOwnProperty.call(configuracionesColumna, 'type')
      tienePropiedadOrdenamiento = G3TableUtils.tieneLaPropiedadYValor(
        configuracionesColumna,
        'sinOrdenamiento',
      )
      tieneSinMovimiento = G3TableUtils.tieneLaPropiedadYValor(
        configuracionesColumna,
        'sinMovimiento',
      )
      tieneFiltroSelector = Object.prototype.hasOwnProperty.call(
        configuracionesColumna,
        'esSelector',
      )
      tieneUnidades = Object.prototype.hasOwnProperty.call(configuracionesColumna, 'unidad')
      tieneTraduccion = Object.prototype.hasOwnProperty.call(
        configuracionesColumna,
        'llaveTraduccion',
      )
      tieneOtrasColumnas = Object.prototype.hasOwnProperty.call(configuracionesColumna, 'columnas')
      tieneOtrasColumnasBooleanas = Object.prototype.hasOwnProperty.call(
        configuracionesColumna,
        'columnasBooleanas',
      )
      tieneInvisibilidad = G3TableUtils.tieneLaPropiedadYValor(
        configuracionesColumna,
        'esInvisible',
      )
      tieneOtrosValores =
        Object.prototype.hasOwnProperty.call(configuracionesColumna, 'valorVerdadero') &&
        Object.prototype.hasOwnProperty.call(configuracionesColumna, 'valorFalso')
      tieneObjetoPropiedadesAMostrar = Object.prototype.hasOwnProperty.call(
        configuracionesColumna,
        'objetoPropiedadesAMostrar',
      )
      // console.log(configuracionesColumna.objetoPropiedadesAMostrar);
      if (tieneFormatoForzado) {
        tipoVariable = G3TableUtils.obtenerTipoVariable(
          propiedad.valorPropiedad,
          configuracionesColumna.type,
        )
      }

      mostrarDatosArray = G3TableUtils.tieneLaPropiedadYValor(
        configuracionesColumna,
        'mostrarDatosArray',
      )

      tieneDivision = Object.prototype.hasOwnProperty.call(
        configuracionesColumna,
        'parametroDivision',
      )
    }

    if (tienePropiedadOrdenamiento) {
      permiteOrdenamiento = !configuracionesColumna.sinOrdenamiento
    }

    return {
      name: propiedad.nombrePropiedad,
      label: startCase(propiedad.nombrePropiedad),
      field: propiedad.nombrePropiedad,
      align: tieneAlineacion ? configuracionesColumna.align : 'center',
      sortable: permiteOrdenamiento,
      tipoVariable: tipoVariable,
      variableForzada: tieneFormatoForzado ? (configuracionesColumna.type ? true : false) : false,
      format: (val) => {
        if (tieneSinFormato) {
          if (configuracionesColumna.sinFormato) {
            return val
          }
        } else if (tieneFormatoForzado) {
          return G3TableUtils.formatearValor(val, lenguaje, configuracionesColumna.type)
        } else {
          return G3TableUtils.formatearValor(val, lenguaje)
        }
      },
      index: propiedad.indexPropiedad,
      sePuedeMover: !tieneSinMovimiento,
      filtroSelector: tieneFiltroSelector
        ? configuracionesColumna.esSelector
          ? true
          : false
        : false,
      unidades: tieneUnidades
        ? configuracionesColumna.unidad
          ? configuracionesColumna.unidad
          : null
        : null,
      llaveTraduccion: tieneTraduccion
        ? configuracionesColumna.llaveTraduccion
          ? configuracionesColumna.llaveTraduccion
          : null
        : null,
      columnasEnColumna: tieneOtrasColumnas
        ? configuracionesColumna.columnas
          ? configuracionesColumna.columnas
          : null
        : null,
      columnasBooleanasEnColumna: tieneOtrasColumnasBooleanas
        ? configuracionesColumna.columnasBooleanas
          ? configuracionesColumna.columnasBooleanas
          : null
        : null,
      columnaSobreOtra: esColumnaSobreOtra,
      columnaBooleanaSobreOtra: esColumnaBooleanaSobreOtra,
      mostrarColumna: mostrarColumna,
      columnaVisible: !tieneInvisibilidad,
      columnaOtrosValores: tieneOtrosValores ? configuracionesColumna : null,
      parametroDivision: tieneDivision
        ? configuracionesColumna.parametroDivision
          ? configuracionesColumna.parametroDivision
          : null
        : null,
      objetoPropiedadesAMostrar: tieneObjetoPropiedadesAMostrar
        ? configuracionesColumna.objetoPropiedadesAMostrar
        : null,
      mostrarDatosArray: mostrarDatosArray,
    }
  })
}

const configurarFiltros = () => {
  const opcionesNumeros = [
    {
      nombre: 'IgualA',
      valor: '=',
    },
    {
      nombre: 'DiferenteDe',
      valor: '!=',
    },
    {
      nombre: 'MayorA',
      valor: '>',
    },
    {
      nombre: 'MenorA',
      valor: '<',
    },
    {
      nombre: 'MayorOIgualA',
      valor: '>=',
    },
    {
      nombre: 'MenorOIgualA',
      valor: '<=',
    },
  ]

  const opcionesBooleanos = [
    {
      nombre: 'Verdadero',
      valor: true,
    },
    {
      nombre: 'Falso',
      valor: false,
    },
  ]

  const opcionesPalabras = [
    {
      nombre: 'Coincide',
      valor: '≡',
    },
    {
      nombre: 'NoCoincide',
      valor: '≢',
    },
    {
      nombre: 'IgualA',
      valor: '=',
    },
    {
      nombre: 'DiferenteDe',
      valor: '!=',
    },
    {
      nombre: 'Contiene',
      valor: '∋',
    },
    {
      nombre: 'NoContiene',
      valor: '∌',
    },
    {
      nombre: 'ComienzaCon',
      valor: '→',
    },
    {
      nombre: 'TerminaCon',
      valor: '←',
    },
  ]

  const opcionesValorDefault = [
    {
      nombre: 'Contiene',
      valor: '∋',
    },
    {
      nombre: 'NoContiene',
      valor: '∌',
    },
  ]

  const opcionesTotales = [
    {
      nombre: 'Suma',
      valor: 'Σ',
    },
    {
      nombre: 'Promedio',
      valor: 'x̄',
    },
    {
      nombre: 'Mayor',
      valor: 'max',
    },
    {
      nombre: 'Menor',
      valor: 'min',
    },
    {
      nombre: 'Mediana',
      valor: 'M',
    },
    {
      nombre: 'Moda',
      valor: 'Mo',
    },
    {
      nombre: 'Rango',
      valor: '∆',
    },
    {
      nombre: 'Varianza',
      valor: 'σ²',
    },
    {
      nombre: 'DesviaciónEstándar',
      valor: 'σ',
    },
  ]

  for (const columna of columnas.value) {
    const propiedad = columna.name
    const tipoVariable = columna.tipoVariable
    const tieneFiltroSelector = columna.filtroSelector

    let valorDefault = ''
    let opcionesDefault = []
    let opcionesValor = []
    let existiraFiltro = false
    let opcionSeleccionada = null
    let miniSearch = null

    let opcionesTotalesColumna = []
    let opcionTotalesSeleccionada = null
    let valorDateOnly = null
    let valorTimeOnly = null

    if (columna.columnaVisible) {
      if (tipoVariable == 'string') {
        existiraFiltro = true

        opcionesDefault = [...opcionesPalabras]
        opcionSeleccionada = opcionesDefault[0]

        if (tieneFiltroSelector) {
          valorDefault = []
          opcionesDefault = opcionesValorDefault
          opcionSeleccionada = opcionesValorDefault[0]

          opcionesValor = [...new Set(datosTabla.value.map((x) => x[propiedad]))].map((valor) => ({
            nombre: valor,
            valor,
          }))
        }

        if (!tieneFiltroSelector) {
          const valoresFilas = datosTabla.value.map((x, index) => {
            return {
              id: index,
              texto: x[propiedad],
            }
          })

          miniSearch = new MiniSearch({
            fields: ['texto'],
            storeFields: ['texto'],
            searchOptions: {
              prefix: true,
              fuzzy: 0.4,
            },
          })

          miniSearch.addAll(valoresFilas)
        }
      } else if (
        tipoVariable == 'number' ||
        tipoVariable == 'dinero' ||
        tipoVariable == 'porcentaje' ||
        tipoVariable == 'Date'
      ) {
        valorDefault = null
        opcionesDefault = opcionesNumeros
        opcionSeleccionada = opcionesNumeros[0]
        existiraFiltro = true

        if (tipoVariable != 'Date') {
          opcionesTotalesColumna = opcionesTotales
          opcionTotalesSeleccionada = opcionesTotales[0]
        }
      } else if (tipoVariable == 'boolean') {
        valorDefault = []
        opcionesDefault = opcionesValorDefault
        opcionesValor = opcionesBooleanos

        //SETEAR EN LAS BUSQUEDAS LOS NOMBRES QUE SE LE COLOCARON
        if (columna.columnaOtrosValores) {
          opcionesValor = [
            {
              nombre: columna.columnaOtrosValores.valorVerdadero,
              valor: true,
            },
            {
              nombre: columna.columnaOtrosValores.valorFalso,
              valor: false,
            },
          ]
        }

        opcionSeleccionada = opcionesValorDefault[0]
        existiraFiltro = true
      }
    }

    //NO SE TOMO EN CUENTA TIPO IMAGEN, ARRAY, OBJECT, UNDEFINED, SYMBOL Y FUNCTION
    filtros.value[propiedad] = {
      tipo: tipoVariable,
      valor: valorDefault,
      opcionesValor: opcionesValor,
      opcionSeleccionada: opcionSeleccionada,
      opciones: opcionesDefault,
      filtrar: existiraFiltro,
      filtroSelector: tieneFiltroSelector,
      valorDefault: valorDefault,
      miniSearch: miniSearch,
      opcionesTotales: opcionesTotalesColumna,
      opcionTotalesSeleccionada: opcionTotalesSeleccionada,
      valorDateOnly: valorDateOnly,
      valorTimeOnly: valorTimeOnly,
    }
  }

  seConfiguraronLosFiltros.value = isEmpty(filtros.value) ? false : true
}

const columnaExisteEnOtraColumna = (nombrePropiedad) => {
  const columnasConColumnas = configuracionesColumnas.value.filter((configuracion) =>
    Object.prototype.hasOwnProperty.call(configuracion, 'columnas'),
  )

  for (let i = 0; i < columnasConColumnas.length; i++) {
    const columna = columnasConColumnas[i]
    if (columna.columnas.includes(nombrePropiedad)) {
      return true // La columna existe dentro de un array de columnas
    }
  }

  return false // La columna no se encuentra en ningún array de columnas
}

const columnaBooleanaExisteEnOtraColumna = (nombrePropiedad) => {
  const columnasConColumnas = configuracionesColumnas.value.filter((configuracion) =>
    Object.prototype.hasOwnProperty.call(configuracion, 'columnasBooleanas'),
  )

  for (let i = 0; i < columnasConColumnas.length; i++) {
    const columna = columnasConColumnas[i]
    if (columna.columnasBooleanas.includes(nombrePropiedad)) {
      return true // La columna existe dentro de un array de columnas
    }
  }

  return false // La columna no se encuentra en ningún array de columnas
}

const onDragStart = (index) => {
  indiceColumnaArrastrada.value = index
}

const onDragOver = (event) => {
  event.preventDefault()
}

const onDrop = (index) => {
  // Verificar si se movió a una posición diferente
  if (index !== indiceColumnaArrastrada.value) {
    // Obtener el nombre de la columna arrastrada y la columna destino

    const propiedadArrastrada = propiedades.value.find(
      (prop) => prop.indexPropiedad == indiceColumnaArrastrada.value,
    )
    const propiedadDestino = propiedades.value.find((prop) => prop.indexPropiedad == index)

    const columnaDestino = columnas.value.find((x) => x.name == propiedadDestino.nombrePropiedad)

    if (!columnaDestino.sePuedeMover) {
      return
    }

    // Intercambiar los nombres de propiedad y los valores en los arrays usando destructuración de arrays
    ;[
      propiedades.value[indiceColumnaArrastrada.value].indexPropiedad,
      propiedades.value[index].indexPropiedad,
    ] = [propiedadDestino.indexPropiedad, propiedadArrastrada.indexPropiedad]

    const filtroArrastrado = filtros.value[propiedadArrastrada.nombrePropiedad]
    const filtroDestino = filtros.value[propiedadDestino.nombrePropiedad]

    // Actualizar los filtros
    filtros.value[propiedadDestino.nombrePropiedad] = filtroDestino
    filtros.value[propiedadArrastrada.nombrePropiedad] = filtroArrastrado

    // Actualizar el índice de la columna arrastrada y orden propiedades
    propiedades.value = sortBy(propiedades.value, (prop) => prop.indexPropiedad)
    indiceColumnaArrastrada.value = index
    configurarColumnasTabla()
  }
}

const metodoParaFiltrados = (filas, objetoFiltro) => {
  if (cargando.value || !seConfiguraronLosFiltros.value) return filas

  const filasEncontradas = filas.filter((fila) => {
    return Object.entries(objetoFiltro).every(([nombre, configuraciones]) => {
      const propiedad = nombre
      const valorFiltro = configuraciones.valor
      const tipoFiltro = configuraciones.tipo
      const seleccionFiltro = configuraciones.opcionSeleccionada
      const valorFila = fila[propiedad]
      const esSelector = configuraciones.filtroSelector
      const valorPreterminado = configuraciones.valorDefault
      const miniSearch = configuraciones.miniSearch

      if (valorFiltro == null || valorFiltro == valorPreterminado || valorFiltro == '') {
        // No se realiza el filtro si el valor del filtro es nulo o igual al valor por defecto
        return true
      }

      if (tipoFiltro === 'string') {
        if (esSelector) {
          if (valorFiltro.length === 0) {
            return true
          }

          if (seleccionFiltro.valor == '∋') {
            return valorFiltro.includes(valorFila)
          }

          return !valorFiltro.includes(valorFila)
        }

        const resultadosBusqueda = miniSearch.search(valorFiltro).map((result) => result.texto)

        switch (seleccionFiltro.valor) {
          case '≡':
            return resultadosBusqueda.includes(valorFila)
          case '≢':
            return !resultadosBusqueda.includes(valorFila)
          case '=':
            return valorFila == valorFiltro
          case '!=':
            return valorFila != valorFiltro
          case '∋':
            return valorFila.includes(valorFiltro)
          case '∌':
            return !valorFila.includes(valorFiltro)
          case '→':
            return valorFila.startsWith(valorFiltro)
          case '←':
            return valorFila.endsWith(valorFiltro)
          default:
            return false
        }

        // return valorFila.includes(valorFiltro);
      } else if (
        tipoFiltro === 'number' ||
        tipoFiltro === 'dinero' ||
        tipoFiltro === 'porcentaje'
      ) {
        switch (seleccionFiltro.valor) {
          case '=':
            return valorFila == valorFiltro
          case '!=':
            return valorFila != valorFiltro
          case '>':
            return valorFila > valorFiltro
          case '<':
            return valorFila < valorFiltro
          case '>=':
            return valorFila >= valorFiltro
          case '<=':
            return valorFila <= valorFiltro
          default:
            return false
        }
      } else if (tipoFiltro === 'Date') {
        const dateValorFila = new Date(valorFila).toLocaleDateString(lenguaje.value, {
          day: '2-digit',
          month: '2-digit',
          year: 'numeric',
        })

        const dateValorFiltro = valorFiltro.toLocaleString(lenguaje.value, {
          day: '2-digit',
          month: '2-digit',
          year: 'numeric',
        })

        switch (seleccionFiltro.valor) {
          case '=':
            return dateValorFila == dateValorFiltro
          case '!=':
            return dateValorFila != dateValorFiltro
          case '>':
            return dateValorFila > dateValorFiltro
          case '<':
            return dateValorFila < dateValorFiltro
          case '>=':
            return dateValorFila >= dateValorFiltro
          case '<=':
            return dateValorFila <= dateValorFiltro
          default:
            return false
        }
      } else if (tipoFiltro === 'boolean') {
        if (valorFiltro.length === 0) {
          return true
        }

        if (seleccionFiltro.valor == '∋') {
          return valorFiltro.includes(valorFila)
        }

        return !valorFiltro.includes(valorFila)
      }

      return true
    })
  })

  return filasEncontradas
}

const limpiarFiltros = () => {
  for (const key in filtros.value) {
    if (Object.prototype.hasOwnProperty.call(filtros.value, key)) {
      const objeto = filtros.value[key]
      // Establecer la propiedad "valor" en el valor predeterminado
      objeto.valor = objeto.valorDefault
    }
  }
}

const reestablecerFiltros = () => {
  for (const key in filtros.value) {
    if (Object.prototype.hasOwnProperty.call(filtros.value, key)) {
      const objeto = filtros.value[key]
      // Establecer la propiedad "valor" en el valor predeterminado
      objeto.opcionSeleccionada = objeto.opciones[0]
    }
  }
}

const reestablecerTotales = () => {
  for (const key in filtros.value) {
    if (Object.prototype.hasOwnProperty.call(filtros.value, key)) {
      const objeto = filtros.value[key]
      // Establecer la propiedad "valor" en el valor predeterminado
      objeto.opcionTotalesSeleccionada = objeto.opcionesTotales[0]
    }
  }
}

const listaValoresTotales = (nombreColumna) => {
  return G3AdvancedTable.value.filteredSortedRows.map((x) => x[nombreColumna])
}

const existenValoresTablaReal = computed(() => {
  if (!G3AdvancedTable.value) {
    return false
  }

  return G3AdvancedTable.value.rows.length > 0
})

const existeValoresTabla = computed(() => {
  if (!G3AdvancedTable.value) {
    return false
  }

  return G3AdvancedTable.value.filteredSortedRows.length > 0
})

//ESTILO TABLA
const separadorTabla = ref('horizontal')

const opcionesSeparador = ref([
  { nombre: 'Horizontal', valor: 'horizontal' },
  { nombre: 'Vertical', valor: 'vertical' },
  { nombre: 'Celda', valor: 'cell' },
  { nombre: 'Ninguno', valor: 'none' },
])

//Exportar Documento
// const exportarDocumentoCSV = () => {
//   // const workbook = XLSX.utils.book_new();
//   // const worksheet = XLSX.utils.json_to_sheet(
//   //   G3AdvancedTable.value.filteredSortedRows
//   // );

//   // XLSX.utils.book_append_sheet(workbook, worksheet, "Data");
//   // XLSX.writeFileXLSX(workbook, "SheetJSVueAoO.xlsx");
// };

//Pantalla Completa
const pantallaCompleta = ref(false)
const establecerPantallaCompleta = (props) => {
  props.toggleFullscreen()
  pantallaCompleta.value = !pantallaCompleta.value
}

//Selecciones
const informacionSeleccionada = ref([])

watch(filasExpandidas, () => {
  // console.log("CAMBIO", filasExpandidas.value)
  if (expandirPorSeleccion.value) {
    // console.log("xd")
    expandirFilasPorArray()
  }
})

const todaInformacionSeleccionada = computed(() => {
  return informacionSeleccionada.value.length >= datosTabla.value.length
})

const actualizarSeleccion = (seleccion) => {
  var registrosSeleccionados
  if (validacionSeleccionRegistro.value != null)
    registrosSeleccionados = validacionSeleccionRegistro.value(seleccion)
  else registrosSeleccionados = seleccion
  informacionSeleccionada.value = registrosSeleccionados
}

const seleccionarTodos = () => {
  if (todaInformacionSeleccionada.value) {
    informacionSeleccionada.value = []
  } else {
    var registrosSeleccionados
    if (validacionSeleccionRegistro.value != null) {
      registrosSeleccionados = validacionSeleccionRegistro.value(datosTabla.value)
    } else {
      registrosSeleccionados = datosTabla.value
    }
    informacionSeleccionada.value = registrosSeleccionados
  }
}

//Expandir
const arrayLlaves = computed(() => {
  if (!G3AdvancedTable.value) {
    return []
  }

  return G3AdvancedTable.value.rows.map((row) => {
    return row[llaveFilaUnica.value]
  })
})

const arrayExpandido = ref([])
const informacionExpandida = ref([])

watch(arrayExpandido, (nuevosDatos, viejosDatos) => {
  if (nuevosDatos === viejosDatos) return

  if (!nuevosDatos) return

  if (nuevosDatos.length == 0) return

  // Limpiamos el array informacionExpandida antes de agregar nuevos datos
  informacionExpandida.value = nuevosDatos.map((elementoExpandido) => {
    return G3AdvancedTable.value?.rows.find((row) => {
      if (row[llaveFilaUnica.value] == elementoExpandido) {
        return row
      }
    })
  })
})

const todasColumnasExpandidas = computed(() => {
  if (!G3AdvancedTable.value) {
    return false
  }

  return arrayLlaves.value.every((llave) => G3AdvancedTable.value.isRowExpanded(llave))
})

const configurarFilasIniciales = () => {
  // console.log("CONFIG INICIAL");
  expandirFilasPorArray()
  seleccionarFilasPorArray()
}

const expandirFilasPorArray = () => {
  // console.log("EXPANDIDAS",filasExpandidas.value);
  // if (filasExpandidas.value.length > 0) {
  const llavesFilasAExpandir = filasExpandidas.value.map((fila) => {
    return fila[llaveFilaUnica.value]
  })

  // G3AdvancedTable.value.setExpanded(llavesFilasAExpandir);
  arrayExpandido.value = llavesFilasAExpandir
  // console.log("000", arrayExpandido.value);

  // }
}

const seleccionarFilasPorArray = () => {
  // console.log("selec", filasSeleccionadas.value);
  if (
    filasSeleccionadas.value.length > 0 &&
    (modoSeleccion.value == 'multiple' || modoSeleccion.value == 'single')
  ) {
    informacionSeleccionada.value = filasSeleccionadas.value
  }
}

const expandirTodos = () => {
  if (todasColumnasExpandidas.value) {
    G3AdvancedTable.value.setExpanded([])
  } else {
    G3AdvancedTable.value.setExpanded(arrayLlaves.value)
  }
}

const existeColumnaParaTotales = () => {
  if (columnas.value.length > 0) {
    const existe = columnas.value.some(
      (columna) =>
        (columna.tipoVariable == 'number' ||
          columna.tipoVariable == 'dinero' ||
          columna.tipoVariable == 'porcentaje') &&
        columna.columnaVisible,
    )

    if (!existe) {
      return false
    }
  }

  return true
}

const mostrarFilaTotales = computed(() => {
  if (ocultarFilaTotales.value) {
    return false
  }

  if (!seConfiguraronLosFiltros.value) {
    return false
  }

  if (cargando.value) {
    return false
  }

  if (!existeValoresTabla.value) {
    return false
  }

  if (!existeColumnaParaTotales.value) {
    return false
  }

  return true
})

const mostrarFilaFiltros = computed(() => {
  if (ocultarFilaFiltros.value) {
    return false
  }

  if (!seConfiguraronLosFiltros.value) {
    return false
  }

  if (cargando.value) {
    return false
  }

  if (!existenValoresTablaReal.value) {
    return false
  }

  return true
})

const mostrarBotonFiltros = computed(() => {
  if (!mostrarFilaTotales.value && !mostrarFilaFiltros.value) {
    return false
  }

  return true
})

const crearArrayDeFilasProporcionar = (longitudArray, longitudNumeroFilasDeseado) => {
  const arraySalida = []

  if (longitudArray >= 100) {
    const paso = Math.ceil(longitudArray / longitudNumeroFilasDeseado)

    for (let i = 1; i < longitudNumeroFilasDeseado; i++) {
      arraySalida.push({
        nombre: `${i * paso}`,
        valor: i * paso,
      })
    }
  }

  if (longitudArray > 10 && longitudArray < 100) {
    arraySalida.push({
      nombre: `${10}`,
      valor: 10,
    })
  }

  if (longitudArray > 20 && longitudArray < 100) {
    arraySalida.push({
      nombre: `${20}`,
      valor: 20,
    })
  }

  if (longitudArray > 30 && longitudArray < 100) {
    arraySalida.push({
      nombre: `${30}`,
      valor: 30,
    })
  }

  if (longitudArray > 40 && longitudArray < 100) {
    arraySalida.push({
      nombre: `${40}`,
      valor: 40,
    })
  }

  if (longitudArray > 50 && longitudArray < 100) {
    arraySalida.push({
      nombre: `${50}`,
      valor: 50,
    })
  }

  if (longitudArray > 60 && longitudArray < 100) {
    arraySalida.push({
      nombre: `${60}`,
      valor: 60,
    })
  }

  if (longitudArray > 70 && longitudArray < 100) {
    arraySalida.push({
      nombre: `${70}`,
      valor: 70,
    })
  }

  if (longitudArray > 80 && longitudArray < 100) {
    arraySalida.push({
      nombre: `${80}`,
      valor: 80,
    })
  }

  if (longitudArray > 90 && longitudArray < 100) {
    arraySalida.push({
      nombre: `${90}`,
      valor: 90,
    })
  }

  arraySalida.push({
    nombre: `${traducir('TodasLasFilas')}`,
    valor: 0,
  })

  return arraySalida
}

//BottomSlot
const configurarFilasPorPaginas = () => {
  const longitudArray = datosTabla.value.length
  const longitudNumeroFilasDeseado = 10

  filasPorPaginas.value = crearArrayDeFilasProporcionar(longitudArray, longitudNumeroFilasDeseado)
}

const configurarPaginacionTabla = () => {
  let paginacion = opcionesPaginacion.value

  if (filasPorPaginas.value.length == 0) {
    paginacion.rowsPerPage = 50
    paginacionTabla.value = paginacion
    return
  }

  if (opcionFilasPorPaginas.value >= 0) {
    paginacion.rowsPerPage = opcionFilasPorPaginas.value
    paginacionTabla.value = paginacion
    return
  }

  paginacion.rowsPerPage = filasPorPaginas.value[0].valor
  paginacionSeleccionada.value = filasPorPaginas.value[0]

  paginacionTabla.value = paginacion
}

//G3 Table estado
const filasFiltradas = computed(() => {
  if (!G3AdvancedTable.value) {
    return []
  }

  if (!G3AdvancedTable.value.filteredSortedRows) {
    return []
  }

  return G3AdvancedTable.value.filteredSortedRows
})

const filasEnVista = computed(() => {
  if (!G3AdvancedTable.value) {
    return []
  }

  return G3AdvancedTable.value.computedRows
})

const numeroFilasEnVista = computed(() => {
  if (!G3AdvancedTable.value) {
    return []
  }

  return G3AdvancedTable.value.computedRowsNumber
})

const filtrosTabla = computed(() => {
  if (!seConfiguraronLosFiltros.value) {
    return []
  }

  const filtrosState = Object.entries(filtros.value).map((filtro) => {
    return {
      propiedad: filtro[0],
      valor: filtro[1].valor,
      modoFiltrado: filtro[1].opcionSeleccionada,
    }
  })

  return filtrosState
})

const configuracionInicialLocalStorage = () => {
  existenDatosLS.value = $q.localStorage.has(`tabla-${identificadorTabla.value}`)

  if (existenDatosLS.value) {
    const datosTablaLS = $q.localStorage.getItem(`tabla-${identificadorTabla.value}`)

    if (datosTablaLS.versionTabla != versionTabla.value) {
      $q.localStorage.remove(`tabla-${identificadorTabla.value}`)
      return
    }

    const sonIguales = verificarPropiedades(
      datosTablaLS.propiedades.map((x) => x.nombrePropiedad),
      propiedades.value.map((x) => x.nombrePropiedad),
    )

    if (!sonIguales) {
      $q.localStorage.remove(`tabla-${identificadorTabla.value}`)
      return
    }

    guardarConfiguracionesTabla.value = datosTablaLS.guardarConfiguracionesTabla
    guardarConfiguracionesFiltros.value = datosTablaLS.guardarConfiguracionesFiltros
    guardarConfiguracionesOrdenDeColumnas.value = datosTablaLS.guardarConfiguracionesOrdenDeColumnas

    if (datosTablaLS.guardarConfiguracionesTabla) {
      columnasUsuario.value = datosTablaLS.columnasUsuario
      separadorTabla.value = datosTablaLS.separadorTabla
    }

    if (datosTablaLS.guardarConfiguracionesOrdenDeColumnas) {
      propiedades.value = datosTablaLS.propiedades
      configurarColumnasTabla()
    }

    //AQUI LO QUE SE VA A RECUPERAR , NO SE PUEDEN RECUPERAR COSAS COMO MINISEARCH
    if (datosTablaLS.guardarConfiguracionesFiltros) {
      actualizarFiltrosPorLocalStorage(datosTablaLS)
    }
  }
}

const verificarPropiedades = (array1, array2) => {
  // Verificar si existe el array
  if (!array1 || !array2) {
    return false
  }

  // Verificar si los tamaños de los arrays son diferentes
  if (array1.length !== array2.length) {
    return false
  }

  // Verificar que cada propiedad de array1 esté en array2
  for (const propiedadArray of array1) {
    if (!array2.includes(propiedadArray)) {
      return false
    }
  }

  // Si no se ha retornado falso hasta este punto, entonces todas las condiciones se cumplen
  return true
}

const actualizarFiltrosPorLocalStorage = (datosTablaLS) => {
  for (const propiedad in filtros.value) {
    if (Object.prototype.hasOwnProperty.call(datosTablaLS.filtros, propiedad)) {
      filtros.value[propiedad].valor = datosTablaLS.filtros[propiedad].valor
      filtros.value[propiedad].opcionSeleccionada =
        datosTablaLS.filtros[propiedad].opcionSeleccionada
      filtros.value[propiedad].opcionTotalesSeleccionada =
        datosTablaLS.filtros[propiedad].opcionTotalesSeleccionada
      filtros.value[propiedad].valorDateOnly = datosTablaLS.filtros[propiedad].valorDateOnly
      filtros.value[propiedad].valorTimeOnly = datosTablaLS.filtros[propiedad].valorTimeOnly
    }
  }
}

watchEffect(() => {
  if (identificadorTabla.value != '' && primeraConfiguracionRealizada.value) {
    const nuevosFiltrosSinMiniSearch = {
      ...filtros.value,
    }

    for (const key in nuevosFiltrosSinMiniSearch) {
      if (Object.hasOwn(nuevosFiltrosSinMiniSearch, key)) {
        nuevosFiltrosSinMiniSearch[key] = {
          ...nuevosFiltrosSinMiniSearch[key],
          miniSearch: null,
        }
      }
    }

    $q.localStorage.set(`tabla-${identificadorTabla.value}`, {
      guardarConfiguracionesTabla: guardarConfiguracionesTabla.value,
      guardarConfiguracionesFiltros: guardarConfiguracionesFiltros.value,
      guardarConfiguracionesOrdenDeColumnas: guardarConfiguracionesOrdenDeColumnas.value,
      columnasUsuario: columnasUsuario.value,
      separadorTabla: separadorTabla.value,
      propiedades: propiedades.value,
      filtros: nuevosFiltrosSinMiniSearch,
      versionTabla: versionTabla.value,
    })
  }
})

defineExpose({
  filtrosTabla,
  columnas,
  filasFiltradas,
  filasEnVista,
  numeroFilasEnVista,
  propiedades,
  filasExpandidas,
  informacionSeleccionada,
  informacionExpandida,
})

const realizandoConfiguracion = ref(false)

onMounted(() => {
  if (
    datosTabla.value.length > 0 &&
    !primeraConfiguracionRealizada.value &&
    !realizandoConfiguracion.value
  ) {
    realizandoConfiguracion.value = true
    configurarPropiedades()
    configurarMostrarFilasDeOpciones()
    configurarColumnasMapeadas()
    configurarColumnasTabla()
    configurarFiltros()
    configurarFilasIniciales()
    configurarFilasPorPaginas()
    configurarPaginacionTabla()

    primeraConfiguracionRealizada.value = true
    configuracionInicialLocalStorage()
    realizandoConfiguracion.value = false
  }
})

onUpdated(() => {
  if (
    datosTabla.value.length > 0 &&
    !primeraConfiguracionRealizada.value &&
    !realizandoConfiguracion.value
  ) {
    realizandoConfiguracion.value = true
    configurarPropiedades()
    configurarMostrarFilasDeOpciones()
    configurarColumnasMapeadas()
    configurarColumnasTabla()
    configurarFiltros()
    configurarFilasIniciales()
    configurarFilasPorPaginas()
    configurarPaginacionTabla()

    primeraConfiguracionRealizada.value = true
    configuracionInicialLocalStorage()
    realizandoConfiguracion.value = false
  }
})
</script>

<style scoped lang="sass">
.fila-filtros
  background-color: #fffffff7

.columna-arrastrada
  background-color: lightblue

.columna-destino
  border-left: 2px solid lightblue
  border-right: 2px solid lightblue

.g3TableSizes
  //Tamaños de la tabla
  min-height: 400px
  max-height: 700px
  max-width: 100%

.g3TableStyle
  .q-table__top,
  .q-table__bottom,
  thead tr:first-child th
    /* bg color is important for th; just specify one */
    background-color: $table-background
    height: 50px
    top: 0

  thead tr:last-child th
    /* bg color is important for th; just specify one */
    background-color: $table-background
    height: 50px
    bottom-: 0

  &.q-table--loading thead tr:last-child th
    /* height of all previous header rows */
    top: 48px

  .q-table tbody .fila-filtros:first-child td
    position: sticky
    z-index: 4
    top: 50px

  td:first-child
    /* bg color is important for td; just specify one */
    z-index: 1

  tr th
    position: sticky
    /* higher than z-index for td below */
    z-index: 2
    /* bg color is important; just specify one */
    background: $table-background

  /* this will be the loading indicator */
  thead tr:last-child th
    /* height of all previous header rows */
    top: 48px
    /* highest z-index */
    z-index: 4

  thead tr:first-child th
    top: 0
    z-index: 1

  tr:first-child th:first-child
    /* highest z-index */
    z-index: 4

  tr:last-child th:last-child
    /* highest z-index */
    z-index: 4

  /* prevent scrolling behind sticky top row on focus */
  tbody
    /* height of all previous header rows */
    scroll-margin-top: 80px

  .q-table tbody .fila-filtros:first-child td:first-child
    position: sticky
    z-index: 4
    background-color: $table-background

.g3ColumnaEstatica
  position: sticky
  left: 0

.g3BackgroundTable
  background-color: $table-background

.g3ColumnaYFilaEstatica
  position: sticky
  left: 0
  bottom: 0

.g3FondoTabla
  background-color: $table-total-background !important

.g3pruebasestilos
  z-index: 8
  position: sticky
  left: 0
  bottom: 0
</style>
