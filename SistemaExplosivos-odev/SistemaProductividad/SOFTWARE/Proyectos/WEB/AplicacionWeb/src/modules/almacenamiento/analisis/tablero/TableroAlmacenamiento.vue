<template>
  <q-page>
    <G3ModuloHeader :modulo="traducir('Tablero')" :requiere-boton-filtro="false" :metodo-refrescar="refrescarDatos" />
  </q-page>
</template>

<script setup>
import { inject } from 'vue'
import G3ModuloHeader from 'src/components/Genericos/G3ModuloHeader.vue'

const traducir = inject('traducir', (key) => key)

const refrescarDatos = () => { }
</script>

<style scoped></style>
