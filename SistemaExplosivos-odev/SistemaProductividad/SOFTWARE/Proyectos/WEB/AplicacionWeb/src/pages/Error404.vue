<!-- eslint-disable vue/multi-word-component-names -->
<template>
  <div class="fullscreen bg-fondo3 flex flex-center q-pa-md">
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
        <!-- 404 GIF Container -->
        <div class="q-mb-lg flex flex-center" style="height: 35dvh; max-height: 350px; width: 100%">
          <img src="~assets/no_data_nobg.gif" style="max-width: 100%; max-height: 100%; object-fit: contain"
            alt="404" />
        </div>

        <div style="font-size: clamp(3rem, 10vw, 6rem); line-height: 1" class="text-primary text-weight-bold q-mb-md">
          404
        </div>

        <div class="text-textprimary text-weight-bold q-mb-sm text-center"
          style="font-size: clamp(1.2rem, 4vw, 2rem); opacity: 0.8">
          {{ traducir('Mensaje404') }}
        </div>

        <div class="text-textsecondary text-center" style="font-size: clamp(1rem, 4vw, 1.5rem); opacity: 0.8">
          {{ traducir('Descripcion404') }}
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
  </div>
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
