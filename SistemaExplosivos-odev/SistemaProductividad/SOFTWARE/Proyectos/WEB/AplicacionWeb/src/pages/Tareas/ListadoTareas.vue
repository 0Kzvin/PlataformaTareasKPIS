<template>
  <q-page class="q-pa-md">
    <div class="row items-center q-mb-md">
      <div class="text-h4 col-grow">Tareas</div>
      <q-btn color="primary" label="Nueva Tarea" icon="add" @click="abrirModal" />
    </div>
    
    <CrearTareaDialog v-model="mostrarModalCrear" :departamentoId="dptoId" @tareaCreada="recargarTareas" />

    <div class="row q-col-gutter-md">
      <!-- Kanban Columns -->
      <div class="col-12 col-md-3" v-for="estado in estados" :key="estado.value">
        <q-card class="column full-height bg-grey-2">
          <q-card-section class="bg-grey-3 text-bold">
            {{ estado.label }}
          </q-card-section>
          <q-card-section class="col q-pa-sm scroll" style="max-height: 70vh">
            <!-- Task Cards -->
            <q-card v-for="tarea in getTareasPorEstado(estado.value)" :key="tarea.id" class="q-mb-sm cursor-pointer" @click="editarTarea(tarea)">
              <q-card-section>
                <div class="text-subtitle1">{{ tarea.titulo }}</div>
                <div class="text-caption text-grey">{{ tarea.asignadoNombre }}</div>
                <q-chip size="sm" :color="getPrioridadColor(tarea.prioridad)">{{ tarea.prioridadTexto }}</q-chip>
              </q-card-section>
              
              <!-- Private Fields (Leader Only) -->
              <q-separator v-if="esLider && (tarea.notasPrivadas || tarea.tiempoEstimadoHoras)" />
              <q-card-section v-if="esLider && (tarea.notasPrivadas || tarea.tiempoEstimadoHoras)" class="bg-blue-1">
                <div v-if="tarea.notasPrivadas" class="text-caption text-indigo"><q-icon name="lock" /> {{ tarea.notasPrivadas }}</div>
                <div v-if="tarea.tiempoEstimadoHoras" class="text-caption text-indigo"><q-icon name="timer" /> Est: {{ tarea.tiempoEstimadoHoras }}h</div>
              </q-card-section>
            </q-card>
          </q-card-section>
        </q-card>
      </div>
    </div>
  </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { api } from 'boot/axios'
import { useSesionStore } from 'src/stores/sesion'
import { storeToRefs } from 'pinia'

import CrearTareaDialog from 'src/components/Tareas/CrearTareaDialog.vue'

const sesionStore = useSesionStore()
const { esLider } = storeToRefs(sesionStore)

const tareas = ref([])
const mostrarModalCrear = ref(false)
const dptoId = 1 // Mock ID

const estados = [
  { label: 'Pendiente', value: 0 },
  { label: 'En Proceso', value: 1 },
  { label: 'Terminada', value: 2 },
  { label: 'Vencida', value: 3 }
]

const getTareasPorEstado = (estadoVal) => tareas.value.filter(t => t.estado === estadoVal)

const getPrioridadColor = (p) => {
  switch(p) {
    case 3: return 'red';
    case 2: return 'orange';
    case 1: return 'blue';
    default: return 'green';
  }
}

const abrirModal = () => { mostrarModalCrear.value = true }
const editarTarea = () => { /* Logic to open Edit Modal */ }

const recargarTareas = async () => {
  try {
    const { data } = await api.get(`/core/Tareas/ListarPorDepartamento/${dptoId}`)
    tareas.value = data
  } catch (e) { console.error(e) }
}

onMounted(recargarTareas)
</script>
