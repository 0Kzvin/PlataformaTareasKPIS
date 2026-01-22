import { createI18n } from 'vue-i18n'
import messages from '../i18n'
import { Quasar } from 'quasar'
import { axiosApiInstancia } from './AxiosService'

const i18nMessages = {
  'es-MX': messages['es_MX'], // Spanish (Mexico)
  'en-US': messages['en-US'], // English
}

const langQuasarPath = (idioma) => {
  return `../../node_modules/quasar/lang/${idioma}.js`
}

const langList = import.meta.glob('../../node_modules/quasar/lang/*.js')

const opcionesLenguaje = [
  { llave: 'Espanol', label: 'Espanol', value: 'es-MX' },
  { llave: 'Ingles', label: 'Ingles', value: 'en-US' },
]

/**
 * Traduce un string usando la instancia de i18n.
 * Si la llave no existe o es null/undefined, devuelve la llave original.
 * @param {string} texto - El texto a traducir.
 * @param {string} fallback - Valor de respaldo si la traducción no existe (opcional).
 * @returns {string} El texto traducido o la llave original.
 */
const traducir = (texto, fallback = null) => {
  if (!texto) return fallback ?? texto
  try {
    // Check if the key exists in the current locale messages
    const messages = i18n.global.messages.value[i18n.global.locale.value] || {}
    if (!(texto in messages)) {
      return fallback ?? texto
    }
    return i18n.global.t(texto)
  } catch {
    return fallback ?? texto
  }
}

/**
 * Carga el idioma guardado en el localStorage.
 * Si no existe un idioma guardado, devuelve "es-MX" como idioma por defecto.
 * @returns {string} El idioma guardado o "es-MX" por defecto.
 */
const obtenerLenguaje = () => {
  return localStorage.getItem('lenguaje') ?? 'es-MX'
}

const obtenerOpcionLenguaje = () => {
  return opcionesLenguaje.find((opcion) => opcion.value === obtenerLenguaje())
}

/**
 * Guarda un nuevo idioma en el localStorage.
 * Si no se proporciona un idioma, se guarda "es-MX" por defecto.
 * @param {string} nuevoLenguaje - El idioma a guardar (por ejemplo, "es-MX" o "en-US").
 */
const guardarLenguaje = (nuevoLenguaje = 'es-MX') => {
  localStorage.setItem('lenguaje', nuevoLenguaje)
  procesarCambiosDeLenguaje()
}

// Crea una instancia de i18n con la configuración inicial.
const i18n = createI18n({
  locale: obtenerLenguaje(), // Establece el idioma actual cargándolo desde localStorage.
  fallbackLocale: 'es-MX', // Idioma de respaldo en caso de que no se encuentre una traducción.
  messages: i18nMessages, // Objeto que contiene los mensajes de traducción para cada idioma.
  legacy: false, // Desactiva el modo Legacy API (preparación para v11+)
})

const cambiarIdiomai18n = () => {
  const lenguaje = obtenerLenguaje()
  i18n.global.locale.value = lenguaje
}

const cambiarIdiomaQuasar = () => {
  const lenguaje = obtenerLenguaje()
  const idiomaQuasar = lenguaje === 'es-MX' ? 'es' : lenguaje

  langList[langQuasarPath(idiomaQuasar)]()
    .then((lang) => {
      Quasar.lang.set(lang.default)
    })
    .catch(() => {})
}

const cambiarIdiomaPeticionesAxios = () => {
  const lenguaje = obtenerLenguaje().split('-')[0]
  axiosApiInstancia.defaults.headers.common['Accept-Language'] = lenguaje
}

const procesarCambiosDeLenguaje = () => {
  cambiarIdiomai18n()
  cambiarIdiomaQuasar()
  cambiarIdiomaPeticionesAxios()
}

// Exporta las funciones y la instancia de i18n para su uso en otros archivos.
export {
  i18n,
  traducir,
  guardarLenguaje,
  obtenerLenguaje,
  obtenerOpcionLenguaje,
  procesarCambiosDeLenguaje,
  opcionesLenguaje,
}
