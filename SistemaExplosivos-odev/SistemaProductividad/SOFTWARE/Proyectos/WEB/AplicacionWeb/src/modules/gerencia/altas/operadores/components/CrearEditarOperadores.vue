<template>
  <q-card class="q-px-md q-pt-md q-pb-lg bg-fondo3" style="max-width: 620px; max-height: 320px; border-radius: 16px">
    <q-card-section>
      <span class="text-textprimary text-weight-regular text-size-32">
        <template v-if="editar">{{ traducir('EditarOperador') }}</template>
        <template v-else>{{ traducir('CrearOperador') }}</template>
      </span>
      <q-separator class="q-mt-md"></q-separator>
    </q-card-section>
    <q-form class="q-px-md">
      <div class="row full-width q-pt-md">
        <div class="col-6 q-pr-sm">
          <q-btn outline class="full-width text-weight-light" color="negative" @click="cerrarDialogo()"
            style="height: 44px" no-caps>
            <i class="pi pi-times q-mr-md text-size-20"></i>
            <span class="text-size-16"> {{ traducir('Cancelar') }}</span>
          </q-btn>
        </div>
        <div class="col-6 q-pl-sm">
          <q-btn color="primary" no-caps @click="enviarFormulario()" class="full-width text-weight-light" type="submit"
            style="height: 44px">
            <i class="pi pi-plus-circle q-mr-md text-size-20"></i>
            <span class="text-size-16"> {{ traducir('CrearOperador') }}</span>
          </q-btn>
        </div>
      </div>
    </q-form>
  </q-card>
</template>

<script setup>
import { ref, inject } from 'vue'
import * as quasarUtils from 'src/utils/quasar-utils.js'
import { operadoresGerencia } from 'src/api/moduloGerencia'

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
const codigoAplicacion = editar ? ref(modelo.codigoAplicacion) : ref('')

const cerrarDialogo = () => {
  emit('onDialogCancel')
}

const enviarFormulario = async () => {
  if (!nombre.value || !codigoAplicacion.value) {
    await quasarUtils.aviso({
      error: true,
      mensaje: traducir('LlenarDatosRequeridos'),
    })
    return
  }

  const modeloAEnviar = {
    id: id.value,
    nombre: nombre.value,
    codigoAplicacion: codigoAplicacion.value,
  }

  let resp = null

  quasarUtils.cargandoSimple()
  if (editar) {
    resp = await operadoresGerencia.modificar(modeloAEnviar)
  } else {
    resp = await operadoresGerencia.registrar(modeloAEnviar)
  }
  quasarUtils.ocultarCargandoSimple()

  if (resp.exito) {
    await quasarUtils.aviso({
      exito: true,
      mensaje: resp.payload.mensaje,
    })

    if (refrescarDatos) {
      await refrescarDatos()
      cerrarDialogo()
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
