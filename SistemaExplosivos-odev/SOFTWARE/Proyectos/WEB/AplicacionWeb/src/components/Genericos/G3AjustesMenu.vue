<template>
    <div class="g3-ajustes-menu-container" :style="estiloContenedor">
        <div :class="dense ? 'q-pa-sm' : 'q-px-md q-pt-md q-pb-md'">
            <q-list>
                <!-- Modo Oscuro Switch -->
                <q-item clickable v-ripple class="q-mb-sm" style="border-radius: 8px" :dense="dense"
                    @click="modoOscuroComputed = !modoOscuroComputed">
                    <q-item-section avatar style="min-width: 0" class="q-pr-md">
                        <i
                            :class="[modoOscuroComputed ? 'pi pi-moon' : 'pi pi-sun', dense ? 'text-size-20' : 'text-size-24']" />
                    </q-item-section>
                    <q-item-section>
                        <span class="ellipsis" :class="dense ? 'text-body2' : 'text-subtitle1'"
                            style="min-width: 120px">
                            {{ modoOscuroComputed ? traducir('ModoOscuro') : traducir('ModoClaro') }}
                        </span>
                    </q-item-section>
                    <q-item-section side>
                        <q-toggle v-model="modoOscuroComputed" color="primary" keep-color :dense="dense"
                            :size="dense ? 'xs' : 'md'" />
                    </q-item-section>
                </q-item>

                <q-expansion-item v-model="estadoExpansionLenguaje" group="ajustesGroup" expand-icon="none"
                    class="q-mb-sm" :header-class="[
                        estadoExpansionLenguaje ? 'bg-primary text-primary-contrast' : 'text-textprimary',
                    ]" header-style="border-radius: 8px" :dense="dense">
                    <template v-slot:header>
                        <div class="row items-center full-width no-wrap">
                            <q-item-section avatar style="min-width: 0" class="q-pr-md">
                                <i class="pi pi-language" :class="dense ? 'text-size-20' : 'text-size-24'" />
                            </q-item-section>
                            <q-item-section>
                                <span class="ellipsis" :class="dense ? 'text-body2' : 'text-subtitle1'">
                                    {{ traducir('Lenguaje') }}
                                </span>
                            </q-item-section>
                        </div>
                    </template>

                    <div class="bg-fondo" :class="dense ? 'q-my-xs q-pa-xs' : 'q-my-sm q-pa-sm'"
                        style="border-radius: 8px">
                        <q-list>
                            <q-item v-for="opcion in preferenciasStore.opcionesLenguaje" :key="opcion.value" clickable
                                v-ripple class="q-mb-sm" :class="preferenciasStore.lenguaje === opcion.value
                                    ? 'bg-primary text-primary-contrast'
                                    : 'bg-fondo3 text-textprimary'
                                    " style="border-radius: 8px" @click="seleccionarLenguaje(opcion.value)"
                                :dense="dense">
                                <q-item-section class="text-weight-medium">
                                    <div class="row items-center justify-between">
                                        <span :class="dense ? 'text-caption' : 'text-body2'">{{
                                            traducir(opcion.llave)
                                            }}</span>
                                        <q-icon v-if="preferenciasStore.lenguaje === opcion.value" name="check"
                                            size="sm" />
                                    </div>
                                </q-item-section>
                            </q-item>
                        </q-list>
                    </div>
                </q-expansion-item>

                <q-expansion-item v-model="estadoExpansionTema" group="ajustesGroup" expand-icon="none" class="q-mt-sm"
                    :header-class="[
                        estadoExpansionTema ? 'bg-primary text-primary-contrast' : 'text-textprimary',
                    ]" header-style="border-radius: 8px" :dense="dense">
                    <template v-slot:header>
                        <div class="row items-center full-width no-wrap">
                            <q-item-section avatar style="min-width: 0" class="q-pr-md">
                                <i class="pi pi-palette" :class="dense ? 'text-size-20' : 'text-size-24'" />
                            </q-item-section>
                            <q-item-section>
                                <span class="ellipsis" :class="dense ? 'text-body2' : 'text-subtitle1'">
                                    {{ traducir('Apariencia') }}
                                </span>
                            </q-item-section>
                        </div>
                    </template>

                    <div class="bg-fondo" :class="dense ? 'q-my-xs q-pa-xs' : 'q-my-sm q-pa-sm'"
                        style="border-radius: 8px">
                        <!-- Selector de Colores Dinámicos -->
                        <div class="q-pa-xs">
                            <div class="row items-center justify-between q-mb-sm">
                                <div class="text-caption text-textsecondary">
                                    {{ traducir('PersonalizarColores') }}
                                </div>
                                <q-btn flat dense round icon="restart_alt" color="textsecondary" size="sm"
                                    @click="restablecerColores">
                                    <i class="" />
                                    <q-tooltip>{{ traducir('RestablecerColores') }}</q-tooltip>
                                </q-btn>
                            </div>

                            <div class="row q-col-gutter-xs justify-center">
                                <template v-for="(colorHex, colorName) in preferenciasStore.ObtenerColores"
                                    :key="colorName">
                                    <!-- En ambos tamaños 3 columnas (col-4 = 12/4 = 3 columnas) -->
                                    <div v-if="colorName !== 'accent'"
                                        class="col-4 flex column flex-center transition-generic q-mb-sm">
                                        <q-btn :style="{ backgroundColor: colorHex }" round
                                            :size="$q.screen.lt.sm ? 'sm' : 'md'" class="shadow-1"
                                            :class="$q.screen.lt.sm ? 'q-mb-none' : 'q-mb-xs'">
                                            <q-popup-proxy transition-show="scale" transition-hide="scale">
                                                <div class="color-picker-container bg-fondo3">
                                                    <!-- Selector de espectro -->
                                                    <q-color v-model="coloresProxy[colorName]" format-model="hex"
                                                        no-header no-footer default-view="spectrum" class="my-picker"
                                                        @update:model-value="(val) => actualizarColor(colorName, val)" />
                                                    <!-- Paleta de colores predefinidos -->
                                                    <q-color v-model="coloresProxy[colorName]" format-model="hex"
                                                        no-header no-footer default-view="palette"
                                                        :palette="paletaColores" class="my-picker-palette"
                                                        @update:model-value="(val) => actualizarColor(colorName, val)" />
                                                </div>
                                            </q-popup-proxy>
                                        </q-btn>
                                        <div class="text-caption text-center ellipsis q-pt-xs"
                                            style="max-width: 100%; line-height: 1.1">
                                            {{ traducir(colorName) }}
                                        </div>
                                    </div>
                                </template>
                            </div>
                        </div>
                    </div>
                </q-expansion-item>

                <q-expansion-item v-model="estadoExpansionTexto" group="ajustesGroup" expand-icon="none" class="q-mt-sm"
                    :header-class="[
                        estadoExpansionTexto ? 'bg-primary text-primary-contrast' : 'text-textprimary',
                    ]" header-style="border-radius: 8px" :dense="dense">
                    <template v-slot:header>
                        <div class="row items-center full-width no-wrap">
                            <q-item-section avatar style="min-width: 0" class="q-pr-md">
                                <i class="pi pi-book" :class="dense ? 'text-size-20' : 'text-size-24'" />
                            </q-item-section>
                            <q-item-section>
                                <span class="ellipsis" :class="dense ? 'text-body2' : 'text-subtitle1'">
                                    {{ traducir('TextoProporcion') }}
                                </span>
                            </q-item-section>
                        </div>
                    </template>

                    <div class="bg-fondo" :class="dense ? 'q-my-xs q-pa-xs' : 'q-my-sm q-pa-sm'"
                        style="border-radius: 8px">
                        <q-list>
                            <q-item v-for="opcion in preferenciasStore.opcionesTextSize" :key="opcion.value" clickable
                                v-ripple class="q-mb-sm" :class="preferenciasStore.textoProporcion === opcion.value
                                    ? 'bg-primary text-primary-contrast'
                                    : 'bg-fondo3 text-textprimary'
                                    " style="border-radius: 8px" @click="seleccionarTexto(opcion.value)"
                                :dense="dense">
                                <q-item-section class="text-weight-medium">
                                    <div class="row items-center justify-between">
                                        <span :class="dense ? 'text-caption' : 'text-body2'">{{
                                            traducir(opcion.llave)
                                            }}</span>
                                        <q-icon v-if="preferenciasStore.textoProporcion === opcion.value" name="check"
                                            size="sm" />
                                    </div>
                                </q-item-section>
                            </q-item>
                        </q-list>
                    </div>
                </q-expansion-item>
            </q-list>
        </div>
    </div>
