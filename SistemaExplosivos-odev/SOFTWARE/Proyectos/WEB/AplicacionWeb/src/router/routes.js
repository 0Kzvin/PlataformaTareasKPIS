import rutasAccesorios from './modulos/rutasAccesorio'
import rutasAdministracion from './modulos/rutasAdministracion'
import rutasAlmacenamiento from './modulos/rutasAlmacenamiento'
import rutasGerencia from './modulos/rutasGerencia'
import rutasRecepcion from './modulos/rutasRecepcion'

const routes = [
  {
    path: '/',
    name: 'Login',
    component: () => import('src/layouts/LoginLayout.vue'),
    children: [
      {
        path: '',
        name: 'LoginPage',
        component: () => import('pages/Login.vue'),
      },
    ],
  },
  {
    path: '/:catchAll(.*)*',
    name: 'Error404',
    component: () => import('pages/Error404.vue'),
  },
  {
    path: '/NoAutorizadoGlobal',
    component: () => import('layouts/LoginLayout.vue'),
    children: [
      {
        path: '',
        name: 'NoAutorizadoGlobal',
        component: () => import('pages/NoAutorizado.vue'),
      },
    ],
  },
  {
    path: '/MantenimientoGlobal',
    component: () => import('layouts/LoginLayout.vue'),
    children: [
      {
        path: '',
        name: 'MantenimientoGlobal',
        component: () => import('pages/Mantenimiento.vue'),
      },
    ],
  },
  ...rutasAdministracion,
  ...rutasAlmacenamiento,
  ...rutasGerencia,
  ...rutasAccesorios,
  ...rutasRecepcion,
]

export default routes
