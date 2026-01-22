import { axiosApiInstancia } from 'boot/axios'
import * as httpErrorCatcher from 'src/utils/httpErrorCatcher.js'
import * as httpSuccessCatcher from 'src/utils/httpSuccessCatcher.js'

const controlador = 'almacenamiento/DepositosAlmacenamiento'

export const listar = async (filtro) => {
  try {
    const resp = await axiosApiInstancia.get(`${controlador}/Listar`, {
      params: filtro,
    })

    return httpSuccessCatcher.catchSuccess(resp.data)
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

export const borrar = async (idModelo) => {
  try {
    const resp = await axiosApiInstancia.put(`${controlador}/Borrar`, null, {
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
