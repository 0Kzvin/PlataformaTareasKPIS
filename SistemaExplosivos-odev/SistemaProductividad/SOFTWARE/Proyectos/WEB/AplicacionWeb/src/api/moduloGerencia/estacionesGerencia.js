import { axiosApiInstancia } from 'boot/axios'
import * as httpErrorCatcher from 'src/utils/httpErrorCatcher.js'
import * as httpSuccessCatcher from 'src/utils/httpSuccessCatcher.js'

const controlador = 'gerencia/EstacionesGerencia'

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

export const registrar = async (estacion) => {
  try {
    const resp = await axiosApiInstancia.post(`${controlador}/Registrar`, estacion)

    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const modificar = async (estacion) => {
  try {
    const resp = await axiosApiInstancia.put(`${controlador}/Modificar`, estacion)
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const cambiarEstado = async (idModelo) => {
  try {
    const resp = await axiosApiInstancia.put(`${controlador}/CambiarEstado`, null, {
      params: {
        id: idModelo,
      },
    })
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const borrar = async (idmodelo) => {
  try {
    const resp = await axiosApiInstancia.put(`${controlador}/Borrar`, null, {
      params: {
        id: idmodelo,
      },
    })
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}
