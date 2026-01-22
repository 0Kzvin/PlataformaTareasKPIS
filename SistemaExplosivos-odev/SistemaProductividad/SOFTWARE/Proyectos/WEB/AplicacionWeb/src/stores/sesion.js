import { defineStore, acceptHMRUpdate } from 'pinia'
import { jwtDecode } from 'jwt-decode'
import { identidadGFuelAdmin } from 'src/api/moduloAdministracion'
import { traducir } from 'src/services/TranslationService.js'
import * as quasarUtils from 'src/utils/quasar-utils.js'

export const useSesionStore = defineStore('sesion', {
  id: 'sesion',
  state: () => ({
    _estaAutentificado: false,
    _mantenerSesion: false,
    tokenInfo: {
      token: null,
      actualizarToken: null,
      expiracion: null,
    },
    usuario: {
      idUsuario: null,
      nombreUsuario: null,
      rol: null,
      nombreCompleto: null,
      nombre: null,
      apellidos: null,
      correo: null,
      estado: null,
      areaSeleccionada: null,
      numeroTelefonico: null,
      fechaRegistro: null,
      fechaModificacion: null,
      esMantenimiento: false,
      foto: null,
      configuracionesModulos: null,
    },
    permisosOtorgados: [],
    modulosOtorgados: [],
  }),
  actions: {
    // * Para obtener mutliple parametros por el store actions es necesario utilizar un objeto restructurado.
    // * El segundo siempre es el parametro
    async guardarTokenBotonLogin({
      tokenInfo,
      mantenerSesionF = false,
      estaAutentificado = false,
    }) {
      cambiarToken(tokenInfo)
      cambiarMantenerSesion(mantenerSesionF)
      cambiarEstaAutenticado(estaAutentificado)
      await this.obtenerModulos({
        usuarioOCorreo: this.$state.usuario.nombreUsuario,
      })
      await this.obtenerPermisos({
        usuarioOCorreo: this.$state.usuario.nombreUsuario,
      })
      if (mantenerSesionF) {
        localStorage.setItem('tokenInfo', JSON.stringify(tokenInfo))
      }
    },
    async mantenerSesion() {
      var tokenInfo = JSON.parse(localStorage.getItem('tokenInfo'))
      var mantenerSesionM = localStorage.getItem('mantenerSesion')
      if (mantenerSesionM !== null && mantenerSesionM !== undefined) {
        cambiarMantenerSesion(mantenerSesionM)
      }
      if (tokenInfo) {
        cambiarToken(tokenInfo)
        const resp = await identidadGFuelAdmin.mantenerSesion(tokenInfo)

        // * Si la operacion no fue exitosa y no cuenta con un status code, desloguear al usuario
        // ? Usualmente pasa cuando la API no se encuentra en la red correctamente
        if (resp.exito === false && !resp.statusCode) {
          await this.cerrarSesion()
          quasarUtils.mostrarSnackbar({
            mensaje: traducir('LaSesionHaExpirado'),
          })
          quasarUtils.mostrarSnackbar({ mensaje: resp.payload.errores[0] })
          return
        }

        // * Si la operacion de refrescar token no fue exitosa y el token no esta expirado, obtener modulos, permisos y autentica el usuario
        if (
          !resp.payload.operacionExitosa &&
          !resp.payload.tokenExpirado &&
          resp.statusCode != 401
        ) {
          await this.obtenerModulos({
            usuarioOCorreo: this.$state.usuario.nombreUsuario,
          })
          await this.obtenerPermisos({
            usuarioOCorreo: this.$state.usuario.nombreUsuario,
          })

          cambiarEstaAutenticado(true)
          return
        }

        // * Si la operacion de refrescar token no fue exitosa y el token esta expirado, desloguear al usuario
        if (
          (!resp.payload.operacionExitosa && resp.payload.tokenExpirado) ||
          resp.statusCode == 401
        ) {
          quasarUtils.mostrarSnackbar({
            mensaje: traducir('LaSesionHaExpirado'),
          })
          cambiarToken('')
          localStorage.removeItem('tokenInfo')
          cambiarEstaAutenticado(false)
          await this.cerrarSesion()
          quasarUtils.mostrarSnackbar({
            mensaje:
              typeof resp.payload.errores[0] === 'string'
                ? resp.payload.errores[0]
                : 'Ocurrió un error inesperado',
          })
          return
        }

        // * Si el token esta activo, guardar el token en el estado y consecuentemente en el localstorage
        const newTokenInfo = {
          token: resp.payload.token,
          actualizarToken: resp.payload.actualizarToken,
          expiracion: resp.payload.expiracion,
          datosUsuario: resp.payload.datosUsuario,
        }
        cambiarToken(newTokenInfo)
        await this.obtenerModulos({
          usuarioOCorreo: this.$state.usuario.nombreUsuario,
        })
        await this.obtenerPermisos({
          usuarioOCorreo: this.$state.usuario.nombreUsuario,
        })
        localStorage.setItem('tokenInfo', JSON.stringify(newTokenInfo))
        cambiarEstaAutenticado(true)
      }
    },
    async cerrarSesion() {
      cambiarToken('')
      insertarPermisos([])
      insertarModulos([])
      cambiarMantenerSesion(false)
      cambiarEstaAutenticado(false)
      localStorage.removeItem('tokenInfo')
      this.$router.push({ name: 'LoginPage' })
    },
    async obtenerPermisos({ usuarioOCorreo = '' }) {
      const resp = await identidadGFuelAdmin.ObtenerPermisosOtorgados(usuarioOCorreo)
      var permisosObtenidos = []
      if (resp.exito) {
        permisosObtenidos = resp.payload.payload || resp.payload || []
      }

      // Hardcode for development if empty
      if (!Array.isArray(permisosObtenidos) || permisosObtenidos.length === 0) {
        permisosObtenidos = []
      }

      insertarPermisos(permisosObtenidos)
    },
    async obtenerModulos({ usuarioOCorreo = '' }) {
      const resp = await identidadGFuelAdmin.ObtenerModulosOtorgados(usuarioOCorreo)
      var modulosOtorgadosResp = []
      if (resp.exito) {
        modulosOtorgadosResp = resp.payload.payload || resp.payload || []
      }

      // Hardcode for development if empty
      if (!Array.isArray(modulosOtorgadosResp) || modulosOtorgadosResp.length === 0) {
        modulosOtorgadosResp = [
          { id: 1, nombre: 'Administracion', activo: true },
          { id: 2, nombre: 'Departamentos', activo: true },
          { id: 3, nombre: 'Tareas', activo: true },
          { id: 4, nombre: 'Kpis', activo: true },
          { id: 5, nombre: 'Reportes', activo: true },
          { id: 6, nombre: 'Notificaciones', activo: true },
          { id: 7, nombre: 'Auditoria', activo: true },
        ]
      }

      insertarModulos(modulosOtorgadosResp)
    },
    estaPermisoOtorgado(param) {
      const nombrePermiso = typeof param === 'object' ? param.nombrePermiso : param
      const permisosOtorgados = this.$state.permisosOtorgados
      return permisosOtorgados.filter((x) => x.nombre == nombrePermiso).length > 0
    },
    estaAlgunPermisoOtorgado({ permisos = [] }) {
      const permisosOtorgados = this.$state.permisosOtorgados
      return permisosOtorgados.some((x) => permisos.includes(x))
    },
    estaTodosPermisosOtorgados({ permisos = [] }) {
      const permisosOtorgados = this.$state.permisosOtorgados
      return permisosOtorgados.every((x) => permisos.includes(x))
    },
    insertEstaAutenticado({ estaAutentificado = false }) {
      cambiarEstaAutenticado(estaAutentificado)
    },
    refrescarDatosUsuarios({ infoUsuario }) {
      cambiarDatos(infoUsuario)
    },
    refrescarFotoUsuarios({ foto }) {
      cambiarFoto(foto)
    },
  },
  getters: {
    token: (state) => state.tokenInfo.token,
    mantenerSesionG: (state) => state._mantenerSesion,
    obtenerPermisosOtorgados: (state) => state.permisosOtorgados,
    obtenerModulosOtorgados: (state) => state.modulosOtorgados,
    obtenerNombreUsuario: (state) => state.usuario.nombreUsuario,
    obtenerNombreCompleto: (state) => state.usuario.nombreCompleto,
    ObtenerUsuario: (state) => state.usuario,
    estaAutentificado: (state) => state._estaAutentificado,
    esLider: (state) => {
      const roles = Array.isArray(state.usuario.rol) ? state.usuario.rol : [state.usuario.rol];
      return roles.some(r => r === 'Lider' || r === 'SuperAdmin' || r === 'Administrador');
    }
  },
})

