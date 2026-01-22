<template>
        <q-img :src="url" :alt="nombre" :ratio="4 / 3" fit loading="eager">
            <div class="absolute-bottom text-subtitle1 text-center">{{ nombre }}</div>
            <template v-slot:loading>
                <q-spinner-gears color="white" />
            </template>
        </q-img>
        <q-img 
         v-if="url1"
         loading="eager"
         :src="url1" :alt="nombre1" :ratio="4 / 3" fit>
            <div class="absolute-bottom text-subtitle1 text-center">{{ nombre1 }}</div>
            <template v-slot:loading>
                <q-spinner-gears color="white" />
            </template>
        </q-img>
<q-card>

    <div class="row no-wrap justify-center">
        
    </div>
</q-card>
</template>

<script>
import { defineComponent } from 'vue';
import { QSpinnerGears } from "quasar";

export default defineComponent({
    name: "ImagenViewer",
    components: {
        QSpinnerGears,
    },
    props: {
        propsCustomForm: {
            url: {
                type: String,
                default: null,
            },
            nombre: {
                type: String,
                default: null,
            },
            url1: {
                type: String,
                default: null,
            },
            nombre1: {
                type: String,
                default: null,
            }
        }
    },
    setup(props) {

        const {
            propsCustomForm: {
                url,
                nombre,
                url1,
                nombre1,
            }
        } = props;

        return {
            url,
            nombre,
            url1,
            nombre1,
        }
    }
});
</script>

<style>
</style>