import { defineStore, acceptHMRUpdate } from 'pinia'
import {
  guardarLenguaje,
  procesarCambiosDeLenguaje,
  opcionesLenguaje,
  obtenerLenguaje,
  obtenerOpcionLenguaje,
} from 'src/services/TranslationService.js'
import {
  inicializarTema,
  toggleDarkMode,
  guardarColores,
  aplicarColores,
  DEFAULT_COLORS,
} from 'src/services/ThemeService'
import {
  opcionesTextos,
  cambiarTextSize,
  obtenerTextSize,
  obtenerOpcionTextSize,
  guardarTextSize,
} from 'src/services/TextSizeService'

export const usePreferenciaStore = defineStore('preferencias', {
  id: 'preferencias',
  state: () => ({
    lenguaje: obtenerLenguaje(),
    opcionLenguaje: obtenerOpcionLenguaje(),
    opcionesLenguaje: opcionesLenguaje,

    // Nuevo estado de temas
    modoOscuro: false,
    coloresPersonalizados: { ...DEFAULT_COLORS },

    textoProporcion: obtenerTextSize(),
    opcionTextSize: obtenerOpcionTextSize(),
    opcionesTextSize: opcionesTextos,
  }),

  actions: {
    // Acciones de Tema
    cambiarModoOscuro(valor) {
      this.modoOscuro = valor
      toggleDarkMode(valor)
    },

    actualizarColor(key, valor) {
      this.coloresPersonalizados[key] = valor
      aplicarColores(this.coloresPersonalizados)
      guardarColores(this.coloresPersonalizados)
    },

    restablecerColores() {
      this.coloresPersonalizados = { ...DEFAULT_COLORS }
      aplicarColores(this.coloresPersonalizados)
      guardarColores(this.coloresPersonalizados)
    },

    // Acciones de Texto y Lenguaje (Mantener igual)
    cambiarAparienciaTexto(nuevaProporcion) {
      this.textoProporcion = nuevaProporcion
      guardarTextSize(nuevaProporcion)
    },
    cambiarAparienciaLenguaje(nuevoLenguaje) {
      this.lenguaje = nuevoLenguaje
      guardarLenguaje(nuevoLenguaje)
    },

    cargarConfiguraciones() {
      procesarCambiosDeLenguaje()
      cambiarTextSize()

      // Cargar tema nuevo
      const { colores, isDark } = inicializarTema()
      this.coloresPersonalizados = colores
      this.modoOscuro = isDark
    },
  },

  getters: {
    ObtenerApariencia: (state) => {
      return state
    },
    ObtenerColores: (state) => state.coloresPersonalizados,
  },
})

if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(usePreferenciaStore, import.meta.hot))
}
