<template>
  <q-card class="q-px-md q-pt-md q-pb-lg bg-fondo3" style="max-width: 770px; max-height: 488px; border-radius: 16px">
    <q-card-section>
      <span class="text-textprimary text-weight-regular text-size-32">
        <template v-if="editar">{{ traducir('EditarMovimiento') }}</template>
        <template v-else>{{ traducir('CrearMovimiento') }}</template>
      </span>
      <q-separator class="q-mt-md"></q-separator>
    </q-card-section>
    <q-form @submit="enviarFormulario">
      <div class="q-px-md">
        <div class="row">
          <div class="col-4 q-pr-md">
            <label class="text-textprimary text-size-16">
              {{ traducir('Producto') + ' *:' }}
              <q-select v-model="producto" filled dense clearable option-label="nombre" label-color="textsecondary"
                input-class="text-textsecondary" class="text-size-16 q-pt-sm" popup-content-class="text-textprimary"
                :options="datosProductos" :rules="[(val) => val != null || traducir('CompletarCampo')]">
              </q-select>
            </label>
          </div>
          <div class="col-4 q-pr-md">
            <label class="text-textprimary text-size-16">
              {{ traducir('Ubicacion') + ' *:' }}
              <q-input v-model="ubicacion" dense filled clearable label-color="textsecondary"
                input-class="text-textsecondary" debounce="200" class="text-size-16 q-pt-sm" :rules="[
                  (val) => ((val?.trim() ?? false) && val.length > 0) || traducir('CompletarCampo'),
                ]"></q-input>
            </label>
          </div>
          <div class="col-4">
            <label class="text-textprimary text-size-16">
              {{ traducir('Observaciones') + ':' }}
              <q-input v-model="observaciones" dense filled clearable label-color="textsecondary"
                input-class="text-textsecondary" debounce="200" class="text-size-16 q-pt-sm"></q-input>
            </label>
          </div>
        </div>
        <div class="row">
          <div class="col-4 q-pr-md">
            <label class="text-textprimary text-size-16">
              {{ traducir('CantidadInicial') + ' *:' }}
              <q-input v-model="cantidadInicial" dense filled clearable type="number" suffix="Kg"
                label-color="textsecondary" input-class="text-textsecondary" debounce="200" class="text-size-16 q-pt-sm"
                :rules="[
                  (val) =>
                    (val !== null && val !== '' && !isNaN(val)) || traducir('CompletarCampo'),
                  (val) => val > 0 || traducir('DebeSerMayorA0'),
                ]"></q-input>
            </label>
          </div>
          <div class="col-4 q-pr-md">
            <label class="text-textprimary text-size-16">
              {{ traducir('Movimiento') + ' *:' }}
              <q-input v-model="cantidadMovimiento" dense filled clearable type="number" suffix="Kg"
                class="text-size-16 q-pt-sm" :rules="[
                  (val) =>
                    (val !== null && val !== '' && !isNaN(val)) || traducir('CompletarCampo'),
                  (val) => val != 0 || traducir('DebeSerDistintoDe0'),
                  (val) => {
                    const inicial = Number(cantidadInicial)
                    const movimiento = Number(val)

                    if (isNaN(inicial) || isNaN(movimiento)) return true

                    return (
                      inicial + movimiento >= 0 || traducir('NoPuedeRetirarMasDeLaCantidadInicial')
                    )
                  },
                ]" />
            </label>
          </div>
          <div class="col-4">
            <label class="text-textprimary text-size-16">
              {{ traducir('CantidadFinal') + ':' }}
              <q-input v-model="cantidadFinal" dense filled clearable readonly type="number" suffix="Kg"
                label-color="textsecondary" input-class="text-textsecondary" debounce="200" class="text-size-16 q-pt-sm"
                :rules="[
                  (val) =>
                    (val !== null && val !== '' && !isNaN(val)) || traducir('CompletarCampo'),
                  (val) => val >= 0 || traducir('DebeSerMayorA0'),
                ]"></q-input>
            </label>
          </div>
        </div>
        <div class="row">
          <div class="col-4 q-pr-md">
            <label class="text-textprimary text-size-16">
              {{ traducir('Fecha') }}:
              <q-input dense filled bg-color="primarylight" input-class="text-textsecondary" clearable
                label-color="textsecondary" class="text-size-16 q-pt-sm" v-model="fecha">
                <template v-slot:prepend>
                  <q-icon name="far fa-calendar" class="cursor-pointer" size="xs">
                    <q-tooltip class="text-size-16"> {{ traducir('SeleccionarFecha') }} </q-tooltip>
                    <q-popup-proxy cover transition-show="scale" transition-hide="scale">
                      <q-date v-model="fecha" today-btn mask="DD/MM/YYYY" class="bg-fondo2 text-textsecondary">
                        <div class="row items-center justify-end">
                          <div class="row items-center justify-end">
                            <q-btn v-close-popup :label="traducir('Guardar')" class="text-bold" color="primary" />
                          </div>
                        </div>
                      </q-date>
                    </q-popup-proxy>
                  </q-icon>
                </template>
              </q-input>
            </label>
          </div>
          <div class="col-4 q-pr-md">
            <label class="text-textprimary text-size-16">
              {{ traducir('Hora') }}:
              <q-input filled dense v-model="hora" input-class="text-textsecondary" label-color="textsecondary"
                class="text-size-16 q-pt-sm" @keydown.prevent>
                <template #append>
                  <q-icon name="schedule" class="cursor-pointer" color="textsecondary">
                    <q-tooltip class="text-size-14">
                      {{ traducir('SeleccioneUnaHora') }}
                    </q-tooltip>
                    <q-popup-proxy cover transition-show="scale" transition-hide="scale">
                      <q-time v-model="hora" class="bg-fondo2 text-textsecondary" format24h mask="HH:mm" />
                    </q-popup-proxy>
                  </q-icon>
                </template>
              </q-input>
            </label>
          </div>
        </div>
        <div class="row full-width q-pt-lg">
          <div class="col-6 q-pr-sm">
            <q-btn outline class="full-width text-weight-light" color="negative" @click="cancelarForm()"
              style="height: 44px" no-caps>
              <i class="pi pi-times q-mr-md text-size-20"></i>
              <span class="text-size-16"> {{ traducir('Cancelar') }}</span>
            </q-btn>
          </div>
          <div class="col-6 q-pl-sm">
            <q-btn color="primary" no-caps class="full-width text-weight-light text-primary-contrast" type="submit"
              style="height: 44px">
              <div class="row items-center" v-if="editar">
                <i class="pi pi-pencil q-mr-md text-size-20"></i>
                <span class="text-size-16"> {{ traducir('EditarMovimiento') }}</span>
              </div>
              <div class="row items-center" v-else>
                <i class="pi pi-plus-circle q-mr-md text-size-20"></i>
                <span class="text-size-16"> {{ traducir('CrearMovimiento') }}</span>
              </div>
            </q-btn>
          </div>
        </div>
      </div>
    </q-form>
  </q-card>
