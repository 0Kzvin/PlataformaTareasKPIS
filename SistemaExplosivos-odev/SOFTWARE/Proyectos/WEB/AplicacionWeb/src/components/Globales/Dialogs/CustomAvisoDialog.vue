<template>
  <q-dialog ref="dialogRef" @hide="onDialogHide">
    <q-card class="q-pa-sm" style="width: 478px">
      <q-card-section v-if="titulo" class="row justify-center">
        <span class="text-size-21">{{ titulo }}</span>
      </q-card-section>
      <q-card-section class="row justify-center">
        <q-icon :name="icono" size="100px"></q-icon>
      </q-card-section>
      <q-card-section class="row justify-center">
        <span class="text-size-14">{{ mensaje }}</span>
      </q-card-section>
      <q-card-actions class="row justify-end">
        <q-btn
          :size="textoProporcion"
          color="negative"
          dense
          label="Enterado"
          @click="onDialogCancel"
        ></q-btn>
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>

<script setup>
import { usePreferenciaStore } from "src/stores/preferencias";
import { useDialogPluginComponent } from "quasar";

defineProps({
  titulo: {
    type: String,
    default: "",
  },
  mensaje: {
    type: String,
    default: "",
  },
  icono: {
    type: String,
    default: "",
  },
});

defineEmits([...useDialogPluginComponent.emits]);

const store = usePreferenciaStore();
const textoProporcion = store.textoProporcion;

const { dialogRef, onDialogHide, onDialogCancel } =
  useDialogPluginComponent();
</script>
