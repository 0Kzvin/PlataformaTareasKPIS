<template>
  <q-card class="q-px-md q-pt-md q-pb-lg bg-fondo3" style="max-width: 770px; max-height: 600px; border-radius: 16px">
    <q-card-section>
      <span class="text-textprimary text-weight-regular text-size-32">
        <template v-if="editar">{{ traducir('EditarTanque') }}</template>
        <template v-else>{{ traducir('CrearTanque') }}</template>
      </span>
      <q-separator class="q-mt-md"></q-separator>
    </q-card-section>
    <div class="q-px-md">
      <div class="row">
        <div class="col-4 q-pr-md">
          <label class="text-textprimary text-size-16">
            {{ traducir('Nombre') + ' *:' }}
            <q-input v-model="nombre" dense filled clearable label-color="textsecondary"
              input-class="text-textsecondary" debounce="200" class="text-size-16 q-pt-sm" :rules="[
                (val) => ((val?.trim() ?? false) && val.length > 0) || traducir('CompletarCampo'),
              ]"></q-input>
          </label>
        </div>
        <div class="col-4 q-pr-md">
          <label class="text-textprimary text-size-16">
            {{ traducir('Apodo') + ':' }}
            <q-input v-model="apodo" dense filled clearable label-color="textsecondary" input-class="text-textsecondary"
              debounce="200" class="text-size-16 q-pt-sm" :rules="[(val) => { }]"></q-input>
          </label>
        </div>
        <div class="col-4">
          <label class="text-textprimary text-size-16">
            {{ traducir('Producto') + ' *:' }}
            <q-select v-model="producto" filled dense clearable option-label="nombre" label-color="textsecondary"
              input-class="text-textsecondary" class="text-size-16 q-pt-sm" popup-content-class="text-textprimary"
              :options="datosProductos" :rules="[(val) => val != null || traducir('CompletarCampo')]">
            </q-select>
          </label>
        </div>
      </div>
      <div class="row">
        <div class="col-4 q-pr-md">
          <label class="text-textprimary text-size-16">
            {{ traducir('Capacidad') + ' *:' }}
            <q-input v-model="capacidad" dense filled clearable type="number" suffix="Kg" label-color="textsecondary"
              input-class="text-textsecondary" debounce="200" class="text-size-16 q-pt-sm" :rules="[
                (val) => (val !== null && val !== '' && !isNaN(val)) || traducir('CompletarCampo'),
                (val) => val > 0 || traducir('DebeSerMayorA0'),
              ]"></q-input>
          </label>
        </div>
        <div class="col-4 q-pr-md">
          <label class="text-textprimary text-size-16">
            {{ traducir('CapacidadOperativa') + ' *:' }}
            <q-input v-model="capacidadOperativa" dense filled clearable suffix="Kg" type="number"
              label-color="textsecondary" input-class="text-textsecondary" debounce="200" class="text-size-16 q-pt-sm"
              :rules="[
                (val) => (val !== null && val !== '' && !isNaN(val)) || traducir('CompletarCampo'),
                (val) => val > 0 || traducir('DebeSerMayorA0'),
                (val) =>
                  Number(val) <= Number(capacidad) ||
                  traducir('LaCapacidadOperativaNoPuedeSerMayorQueLaCapacidadMaxima'),
              ]"></q-input>
          </label>
        </div>
        <div class="col-4">
          <label class="text-textprimary text-size-16">
            {{ traducir('Altura') + ' *:' }}
            <q-input v-model="altura" dense filled suffix="mm" clearable type="number" label-color="textsecondary"
              input-class="text-textsecondary" debounce="200" class="text-size-16 q-pt-sm" :rules="[
                (val) => (val !== null && val !== '' && !isNaN(val)) || traducir('CompletarCampo'),
                (val) => val > 0 || traducir('DebeSerMayorA0'),
              ]"></q-input>
          </label>
        </div>
      </div>
      <div class="row">
        <div class="col-4 q-pr-md">
          <label class="text-textprimary text-size-16">
            {{ traducir('AlturaOperativa') + ' *:' }}
            <q-input v-model="alturaOperativa" dense filled clearable suffix="mm" type="number"
              label-color="textsecondary" input-class="text-textsecondary" debounce="200" class="text-size-16 q-pt-sm"
              :rules="[
                (val) => (val !== null && val !== '' && !isNaN(val)) || traducir('CompletarCampo'),
                (val) => val > 0 || traducir('DebeSerMayorA0'),
                (val) =>
                  Number(val) <= Number(altura) ||
                  traducir('LaAlturaOperativaNoPuedeSerMayorQueLaAlturaMaxima'),
              ]"></q-input>
          </label>
        </div>
        <div class="col-4 q-pr-md">
          <label class="text-textprimary text-size-16">
            {{ traducir('LimiteMaximo') + ' *:' }}
            <q-input v-model="limiteMaximo" dense filled clearable suffix="mm" type="number" label-color="textsecondary"
              input-class="text-textsecondary" debounce="200" class="text-size-16 q-pt-sm" :rules="[
                (val) => (val !== null && val !== '' && !isNaN(val)) || traducir('CompletarCampo'),
                (val) => val > 0 || traducir('DebeSerMayorA0'),
                (val) =>
                  Number(val) <= Number(capacidad) || traducir('NoPuedeSerMayorALaCapacidadMaximo'),
              ]"></q-input>
          </label>
        </div>
        <div class="col-4">
          <label class="text-textprimary text-size-16">
            {{ traducir('LimiteAlto') + ' *:' }}
            <q-input v-model="limiteAlto" dense filled clearable suffix="mm" type="number" label-color="textsecondary"
              input-class="text-textsecondary" debounce="200" class="text-size-16 q-pt-sm" :rules="[
                (val) => (val !== null && val !== '' && !isNaN(val)) || traducir('CompletarCampo'),
                (val) => val > 0 || traducir('DebeSerMayorA0'),
                (val) =>
                  Number(val) <= Number(limiteMaximo) || traducir('NoPuedeSerMayorALimiteMaximo'),
              ]"></q-input>
          </label>
        </div>
      </div>
      <div class="row">
        <div class="col-4 q-pr-md">
          <label class="text-textprimary text-size-16">
            {{ traducir('LimiteBajo') + ' *:' }}
            <q-input v-model="limiteBajo" dense filled clearable suffix="mm" type="number" label-color="textsecondary"
              input-class="text-textsecondary" debounce="200" class="text-size-16 q-pt-sm" :rules="[
                (val) => (val !== null && val !== '' && !isNaN(val)) || traducir('CompletarCampo'),
                (val) =>
                  Number(val) <= Number(limiteAlto) || traducir('NoPuedeSerMayorALimiteAlto'),
              ]"></q-input>
          </label>
        </div>
        <div class="col-4 q-pr-md">
          <label class="text-textprimary text-size-16">
            {{ traducir('LimiteMinimo') + ' *:' }}
            <q-input v-model="limiteMinimo" dense filled clearable suffix="mm" type="number" label-color="textsecondary"
              input-class="text-textsecondary" debounce="200" class="text-size-16 q-pt-sm" :rules="[
                (val) => (val !== null && val !== '' && !isNaN(val)) || traducir('CompletarCampo'),
                (val) =>
                  Number(val) <= Number(limiteBajo) || traducir('NoPuedeSerMayorALimiteBajo'),
              ]"></q-input>
          </label>
        </div>
        <div class="col-4">
          <label class="text-textprimary text-size-16">
            {{ traducir('Ubicacion') + ' *:' }}
            <q-input v-model="ubicacion" dense filled clearable label-color="textsecondary"
              input-class="text-textsecondary" debounce="200" class="text-size-16 q-pt-sm" :rules="[
                (val) => ((val?.trim() ?? false) && val.length > 0) || traducir('CompletarCampo'),
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
            style="height: 44px" @click="enviarFormulario">
            <div class="row items-center" v-if="editar">
              <i class="pi pi-pencil q-mr-md text-size-20"></i>
              <span class="text-size-16"> {{ traducir('EditarTanque') }}</span>
            </div>
            <div class="row items-center" v-else>
              <i class="pi pi-plus-circle q-mr-md text-size-20"></i>
              <span class="text-size-16"> {{ traducir('CrearTanque') }}</span>
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
import { productosGerencia } from 'src/api/moduloGerencia'
import { tanquesAlmacenamiento } from 'src/api/moduloAlmacenamiento'

