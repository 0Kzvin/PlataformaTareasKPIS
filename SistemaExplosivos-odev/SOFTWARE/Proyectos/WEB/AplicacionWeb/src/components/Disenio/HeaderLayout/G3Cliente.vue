<template>
  <q-card
    class="user-card row items-center no-wrap shadow-1 transition-generic cursor-pointer relative-position bg-fondo3"
    :class="[$q.screen.lt.sm ? 'justify-center' : 'justify-between', esLogueado ? 'q-px-sm' : 'q-px-md']"
    :style="estiloCard" v-ripple>
    <q-tooltip v-if="esLogueado" class="text-size-14" :offset="[0, 5]">
      {{ traducir('VerPerfilAjustes') }}
    </q-tooltip>
    <q-tooltip v-else class="text-size-14" :offset="[0, 5]">
      {{ traducir('ConfiguracionAplicacion') }}
    </q-tooltip>

    <!-- VISTA CUANDO NO ESTÁ LOGUEADO -->
    <template v-if="!esLogueado">
      <div class="row items-center no-wrap full-width justify-center">
        <q-icon name="settings" color="textsecondary" class="q-mr-sm text-size-24" />
        <span class="text-textsecondary text-size-16 text-weight-medium">
          {{ traducir('Ajustes') }}
        </span>
      </div>
    </template>

    <!-- VISTA CUANDO ESTÁ LOGUEADO -->
    <template v-else>
      <div class="col-auto row items-center">
        <q-avatar size="32px" rounded>
          <q-img :src="usuario.foto || noUsuarioFoto" alt="Avatar" loading="eager" />
        </q-avatar>
      </div>

      <div v-if="!$q.screen.lt.sm" class="col q-px-md overflow-hidden text-left" style="min-width: 0">
        <div class="text-textsecondary text-size-16 ellipsis text-weight-bold">
          {{ usuario.nombreCompleto }}
        </div>
        <div class="text-textsecondary text-caption ellipsis" style="margin-top: -2px">
          {{ usuario.correo }}
        </div>
      </div>

      <div class="col-auto row items-center" :class="$q.screen.lt.sm ? 'q-ml-sm' : ''">
        <q-icon name="keyboard_arrow_down" color="textsecondary" size="20px" />
      </div>
    </template>

    <!-- MENÚ PARA NO LOGUEADO -->
    <q-menu v-if="!esLogueado" anchor="bottom middle" self="top middle" :offset="[0, 10]"
      class="shadow-2 bg-fondo3 menu-ajustes-no-logueado" :style="estiloMenuNoLogueado">
      <div class="column">
        <G3AjustesMenu :dense="$q.screen.lt.sm" />
      </div>
    </q-menu>

    <!-- MENÚ PARA LOGUEADO -->
    <q-menu v-else anchor="bottom right" self="top right" :offset="[0, 10]" class="shadow-1 bg-fondo3"
      :style="estiloMenu">
      <div class="column">
        <div class="q-pa-md">
          <div class="row items-center no-wrap q-mb-md">
            <q-avatar :size="$q.screen.lt.sm ? '48px' : '56px'" class="q-mr-md">
              <q-img :src="usuario.foto || noUsuarioFoto" loading="eager" />
            </q-avatar>

            <div class="col overflow-hidden">
              <div class="text-subtitle2 text-textsecondary text-weight-bold ellipsis">
                {{ usuario.nombreCompleto }}
              </div>
              <div class="text-caption text-textsecondary ellipsis">
                {{ usuario.correo }}
              </div>
              <div class="text-caption text-textsecondary text-weight-medium ellipsis">
                {{ usuario.rol || 'Usuario' }}
              </div>
            </div>
          </div>

          <q-btn unelevated rounded color="primary" no-caps class="full-width text-primary-contrast text-size-14"
            @click="mostrarDetallesDialog">
            {{ traducir('GestionarPerfil') }}
          </q-btn>
        </div>

        <q-separator class="q-mx-md" />

        <G3AjustesMenu :dense="$q.screen.lt.sm" />

        <q-separator class="q-mx-md" />

        <div class="q-px-md q-pb-lg q-pt-md row justify-center">
          <q-btn unelevated no-caps @click="cerrarSesion" class="btn-cerrar-sesion full-width"
            :class="$q.screen.lt.sm ? 'q-py-xs' : ''">
            <i class="pi pi-sign-out q-mr-md" :class="$q.screen.lt.sm ? 'text-size-18' : 'text-size-20'" />
            <span :class="$q.screen.lt.sm ? 'text-size-16' : 'text-size-20'">
              {{ traducir ? traducir('CerrarSesion') : 'Cerrar sesión' }}
            </span>
          </q-btn>
        </div>
      </div>
    </q-menu>
  </q-card>
</template>

