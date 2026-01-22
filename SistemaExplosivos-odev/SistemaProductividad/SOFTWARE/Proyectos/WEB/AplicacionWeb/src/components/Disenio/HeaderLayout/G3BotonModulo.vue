<template>
  <q-btn no-caps color="black" style="min-height: 55px; width: 260px" @mouseenter="!disabled && handleOpen()"
    @mouseleave="!disabled && handleLeave()">
    <div class="row items-center justify-between full-width no-wrap">
      <div class="row items-center" style="width: 24px">
        <i :class="`${moduloActual.icon}`" class="text-size-24" />
      </div>
      <span class="text-size-16 text-weight-medium"> {{ traducir(moduloActual.llave) }} </span>
      <div v-if="!disabled" class="row items-center" style="width: 24px">
        <i class="pi pi-bars text-size-21" />
      </div>
      <div v-else style="width: 24px"></div>
    </div>

    <q-menu v-model="isOpen" style="width: 260px" @mouseenter="isHovering = true" @mouseleave="handleLeave"
      no-parent-event :offset="[0, 0]" anchor="bottom left" self="top left">
      <div class="q-pa-sm">
        <q-list>
          <q-item v-show="child.mostrar" v-for="(child, index) in itemsFiltrados" :key="index" :to="child.ruta"
            clickable class="q-pa-sm" :class="{ 'q-mb-sm': index !== listadoModulos.itemsBotonUnico.length - 1 }"
            style="border: 1px solid black; border-radius: 8px; min-height: 53px">
            <q-item-section>
              <div class="row items-center justify-between full-width no-wrap">
                <div class="col-3">
                  <div class="row justify-start q-pl-sm">
                    <i :class="`${child.icon} text-textprimary text-size-24`" />
                  </div>
                </div>
                <div class="col-6">
                  <span class="text-subtitle1 text-weight-medium text-textprimary text-center">
                    {{ traducir(child.llave) }}
                  </span>
                </div>
                <div class="col-3">
                  <div class="row justify-end q-pr-sm">
                    <i class="pi pi-arrow-circle-right text-textprimary text-size-21 arrow-icon" />
                  </div>
                </div>
              </div>
            </q-item-section>
          </q-item>
        </q-list>
      </div>
    </q-menu>
  </q-btn>
</template>

<script setup>
import { computed, ref, toRefs, inject } from 'vue'

const traducir = inject('traducir', (key) => key)

const props = defineProps({
  listadoModulos: {
    type: Object,
    default: () => ({ itemsBotonUnico: [] }),
  },
  moduloActual: {
    type: Object,
    default: () => ({ icon: '', llave: '', idModulo: 0 }),
  },
  disabled: {
    type: Boolean,
    default: false,
  },
})

const { listadoModulos, moduloActual } = toRefs(props)

const itemsFiltrados = computed(() => {
  if (!listadoModulos.value?.itemsBotonUnico) return []
  return listadoModulos.value.itemsBotonUnico.filter(
    (item) => item.idModulo !== moduloActual.value.idModulo,
  )
})

let hoverTimer = null
let isOpen = ref(false)
let isHovering = ref(false)

function handleOpen() {
  isHovering.value = true
  clearTimeout(hoverTimer)
  hoverTimer = setTimeout(() => {
    isOpen.value = true
  }, 180)
}

function handleLeave() {
  isHovering.value = false
  clearTimeout(hoverTimer)
  hoverTimer = setTimeout(() => {
    if (!isHovering.value) {
      isOpen.value = false
    }
  }, 80)
}
</script>

<style scoped>
.arrow-icon {
  opacity: 0;
  transition: opacity 0.3s ease;
}

.menu-item:hover .arrow-icon {
  opacity: 1;
}
</style>
