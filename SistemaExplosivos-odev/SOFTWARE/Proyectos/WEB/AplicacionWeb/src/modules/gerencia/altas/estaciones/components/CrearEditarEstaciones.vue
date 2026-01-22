<template>
  <q-card class="q-px-md q-pt-md q-pb-lg bg-fondo3" style="max-width: 560px; max-height: 320px; border-radius: 16px">
    <q-card-section>
      <span class="text-textprimary text-weight-regular text-size-32">
        <template v-if="editar">{{ traducir('EditarEstacion') }}</template>
        <template v-else>{{ traducir('CrearEstacion') }}</template>
      </span>
      <q-separator class="q-mt-md"></q-separator>
    </q-card-section>
    <div class="q-px-md">
      <div class="row">
        <div class="col-6 q-pr-md">
          <label class="text-textprimary text-size-16">
            {{ traducir('Nombre') + ' *:' }}
            <q-input v-model="nombre" dense filled clearable label-color="textsecondary"
              input-class="text-textsecondary" debounce="200" class="text-size-16 q-pt-sm" :rules="[
                (val) => ((val?.trim() ?? false) && val.length > 0) || traducir('CompletarCampo'),
              ]"></q-input>
          </label>
        </div>
        <div class="col-6 q-pl-md">
          <label class="text-textprimary text-size-16">
            {{ traducir('Ip') + ' *:' }}
            <q-input v-model="ip" dense filled clearable label-color="textsecondary" input-class="text-textsecondary"
              debounce="200" class="text-size-16 q-pt-sm" :rules="[
                (val) => ((val?.trim() ?? false) && val.length > 0) || traducir('CompletarCampo'),
              ]"></q-input>
          </label>
        </div>
      </div>
      <div class="row full-width q-pt-md">
        <div class="col-6 q-pr-sm">
          <q-btn outline class="full-width text-weight-light" color="negative" @click="cerrarDialogo()"
            style="height: 44px" no-caps>
            <i class="pi pi-times q-mr-md text-size-20"></i>
            <span class="text-size-16"> {{ traducir('Cancelar') }}</span>
          </q-btn>
        </div>
        <div class="col-6 q-pl-sm">
          <q-btn color="primary" no-caps @click="enviarFormulario" class="full-width text-weight-light" type="submit"
            style="height: 44px">
            <div class="row items-center" v-if="editar">
              <i class="pi pi-pencil q-mr-md text-size-20"></i>
              <span class="text-size-16"> {{ traducir('EditarEstacion') }}</span>
            </div>
            <div class="row items-center" v-else>
              <i class="pi pi-plus-circle q-mr-md text-size-20"></i>
              <span class="text-size-16"> {{ traducir('CrearEstacion') }}</span>
            </div>
          </q-btn>
        </div>
      </div>
    </div>
  </q-card>
</template>

<script setup>
import { ref, inject } from 'vue'
import * as quasarUtils from 'src/utils/quasar-utils.js'
import { estacionesGerencia } from 'src/api/moduloGerencia'

const traducir = inject('traducir', (key) => key)

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

const emit = defineEmits(['onDialogCancel'])

const id = editar ? ref(modelo.id) : ref(null)
const nombre = editar ? ref(modelo.nombre) : ref('')
const ip = editar ? ref(modelo.ip) : ref('')

const cerrarDialogo = () => {
  emit('onDialogCancel')
}

const enviarFormulario = async () => {
  if (!nombre.value || !ip.value) {
    await quasarUtils.aviso({
      error: true,
      mensaje: traducir('LlenarDatosRequeridos'),
    })

    return
  }

  const estacion = {
    id: id.value,
    nombre: nombre.value,
    ip: ip.value,
  }

  let resp = null

  quasarUtils.cargandoSimple()
  if (editar) {
    resp = await estacionesGerencia.modificar(estacion)
  } else {
    resp = await estacionesGerencia.registrar(estacion)
  }
  quasarUtils.ocultarCargandoSimple()

  if (resp.exito) {
    await quasarUtils.aviso({
      exito: true,
      mensaje: resp.payload.mensaje,
    })

    if (refrescarDatos) {
      await refrescarDatos()
      emit('onDialogCancel')
    }
  } else {
    await quasarUtils.aviso({
      error: true,
      mensaje: resp.payload.errores[0],
    })
  }
}
</script>

<style lang="scss" scoped></style>