const traducir = inject('traducir', (key) => key)
const emits = defineEmits(['onDialogCancel', 'onDialogHide', 'onDialogOK'])

const props = defineProps({
  propsCustomForm: {
    type: Object,
    default: () => {
      return {
        refrescarDatos: () => { },
        editar: false,
        modelo: {},
      }
    },
  },
})

const {
  propsCustomForm: { editar, modelo, refrescarDatos },
} = props

const id = editar ? ref(modelo.id) : ref('')
const nombre = editar ? ref(modelo.nombre) : ref('')
const apodo = editar ? ref(modelo.apodo) : ref('')
const ubicacion = editar ? ref(modelo.ubicacion) : ref('')
const capacidad = editar ? ref(modelo.capacidadMaxima) : ref(0)
const capacidadOperativa = editar ? ref(modelo.capacidadOperativa) : ref(0)
const altura = editar ? ref(modelo.alturaMaxima) : ref(0)
const alturaOperativa = editar ? ref(modelo.alturaOperativa) : ref(0)
const limiteMaximo = editar ? ref(modelo.limiteMaximo) : ref(0)
const limiteAlto = editar ? ref(modelo.limiteAlto) : ref(0)
const limiteBajo = editar ? ref(modelo.limiteBajo) : ref(0)
const limiteMinimo = editar ? ref(modelo.limiteMinimo) : ref(0)
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
  if (!nombre.value || !ubicacion.value || !producto.value) {
    await quasarUtils.aviso({
      error: true,
      mensaje: traducir('LLeneLosDatosRequeridos'),
    })

    return
  }

  validarCapacidades()

  validarAltura()

  validarLimites()

  const deposito = {
    id: id.value,
    nombre: nombre.value,
    apodo: apodo.value,
    idProducto: producto.value.idUnico,
    ubicacion: ubicacion.value,
    capacidadMaxima: capacidad.value,
    capacidadOperativa: capacidadOperativa.value,
    alturaMaxima: altura.value,
    alturaOperativa: alturaOperativa.value,
    limiteMaximo: limiteMaximo.value,
    limiteAlto: limiteAlto.value,
    limiteBajo: limiteBajo.value,
    limiteMinimo: limiteMinimo.value,
  }

  let resp = null

  quasarUtils.cargandoSimple()
  if (editar) {
    resp = await tanquesAlmacenamiento.modificar(deposito)
  } else {
    resp = await tanquesAlmacenamiento.registrar(deposito)
  }
  quasarUtils.ocultarCargandoSimple()

  if (resp.exito) {
    await quasarUtils.aviso({
      exito: true,
      mensaje: resp.payload.mensaje,
    })

    if (refrescarDatos) {
      await refrescarDatos()
      emits('onDialogCancel')
    }
  } else {
    await quasarUtils.aviso({
      error: true,
      mensaje: resp.payload.mensaje,
    })
  }
}

