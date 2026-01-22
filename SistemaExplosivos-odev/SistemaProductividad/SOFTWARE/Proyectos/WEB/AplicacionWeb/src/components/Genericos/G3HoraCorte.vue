<template>
  <q-btn color="black" outline no-caps style="width: 180px; min-height: 60px" @click="menuAbierto = true">
    <q-tooltip class="text-size-16">
      {{ traducir('TooltipSeleccionarHoraCorte') }}
    </q-tooltip>

    <div v-if="horaCorteSeleccionada" class="full-width column justify-start items-start q-pt-xs">
      <span class="text-bold text-left"> {{ traducir('EtiquetaHoraCorte') }} </span>

      <div class="row items-center no-wrap justify-between full-width">
        <span class="text-size-14 cursor-pointer ellipsis">
          {{ horaCorteSeleccionada.horaVisualizar }}
        </span>

        <i class="pi pi-clock q-ml-sm" />
      </div>
    </div>

    <q-menu v-model="menuAbierto" @before-show="abriendoMenu = true" @show="abriendoMenu = false"
      @before-hide="abriendoMenu = false" :persistent="mouseEnBoton" style="width: 180px" transition-show="jump-down"
      transition-hide="jump-up" no-parent-event class="bg-fondo2">
      <q-list>
        <q-item v-for="hora in horasEnElDia" :key="hora.hora" clickable v-close-popup
          @click="horaCorteSeleccionada = hora">
          <q-item-section class="text-textprimary">
            {{ hora.horaVisualizar }}
          </q-item-section>
        </q-item>
      </q-list>
    </q-menu>
  </q-btn>
</template>

<script setup>
import { horasEnElDia } from 'src/utils/utils'
import { onBeforeMount, ref, watch, inject } from 'vue'

const traducir = inject('traducir', (key) => key)

const horaCorteSeleccionada = ref()

onBeforeMount(() => {
  horaCorteSeleccionada.value = horasEnElDia.find((x) => x.hora == 7)
})

const emit = defineEmits(['update:hora'])

const menuAbierto = ref(false)
const mouseEnBoton = ref(false)
const abriendoMenu = ref(false)

watch(horaCorteSeleccionada, (nuevaHoraCorte) => {
  if (!nuevaHoraCorte) return

  emit('update:hora', nuevaHoraCorte)
})
</script>

<style lang="scss" scoped></style>
