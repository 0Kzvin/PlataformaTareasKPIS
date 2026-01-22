<template>
  <div class="bg-fondo3 q-pt-sm"
    style="height: calc(100% - 150px); margin-top: 150px; overflow-y: auto; width: fit-content; min-width: 100%;">
    <transition name="list-fade" mode="out-in">
      <q-list class="q-px-sm" :key="listadoKey">
        <div v-for="item in listado" :key="item.title" v-show="item.seccionOculta">
          <q-expansion-item v-if="!miniState || $q.screen.lt.md" class="q-mb-sm"
            header-class="bg-primary text-primary-contrast" expand-icon-class="text-primary-contrast"
            header-style="border-radius: 6px; height: 55px" :default-opened="item.active">
            <template v-slot:header>
              <q-item-section avatar>
                <i class="text-size-24" :class="`${item.icono}`" />
              </q-item-section>
              <q-item-section class="text-left col row justify-start content-center no-wrap"
                style="align-items: flex-start; min-width: max-content;">
                <div class="text-weight-medium text-size-18 full-width text-left q-pr-md" style="white-space: nowrap;">
                  {{ traducir(item.llave) }}
                </div>
              </q-item-section>
            </template>

            <q-item v-show="child.paginaOculta" v-for="(child, i) in item.items" :key="i" :to="child.ruta"
              :class="`text-textprimary q-my-xs q-mx-sm hover-grow ${$route.path === child.ruta ? 'item-seleccionado' : ''}`"
              :inset-level="0.4" style="border-radius: 8px">
              <q-item-section avatar style="min-width: 40px; padding-right: 0px">
                <div class="row justify-center">
                  <i class="text-size-21 text-textprimary" :class="`${child.icono}`" />
                </div>
              </q-item-section>

              <q-item-section class="text-left col row justify-start content-center no-wrap"
                style="align-items: flex-start; min-width: max-content;">
                <div class="full-width text-left q-pb-xs q-pr-sm" style="white-space: nowrap;">
                  <span
                    :class="`underline-effect ${$route.path === child.ruta ? 'active text-textprimary text-weight-medium' : 'text-textprimary'}`">
                    <span class="text-size-14">{{ traducir(child.llave) }}</span>
                  </span>
                </div>
              </q-item-section>
            </q-item>
          </q-expansion-item>

          <div v-else-if="miniState && !$q.screen.lt.md" class="q-mb-sm relative-position">
            <q-item clickable class="justify-center items-center" :class="esSeccionActiva(item)
              ? 'bg-primary text-primary-contrast'
              : 'bg-fondo text-textprimary'
              " style="border-radius: 12px; height: 50px; width: 100%">
              <q-item-section avatar style="min-width: 0; padding-right: 0">
                <i class="text-size-24" :class="`${item.icono}`" />
              </q-item-section>

              <q-tooltip anchor="center right" self="center left" :offset="[10, 0]" class="bg-black text-body2">
                {{ traducir(item.llave) }}
              </q-tooltip>

              <q-menu anchor="top right" self="top left" :offset="[10, 0]" auto-close>
                <q-list style="min-width: 220px">
                  <q-item-label header class="bg-primary text-primary-contrast text-weight-bold text-subtitle1">
                    {{ traducir(item.llave) }}
                  </q-item-label>
                  <q-separator />

                  <q-item v-for="(child, i) in item.items" :key="i" v-show="child.paginaOculta" :to="child.ruta"
                    clickable v-ripple
                    :class="`text-textprimary q-ma-sm hover-grow ${$route.path === child.ruta ? 'item-seleccionado' : ''}`"
                    :inset-level="0.1">
                    <q-item-section avatar style="min-width: 25px; padding-right: 10px">
                      <i :class="[child.icono, 'text-size-18']" />
                    </q-item-section>
                    <q-item-section>
                      <div>
                        <span
                          :class="`underline-effect ${$route.path === child.ruta ? 'active text-textprimary text-weight-medium' : 'text-textprimary'}`">
                          <span class="text-size-14">{{ traducir(child.llave) }}</span>
                        </span>
                      </div>
                    </q-item-section>
                  </q-item>
                </q-list>
              </q-menu>
            </q-item>
          </div>
        </div>
      </q-list>
    </transition>
  </div>
</template>

<script setup>
import { toRefs, watch, inject, computed } from 'vue'
import datosClientes from 'src/utils/datosClientes.js'
import { useSesionStore } from 'src/stores/sesion'
import { useRoute } from 'vue-router'

const traducir = inject('traducir')
const route = useRoute()
const store = useSesionStore()
const usuario = store.ObtenerUsuario
const esMantenimiento = usuario.esMantenimiento
const datosCliente = datosClientes.obtenerDatosCliente(process.env.clienteActual)

const props = defineProps({
  listado: Array,
  permisos: Array,
  miniState: {
    type: Boolean,
    default: true,
  },
})

const { listado, permisos, miniState } = toRefs(props)

const esSeccionActiva = (item) => {
  if (!item.items) return false
  return item.items.some((child) => child.ruta === route.path)
}

// Key única para triggear la transición cuando cambia el módulo
const listadoKey = computed(() => {
  if (!listado.value || listado.value.length === 0) return 'empty'
  return listado.value.map(item => item.title || item.llave).join('-')
})

watch(
  listado,
  () => {
    listado.value.forEach((seccion) => {
      seccion.items.forEach((pagina) => {
        const listaPermisos = permisos.value.map((x) => x.nombre)
        const permisosPagina = pagina.permisos
        const tienePermisos =
          permisosPagina != undefined
            ? listaPermisos.some((permiso) => permisosPagina.includes(permiso))
            : false
        pagina.paginaOculta = tienePermisos

        if (pagina.isAdminPage != null && !esMantenimiento) pagina.paginaOculta = false
        if (pagina.llave == 'ControlDiesel' && datosCliente.cliente != 'NACOZARI')
          pagina.paginaOculta = false
        if (pagina.ruta == '/Recepcion/ReportesRecepcion' && datosCliente.cliente == 'NACOZARI')
          pagina.paginaOculta = false
      })
      seccion.seccionOculta = seccion.items.some((pagina) => pagina.paginaOculta === true)
    })
  },
  { immediate: true },
)
</script>

<style scoped>
.underline-effect {
  position: relative;
  display: inline-block;
  cursor: pointer;
}

.underline-effect::after {
  content: '';
  position: absolute;
  bottom: -4px;
  left: 0;
  width: 100%;
  height: 2px;
  background-color: var(--q-secondary);
  transform: scaleX(0);
  transform-origin: left;
  transition:
    transform 0.3s ease,
    background-color 0.3s ease;
}

.underline-effect.active::after {
  transform: scaleX(1);
}

.item-seleccionado {
  transition: transform 0.2s ease-in-out;
  transform: scale(1.05);
}

/* Transiciones suaves para cambio de módulo */
.list-fade-enter-active,
.list-fade-leave-active {
  transition: all 0.3s ease-out;
}

.list-fade-enter-from {
  opacity: 0;
  transform: translateY(10px);
}

.list-fade-leave-to {
  opacity: 0;
  transform: translateY(-10px);
}
</style>
