<template>
  <q-page class="q-pa-md">
    <div class="text-h4 q-mb-md">Gestión de Departamentos</div>
    
    <q-table
      title="Departamentos"
      :rows="departamentos"
      :columns="columns"
      row-key="id"
    >
      <template v-slot:top-right>
        <q-btn color="primary" label="Nuevo Departamento" icon="add" @click="mostrarModal = true" />
      </template>
    </q-table>
    
    <CrearDepartamentoDialog v-model="mostrarModal" @creado="cargarDepartamentos" />
  </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { api } from 'boot/axios'

import CrearDepartamentoDialog from 'src/components/Departamentos/CrearDepartamentoDialog.vue'

defineOptions({
  name: 'PageDepartamentos'
})

const departamentos = ref([])
const mostrarModal = ref(false)
const columns = [
  { name: 'nombre', label: 'Nombre', field: 'nombre', align: 'left' },
  { name: 'lider', label: 'Líder', field: 'liderNombre', align: 'left' },
  { name: 'miembros', label: 'Miembros', field: 'numeroMiembros', align: 'center' },
  { name: 'estado', label: 'Estado', field: 'estado', format: val => val ? 'Activo' : 'Inactivo' }
]

const cargarDepartamentos = async () => {
    try {
    const { data } = await api.get('/core/Departamentos/Listar')
    departamentos.value = data
  } catch (e) {
    console.error(e)
  }
}

onMounted(cargarDepartamentos)
</script>
