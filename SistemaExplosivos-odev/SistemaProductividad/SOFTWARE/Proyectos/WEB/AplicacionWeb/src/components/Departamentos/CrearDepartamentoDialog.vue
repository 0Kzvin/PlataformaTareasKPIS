<template>
  <q-dialog v-model="isOpen" persistent>
    <q-card style="min-width: 400px">
      <q-card-section>
        <div class="text-h6">Registrar Departamento</div>
      </q-card-section>

      <q-card-section>
        <q-form @submit="onSubmit">
          <q-input v-model="form.nombre" label="Nombre" :rules="[val => !!val || 'Requerido']" />
          <q-input v-model="form.descripcion" label="Descripción" />
          
          <div class="row justify-end q-mt-md">
             <q-btn label="Cancelar" flat color="negative" v-close-popup />
             <q-btn label="Guardar" color="primary" type="submit" />
          </div>
        </q-form>
      </q-card-section>
    </q-card>
  </q-dialog>
</template>

<script setup>
import { ref, watch } from 'vue'
import { api } from 'boot/axios'
import { useQuasar } from 'quasar'

const props = defineProps({
  modelValue: Boolean
})
const emit = defineEmits(['update:modelValue', 'creado'])

const $q = useQuasar()
const isOpen = ref(props.modelValue)

watch(() => props.modelValue, (val) => isOpen.value = val)
watch(isOpen, (val) => emit('update:modelValue', val))

const form = ref({
  nombre: '',
  descripcion: ''
})

const onSubmit = async () => {
  try {
    // Assuming endpoint expects { nombre, descripcion }
    // Add validation for 'LiderId' if required later
    await api.post('/core/Departamentos/Registrar', form.value)
    
    $q.notify({ type: 'positive', message: 'Departamento creado correctamente' })
    emit('creado')
    isOpen.value = false
    
    // Reset form
    form.value = { nombre: '', descripcion: '' }
  } catch (e) {
    console.error(e)
    $q.notify({ type: 'negative', message: 'Error al crear departamento' })
  }
}
</script>
