<template>
  <div class="row text-textprimary">
    <div class="col-12">
      <div class="row q-mb-md justify-end">
        <q-input dense outlined :label="traducir('BuscarEquipo')" style="width: 320px">
          <template #prepend>
            <i class="pi pi-search q-pr-sm" />
          </template>
        </q-input>
      </div>

      <div class="row">
        <div class="col-12">
          <draggable
            :move="movimientoCard"
            @end="terminarMovimiento"
            class="row q-col-gutter-md"
            :list="equipos"
            :animation="300"
          >
            <template #item="{ element }">
              <transition-group
                name="fade-move"
                tag="div"
                class="col-lg-4 col-md-6 col-sm-12 col-xs-12"
              >
                <EquipoAlmacenamientoCard :equipo="element" />
              </transition-group>
            </template>
          </draggable>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref, inject } from 'vue'
import draggable from 'vuedraggable'
import { equiposAlmacenamiento } from 'src/api/moduloAlmacenamiento'
import EquipoAlmacenamientoCard from './components/EquipoAlmacenamientoCard.vue'

const traducir = inject('traducir', (key) => key)

const equipos = ref([])
const cargandoEquipo = ref(false)

onMounted(async () => {
  await refrescarDatos()
})

const refrescarDatos = async () => {
  const filtro = {
    borrado: false,
    estado: true,
  }

  cargandoEquipo.value = true
  const resp = await equiposAlmacenamiento.listar(filtro)
  cargandoEquipo.value = false

  if (!resp.exito) {
    equipos.value = []
    return
  }

  equipos.value = resp.payload.respuesta
}
</script>

<style lang="scss" scoped></style>