</template>

<script setup>
import { productosGerencia } from 'src/api/moduloGerencia'
import { computed, onMounted, ref, watchEffect, inject } from 'vue'
import * as quasarUtils from 'src/utils/quasar-utils.js'
import { supersacosAlmacenamiento } from 'src/api/moduloAlmacenamiento'
import { date } from 'quasar'

const traducir = inject('traducir', (key) => key)

const emits = defineEmits(['onDialogCancel', 'onDialogHide', 'onDialogOK'])

const props = defineProps({
  propsCustomForm: {
    type: Object,
    default: () => {
      return {
        refrescarDatos: () => { },
        editar: false,
        cantidadInicialModelo: 0,
        modelo: {},
      }
    },
  },
})

const {
  propsCustomForm: { editar, modelo, cantidadInicialModelo, refrescarDatos },
} = props

const id = editar ? ref(modelo.id) : ref('')
const ubicacion = editar ? ref(modelo.ubicacion) : ref('')
const observaciones = editar ? ref(modelo.observaciones) : ref('')
const cantidadInicial = editar
  ? ref(modelo.cantidadInicial)
  : cantidadInicialModelo
    ? ref(cantidadInicialModelo)
    : ref(0)

const cantidadMovimiento = editar ? ref(modelo.cantidadMovimiento) : ref(0)
const fechaHora = editar ? ref(modelo.fechaHora) : ref('')
const fecha = ref('')
const hora = ref('')
const producto = editar ? ref(modelo.productoDTO) : ref(null)

