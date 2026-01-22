<template>
  <q-btn-dropdown v-if="items.length > 2" class="q-ml-md q-mb-sm without-icon btn-opciones-hover" color="info" no-caps
    unelevated align="center" menu-anchor="bottom middle" menu-self="top left" direction="right" :dropdown-icon="icono"
    :flat="flat" @click.stop rounded>
    <template v-slot:label>
      <i class="pi pi-ellipsis-v text-textprimary icono-opciones" />
    </template>
    <q-list class="q-px-xs q-py-sm bg-fondo3">
      <q-item class="text-textprimary">
        <q-item-section>
          <div class="row justify-center items-center q-pb-sm">
            <i class="pi pi-exclamation-circle q-mr-sm text-size-20" />
            <span class="text-weight-medium text-size-16">{{ 'Acciones' }}</span>
            <q-tooltip class="text-size-14" anchor="center left" self="center right" :offset="[10, 0]">
              {{ traducir('ListaDeAcciones') }}
            </q-tooltip>
          </div>
          <q-separator></q-separator>
        </q-item-section>
      </q-item>
      <q-item v-for="(item, i) in items" :key="i" clickable v-close-popup v-ripple class="q-my-xs rounded-borders"
        @click.stop="ejecutarAccion(item)" :class="`text-${item.color || 'bg-fondo2'}`">
        <q-item-section avatar class="justify-center items-center">
          <i :class="`${item.icono}`" class="text-size-20" />
        </q-item-section>
        <q-item-section>
          <q-item-label class="text-weight-bold">{{ item.titulo }}</q-item-label>
          <q-item-label caption class="text-textsecondary">
            {{ item.descripcion }}
          </q-item-label>
        </q-item-section>
      </q-item>
    </q-list>
  </q-btn-dropdown>
  <div class="row justify-center items-center" v-else>
    <q-btn v-for="(item, index) in items" :key="index" flat round @click="ejecutarAccion(item)"
      class="q-mr-sm btn-opciones-single-hover">
      <q-tooltip class="text-size-14">
        {{ item.descripcion }}
      </q-tooltip>
      <i :class="`${item.icono} ${item.color ? 'text-' + item.color : 'text-textsecondary'} text-size-20`" />
    </q-btn>
  </div>
</template>

<script setup>
import { computed, inject } from 'vue'
import * as quasarUtils from 'src/utils/quasar-utils.js'

const traducir = inject('traducir')

const ejecutarAccion = async (item) => {
  if (typeof item.accion !== 'function') return

  // If explicitly disabled
  if (item.autoConfirm === false) {
    item.accion()
    return
  }

  const titulo = (item.titulo || '').toLowerCase()
  const descripcion = (item.descripcion || '').toLowerCase()

  // Caso 1: Borrar / Eliminar
  if (titulo.includes('borrar') || titulo.includes('eliminar')) {
    const confirmed = await quasarUtils.decision({
      titulo: item.titulo,
      mensaje: traducir('BorrarRegistroPregunta') || '¿Desea borrar este registro permanentemente?',
      icono: 'warning',
      iconoColor: 'negative',
      checkbox: true,
      checkBoxLabel: traducir('ConfirmarBorrado') || 'Si quiero borrarlo',
      checkBoxErr: traducir('CheckboxErrorConfirmacion') || 'Debe confirmar para continuar',
    })
    if (confirmed) item.accion()
    return
  }

  // Caso 2: Cambio de Estado
  if (titulo.includes('estado')) {
    const esDesactivar = descripcion.includes('desactivar') || (item.icono && item.icono.includes('ban'))

    const confirmed = await quasarUtils.decision({
      titulo: item.titulo,
      mensaje: esDesactivar
        ? traducir('DesactivarRegistroPregunta') || '¿Desea desactivar este registro?'
        : traducir('ActivarRegistroPregunta') || '¿Desea activar este registro?',
      icono: 'warning',
      iconoColor: esDesactivar ? 'warning' : 'positive',
    })
    if (confirmed) item.accion()
    return
  }

  // Ejecución normal
  item.accion()
}

const props = defineProps({
  items: {
    type: Array,
    default: () => [
      {
        titulo: '',
        descripcion: '',
        icono: '',
        color: '',
        accion: null,
      },
    ],
  },
  icono: {
    type: String,
    default: 'fa fa-ellipsis-v',
  },
  texto: {
    type: String,
    default: '',
  },
  colorIcon: {
    type: String,
    default: 'grey-9',
  },
  flat: {
    type: Boolean,
    default: true,
  },
})

const items = computed(() => {
  return props.items
})
const icono = computed(() => {
  return props.icono
})
const flat = computed(() => {
  return props.flat
})
</script>

<style scoped>
/* Elimina la flecha del dropdown por completo */
button.without-icon :nth-child(2) :nth-child(2) {
  display: none;
}

/* Efecto hover para el botón de opciones dropdown */
.btn-opciones-hover {
  transition: all 0.2s ease-in-out;
}

.btn-opciones-hover:hover {
  background-color: var(--q-info, #f5f5f5) !important;
  transform: scale(1.1);
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.5);
}

/* Cambiar color del icono en dropdown hover */
.btn-opciones-hover:hover .icono-opciones {
  color: var(--q-info-contrast, #000000) !important;
}

/* Efecto hover para botones individuales (sin cambio de fondo) */
.btn-opciones-single-hover {
  transition: all 0.2s ease-in-out;
}

.btn-opciones-single-hover:hover {
  transform: scale(1.1);
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.5);
  /* No cambiamos el background-color aquí para respetar el color original o transparence */
}
</style>
