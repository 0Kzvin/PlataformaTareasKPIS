import { axiosApiInstancia } from 'boot/axios'
import * as httpErrorCatcher from 'src/utils/httpErrorCatcher.js'
import * as httpSuccessCatcher from 'src/utils/httpSuccessCatcher.js'

const controlador = 'gerencia/OperadoresGerencia'

export const listar = async () => {
  try {
    const resp = await axiosApiInstancia.get(`${controlador}/Listar`)

    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const registrar = async (modelo) => {
  try {
    const resp = await axiosApiInstancia.post(`${controlador}/Registrar`, modelo)

    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const modificar = async (modelo) => {
  try {
    const resp = await axiosApiInstancia.put(`${controlador}/Modificar`, modelo)
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const cambiarEstado = async (idmodelo) => {
  try {
    const resp = await axiosApiInstancia.put(`${controlador}/CambiarEstado`, idmodelo)
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const borrar = async (idmodelo) => {
  try {
    const resp = await axiosApiInstancia.put(`${controlador}/Borrar`, idmodelo)
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}
