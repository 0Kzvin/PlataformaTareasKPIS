<template>
    <q-dialog ref="dialogRef" persistent @hide="onDialogHide">
        <q-card class="q-dialog-plugin tarjeta-loading">
            <div class="row justify-center text-center">
                <!-- <q-spinner-cube :color='loadingColor' size="10em"></q-spinner-cube> -->
                <q-spinner-gears color="primary" size="150px"></q-spinner-gears>
            </div>
        </q-card>
    </q-dialog>
</template>

<script>
import { useDialogPluginComponent, QSpinnerGears } from "quasar";
import { defineComponent } from "vue";

export default defineComponent({
    components: { QSpinnerGears },
    name: "TransparentLoadingDialog",
    props: {
        titulo: {
            type: String,
            default: '',
        },
        mensaje: {
            type: String,
            default: '',
        },
        loadingColor: {
            type: String,
            default: 'secondary',
        },
    },
    emits: [
        ...useDialogPluginComponent.emits
    ],
    setup() {
        const { dialogRef, onDialogHide } = useDialogPluginComponent();

        return {
            dialogRef,
            onDialogHide,
        };
    }
});
</script>

<style scoped>
.tarjeta-loading {
    background-color: transparent;
    box-shadow: none;
}
</style>