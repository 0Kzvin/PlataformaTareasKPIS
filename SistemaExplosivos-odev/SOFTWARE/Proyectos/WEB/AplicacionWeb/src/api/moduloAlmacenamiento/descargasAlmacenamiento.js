import { axiosApiInstancia } from 'boot/axios'
import * as httpErrorCatcher from 'src/utils/httpErrorCatcher.js'
import * as httpSuccessCatcher from 'src/utils/httpSuccessCatcher.js'

const controlador = 'almacenamiento/CargasDepositosAlmacenamiento'

export const listar = async (filtro = null) => {
  try {
    const resp = await axiosApiInstancia.get(`${controlador}/Listar`, {
      params: filtro,
    })

    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}
