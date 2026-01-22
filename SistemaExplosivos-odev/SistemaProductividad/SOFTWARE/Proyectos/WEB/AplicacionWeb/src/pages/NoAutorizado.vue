<template>
  <q-page class="row items-center justify-center bg-fondo3" padding>
    <q-card class="bg-fondo2 shadow-3 rounded-borders q-pa-md column flex-center" style="
        width: 90vw;
        height: 85dvh;
        max-width: none;
        max-height: none;
        border-radius: 20px;
        position: relative;
        overflow-y: auto;
      ">
      <q-card-section class="column items-center full-width q-mb-xl">
        <!-- Responsive Icon Container -->
        <div class="q-mb-lg flex flex-center" style="height: 25dvh; max-height: 250px; width: 100%">
          <q-icon name="shield" color="primary" style="font-size: clamp(80px, 20dvh, 200px)" />
        </div>

        <div class="text-textprimary text-weight-bold q-mb-sm text-center"
          style="font-size: clamp(1.5rem, 5vw, 2.5rem); line-height: 1.2">
          {{ traducir('AccesoDenegado') }}
        </div>

        <div class="text-textsecondary text-center"
          style="font-size: clamp(1.2rem, 4vw, 2rem); line-height: 1.2; margin-bottom: 10px">
          {{ traducir('ContenidoRestringido') }}
        </div>

        <div class="text-textsecondary text-center" style="opacity: 0.7; font-size: clamp(1rem, 3vw, 1.25rem)">
          {{ traducir('SinPermisos') }}
        </div>
      </q-card-section>

      <q-card-actions align="center" class="q-pt-md">
        <q-btn color="primary" text-color="primary-contrast" unelevated rounded no-caps class="text-size-18"
          style="padding: 10px 40px; width: fit-content" @click="goBack">
          <q-icon left name="arrow_back" size="1.2em" />
          <div>{{ traducir('Volver') }}</div>
        </q-btn>
      </q-card-actions>
    </q-card>
  </q-page>
</template>

<script setup>
import { inject } from 'vue'
import { useRouter } from 'vue-router'

const traducir = inject('traducir')
const router = useRouter()

const goBack = () => {
  if (window.history.state?.back) {
    router.back()
  } else {
    router.push({ name: 'LoginPage' })
  }
}
</script>
