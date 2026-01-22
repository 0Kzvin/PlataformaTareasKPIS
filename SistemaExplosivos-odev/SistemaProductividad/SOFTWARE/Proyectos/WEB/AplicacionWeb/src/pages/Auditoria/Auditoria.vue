<template>
  <q-page class="q-pa-md">
    <div class="row items-center q-mb-md">
      <div class="text-h4 col-grow">Auditoría</div>
      <q-btn flat icon="refresh" label="Actualizar" @click="cargarRegistros" />
    </div>

    <q-table
      title="Registro de acciones"
      :rows="registros"
      :columns="columns"
      row-key="id"
      :loading="cargando"
    />
  </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { api } from 'boot/axios'

const registros = ref([])
const cargando = ref(false)
const columns = [
  { name: 'usuario', label: 'Usuario', field: 'usuarioNombre', align: 'left' },
  { name: 'entidad', label: 'Entidad', field: 'entidad', align: 'left' },
  { name: 'accion', label: 'Acción', field: 'accion', align: 'left' },
  { name: 'fecha', label: 'Fecha', field: 'fecha', align: 'left' },
]

const cargarRegistros = async () => {
  cargando.value = true
  try {
    const { data } = await api.get('/auditoria/Registros/Listar')
    registros.value = data
  } catch (error) {
    console.error(error)
  } finally {
    cargando.value = false
  }
}

onMounted(cargarRegistros)
</script>
