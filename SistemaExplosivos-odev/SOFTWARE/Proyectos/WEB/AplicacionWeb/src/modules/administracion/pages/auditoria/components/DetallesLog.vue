<template>
    <q-card class="q-pa-md" style="max-width: 800px; max-height: 850px; min-width: 460px">
        <q-card-section class="row justify-center">
            <q-icon name="info" color="accent" :size="textoProporcion" />
            <span class="text-weight-normal text-size-21 q-ml-sm">{{ $t('InformacionDetalladaLog') }}</span>
        </q-card-section>

        <q-card-section>
            <div class="row justify-between">
                <span class="text-size-14">{{ $t('Mensaje') }}:</span>
                <span class="text-size-14 text-weight-bold">{{ log.mensaje }}</span>
            </div>

            <div class="row justify-between">
                <span class="text-size-14">{{ $t('Nivel') }}:</span>
                <span class="text-size-14 text-weight-bold">{{ log.nivel }}</span>
            </div>

            <div class="row justify-between">
                <span class="text-size-14">{{ $t('Excepcion') }}:</span>
                <span
                    class="text-size-14 text-weight-bold"
                >{{ (log.excepcion) ? log.excepcion : '-' }}</span>
            </div>

            <div class="row justify-between">
                <span class="text-size-14">{{ $t('Usuario') }}:</span>
                <span class="text-size-14 text-weight-bold">{{ log.usuario }}</span>
            </div>

            <div class="row justify-between">
                <span class="text-size-14">{{ $t('Accion') }}:</span>
                <span class="text-size-14 text-weight-bold">{{ log.accion }}</span>
            </div>

            <div class="row justify-between">
                <span class="text-size-14">{{ $t('FechaHora') }}:</span>
                <span
                    class="text-size-14 text-weight-bold"
                >{{ log.fechaHora }}</span>
            </div>

            <!-- <q-virtual-scroll
            :items="(log.datosPeticion) ? [log.datosPeticion] : []"
            style="max-height: 200px; height: 150px;"
        >
            <template v-slot="{ item }">
                {{item}}
            </template>
            </q-virtual-scroll>-->
        </q-card-section>
    </q-card>
</template>

<script>
import { defineComponent } from 'vue';
import { date } from "quasar";
import { usePreferenciaStore } from "src/stores/preferencias";

export default defineComponent({
    name: "DetallesLog",
    props: {
        propsCustomForm: {
            type: Object,
        },
    },
    emits: [
        "onDialogCancel",
        "onDialogHide",
        "onDialogOK",
    ],
    setup(props) {
        const store = usePreferenciaStore();
        const textoProporcion = store.textoProporcion;
        const {
            propsCustomForm: {
                log,
            }
        } = props;


        return {
            log,
            date,
            textoProporcion,
        };
    },
});
</script>