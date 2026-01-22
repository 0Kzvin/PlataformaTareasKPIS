<template>
  <q-dialog ref="dialogRef" :no-backdrop-dismiss="noBackdropDismiss" maximized>
    <component
      :is="formularioComponent"
      :propsCustomForm="formularioComponentProps"
      @onDialogCancel="onDialogCancel"
      @onDialogOK="onDialogOK"
    ></component>
  </q-dialog>
</template>

<script setup>
import { toRefs } from 'vue'
import { useDialogPluginComponent } from 'quasar'

const props = defineProps({
  formularioComponentProps: {
    type: Object,
    default: () => ({}),
  },
  loadingColor: {
    type: String,
    default: 'secondary',
  },
  formularioComponent: {
    required: true,
    type: Object,
  },
  noBackdropDismiss: {
    type: Boolean,
    default: true,
  },
})

const { formularioComponent, formularioComponentProps, noBackdropDismiss } = toRefs(props)
const { dialogRef, onDialogCancel, onDialogOK } = useDialogPluginComponent()
</script>
