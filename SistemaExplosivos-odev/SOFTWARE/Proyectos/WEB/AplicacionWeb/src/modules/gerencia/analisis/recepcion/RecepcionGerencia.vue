<template>
  <q-page>{{ traducir('Recepcion') }}</q-page>
</template>

<script setup>
import { inject } from 'vue'

const traducir = inject('traducir', (key) => key)
</script>

<style lang="scss" scoped></style>
