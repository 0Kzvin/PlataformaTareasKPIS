<template>
  <q-page>{{ traducir('Almacenamiento') }}</q-page>
</template>

<script setup>
import { inject } from 'vue'

const traducir = inject('traducir', (key) => key)
</script>

<style lang="scss" scoped></style>
