<template>
  <q-page padding>
    <div class="text-h5 text-weight-bold q-mb-sm">{{ traducir('Reportes') }}</div>
    <div class="text-body1 text-textsecondary">
      {{ traducir('ReportesDescripcion') }}
    </div>
  </q-page>
</template>

<script setup>
import { inject } from 'vue'

const traducir = inject('traducir', (key) => key)
</script>