const cantidadFinal = computed(() => {
  const inicial = Number(cantidadInicial.value)
  const movimiento = Number(cantidadMovimiento.value)

  if (isNaN(inicial) || isNaN(movimiento)) {
    return null
  }

  return inicial + movimiento || 0
})

const cancelarForm = () => {
  emits('onDialogCancel')
}

onMounted(async () => {
  if (!editar) {
    obtenerHoraActual()
  }

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
    producto.value = null
    return
  }

  datosProductos.value = resp.payload.respuesta

  if (!editar) {
    const productoNitrato = datosProductos.value.find((p) =>
      p.nombre?.toLowerCase().includes('nitr'),
    )

    producto.value = productoNitrato || null
  }
}

const enviarFormulario = async () => {
  fechaHora.value = construirFechaHoraISO(fecha.value, hora.value)
  console.log(fechaHora.value)

  const movimiento = {
    id: id.value,
    idProducto: producto.value.idUnico,
    ubicacion: ubicacion.value,
    observaciones: observaciones.value,
    cantidadInicial: cantidadInicial.value,
    cantidadFinal: cantidadFinal.value,
    cantidadMovimiento: cantidadMovimiento.value,
    fechaHora: fechaHora.value,
  }

  let resp = null

  quasarUtils.cargandoSimple()
  if (editar) {
    resp = await supersacosAlmacenamiento.modificar(movimiento)
  } else {
    resp = await supersacosAlmacenamiento.registrar(movimiento)
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

const obtenerHoraActual = () => {
  const ahora = new Date()
  const horas = ahora.getHours().toString().padStart(2, '0')
  const minutos = ahora.getMinutes().toString().padStart(2, '0')

  if (!editar) hora.value = `${horas}:${minutos}`
  if (!editar) fecha.value = date.formatDate(ahora, 'DD/MM/YYYY')
}

const parseFechaHora = (str) => {
  if (!str) return { fecha: '', hora: '' }

  const [fechaParte, horaParte, meridiano] = str.split(' ')
  const [dia, mes, anioCorto] = fechaParte.split('/')
  let [horas, minutos] = horaParte.split(':').map(Number)

  // Convertir a formato 24h
  if (meridiano === 'PM' && horas !== 12) horas += 12
  if (meridiano === 'AM' && horas === 12) horas = 0

  return {
    fecha: `20${anioCorto}-${mes.padStart(2, '0')}-${dia.padStart(2, '0')}`,
    hora: `${String(horas).padStart(2, '0')}:${String(minutos).padStart(2, '0')}`,
  }
}

watchEffect(() => {
  if (!editar || !fechaHora.value) return

  const { fecha: f, hora: h } = parseFechaHora(fechaHora.value)
  fecha.value = f
  hora.value = h
})

const construirFechaHoraISO = (fecha, hora) => {
  if (!fecha || !hora) return null

  console.log('Construyendo Fecha:', fecha, 'Hora:', hora)

  if (fecha.includes('/')) {
    const [dia, mes, anio] = fecha.split('/')
    return `${anio}-${mes}-${dia}T${hora}:00`
  }

  return `${fecha}T${hora}:00`
}
</script>

<style scoped></style>
