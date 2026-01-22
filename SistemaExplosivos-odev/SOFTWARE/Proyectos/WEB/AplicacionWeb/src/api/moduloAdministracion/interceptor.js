import { axiosApiInstancia } from 'boot/axios'
import { identidadGFuelAdmin } from 'src/api/moduloAdministracion'
import * as quasarUtils from 'src/utils/quasar-utils.js'
import { useSesionStore } from '../../stores/sesion'
import { i18n } from 'src/boot/i18n.js'
import { date } from 'quasar'

const t = (string) => {
  return i18n.global.t(string)
}

const token = () => {
  return useSesionStore().tokenInfo.token
}

const tokenInfo = () => {
  return useSesionStore().tokenInfo
}

const cerrarSesion = () => {
  return useSesionStore().cerrarSesion()
}
const mantenerSesion = () => {
  return useSesionStore().mantenerSesionG
}
const guardarTokenBotonLogin = (infoToken) => {
  return useSesionStore().guardarTokenBotonLogin(infoToken)
}

export default function setup(/* store */) {
  axiosApiInstancia.interceptors.request.use(async (request) => {
    request.headers['Authorization'] = `Bearer ${token()}`
    if (
      request.url.includes('IniciarSesion') ||
      request.url.includes('SolicitarRecuperacionDeCuenta') ||
      request.url.includes('VerificarCodigoRecuperacion') ||
      request.url.includes('RecuperarCuenta')
    ) {
      return request
    }

    const segundosRestantes = date.getDateDiff(tokenInfo().expiracion, new Date(), 'seconds')

    if (!request.url.includes('ActualizarToken') && segundosRestantes <= 300) {
      try {
        const resp = await identidadGFuelAdmin.mantenerSesion(tokenInfo())

        // * Si la operacion no fue exitosa y no cuenta con un status code, desloguear al usuario
        // ? Usualmente pasa cuando la API no se encuentra en la red correctamente
        if (resp.exito === false && !resp.statusCode) {
          cerrarSesion()
          quasarUtils.mostrarSnackbar({ mensaje: t('LaSesionHaExpirado') })
          quasarUtils.mostrarSnackbar({ mensaje: resp.payload.errores[0] })
          return
        }
        // * Si la operacion de refrescar token no fue exitosa y el token no esta expirado, dejar pasar la peticion
        if (
          !resp.payload.operacionExitosa &&
          !resp.payload.tokenExpirado &&
          resp.statusCode != 401
        ) {
          return request
        }

        // * Si la operacion de refrescar token no fue exitosa y el token esta expirado, deslogear al usuario
        if (
          (!resp.payload.operacionExitosa && resp.payload.tokenExpirado) ||
          resp.statusCode == 401
        ) {
          cerrarSesion()
          quasarUtils.mostrarSnackbar({ mensaje: t('LaSesionHaExpirado') })
          quasarUtils.mostrarSnackbar({ mensaje: resp.payload.errores[0] })
          return
        }

        const newTokenInfo = {
          token: resp.payload.token,
          actualizarToken: resp.payload.actualizarToken,
          expiracion: resp.payload.expiracion,
          datosUsuario: resp.payload.datosUsuario,
        }

        guardarTokenBotonLogin({
          tokenInfo: newTokenInfo,
          estaAutentificado: true,
          mantenerSesion: mantenerSesion(),
        })
        return request
      } catch (err) {
        cerrarSesion()
        quasarUtils.mostrarSnackbar({ mensaje: t('LaSesionHaExpirado') })
        quasarUtils.mostrarSnackbar({ mensaje: err })
      }
    }
    return request
  })
}
