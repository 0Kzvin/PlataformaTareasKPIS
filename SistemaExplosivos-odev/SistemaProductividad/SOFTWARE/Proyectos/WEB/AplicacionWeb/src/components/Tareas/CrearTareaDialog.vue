<template>
  <q-dialog v-model="isOpen" persistent>
    <q-card style="min-width: 500px">
      <q-card-section>
        <div class="text-h6">Registrar Tarea</div>
      </q-card-section>

      <q-card-section>
        <q-form @submit="onSubmit">
          <q-input v-model="form.titulo" label="Título" :rules="[val => !!val || 'Requerido']" />
          <q-input v-model="form.descripcion" label="Descripción" type="textarea" />
          
          <q-select 
             v-model="form.prioridad" 
             :options="opcionesPrioridad" 
             label="Prioridad" 
             emit-value 
             map-options 
          />

          <q-input v-model="form.deadline" label="Fecha Límite" type="date" stack-label />
          
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
  modelValue: Boolean,
  departamentoId: { type: Number, required: true }
})
const emit = defineEmits(['update:modelValue', 'tareaCreada'])

const $q = useQuasar()
const isOpen = ref(props.modelValue)

watch(() => props.modelValue, (val) => isOpen.value = val)
watch(isOpen, (val) => emit('update:modelValue', val))

const form = ref({
  titulo: '',
  descripcion: '',
  prioridad: 1,
  deadline: null
})

const opcionesPrioridad = [
  { label: 'Baja', value: 0 },
  { label: 'Media', value: 1 },
  { label: 'Alta', value: 2 },
  { label: 'Crítica', value: 3 }
]

const onSubmit = async () => {
  try {
    const payload = {
      ...form.value,
      departamentoId: props.departamentoId
    }
    
    await api.post('/core/Tareas/Registrar', payload)
    
    $q.notify({ type: 'positive', message: 'Tarea creada correctamente' })
    emit('tareaCreada')
    isOpen.value = false
    
    // Reset form
    form.value = { titulo: '', descripcion: '', prioridad: 1, deadline: null }
  } catch (e) {
    console.error(e)
    $q.notify({ type: 'negative', message: 'Error al crear tarea' })
  }
}
</script>
