<template>
  <q-page>
    <G3ModuloHeader :modulo="traducir('LimitesSedena')" />
  </q-page>
</template>

<script setup>
import { inject } from 'vue'
import G3ModuloHeader from 'src/components/Genericos/G3ModuloHeader.vue'

const traducir = inject('traducir', (key) => key)
</script>

<style lang="scss" scoped></style>
