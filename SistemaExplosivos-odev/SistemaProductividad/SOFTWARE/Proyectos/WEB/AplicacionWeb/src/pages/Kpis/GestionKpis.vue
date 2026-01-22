<template>
  <q-page class="q-pa-md">
    <div class="row items-center q-mb-md">
      <div class="text-h4 col-grow">Gestión de KPIs</div>
    </div>

    <q-card class="q-mb-md">
      <q-card-section>
        <div class="text-subtitle1 text-weight-medium">KPIs configurados</div>
        <div class="text-caption text-textsecondary">
          Administra las métricas de productividad por departamento y define metas por periodo.
        </div>
      </q-card-section>
      <q-separator />
      <q-card-section>
        <q-table
          flat
          :rows="kpis"
          :columns="columns"
          row-key="id"
          hide-pagination
        />
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script setup>
import { ref } from 'vue'

const kpis = ref([
  { id: 1, nombre: 'Cumplimiento de tareas', formula: 'Terminadas / Total', meta: '90%' },
  { id: 2, nombre: 'Tiempo promedio de cierre', formula: 'Horas promedio', meta: '48h' },
])

const columns = [
  { name: 'nombre', label: 'KPI', field: 'nombre', align: 'left' },
  { name: 'formula', label: 'Fórmula', field: 'formula', align: 'left' },
  { name: 'meta', label: 'Meta', field: 'meta', align: 'left' },
]
</script>
