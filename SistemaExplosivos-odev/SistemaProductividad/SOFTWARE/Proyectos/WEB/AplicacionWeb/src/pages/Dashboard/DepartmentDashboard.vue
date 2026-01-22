<template>
  <q-page class="q-pa-md">
    <div class="row items-center q-mb-md">
      <div class="text-h4 col-grow">Dashboard: {{ stats.nombreDepartamento }}</div>
      <q-btn color="primary" icon="download" label="Reporte PDF" @click="descargarReporte" />
    </div>
    
    <div class="row q-col-gutter-md q-mb-xl">
      <div class="col-12 col-md-3">
        <q-card class="bg-blue text-white">
          <q-card-section>
            <div class="text-subtitle2">Pendientes</div>
            <div class="text-h3">{{ stats.pendientes }}</div>
          </q-card-section>
        </q-card>
      </div>
      <div class="col-12 col-md-3">
        <q-card class="bg-orange text-white">
          <q-card-section>
            <div class="text-subtitle2">En Proceso</div>
            <div class="text-h3">{{ stats.enProceso }}</div>
          </q-card-section>
        </q-card>
      </div>
      <div class="col-12 col-md-3">
        <q-card class="bg-green text-white">
          <q-card-section>
            <div class="text-subtitle2">Terminadas</div>
            <div class="text-h3">{{ stats.terminadas }}</div>
          </q-card-section>
        </q-card>
      </div>
      <div class="col-12 col-md-3">
        <q-card class="bg-red text-white">
          <q-card-section>
            <div class="text-subtitle2">Vencidas</div>
            <div class="text-h3">{{ stats.vencidas }}</div>
          </q-card-section>
        </q-card>
      </div>
    </div>

    <!-- Charts will go here (ChartJS / ApexCharts) -->
  </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { api } from 'boot/axios'
import { useQuasar } from 'quasar'
import { obtenerDireccionAPI } from 'src/services/AxiosService.js' // Import helper

const $q = useQuasar()
// const sesionStore = useSesionStore()

// Mock logic for generic department view (e.g. Leader's department)
const deptId = 1; 

const stats = ref({
  nombreDepartamento: 'Cargando...',
  pendientes: 0,
  enProceso: 0,
  terminadas: 0,
  vencidas: 0
})

const descargarReporte = () => {
  // Direct window open to download file (simple auth handled by browser cookies if applicable, or pass token in URL if permitted/secure enough for internal tool)
  // BETTER: Use axios to get blob and download.
  
  const url = `${obtenerDireccionAPI()}/core/Reportes/Departamento/${deptId}`
  
  // For JWT Auth, we need to pass the header, so window.open might fail if API requires Header.
  // Using Axios Blob approach:
  api.get(url, { responseType: 'blob' })
    .then((response) => {
       const href = URL.createObjectURL(response.data);
       const link = document.createElement('a');
       link.href = href;
       link.setAttribute('download', 'Reporte.pdf');
       document.body.appendChild(link);
       link.click();
       document.body.removeChild(link);
       URL.revokeObjectURL(href);
    })
    .catch(() => {
      $q.notify({ type: 'negative', message: 'Error al descargar reporte' })
    })
}

onMounted(async () => {
  try {
    const { data } = await api.get(`/core/Dashboards/Departamento/${deptId}`)
    Object.assign(stats.value, data)
  } catch (e) {
    console.error(e)
  }
})
</script>