const cambiarToken = (tokenInfoX) => {
  var stateSesion = useSesionStore()
  if (tokenInfoX === '' || tokenInfoX === null) {
    stateSesion.$state.usuario = {}
    stateSesion.$state.tokenInfo.token = null
    stateSesion.$state.tokenInfo.actualizarToken = null
    stateSesion.$state.tokenInfo.expiracion = null
  } else {
    const datosToken = jwtDecode(tokenInfoX.token)
    const datosUsuario = tokenInfoX.datosUsuario || {}

    stateSesion.$state.tokenInfo.token = tokenInfoX.token
    stateSesion.$state.tokenInfo.actualizarToken = tokenInfoX.actualizarToken
    stateSesion.$state.tokenInfo.expiracion = tokenInfoX.expiracion
    stateSesion.$state.usuario.idUsuario = datosToken.Id
    stateSesion.$state.usuario.nombreUsuario = datosToken.sub
    stateSesion.$state.usuario.rol = datosToken.role
    stateSesion.$state.usuario.nombreCompleto = datosToken.NombreCompleto
    stateSesion.$state.usuario.correo = datosToken.email
    stateSesion.$state.usuario.estado = datosUsuario.estado ?? null
    stateSesion.$state.usuario.areaSeleccionada = datosUsuario.areaSeleccionada ?? null
    stateSesion.$state.usuario.numeroTelefonico = datosUsuario.numeroTelefonico ?? null
    stateSesion.$state.usuario.fechaRegistro = datosUsuario.fechaRegistro ?? null
    stateSesion.$state.usuario.fechaModificacion = datosUsuario.fechaModificacion ?? null
    stateSesion.$state.usuario.nombre = datosUsuario.nombre ?? null
    stateSesion.$state.usuario.apellidos = datosUsuario.apellidos ?? null
    stateSesion.$state.usuario.foto = datosUsuario.foto ?? null
    stateSesion.$state.usuario.configuracionesModulos =
      datosUsuario.configuracionesModulos ?? null
    stateSesion.$state.usuario.esMantenimiento = tokenInfoX.esMantenimiento ?? false
  }
}
const cambiarMantenerSesion = (valor) => {
  var stateSesion = useSesionStore()
  localStorage.setItem('mantenerSesion', valor)
  stateSesion.$state._mantenerSesion = valor
}
const cambiarEstaAutenticado = (valor) => {
  var stateSesion = useSesionStore()
  stateSesion.$state._estaAutentificado = valor
}
const insertarPermisos = (permisos) => {
  var stateSesion = useSesionStore()
  stateSesion.$state.permisosOtorgados = permisos
}
const insertarModulos = (modulos) => {
  var stateSesion = useSesionStore()
  stateSesion.$state.modulosOtorgados = modulos
}
const cambiarDatos = (valor) => {
  var stateSesion = useSesionStore()
  stateSesion.$state.usuario.nombre = valor.Nombre
  stateSesion.$state.usuario.apellidos = valor.Apellidos
  stateSesion.$state.usuario.nombreCompleto = valor.NombreCompleto
  stateSesion.$state.usuario.nombreUsuario = valor.Username
  stateSesion.$state.usuario.numeroTelefonico = valor.PhoneNumber
  stateSesion.$state.usuario.correo = valor.Email
}
const cambiarFoto = (valor) => {
  var stateSesion = useSesionStore()
  stateSesion.$state.usuario.foto = valor
}

if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(useSesionStore, import.meta.hot))
}
