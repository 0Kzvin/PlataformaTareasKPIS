import rutasAdministracion from './modulos/rutasAdministracion'
import rutasDepartamentos from './modulos/rutasDepartamentos'
import rutasTareas from './modulos/rutasTareas'
import rutasKpis from './modulos/rutasKpis'
import rutasReportes from './modulos/rutasReportes'
import rutasNotificaciones from './modulos/rutasNotificaciones'
import rutasAuditoria from './modulos/rutasAuditoria'

const routes = [
  {
    path: '/',
    name: 'Login',
    component: () => import('layouts/LoginLayout.vue'), // Keep existing login layout
    children: [
      {
        path: '',
        name: 'LoginPage',
        component: () => import('pages/Login.vue'),
      },
    ],
  },
  ...rutasAdministracion,
  ...rutasDepartamentos,
  ...rutasTareas,
  ...rutasKpis,
  ...rutasReportes,
  ...rutasNotificaciones,
  ...rutasAuditoria,
  {
    path: '/no-autorizado',
    name: 'NoAutorizado',
    component: () => import('pages/NoAutorizado.vue'),
  },
  {
    path: '/mantenimiento',
    name: 'Mantenimiento',
    component: () => import('pages/Mantenimiento.vue'),
  },
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/Error404.vue'),
  }
]

export default routes