const validarCapacidades = async () => {
  if (capacidad.value < 1 || capacidadOperativa.value < 1) {
    await quasarUtils.aviso({
      error: true,
      mensaje: traducir('LasCapacidadesTienenQueSerMayoresQue0'),
    })

    return
  }

  if (capacidadOperativa.value > capacidad.value) {
    await quasarUtils.aviso({
      error: true,
      mensaje: traducir('LaCapacidadOperativaNoPuedeSerMayorQueLaCapacidadMaxima'),
    })

    return
  }
}

const validarAltura = async () => {
  if (altura.value < 1 || alturaOperativa.value < 1) {
    await quasarUtils.aviso({
      error: true,
      mensaje: traducir('LasAlturasTienenQueSerMayoresQue0'),
    })

    return
  }

  if (alturaOperativa.value > altura.value) {
    await quasarUtils.aviso({
      error: true,
      mensaje: traducir('LaAlturaOperativaNoPuedeSerMayorQueLaAlturaMaxima'),
    })

    return
  }
}

const validarLimites = async () => {
  const max = limiteMaximo.value
  const alto = limiteAlto.value
  const bajo = limiteBajo.value
  const min = limiteMinimo.value

  if ([max, alto, bajo, min].some((v) => v === null || v === undefined || isNaN(v))) {
    await quasarUtils.aviso({
      error: true,
      mensaje: traducir('TodosLosLimitesDebenTenerUnValorNumerico'),
    })
    return false
  }

  if (max < 1 || alto < 1 || bajo < 0 || min < 0) {
    await quasarUtils.aviso({
      error: true,
      mensaje: traducir('ConfigureLimitesValidosParaEsteDeposito'),
    })
    return false
  }

  if (alto > max) {
    await quasarUtils.aviso({
      error: true,
      mensaje: traducir('ElLimiteAltoNoPuedeSerMayorQueElLimiteMaximo'),
    })
    return false
  }

  if (bajo > alto) {
    await quasarUtils.aviso({
      error: true,
      mensaje: traducir('ElLimiteBajoNoPuedeSerMayorQueElLimiteAlto'),
    })
    return false
  }

  if (min > bajo) {
    await quasarUtils.aviso({
      error: true,
      mensaje: traducir('ElLimiteMinimoNoPuedeSerMayorQueElLimiteBajo'),
    })
    return false
  }

  if (max <= min) {
    await quasarUtils.aviso({
      error: true,
      mensaje: traducir('ElLimiteMaximoDebeSerMayorQueElLimiteMinimo'),
    })
    return false
  }

  return true
}
</script>

<style lang="scss" scoped></style>
