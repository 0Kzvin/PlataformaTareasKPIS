<template>
  <q-page class="q-pa-md">
    <div class="row items-center q-mb-md">
      <div class="text-h4 col-grow">Notificaciones</div>
      <q-btn flat icon="refresh" label="Actualizar" @click="cargarNotificaciones" />
    </div>

    <q-list bordered separator>
      <q-item v-for="notificacion in notificaciones" :key="notificacion.id">
        <q-item-section>
          <q-item-label class="text-weight-medium">
            {{ notificacion.titulo }}
          </q-item-label>
          <q-item-label caption>{{ notificacion.mensaje }}</q-item-label>
        </q-item-section>
        <q-item-section side>
          <q-btn
            size="sm"
            color="primary"
            outline
            :label="notificacion.leido ? 'Leída' : 'Marcar leída'"
            :disable="notificacion.leido"
            @click="marcarLeida(notificacion)"
          />
        </q-item-section>
      </q-item>
      <q-item v-if="notificaciones.length === 0">
        <q-item-section>
          <q-item-label>No hay notificaciones registradas.</q-item-label>
        </q-item-section>
      </q-item>
    </q-list>
  <q-page padding>
    <div class="text-h5 text-weight-bold q-mb-sm">{{ traducir('Notificaciones') }}</div>
    <div class="text-body1 text-textsecondary">
      {{ traducir('NotificacionesDescripcion') }}
    </div>
  </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { api } from 'boot/axios'
import { useQuasar } from 'quasar'

const $q = useQuasar()
const notificaciones = ref([])

const cargarNotificaciones = async () => {
  try {
    const { data } = await api.get('/notificaciones/Listar')
    notificaciones.value = data
  } catch (error) {
    console.error(error)
    $q.notify({ type: 'negative', message: 'No se pudieron cargar las notificaciones.' })
  }
}

const marcarLeida = async (notificacion) => {
  try {
    await api.post('/notificaciones/MarcarLeida', { id: notificacion.id, leido: true })
    notificacion.leido = true
  } catch (error) {
    console.error(error)
    $q.notify({ type: 'negative', message: 'No se pudo actualizar la notificación.' })
  }
}

onMounted(cargarNotificaciones)
import { inject } from 'vue'

const traducir = inject('traducir', (key) => key)
</script>