<script setup>
import { computed, inject } from 'vue'
import { useQuasar } from 'quasar'
import TransparentLoadingDialog from 'src/components/Globales/Dialogs/TransparentLoadingDialog.vue'
import CustomFormDialog from 'src/components/Globales/Dialogs/CustomFormDialog.vue'
import G3LogoLightMini from 'src/assets/G3LogoLightMini.svg'
import G3LogoDarkMini from 'src/assets/G3LogoDarkMini.svg'
import { useSesionStore } from 'src/stores/sesion'
import G3AjustesMenu from 'src/components/Genericos/G3AjustesMenu.vue'
import DetallesUsuario from 'src/components/Globales/DetallesUsuario.vue'

const $q = useQuasar()
const storeSesion = useSesionStore()
const traducir = inject('traducir', (key) => key)
const usuario = storeSesion.ObtenerUsuario

// Determinar si el usuario está logueado
const esLogueado = computed(() => {
  return usuario && usuario.nombreCompleto && usuario.correo
})

const noUsuarioFoto = computed(() => ($q.dark.isActive ? G3LogoDarkMini : G3LogoLightMini))

// Estilo de la tarjeta principal
const estiloCard = computed(() => {
  if (!esLogueado.value) {
    // Estilo para NO logueado - SIN COMPRIMIR en mobile
    return {
      width: 'auto',
      height: '50px',
      borderRadius: '25px',
      paddingLeft: $q.screen.lt.sm ? '16px' : '16px',
      paddingRight: $q.screen.lt.sm ? '16px' : '16px',
      minWidth: $q.screen.lt.sm ? '50px' : '140px', // Mantiene círculo en mobile
    }
  }

  // Estilo para logueado (original)
  return $q.screen.lt.sm
    ? {
      width: 'auto',
      height: '60px',
      borderRadius: '25px',
      paddingLeft: '12px',
      paddingRight: '12px',
    }
    : {
      minWidth: '260px',
      height: '60px',
      borderRadius: '25px',
    }
})

// Estilo del menú para NO logueado
const estiloMenuNoLogueado = computed(() => {
  return $q.screen.lt.sm
    ? {
      borderRadius: '16px',
      width: '95dvw',
      maxWidth: '95dvw',
      maxHeight: '95dvh', // Deja espacio arriba y abajo
    }
    : {
      borderRadius: '20px',
      width: '320px',
      minWidth: '320px',
      maxHeight: '95dvh',
    }
})

// Estilo del menú desplegable para logueado
const estiloMenu = computed(() => {
  return $q.screen.lt.sm
    ? {
      borderRadius: '16px',
      width: '320px',
      maxWidth: '95vw',
      maxHeight: '90dvh',
      overflowY: 'auto',
      overflowX: 'hidden',
    }
    : {
      borderRadius: '25px',
      width: '380px',
      maxWidth: '500px',
      maxHeight: '90dvh',
      overflowY: 'auto',
      overflowX: 'hidden',
    }
})

const cerrarSesion = () => {
  const dialog = $q.dialog({ component: TransparentLoadingDialog })
  storeSesion.cerrarSesion().then(() => {
    dialog.hide()
  })
}

const mostrarDetallesDialog = () => {
  $q.dialog({
    component: CustomFormDialog,
    componentProps: {
      noBackdropDismiss: false,
      formularioComponent: DetallesUsuario,
      formularioComponentProps: { usuario: usuario },
    },
  })
}
</script>

<style scoped>
.user-card {
  transition: all 0.3s ease-in-out;
}

.user-card:hover {
  transform: translateY(-1px);
}

.ellipsis {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.btn-cerrar-sesion {
  height: v-bind("$q.screen.lt.sm ? '40px' : '48px'");
  border-radius: 8px;
  transition: all 0.3s ease;
  border: 1px solid var(--q-text-primary);
  color: var(--q-text-primary);
  background-color: transparent;
}

.btn-cerrar-sesion:hover {
  border-color: var(--q-primary);
  background-color: var(--q-primary);
  color: white;
}

/* Estilo específico para el menú de ajustes cuando no está logueado */
.menu-ajustes-no-logueado {
  overflow-y: auto;
}

.menu-ajustes-no-logueado .column {
  min-width: 0;
  width: 100%;
}

/* Scroll personalizado para el menú */
.menu-ajustes-no-logueado::-webkit-scrollbar {
  width: 6px;
}

.menu-ajustes-no-logueado::-webkit-scrollbar-track {
  background: transparent;
}

.menu-ajustes-no-logueado::-webkit-scrollbar-thumb {
  background: rgba(128, 128, 128, 0.3);
  border-radius: 3px;
}

.menu-ajustes-no-logueado::-webkit-scrollbar-thumb:hover {
  background: rgba(128, 128, 128, 0.5);
}
</style>