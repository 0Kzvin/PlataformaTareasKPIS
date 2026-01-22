<template>
  <q-card class="q-px-md q-pt-md q-pb-lg bg-fondo3" style="max-width: 600px; max-height: 600px; border-radius: 16px">
    <q-card-section>
      <span class="text-textprimary text-weight-medium text-size-32">
        <template v-if="editar">{{ traducir('EditarUsuario') }}</template>
        <template v-else>{{ traducir('CrearUsuario') }}</template>
      </span>
      <q-separator class="q-mt-md"></q-separator>
    </q-card-section>
    <q-form @submit="enviarFormulario">
      <q-card-section class="row q-col-gutter-y-sm text-weight-light">
        <div class="col-6 q-pr-sm">
          <label class="text-textprimary text-size-18">
            {{ traducir('Nombre') + ' *:' }}
            <q-input v-model="nombres" dense filled clearable label-color="textsecondary"
              input-class="text-textsecondary" debounce="200" class="text-size-18 q-pt-sm" :rules="[
                (val) => ((val?.trim() ?? false) && val.length > 0) || traducir('CompletarCampo'),
              ]"></q-input>
          </label>
        </div>
        <div class="col-6 q-pl-sm">
          <label class="text-textprimary text-size-18">
            {{ traducir('Apellido') + ' *:' }}
            <q-input v-model="apellidos" dense clearable filled label-color="textsecondary"
              input-class="text-textsecondary" debounce="200" color="primary" class="text-size-18 q-pt-sm" :rules="[
                (val) => ((val?.trim() ?? false) && val.length > 0) || traducir('CompletarCampo'),
              ]"></q-input>
          </label>
        </div>
        <div class="col-6 q-pr-sm">
          <label class="text-textprimary text-size-18">
            {{ traducir('NombreUsuario') + ' *:' }}
            <q-input v-model="nombreUsuario" dense clearable filled debounce="200" label-color="textsecondary"
              input-class="text-textsecondary" color="primary" class="text-size-18 q-pt-sm" :rules="[
                (val) => ((val?.trim() ?? false) && val.length > 0) || traducir('CompletarCampo'),
              ]"></q-input>
          </label>
        </div>
        <div class="col-6 q-pl-sm">
          <label class="text-textprimary text-size-18">
            {{ traducir('Password') + ' *:' }}
            <q-input v-model="password" dense clearable filled debounce="200" color="primary"
              label-color="textsecondary" input-class="text-textsecondary" type="password" class="text-size-18 q-pt-sm"
              :hint="editar ? traducir('VacioParaMantenerActual') : ''" :rules="editar ? null : [(val) => (val && val.length > 0) || traducir('CompletarCampo')]
                "></q-input>
          </label>
        </div>
        <div class="col-6 q-pr-sm">
          <label class="text-textprimary text-size-18">
            {{ traducir('Rol') + ' *:' }}
            <q-select dense class="q-mb-sm text-size-18 q-pt-sm" clearable filled label-color="textsecondary"
              input-class="text-textsecondary" popup-content-class="text-textprimary"
              @click:clear="rolSeleccionado = null" v-model="rolSeleccionado" :options="roles" option-value="id"
              option-label="nombreRol" :rules="[(val) => val || traducir('CompletarCampo')]" />
          </label>
        </div>
        <div class="col-6 q-pl-sm">
          <label class="text-textprimary text-size-18">
            {{ traducir('CorreoElectronico') }}
            <q-input v-model="correo" dense filled clearable label-color="textsecondary"
              input-class="text-textsecondary" class="text-size-18 q-pt-sm" debounce="200" color="primary"
              type="email"></q-input>
          </label>
        </div>
      </q-card-section>

      <q-card-actions>
        <div class="row full-width">
          <div class="col-6 q-px-sm">
            <q-btn outline class="full-width text-weight-light" color="negative" @click="cancelarForm()"
              style="height: 44px" no-caps>
              <i class="pi pi-times q-mr-md text-size-20"></i>
              <span class="text-size-18"> {{ traducir('Cancelar') }}</span>
            </q-btn>
          </div>
          <div class="col-6 q-px-sm">
            <q-btn color="primary" no-caps class="full-width text-weight-light text-primary-contrast" type="submit"
              style="height: 44px">
              <i :class="editar ? 'pi pi-save' : 'pi pi-plus-circle'" class="q-mr-md text-size-20"></i>
              <span class="text-size-18">
                {{ traducir(editar ? 'GuardarCambios' : 'CrearUsuario') }}</span>
            </q-btn>
          </div>
        </div>
      </q-card-actions>
    </q-form>
  </q-card>
