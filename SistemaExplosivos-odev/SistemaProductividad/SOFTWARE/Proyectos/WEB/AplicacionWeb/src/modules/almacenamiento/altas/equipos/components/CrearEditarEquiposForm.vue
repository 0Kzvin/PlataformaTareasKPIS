<template>
  <q-card class="q-px-md q-pt-md q-pb-lg bg-fondo3" style="max-width: 530px; max-height: 512px; border-radius: 16px">
    <q-card-section>
      <span class="text-textprimary text-weight-regular text-size-32">
        <template v-if="editar">{{ traducir('EditarEquipo') }}</template>
        <template v-else>{{ traducir('CrearEquipo') }}</template>
      </span>
      <q-separator class="q-mt-md"></q-separator>
    </q-card-section>
    <div class="q-px-md">
      <div class="row">
        <div class="col-6 q-pr-md">
          <label class="text-textprimary text-size-16">
            {{ traducir('NumeroEconomico') + ' *:' }}
            <q-input v-model="numeroEconomico" dense filled clearable label-color="textsecondary"
              input-class="text-textsecondary" debounce="200" class="text-size-16 q-pt-sm" :rules="[
                (val) => ((val?.trim() ?? false) && val.length > 0) || traducir('CompletarCampo'),
              ]"></q-input>
          </label>
        </div>
        <div class="col-6">
          <label class="text-textprimary text-size-16">
            {{ traducir('Apodo') + ':' }}
            <q-input v-model="apodo" dense filled clearable label-color="textsecondary" input-class="text-textsecondary"
              debounce="200" class="text-size-16 q-pt-sm"></q-input>
          </label>
        </div>
      </div>
      <div class="row">
        <div class="col-6 q-pr-md">
          <label class="text-textprimary text-size-16">
            {{ traducir('Producto') + ':' }}
            <q-select v-model="producto" filled dense clearable option-label="nombre" label-color="textsecondary"
              input-class="text-textsecondary" class="text-size-16 q-pt-sm" popup-content-class="text-black"
              :options="datosProductos" :rules="[(val) => val != null || traducir('CompletarCampo')]">
            </q-select>
          </label>
        </div>
        <div class="col-6">
          <label class="text-textprimary text-size-16">
            {{ traducir('Tipo') + ' *:' }}
            <q-select v-model="esExterno" dense filled clearable emit-value map-options option-value="valor"
              option-label="label" :options="opcionesTipos" popup-content-class="text-black" label-color="textsecondary"
              input-class="text-textsecondary" debounce="200" class="text-size-16 q-pt-sm" />
          </label>
        </div>
      </div>
      <div class="row">
        <div class="col-6 q-pr-md">
          <label class="text-textprimary text-size-16">
            {{ traducir('CantidadActual') + ' *:' }}
            <q-input v-model="cantidadActual" dense filled type="number" clearable label-color="textsecondary"
              input-class="text-textsecondary" debounce="200" class="text-size-16 q-pt-sm" :rules="[
                (val) => (val !== null && val !== '' && !isNaN(val)) || traducir('CompletarCampo'),
                (val) => val >= 0 || traducir('Debe ser positivo'),
              ]"></q-input>
          </label>
        </div>
        <div class="col-6">
          <label class="text-textprimary text-size-16">
            {{ traducir('Capacidad') + ' *:' }}
            <q-input v-model="capacidad" dense filled clearable type="number" label-color="textsecondary"
              input-class="text-textsecondary" debounce="200" class="text-size-16 q-pt-sm" :rules="[
                (val) => (val !== null && val !== '' && !isNaN(val)) || traducir('CompletarCampo'),
                (val) => val > 0 || traducir('Debe ser mayor a 0'),
              ]"></q-input>
          </label>
        </div>
      </div>
      <div class="row full-width q-pt-md">
        <div class="col-6 q-pr-sm">
          <q-btn outline class="full-width text-weight-light" color="negative" @click="cancelarForm()"
            style="height: 44px" no-caps>
            <i class="pi pi-times q-mr-md text-size-20"></i>
            <span class="text-size-16"> {{ traducir('Cancelar') }}</span>
          </q-btn>
        </div>
        <div class="col-6 q-pl-sm">
          <q-btn color="primary" no-caps class="full-width text-weight-light text-primary-contrast" type="submit"
            @click="enviarFormulario()" style="height: 44px">
            <div class="row items-center" v-if="editar">
              <i class="pi pi-pencil q-mr-md text-size-20"></i>
              <span class="text-size-16"> {{ traducir('EditarEquipo') }}</span>
            </div>
            <div class="row items-center" v-else>
              <i class="pi pi-plus-circle q-mr-md text-size-20"></i>
              <span class="text-size-16"> {{ traducir('CrearEquipo') }}</span>
            </div>
          </q-btn>
        </div>
      </div>
    </div>
  </q-card>
