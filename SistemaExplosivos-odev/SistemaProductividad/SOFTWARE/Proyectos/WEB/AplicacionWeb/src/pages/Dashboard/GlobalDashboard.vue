<template>
  <q-page class="q-pa-md">
    <div class="text-h4 q-mb-md">Dashboard Global</div>
    <div class="row q-col-gutter-md">
      <div class="col-12 col-md-3" v-for="(stat, index) in stats" :key="index">
        <q-card class="bg-primary text-white">
          <q-card-section>
            <div class="text-h6">{{ stat.label }}</div>
            <div class="text-h3">{{ stat.value }}</div>
          </q-card-section>
        </q-card>
      </div>
    </div>
  </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { api } from 'boot/axios'

const stats = ref([
  { label: 'Departamentos', value: 0 },
  { label: 'Usuarios', value: 0 },
  { label: 'Tareas', value: 0 },
  { label: 'Vencidas', value: 0 }
])

onMounted(async () => {
  try {
    const { data } = await api.get('/core/Dashboards/Global')
    stats.value[0].value = data.totalDepartamentos
    stats.value[1].value = data.totalUsuarios
    stats.value[2].value = data.totalTareas
    stats.value[3].value = data.tareasVencidas
  } catch (e) {
    console.error(e)
  }
})
</script>
