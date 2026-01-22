<template>
  <div class="full-width row justify-center q-my-md">
    <div class="col-12 self-center text-center">
      <span class="text-textsecondary text-weight-bold text-size-21">
        {{ traducir("RealizandoConsulta") }}...
      </span>
    </div>
    <q-img src="../../assets/loading-files-nobg.gif" style="max-width: 200px" loading="eager" />
  </div>
</template>

<script setup>
import { inject } from 'vue';
const traducir = inject('traducir')
</script>

<style scoped>
div {
  z-index: 99;
}
</style>
