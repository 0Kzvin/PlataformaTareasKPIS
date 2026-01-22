<template>
  <q-btn
    color="primary"
    :loading="cargando"
    :disable="cargando"
    @click="metodoAEmitir"
  >
    <div class="row items-center text-bold">
      <i class="pi pi-refresh q-mr-sm" />
      <span>{{ traducir("ActualizarTabla") }}</span>
    </div>
    <q-tooltip>
      <span class="text-size-12">{{ traducir("ActualizarTablaDescripcion") }}</span>
    </q-tooltip>
    <template #loading>
      <q-spinner-hourglass class="on-left" />
      <span class="text-size-12">{{ traducir("RealizandoConsulta") }}</span>
    </template>
  </q-btn>
</template>

<script setup>
import { inject, toRefs } from "vue";
const traducir = inject('traducir')

const props = defineProps({
  cargando: {
    type: Boolean,
    default: false,
    required: true,
  },
  metodoAEmitir: {
    type: Function,
    default: () => {},
    required: true,
  },
});

const { cargando, metodoAEmitir } = toRefs(props);
</script>
