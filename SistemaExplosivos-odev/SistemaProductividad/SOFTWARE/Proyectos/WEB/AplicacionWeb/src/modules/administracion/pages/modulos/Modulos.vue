<!-- eslint-disable vue/multi-word-component-names -->
<template>
  <q-page>
    <G3ModuloHeader :modulo="traducir('Modulos')" />

    <div class="row q-mt-md">
      <div class="col-12">
        <G3CustomTable :columnasVisibles="['opciones', 'nombreModulo', 'descripcion', 'estado']" :columnas="[
          {
            name: 'id',
            label: '',
            field: (modulos) => modulos.id,
          },
          {
            name: 'opciones',
            label: traducir('Opciones'),
            align: 'center',
          },
          {
            name: 'nombreModulo',
            label: traducir('Nombre'),
            align: 'center',
            field: (modulos) => traducir(modulos.nombreNormalizado, modulos.nombre),
          },
          {
            name: 'descripcion',
            label: traducir('Descripcion'),
            align: 'center',
            field: (modulos) => traducir(modulos.descripcion, '-'),
          },
          {
            name: 'estado',
            label: traducir('Estado'),
            align: 'center',
          },
        ]" :filas="modulos" :cargando="cargandoTablaModulos" :filasPorPagina="10" :filtros="{
            nombreModulo: '',
            descripcion: '',
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
          }">
          <template v-slot:col-end-estado="{ props }">
            <q-chip dense square :color="props.row.estado ? 'green-1' : 'red-1'"
              :text-color="props.row.estado ? 'green-9' : 'red-8'"
              class="text-weight-bold q-px-md q-py-xs rounded-badge">
              {{ props.row.estado ? traducir('Activado') : traducir('Desactivado') }}
            </q-chip>
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
import { onMounted, ref, inject } from 'vue'
import { modulosGFuelAdmin } from 'src/api/moduloAdministracion'
import G3CustomTable from 'src/components/Globales/G3CustomTable.vue'
import BotonOpciones from 'src/components/Paginas/General/OpcionesPage/BotonOpciones.vue'
import * as quasarUtils from 'src/utils/quasar-utils.js'
import { refreshSource } from 'src/utils/utils'
import G3ModuloHeader from 'src/components/Genericos/G3ModuloHeader.vue'

const traducir = inject('traducir')
const modulos = ref([])
const cargandoTablaModulos = ref(false)

const refrescarModulos = async () => {
  cargandoTablaModulos.value = true
  const resp = await modulosGFuelAdmin.listarModulos()
  cargandoTablaModulos.value = false
  if (!resp.exito) {
    modulos.value = []
    return
  }
  modulos.value = resp.payload
}

const cambiarEstadoRol = async (idModulo, estado, nombreModulo) => {
  const decision = await quasarUtils.decision({
    titulo: `${traducir('Modulo')}: ${nombreModulo}`,
    mensaje: estado ? traducir('DesactivarRegistroPregunta') : traducir('ActivarRegistroPregunta'),
    icono: 'secondary',
    iconoColor: estado ? 'secondary' : 'positive',
  })
  if (decision) {
    quasarUtils.cargandoSimple()
    const resp = await modulosGFuelAdmin.cambiarEstado(idModulo)
    quasarUtils.ocultarCargandoSimple()
    if (!resp.exito) {
      quasarUtils.aviso({
        error: true,
        mensaje: resp.payload.errores[0],
      })
      return
    }
    refrescarModulos()
    await quasarUtils.aviso({
      exito: true,
      mensaje: estado
        ? traducir('RegistroDesactivadoConExitoMensaje')
        : traducir('RegistroActivadoConExitoMensaje'),
    })
  }
}

onMounted(async () => {
  refreshSource()
  refrescarModulos()
})

const llenarArrayOpciones = (props) => {
  const arrayOpciones = []

  arrayOpciones.push({
    titulo: traducir('Estado'),
    descripcion: props.row.estado ? traducir('DesactivarRegistro') : traducir('ActivarRegistro'),
    icono: props.row.estado ? 'fa fa-ban' : 'fa fa-check',
    color: props.row.estado ? 'negative' : 'positive',
    accion: () =>
      cambiarEstadoRol(
        props.row.id,
        props.row.estado,
        traducir(props.row.nombreNormalizado, props.row.nombre),
      ),
  })

  return arrayOpciones
}
</script>
