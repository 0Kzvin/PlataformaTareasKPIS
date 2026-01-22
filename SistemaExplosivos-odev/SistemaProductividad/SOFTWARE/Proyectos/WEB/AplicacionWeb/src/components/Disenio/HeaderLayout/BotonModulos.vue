<template>
  <G3BotonModulo
    :listadoModulos="listadoModulos"
    :moduloActual="moduloActual"
    :disabled="modulosOtorgadosIds.length <= 1"
  />
</template>

<script setup>
import { ref, computed } from 'vue'
import { useRoute } from 'vue-router'
import { useSesionStore } from 'src/stores/sesion'
import { modulosItems as moduloItems } from 'src/core/modulos'
import G3BotonModulo from './G3BotonModulo.vue'

const store = useSesionStore()
const route = useRoute()
const numeroMaximoModulosPorMostrar = ref(1)

// Calculamos los IDs permitidos de forma reactiva
const modulosOtorgadosIds = computed(() => {
  const modulos = store.modulosOtorgados
  return Array.isArray(modulos) ? modulos.map((m) => m.id) : []
})

// 1. Encontrar el módulo actual basado en la ruta
const moduloActual = computed(() => {
  const rootPath = route.path.split('/')[1]
  return moduloItems.find(
    (item) =>
      modulosOtorgadosIds.value.includes(item.idModulo) && item.ruta.split('/')[1] === rootPath,
  )
})

// 2. Filtrar, ACTIVAR (mostrar=true) y Ordenar
const modulosMostrados = computed(() => {
  // A. Filtramos solo los que el usuario tiene permiso
  const permitidos = moduloItems.filter((modulo) =>
    modulosOtorgadosIds.value.includes(modulo.idModulo),
  )

  // B. IMPORTANTE: Activamos la propiedad 'mostrar = true' en los modulos permitidos
  permitidos.forEach((m) => (m.mostrar = true))

  // C. Ordenamos: el actual va primero
  const llaveActual = moduloActual.value?.llave

  return permitidos.sort((a, b) => {
    if (a.llave === llaveActual) return -1
    if (b.llave === llaveActual) return 1
    return 0
  })
})

// 3. Construir el objeto final para el componente hijo
const listadoModulos = computed(() => {
  const lista = modulosMostrados.value

  return {
    itemsBotonUnico: lista,
    itemsAMostrar: lista.slice(0, numeroMaximoModulosPorMostrar.value),
    itemsExtras: lista.slice(numeroMaximoModulosPorMostrar.value),
  }
})
</script>