</template>

<script setup>
import { ref, inject, computed, watch } from 'vue'
import { usePreferenciaStore } from 'src/stores/preferencias'

// PROPS
defineProps({
    dense: { type: Boolean, default: false },
})

// INYECCIONES
const traducir = inject('traducir', (key) => key)
const preferenciasStore = usePreferenciaStore()

// MÉTODOS
const seleccionarLenguaje = (valor) => {
    preferenciasStore.cambiarAparienciaLenguaje(valor)
}

// Computada para el v-model del toggle con el store
const modoOscuroComputed = computed({
    get: () => preferenciasStore.modoOscuro,
    set: (val) => preferenciasStore.cambiarModoOscuro(val),
})

// Objeto local para los v-model de los color pickers (para reactividad inmediata en el popup)
const coloresProxy = ref({ ...preferenciasStore.coloresPersonalizados })

// Sincronizar proxy si cambian externamente
watch(
    () => preferenciasStore.coloresPersonalizados,
    (newVal) => {
        coloresProxy.value = { ...newVal }
    },
    { deep: true },
)

const actualizarColor = (nombre, valor) => {
    preferenciasStore.actualizarColor(nombre, valor)
}

const restablecerColores = () => {
    preferenciasStore.restablecerColores()
}

const seleccionarTexto = (valor) => {
    preferenciasStore.cambiarAparienciaTexto(valor)
}

