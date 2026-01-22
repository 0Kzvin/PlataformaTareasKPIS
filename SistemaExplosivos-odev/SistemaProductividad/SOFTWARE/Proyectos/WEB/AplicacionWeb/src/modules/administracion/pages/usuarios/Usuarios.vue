<!-- eslint-disable vue/multi-word-component-names -->
<template>
  <q-page>
    <G3ModuloHeader :modulo="traducir('Usuarios')" :requiere-boton-crear="true"
      :boton-crear-texto="traducir('CrearUsuario')" boton-crear-icono="pi pi-plus-circle"
      :metodo-crear="mostrarCrearEditarUsuarioDialog" />

    <div class="row q-mt-md">
      <div class="col-12">
        <G3CustomTable :columnasVisibles="[
          'opciones',
          'usuario',
          'nombre',
          'apellidos',
          'email',
          'nombreRol',
          'estado',
        ]" :columnas="[
          {
            name: 'id',
            label: '',
            field: (usuarios) => usuarios.id,
          },
          {
            name: 'opciones',
            label: traducir('Opciones'),
            align: 'center',
          },
          {
            name: 'usuario',
            label: traducir('Usuario'),
            align: 'center',
            field: (usuarios) => usuarios.usuario,
          },
          {
            name: 'nombre',
            label: traducir('NombrePersona'),
            align: 'center',
            field: (usuarios) => usuarios.nombre,
          },
          {
            name: 'apellidos',
            label: traducir('Apellido'),
            align: 'center',
            field: (usuarios) => usuarios.apellidos,
          },
          {
            name: 'email',
            label: traducir('CorreoElectronico'),
            align: 'center',
            field: (usuarios) => usuarios.email,
          },
          {
            name: 'nombreRol',
            label: traducir('Rol'),
            align: 'center',
            field: (usuarios) => usuarios.nombreRol,
          },
          {
            name: 'estado',
            label: traducir('Estado'),
            align: 'center',
            sortable: true,
          },
        ]" :filas="usuarios" :filasPorPagina="10" :cargando="cargandoTablaUsuarios" :filtros="filtroUsuariosQuery">
          <template v-slot:col-end-estado="{ props }">
            <q-chip dense square :color="props.row.estado ? 'green-1' : 'red-1'"
              :text-color="props.row.estado ? 'green-9' : 'red-8'"
              class="text-weight-bold q-px-md q-py-xs rounded-badge text-size-12">
              {{ props.row.estado ? traducir('Activado') : traducir('Desactivado') }}
            </q-chip>
          </template>
          <template v-slot:col-start-opciones="{ props }">
            <div v-if="
              !estaPermisoOtorgado(pIdentidad.PermisoEditarUsuario) &&
              !estaPermisoOtorgado(pIdentidad.PermisoCambiarEstadoUsuario) &&
              !estaPermisoOtorgado(pIdentidad.PermisoBorrarUsuario)
            "></div>
            <boton-opciones :items="llenarArrayOpciones(props)" />
          </template>
        </G3CustomTable>
      </div>
    </div>
  </q-page>
</template>

<script setup>
import { onMounted, ref, reactive, inject } from 'vue'
import { identidadGFuelAdmin } from 'src/api/moduloAdministracion'
import { useQuasar } from 'quasar'
import CustomFormDialog from 'src/components/Globales/Dialogs/CustomFormDialog.vue'
import CrearEditarUsuarioForm from 'src/modules/administracion/pages/usuarios/components/CrearEditarUsuarioForm.vue'
import BotonOpciones from 'src/components/Paginas/General/OpcionesPage/BotonOpciones.vue'
import G3CustomTable from 'src/components/Globales/G3CustomTable.vue'
import { pIdentidad } from 'src/core/permisos/moduloAdministracion'
import { estaPermisoOtorgado } from 'src/core/permisos/check.js'
import * as quasarUtils from 'src/utils/quasar-utils.js'
import { refreshSource } from 'src/utils/utils'
import G3ModuloHeader from 'src/components/Genericos/G3ModuloHeader.vue'

const traducir = inject('traducir')

const $q = useQuasar()
const usuarios = ref([])
const cargandoTablaUsuarios = ref(false)

const filtroUsuariosQuery = reactive({
  usuario: '',
  nombre: '',
  apellidos: '',
  email: '',
  nombreRol: '',
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
})

const refrescarUsuarios = async () => {
  cargandoTablaUsuarios.value = true
  const resp = await identidadGFuelAdmin.listarUsuarios()
  cargandoTablaUsuarios.value = false
  if (!resp.exito) {
    usuarios.value = []
    return
  }
  usuarios.value = resp.payload
}

const mostrarCrearEditarUsuarioDialog = async (args = {}) => {
  $q.dialog({
    component: CustomFormDialog,
    componentProps: {
      formularioComponent: CrearEditarUsuarioForm,
      noBackdropDismiss: false,
      formularioComponentProps: {
        refrescarUsuarios,
        ...args,
      },
    },
  })
}

const cambiarEstadoUsuario = async (id, estado, nombreUsuario) => {
  const decision = await quasarUtils.decision({
    titulo: `${traducir('Usuario')}: ${nombreUsuario}`,
    mensaje: estado ? traducir('DesactivarUsuarioPreguntna') : traducir('ActivarUsuarioPregunta'),
    icono: 'warning',
    iconoColor: estado ? 'warning' : 'positive',
  })
  if (decision) {
    quasarUtils.cargandoSimple()
    const resp = await identidadGFuelAdmin.cambiarEstadoUsuario(id)
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
  }
}

const borrarUsuario = async (id, nombreUsuario) => {
  const decision = await quasarUtils.decision({
    titulo: `${traducir('Usuario')}: ${nombreUsuario}`,
    mensaje: traducir('BorrarUsuarioPregunta'),
    icono: 'warning',
    iconoColor: 'negative',
  })
  if (decision) {
    quasarUtils.cargandoSimple({})
    const resp = await identidadGFuelAdmin.borrarUsuario(id)
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
  }
}

onMounted(async () => {
  refreshSource()
  refrescarUsuarios()
})

const llenarArrayOpciones = (props) => {
  const arrayOpciones = []
  if (estaPermisoOtorgado(pIdentidad.PermisoEditarUsuario)) {
    arrayOpciones.push({
      titulo: traducir('Editar'),
      descripcion: traducir('EditarInformacion'),
      icono: 'fa fa-pen',
      color: 'warning',
      accion: () => mostrarCrearEditarUsuarioDialog({ editar: true, usuario: props.row }),
    })
  }

  if (estaPermisoOtorgado(pIdentidad.PermisoCambiarEstadoUsuario)) {
    arrayOpciones.push({
      titulo: traducir('Estado'),
      descripcion: props.row.estado ? traducir('DesactivarRegistro') : traducir('ActivarRegistro'),
      icono: props.row.estado ? 'fa fa-ban' : 'fa fa-check',
      color: props.row.estado ? 'negative' : 'positive',
      accion: () => cambiarEstadoUsuario(props.row.id, props.row.estado, props.row.usuario),
    })
  }

  if (estaPermisoOtorgado(pIdentidad.PermisoBorrarUsuario)) {
    arrayOpciones.push({
      titulo: traducir('Borrar'),
      descripcion: traducir('EliminarRegistro'),
      icono: 'fa fa-trash',
      color: 'negative',
      accion: () => borrarUsuario(props.row.id, props.row.usuario),
    })
  }

  return arrayOpciones
}
</script>

<style scoped>
.rounded-badge {
  height: 26px;
  border-radius: 6px;
  /* 👈 le da la forma redondeada */
}
</style>
