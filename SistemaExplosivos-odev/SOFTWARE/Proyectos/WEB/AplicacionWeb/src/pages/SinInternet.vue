<!-- eslint-disable vue/multi-word-component-names -->
<template>
    <div class="fullscreen bg-fondo3 flex flex-center q-pa-md" style="z-index: 9999">
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
                <div class="q-mb-lg flex flex-center" style="height: 35dvh; max-height: 350px; width: 100%">
                    <q-icon name="wifi_off" size="150px" color="primary" />
                </div>

                <div style="font-size: clamp(3rem, 10vw, 6rem); line-height: 1"
                    class="text-primary text-weight-bold q-mb-md text-center">
                    {{ traducir('SinInternetTitulo') }}
                </div>

                <div class="text-textprimary text-weight-bold q-mb-sm text-center"
                    style="font-size: clamp(1.2rem, 4vw, 2rem); opacity: 0.8">
                    {{ traducir('SinInternetDescripcion') }}
                </div>
            </q-card-section>
        </q-card>
    </div>
</template>

<script setup>
import { inject } from 'vue'

const traducir = inject('traducir')
</script>
