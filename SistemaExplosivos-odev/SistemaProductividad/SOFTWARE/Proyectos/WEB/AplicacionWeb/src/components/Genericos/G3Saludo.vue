<template>
  <div class="saludo full-width row q-gutter-none">
    <div class="text-textprimary text-size-20">
      {{ traducir('Hola') }}, {{ usuario.nombreCompleto }}
    </div>
    <div class="text-textprimary text-h3">
      {{ saludo }}
    </div>
  </div>
</template>

<script setup>
import { computed, inject } from 'vue'
import { useSesionStore } from 'src/stores/sesion'

const storeSesion = useSesionStore()
const usuario = storeSesion.ObtenerUsuario
const traducir = inject('traducir', (key) => key)

const hora = new Date().getHours()

const saludo = computed(() => {
  if (hora < 12) return traducir('BuenosDias')
  if (hora < 19) return traducir('BuenasTardes')
  return traducir('BuenasNoches')
})
</script>

<style scoped>
.saludo {
  width: 100%;
  /* Se adapta al ancho del contenedor */
  display: block;
  box-sizing: border-box;
  /* Evita que el padding afecte el ancho total */
}
</style>
