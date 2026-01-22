import { defineRouter } from '#q-app/wrappers'
import {
  createRouter,
  createMemoryHistory,
  createWebHistory,
  createWebHashHistory,
} from 'vue-router'
import routes from './routes'

import { idModuloIds, detallesModulos } from 'src/core/modulos'
import { useSesionStore } from 'src/stores/sesion'
import { source } from 'src/utils/utils'
import { verificarVersion } from 'src/services/AppVersionService'

let routerExp = null

const routerGuard = async (to, from, next, store, Router) => {
  if (source) {
    source.cancel()
  }

  await verificarVersion()

  const rutaResuelta = Router.resolve({ name: to.name })

  if (['Error404', 'NoAutorizado'].includes(rutaResuelta.name)) {
    next()
    return
  }

  const storeSesion = useSesionStore()
  let estaAutentificado = store.state.value.sesion._estaAutentificado

  if (!estaAutentificado) {
    await storeSesion.mantenerSesion()
    estaAutentificado = store.state.value.sesion._estaAutentificado
  }

  const permisosOtorgados = store.state.value.sesion.permisosOtorgados
  const modulosOtorgados = store.state.value.sesion.modulosOtorgados

  if (estaAutentificado) {
    if (
      to.name && (to.name.toUpperCase().includes('NOAUTORIZADO') ||
        to.name.toUpperCase().includes('MANTENIMIENTO'))
    ) {
      next()
      return
    }

    // If no permissions/modules configured, allow all access (development mode)
    if (!Array.isArray(permisosOtorgados) || !Array.isArray(modulosOtorgados) ||
      permisosOtorgados.length === 0 || modulosOtorgados.length === 0) {
      ocultarLoadingGlobal()
      next()
      return
    }

    const tienePermiso = permisosOtorgados.some((po) =>
      to.matched.some((r) => r.meta.permisos?.includes(po.nombre) ?? false),
    )
    const tieneAccesoModulo = modulosOtorgados.some((mo) => to.meta.idModulo === mo.id)

    ocultarLoadingGlobal()

    if (!tieneAccesoModulo) {
      enviarAModulo(modulosOtorgados, next, to, permisosOtorgados)
      return
    }

    const usuario = store.state.value.sesion.usuario
    const esPermitidoMantenimiento = usuario?.esMantenimiento

    if (!esPermitidoMantenimiento && to.matched.some((r) => r.meta.enMantenimiento)) {
      const moduloDestino = routes?.find((x) => x.name == to.path?.split('/')[1])
      const detallesMod = detallesModulos.find((x) => x.nombre == moduloDestino?.name)
      const pagMantenimiento = detallesMod?.npMantenimiento ?? 'Error404'

      next({ name: pagMantenimiento })
      return
    }

    if (tienePermiso) {
      next()
      return
    }

    const detallesModulo = detallesModulos.find((x) => x.id == to.meta.idModulo)
    const routeName = detallesModulo?.npNoAutorizado ?? 'Error404'

    next({ name: routeName })
    return
  }

  if (to.path === '/' && from.path === '/') {
    next()
    return
  }

  if (to.name === 'LoginPage') {
    next()
    return
  }

  next({
    name: 'LoginPage',
  })
}

export default defineRouter(function ({ store }) {
  const createHistory = process.env.SERVER
    ? createMemoryHistory
    : process.env.VUE_ROUTER_MODE === 'history'
      ? createWebHistory
      : createWebHashHistory

  const Router = createRouter({
    scrollBehavior: () => ({ left: 0, top: 0 }),
    routes,

    // Leave this as is and make changes in quasar.conf.js instead!
    // quasar.conf.js -> build -> vueRouterMode
    // quasar.conf.js -> build -> publicPath
    history: createHistory(process.env.VUE_ROUTER_BASE),
  })

  Router.beforeEach(async (to, from, next) => {
    await routerGuard(to, from, next, store, Router)
  })

  routerExp = Router

  return Router
})

const enviarAModulo = async (modulosOtorgados, next, to = null, permisosOtorgados = null) => {
  const moduloHome = modulosOtorgados.find((x) => x.id !== idModuloIds.idModuloAdmin)
  const detallesModulo = detallesModulos.find((x) => x.id == moduloHome?.id)
  const routeName = detallesModulo?.npPrincipal ?? 'Error404'

  if (to != null) {
    if (to.name != 'LoginPage') {
      const moduloDestino = routes?.find((x) => x.name == to?.redirectedFrom?.path?.split('/')[1])
      const paginaDestino = moduloDestino?.children?.find((x) => x.path == to?.path)
      const permisosPaginaDestino = paginaDestino?.meta?.permisos
      const tienePermiso = permisosOtorgados
        ?.map((x) => x.nombre)
        ?.some((x) => permisosPaginaDestino?.includes(x))

      if (permisosPaginaDestino != null && tienePermiso) {
        next({ name: to.redirectedFrom.name })
        return
      } else {
        next({ name: 'NoAutorizado' })
        return
      }
    }
  }

  next({ name: routeName })
}

const ocultarLoadingGlobal = () => {
  try {
    const loadingPage = document.getElementById('loadingGlobal')

    if (!loadingPage) {
      return
    }

    loadingPage.remove()
  } catch {
    console.log('El loading global no se ha podido borrar')
  }
}

export { routerExp }
