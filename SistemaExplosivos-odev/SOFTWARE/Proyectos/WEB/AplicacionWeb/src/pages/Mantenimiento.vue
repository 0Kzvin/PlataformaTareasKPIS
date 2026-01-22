<!-- eslint-disable vue/multi-word-component-names -->
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
        <!-- Responsive Image Container -->
        <div class="q-mb-lg flex flex-center" style="height: 35dvh; max-height: 300px; width: 100%">
          <img src="~assets/maintenance_web_nobg.gif" style="max-width: 100%; max-height: 100%; object-fit: contain"
            alt="Mantenimiento" />
        </div>

        <!-- Responsive Typography using clamp() -->
        <div class="text-textprimary text-weight-bold q-mb-sm text-center"
          style="font-size: clamp(1.5rem, 5vw, 2.5rem); line-height: 1.2">
          {{ traducir('AccesoDenegado') }}
        </div>

        <div class="text-primary text-center q-mb-md" style="font-size: clamp(1.2rem, 4vw, 2rem); line-height: 1.2">
          {{ traducir('SeccionMantenimiento') }}
        </div>

        <div class="text-textsecondary text-center" style="font-size: clamp(1rem, 3vw, 1.25rem); opacity: 0.8">
          {{ traducir('DisponibleProx') }}
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
