<template>
  <q-page class="column">
    <G3ModuloHeader :modulo="traducir('Monitoreo')" :requiere-boton-filtro="false" />

    <div class="q-mt-md q-pa-sm bg-fondo3" style="border-radius: 12px">
      <q-btn-toggle v-model="tabSeleccionado" spread unelevated style="height: 46px" class="animated-toggle"
        :data-value="tabSeleccionado" :options="[
          { value: 'silos', slot: 'silos' },
          { value: 'equipos', slot: 'equipos' },
          { value: 'piso', slot: 'piso' },
        ]">
        <template #silos>
          <div class="row items-center">
            <i class="pi pi-database q-pr-sm" />
            <span>{{ traducir('Silos') }}</span>
          </div>
        </template>

        <template #equipos>
          <div class="row items-center">
            <i class="pi pi-truck q-pr-sm" />
            <span>{{ traducir('Equipos') }}</span>
          </div>
        </template>

        <template #piso>
          <div class="row items-center">
            <i class="pi pi-box q-pr-sm" />
            <span>{{ traducir('Piso') }}</span>
          </div>
        </template>
      </q-btn-toggle>
    </div>

    <div class="col q-mt-md overflow-hidden">
      <MonitoreoPisoAlmacenamiento v-if="tabSeleccionado === 'piso'" class="fit" />
      <MonitoreoDepositosAlmacenamiento v-if="tabSeleccionado === 'silos'" class="fit" />
      <MonitoreoEquiposAlmacenamiento v-if="tabSeleccionado === 'equipos'" class="fit" />
    </div>
  </q-page>
</template>

<script setup>
import { ref, inject } from 'vue'
import G3ModuloHeader from 'src/components/Genericos/G3ModuloHeader.vue'
import MonitoreoDepositosAlmacenamiento from './components/depositos/MonitoreoDepositosAlmacenamiento.vue'
import MonitoreoEquiposAlmacenamiento from './components/equipos/MonitoreoEquiposAlmacenamiento.vue'
import MonitoreoPisoAlmacenamiento from './components/piso/MonitoreoPisoAlmacenamiento.vue'

const traducir = inject('traducir', (key) => key)

const tabSeleccionado = ref('silos')
</script>

<style scoped>
.animated-toggle {
  position: relative;
  border-radius: 4px;
  overflow: hidden;
  border: 1px solid #d0d0d0;
}

/* Indicador rojo deslizante */
.animated-toggle::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  width: 33.333%;
  height: 100%;
  background-color: var(--q-primary);
  border-radius: 8px;
  transition: transform 0.35s cubic-bezier(0.4, 0, 0.2, 1);
  z-index: 0;
}

/* Posiciones según valor */
.animated-toggle[data-value='silos']::before {
  transform: translateX(0%);
}

.animated-toggle[data-value='equipos']::before {
  transform: translateX(100%);
}

.animated-toggle[data-value='piso']::before {
  transform: translateX(200%);
}

/* Botones */
.animated-toggle .q-btn {
  background: transparent !important;
  border: none;
  z-index: 1;
  color: var(--q-text-secondary);
  transition: color 0.25s ease;
}

/* Texto activo */
.animated-toggle .q-btn--active,
.animated-toggle .q-btn--active .q-icon,
.animated-toggle .q-btn--active .block {
  color: var(--q-primary-contrast) !important;
}
</style>
