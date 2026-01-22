import { axiosApiInstancia } from 'boot/axios'
import * as httpErrorCatcher from 'src/utils/httpErrorCatcher.js'
import * as httpSuccessCatcher from 'src/utils/httpSuccessCatcher.js'

const controlador = 'administracion/CorreosAutomaticos'

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

export const listarDestinatarios = async () => {
  try {
    const resp = await axiosApiInstancia.get(`${controlador}/ListarDestinatarios`)
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const crear = async (model) => {
  try {
    const resp = await axiosApiInstancia.post(`${controlador}/Crear`, model)
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const actualizar = async (model) => {
  try {
    const resp = await axiosApiInstancia.put(`${controlador}/Actualizar`, model)
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const cambiarEstado = async (id) => {
  try {
    const resp = await axiosApiInstancia.put(`${controlador}/CambiarEstado/${id}`)
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}