</template>

<script setup>
import { onMounted, ref, inject } from 'vue'
import { identidadGFuelAdmin } from 'src/api/moduloAdministracion'
import * as quasarUtils from 'src/utils/quasar-utils.js'

const traducir = inject('traducir', (key) => key)

const emits = defineEmits(['onDialogCancel', 'onDialogHide', 'onDialogOK'])

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

const {
  propsCustomForm: { refrescarUsuarios, editar, usuario },
} = props

const obtenerRoles = async () => {
  const resp = await identidadGFuelAdmin.listarRoles(true)

  if (!resp.exito) {
    roles.value = []
    return
  }

  roles.value = resp.payload
}

const nombres = editar ? ref(usuario.nombre) : ref('')
const apellidos = editar ? ref(usuario.apellidos) : ref('')
const nombreUsuario = editar ? ref(usuario.usuario) : ref('')
const password = ref('')
const correo = editar ? ref(usuario.email) : ref('')
let rolSeleccionado = ref(null)

const roles = ref([])

onMounted(async () => {
  await obtenerRoles()
  if (editar) rolSeleccionado.value = roles.value.find((e) => e.id === usuario.idRol)
})

const cancelarForm = () => {
  emits('onDialogCancel')
}
const enviarFormulario = async () => {
  const nombresTrim = nombres.value.trim()
  const nombreUsuarioTrim = nombreUsuario.value.trim()
  const apellidosTrim = apellidos.value.trim()
  const correoTrim = correo.value?.trim()
  if (
    !nombresTrim ||
    !apellidosTrim ||
    !nombreUsuarioTrim ||
    (!password.value && !editar) ||
    !rolSeleccionado.value
  ) {
    quasarUtils.aviso({
      error: true,
      mensaje: traducir('FormularioInvalido'),
    })
    return
  }
  const decision = await quasarUtils.decision({
    mensaje: editar ? traducir('EditarUsuarioPregunta') : traducir('CrearUsuarioPregunta'),
  })
  if (decision) {
    const registrarEditarModel = {
      Username: nombreUsuarioTrim,
      Email: correoTrim,
      Nombre: nombresTrim,
      Apellidos: apellidosTrim,
      Password: password.value,
      RolId: rolSeleccionado.value.id,
    }

    let resp

    quasarUtils.cargandoSimple()
    if (editar) {
      registrarEditarModel.IdUsuario = usuario.id
      if (rolSeleccionado.value.id !== usuario.idRol)
        registrarEditarModel.idRol = rolSeleccionado.value.id
      if (password.value) registrarEditarModel.password = password.value
      resp = await identidadGFuelAdmin.editarUsuario(registrarEditarModel)
    } else {
      resp = await identidadGFuelAdmin.registrarUsuario(registrarEditarModel)
    }
    quasarUtils.ocultarCargandoSimple()

    if (!resp.exito) {
      quasarUtils.aviso({
        error: true,
        mensaje: resp.payload.errores[0],
      })
      return
    }

    refrescarUsuarios()

    await quasarUtils.aviso({
      exito: true,
      mensaje: resp.payload.mensaje,
    })

    emits('onDialogCancel')
  }
}
</script>