</template>

<script setup>
import { onMounted, ref, inject } from 'vue'
import * as quasarUtils from 'src/utils/quasar-utils.js'
import { equiposAlmacenamiento } from 'src/api/moduloAlmacenamiento'
import { productosGerencia } from 'src/api/moduloGerencia'

const traducir = inject('traducir', (key) => key)
const emits = defineEmits(['onDialogCancel', 'onDialogHide', 'onDialogOK'])

const props = defineProps({
  propsCustomForm: {
    type: Object,
    default: () => {
      return {
        refrescarDatos: () => { },
        modelo: {},
        editar: false,
      }
    },
  },
})

const {
  propsCustomForm: { refrescarDatos, editar, modelo },
} = props

const id = editar ? ref(modelo.id) : ref('')
const numeroEconomico = editar ? ref(modelo.numeroEconomico) : ref('')
const apodo = editar ? ref(modelo.apodo) : ref('')
const cantidadActual = editar ? ref(modelo.cantidadActual) : ref(0)
const capacidad = editar ? ref(modelo.capacidad) : ref(0)
const esExterno = editar ? ref(modelo.esExterno) : ref(false)
const producto = editar ? ref(modelo.productoDTO) : ref(null)

const cancelarForm = () => {
  emits('onDialogCancel')
}

onMounted(async () => {
  await refrescarProductos()
})

const cargandoProductos = ref(false)
const datosProductos = ref([])

const refrescarProductos = async () => {
  cargandoProductos.value = true
  const resp = await productosGerencia.listar()
  cargandoProductos.value = false

  if (!resp.exito) {
    datosProductos.value = []
    return
  }

  datosProductos.value = resp.payload.respuesta
}

const enviarFormulario = async () => {
  if (!numeroEconomico.value || !producto.value) {
    await quasarUtils.aviso({
      error: true,
      mensaje: traducir('LlenarDatosRequeridos'),
    })
    return
  }

  if (capacidad.value < 0) {
    await quasarUtils.aviso({
      error: true,
      mensaje: traducir('La capacidad del equipo debe de ser mayor a 0'),
    })
    return
  }

  if (cantidadActual.value < 0) {
    await quasarUtils.aviso({
      error: true,
      mensaje: traducir('La cantidad actual del equipo debe de ser mayor a 0'),
    })
    return
  }

  const modeloAEnviar = {
    id: id.value,
    apodo: apodo.value,
    numeroEconomico: numeroEconomico.value,
    cantidadActual: cantidadActual.value,
    capacidad: capacidad.value,
    esExterno: esExterno.value,
    idProducto: producto.value.idUnico,
  }

  let resp = null

  quasarUtils.cargandoSimple()
  if (editar) {
    resp = await equiposAlmacenamiento.modificar(modeloAEnviar)
  } else {
    resp = await equiposAlmacenamiento.registrar(modeloAEnviar)
  }
  quasarUtils.ocultarCargandoSimple()

  if (resp.exito) {
    await quasarUtils.aviso({
      exito: true,
      mensaje: resp.payload.mensaje,
    })

    if (refrescarDatos) {
      await refrescarDatos()
      cancelarForm()
    }
  } else {
    await quasarUtils.aviso({
      error: true,
      mensaje: resp.payload.mensaje,
    })
  }
}

const opcionesTipos = [
  {
    label: traducir('Local'),
    valor: false,
  },
  {
    label: traducir('Externo'),
    valor: true,
  },
]
</script>

<style lang="scss" scoped></style>
