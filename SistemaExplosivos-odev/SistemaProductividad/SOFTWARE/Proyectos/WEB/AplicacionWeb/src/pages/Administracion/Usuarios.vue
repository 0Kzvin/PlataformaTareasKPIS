<template>
  <q-page class="q-pa-md">
    <div class="row items-center q-mb-md">
      <div class="text-h4 col-grow">Gestión de Usuarios</div>
    </div>

    <q-table
      title="Usuarios"
      :rows="usuarios"
      :columns="columns"
      row-key="id"
      :loading="cargando"
    />
  </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { api } from 'boot/axios'

defineOptions({
  name: 'UsuariosPage',
})

const usuarios = ref([])
const cargando = ref(false)
const columns = [
  { name: 'nombre', label: 'Nombre', field: 'nombre', align: 'left' },
  { name: 'apellidos', label: 'Apellidos', field: 'apellidos', align: 'left' },
  { name: 'email', label: 'Correo', field: 'email', align: 'left' },
  { name: 'departamento', label: 'Departamento', field: 'departamentoPrincipal', align: 'left' },
  { name: 'estado', label: 'Estado', field: 'estado', format: val => (val ? 'Activo' : 'Inactivo') },
]

const cargarUsuarios = async () => {
  cargando.value = true
  try {
    const { data } = await api.get('/administracion/Usuarios/Listar')
    usuarios.value = data
  } catch (e) {
    console.error(e)
  } finally {
    cargando.value = false
  }
}

onMounted(cargarUsuarios)
</script>
