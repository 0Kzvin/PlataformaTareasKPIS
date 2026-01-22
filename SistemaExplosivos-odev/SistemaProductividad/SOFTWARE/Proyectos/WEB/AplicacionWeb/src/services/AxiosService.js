import datosClientes from 'src/utils/datosClientes.js'
import { identidadGFuelAdmin } from 'src/api/moduloAdministracion'
import * as quasarUtils from 'src/utils/quasar-utils.js'
import { useSesionStore } from 'src/stores/sesion'
import { traducir } from './TranslationService.js'
import { date } from 'quasar'
import axios from 'axios'

/**
 * Obtiene la URL base de la API según el entorno.
 * @returns {string} La URL base de la API.
 */
export const obtenerDireccionAPI = () => {
  const userHost = window.location.hostname
  const cliente = datosClientes.obtenerDatosCliente(process.env.clienteActual)
  // const apiDev = 'http://192.168.1.71:8082'
  const apiDev = 'http://localhost:5285'

  // 1. Si estamos en localhost (desarrollo)
  if (userHost === 'localhost' || userHost === '127.0.0.1') {
    return apiDev // URL de desarrollo local
  }

  // 2. Si es un dominio (producción en la nube)
  if (!/^\d+\.\d+\.\d+\.\d+$/.test(userHost)) {
    // Buscar la URL que comience con https:// (apiRemota)
    return cliente.URLs.find((url) => url.startsWith('https://')) || apiDev
  }

  // 3. Si es una IP (red local o corporativa)
  const userNetwork = userHost.split('.').slice(0, 3).join('.')

  // Buscar una URL que coincida con los primeros 3 octetos
  const matchingUrl = cliente.URLs.find((url) => {
    try {
      const urlHost = new URL(url).hostname
      const urlNetwork = urlHost.split('.').slice(0, 3).join('.')
      return urlNetwork === userNetwork
    } catch {
      return false
    }
  })

  return matchingUrl || apiDev
}

/**
 * Instancia de Axios configurada con la URL base y opciones por defecto.
 */
const axiosApiInstancia = axios.create({
  baseURL: obtenerDireccionAPI(),
  timeout: 1000 * 120, // 2 minutos
  headers: { 'Accept-Language': 'es' },
  paramsSerializer: {
    serialize: (params) => new URLSearchParams(params).toString(),
  },
})

/**
 * Configura los interceptores de Axios para manejar la autenticación y la renovación del token.
 */
axiosApiInstancia.interceptors.request.use(async (request) => {
  const sesionStore = useSesionStore()

  const rutasExcluidas = [
    'IniciarSesion',
    'SolicitarRecuperacionDeCuenta',
    'VerificarCodigoRecuperacion',
    'RecuperarCuenta',
  ]

  if (rutasExcluidas.some((ruta) => request.url.includes(ruta))) {
    return request
  }

  // Add Authorization header if token exists
  if (sesionStore.tokenInfo && sesionStore.tokenInfo.token) {
    request.headers['Authorization'] = `Bearer ${sesionStore.tokenInfo.token}`
  }

  const segundosRestantes = date.getDateDiff(
    sesionStore.tokenInfo.expiracion,
    new Date(),
    'seconds',
  )

  if (!request.url.includes('MantenerSesion') && segundosRestantes <= 300) {
    try {
      const respuesta = await identidadGFuelAdmin.mantenerSesion(sesionStore.tokenInfo)

      if (!respuesta.exito && !respuesta.statusCode) {
        sesionStore.cerrarSesion()
        quasarUtils.mostrarSnackbar({
          mensaje: traducir('LaSesionHaExpirado'),
        })
        quasarUtils.mostrarSnackbar({ mensaje: respuesta.payload.errores[0] })
        return
      }

      if (
        !respuesta.payload.operacionExitosa &&
        !respuesta.payload.tokenExpirado &&
        respuesta.statusCode !== 401
      ) {
        return request
      }

      if (
        (!respuesta.payload.operacionExitosa && respuesta.payload.tokenExpirado) ||
        respuesta.statusCode === 401
      ) {
        sesionStore.cerrarSesion()
        quasarUtils.mostrarSnackbar({
          mensaje: traducir('LaSesionHaExpirado'),
        })
        quasarUtils.mostrarSnackbar({ mensaje: respuesta.payload.errores[0] })
        return
      }

      sesionStore.guardarTokenBotonLogin({
        tokenInfo: {
          token: respuesta.payload.token,
          actualizarToken: respuesta.payload.actualizarToken,
          expiracion: respuesta.payload.expiracion,
          datosUsuario: respuesta.payload.datosUsuario,
        },
        estaAutentificado: true,
        mantenerSesion: sesionStore.mantenerSesionG,
      })

      request.headers['Authorization'] = `Bearer ${respuesta.payload.token}`
      return request
    } catch {
      // console.error("Error en el interceptor de Axios:", error);
    }
  }

  return request
})

axiosApiInstancia.interceptors.response.use(
  function (response) {
    // Any status code that lie within the range of 2xx cause this function to trigger
    // Do something with response data
    return response
  },
  function (error) {
    // Any status codes that falls outside the range of 2xx cause this function to trigger
    // Do something with response error
    return Promise.reject(error)
  },
)

export { axiosApiInstancia }
