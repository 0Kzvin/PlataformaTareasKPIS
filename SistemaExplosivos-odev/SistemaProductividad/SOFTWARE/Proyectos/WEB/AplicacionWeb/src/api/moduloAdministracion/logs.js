import { axiosApiInstancia } from 'boot/axios'
import * as httpErrorCatcher from 'src/utils/httpErrorCatcher.js'
import * as httpSuccessCatcher from 'src/utils/httpSuccessCatcher.js'

const controlador = 'administracion/Logs'

export const listar = async (fecha1 = null, fecha2 = null) => {
  try {
    const resp = await axiosApiInstancia.get(`${controlador}/Listar`, {
      params: {
        fecha1,
        fecha2,
      },
    })
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}
