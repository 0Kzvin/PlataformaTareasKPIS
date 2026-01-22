<template>
  <q-header class="bg-fondo2" style="padding-top: 24px; padding-left: 24px; padding-right: 16px">
    <div class="column q-gutter-md">
      <div class="row items-center justify-between">
        <div class="row items-center q-gutter-sm">
          <q-btn dense round flat icon="menu" color="textprimary" @click="$emit('toggleDrawer')" />
          <BotonModulos />
        </div>
        <G3Cliente />
      </div>
      <div class="row q-mb-sm">
        <G3Saludo />
      </div>
    </div>

  </q-header>
</template>

<script setup>
import BotonModulos from './BotonModulos.vue'
import G3Cliente from './G3Cliente.vue'
import G3Saludo from 'src/components/Genericos/G3Saludo.vue'

// Definimos el evento que escuchará el Layout principal
defineEmits(['toggleDrawer'])
</script>
