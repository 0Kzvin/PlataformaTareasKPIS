import { axiosApiInstancia } from 'boot/axios'
import * as httpErrorCatcher from 'src/utils/httpErrorCatcher.js'
import * as httpSuccessCatcher from 'src/utils/httpSuccessCatcher.js'

const controlador = 'administracion/Modulos'

export const listar = async (agruparPermisosEnModulo = false) => {
  try {
    const resp = await axiosApiInstancia.get(`${controlador}/ListarPorAdministrador`, {
      params: {
        agruparPermisosEnModulo,
      },
    })
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const listarModulos = async () => {
  try {
    const resp = await axiosApiInstancia.get(`${controlador}/Listar`)
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const cambiarEstado = async (id) => {
  try {
    const resp = await axiosApiInstancia.patch(`${controlador}/CambiarEstado/${id}`)
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}
