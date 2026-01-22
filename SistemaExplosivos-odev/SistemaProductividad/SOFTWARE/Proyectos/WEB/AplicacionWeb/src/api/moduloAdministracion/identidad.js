import { axiosApiInstancia } from 'boot/axios'
import * as httpErrorCatcher from 'src/utils/httpErrorCatcher.js'
import * as httpSuccessCatcher from 'src/utils/httpSuccessCatcher.js'

const controlador = 'administracion/Identidad'

export const login = async (usuarioOCorreo, password) => {
  try {
    const resp = await axiosApiInstancia.post(`${controlador}/IniciarSesion`, {
      usuarioOCorreo: usuarioOCorreo,
      password: password,
    })
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const solicitarRecuperacionCuenta = async (email) => {
  try {
    const resp = await axiosApiInstancia.post(`${controlador}/SolicitarRecuperacionDeCuenta`, {
      usuarioOcorreo: email,
    })

    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const verificarCodigoRecuperacion = async (codigo) => {
  try {
    const resp = await axiosApiInstancia.post(`${controlador}/VerificarCodigoRecuperacion`, {
      CodigoRecuperacion: codigo,
    })

    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const recuperarCuenta = async (data) => {
  try {
    const resp = await axiosApiInstancia.post(`${controlador}/RecuperarCuenta`, {
      Password: data.password,
      ConfirmarPassword: data.confirmarPassword,
      Codigo: data.codigo,
    })

    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const mantenerSesion = async (tokenInfo) => {
  try {
    const resp = await axiosApiInstancia.post(`${controlador}/MantenerSesion`, tokenInfo)
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const listarUsuarios = async () => {
  try {
    const resp = await axiosApiInstancia.get(`${controlador}/ListarUsuarios`)
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const listarPermisos = async (agrupar = false, idModulos = [], incluirOcultados = true) => {
  try {
    const resp = await axiosApiInstancia.get(`${controlador}/ListarPermisos`, {
      params: {
        grupo: agrupar,
        idModulos: idModulos,
        incluirOcultados,
      },
      // paramsSerializer: params => {
      //     return qs.stringify(params)
      // }
    })
    //console.log(resp);
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const listarRoles = async (soloActivos = false) => {
  try {
    const resp = await axiosApiInstancia.get(`${controlador}/ListarRoles/${soloActivos}`)
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const crearRol = async (model) => {
  try {
    const resp = await axiosApiInstancia.post(`${controlador}/CrearRol`, model)
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const registrarUsuario = async (model) => {
  try {
    const resp = await axiosApiInstancia.post(`${controlador}/Registrar`, model)
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const editarUsuario = async (model) => {
  try {
    const resp = await axiosApiInstancia.put(`${controlador}/EditarUsuario`, model)
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const editarMiUsuario = async (model) => {
  try {
    const resp = await axiosApiInstancia.put(`${controlador}/EditarMiUsuario`, model)
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const cambiarFotoPerfil = async (model) => {
  try {
    const form = new FormData()
    for (const key in model) {
      form.append(key, model[key])
    }

    const resp = await axiosApiInstancia.put(`${controlador}/CambiarFotoPerfil`, form, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    })
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const editarRol = async (model) => {
  try {
    const resp = await axiosApiInstancia.put(`${controlador}/EditarRol`, model)
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const borrarUsuario = async (id) => {
  try {
    const resp = await axiosApiInstancia.delete(`${controlador}/BorrarUsuario/${id}`)
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const borrarRol = async (id) => {
  try {
    const resp = await axiosApiInstancia.delete(`${controlador}/BorrarRol/${id}`)
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const cambiarEstadoUsuario = async (id) => {
  try {
    const resp = await axiosApiInstancia.patch(`${controlador}/CambiarEstadoUsuario`, {
      id,
    })
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const cambiarEstadoRol = async (model) => {
  try {
    const resp = await axiosApiInstancia.patch(`${controlador}/CambiarEstadoRol`, model)
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const ObtenerPermisosOtorgados = async (usuarioOCorreo) => {
  try {
    const resp = await axiosApiInstancia.get(
      `${controlador}/ObtenerPermisosOtorgados/${usuarioOCorreo}`,
    )
    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}

export const ObtenerModulosOtorgados = async (usuarioOCorreo) => {
  try {
    const resp = await axiosApiInstancia.get(
      `${controlador}/ObtenerModulosOtorgados/${usuarioOCorreo}`,
    )

    if (resp) {
      return httpSuccessCatcher.catchSuccess(resp.data)
    }
  } catch (err) {
    return httpErrorCatcher.catchError(err)
  }
}
