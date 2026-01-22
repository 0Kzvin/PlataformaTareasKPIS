import { axiosApiInstancia } from 'boot/axios'
import * as httpErrorCatcher from 'src/utils/httpErrorCatcher.js'
import * as httpSuccessCatcher from 'src/utils/httpSuccessCatcher.js'

const controlador = 'gerencia/AccesoriosGerencia'

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
