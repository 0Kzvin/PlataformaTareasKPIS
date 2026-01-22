import { axiosApiInstancia } from 'boot/axios'
import * as httpErrorCatcher from 'src/utils/httpErrorCatcher.js'
import * as httpSuccessCatcher from 'src/utils/httpSuccessCatcher.js'

const controlador = 'almacenamiento/EstatusDepositosAlmacenamiento'

export const listarTanquesEstatus = async (filtro) => {
  try {
    const resp = await axiosApiInstancia.get(`${controlador}/ListarTanquesEstatus`, {
      params: filtro,
    })

    return httpSuccessCatcher.catchSuccess(resp.data)
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const listarSuperSacosEstatus = async (filtro) => {
  try {
    const resp = await axiosApiInstancia.get(`${controlador}/ListarSuperSacosEstatus`, {
      params: filtro,
    })

    return httpSuccessCatcher.catchSuccess(resp)
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}