// ESTADO LOCAL
const estadoExpansionLenguaje = ref(false)
const estadoExpansionTema = ref(false)
const estadoExpansionTexto = ref(false)

// ESTILO DINÁMICO DEL CONTENEDOR
// El menú crece dinámicamente hasta un máximo del 80% de la altura de la pantalla
// Solo muestra scroll cuando supera ese límite
const estiloContenedor = computed(() => ({
    width: '100%',
    maxHeight: '85dvh',
    overflowY: 'auto',
    overflowX: 'hidden',
}))

// Paleta de colores predefinidos para selección rápida (30 colores)
// Colores seleccionados para buena visibilidad en tema claro y oscuro
const paletaColores = [
    // Rojos y naranjas
    '#e41321', '#ff6b6b', '#ff8c42', '#e17055', '#d63031',
    // Amarillos y dorados
    '#f2c037', '#f1c40f', '#e67e22', '#f39c12', '#d35400',
    // Verdes
    '#149849', '#00b894', '#27ae60', '#a3cb38', '#2ecc71',
    // Azules y turquesas
    '#0984e3', '#3498db', '#45b7d1', '#1abc9c', '#00cec9',
    // Morados y rosas
    '#6c5ce7', '#8e44ad', '#fd79a8', '#e84393', '#9b59b6',
    // Neutros (tonos medios que funcionan en ambos temas)
    '#7e7e7e', '#636e72', '#95a5a6', '#34495e', '#576574',
]
</script>

<style lang="scss">
.my-picker {

    // Asegurar que los inputs sean visibles y estilo "editable"
    input {
        border: 1px solid var(--q-primary) !important;
        border-radius: 4px;
        padding: 4px 8px;
        background: rgba(var(--q-primary), 0.1);
        text-align: center;
        font-weight: bold;
        transition: all 0.3s ease;

        &:focus {
            outline: none;
            box-shadow: 0 0 5px var(--q-primary);
            background: rgba(var(--q-primary), 0.2);
        }
    }

    // Ajustes opcionales para mejorar la visualización general
    .q-color-picker__header-tabs {
        background: transparent;
    }
}

// Contenedor para los dos q-color
.color-picker-container {
    display: flex;
    flex-direction: column;
    border-radius: 12px;
    overflow: hidden;
    padding: 6px;
    gap: 4px;
    max-width: 220px;

    // Ajuste para móviles
    @media (max-width: 600px) {
        max-width: 280px;
        padding: 10px;
    }
}

// Spectrum picker más pequeño
.my-picker {
    max-width: 200px;

    @media (max-width: 600px) {
        max-width: 100%;
    }

    .q-color-picker__spectrum {
        height: 150px !important;
    }
}

// Paleta de colores más compacta
.my-picker-palette {
    max-width: 200px;

    @media (max-width: 600px) {
        max-width: 100%;
    }

    .q-color-picker__cube {
        width: 20px !important;
        height: 20px !important;
        border-radius: 3px;

        // Cubos más grandes en móvil para facilitar el toque
        @media (max-width: 600px) {
            width: 28px !important;
            height: 28px !important;
        }
    }
}

.g3-ajustes-menu-container {
    // Transición suave al expandir/contraer
    transition: height 0.3s ease, max-height 0.3s ease;

    // Scroll personalizado usando colores del tema
    &::-webkit-scrollbar {
        width: 6px;
    }

    &::-webkit-scrollbar-track {
        background: var(--q-fondo);
        border-radius: 3px;
    }

    &::-webkit-scrollbar-thumb {
        background: var(--q-textsecondary);
        border-radius: 3px;
        opacity: 0.5;
    }

    &::-webkit-scrollbar-thumb:hover {
        background: var(--q-primary);
    }
}
</style>
