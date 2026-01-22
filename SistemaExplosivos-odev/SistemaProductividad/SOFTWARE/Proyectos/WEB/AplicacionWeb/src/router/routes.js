import rutasAdministracion from './modulos/rutasAdministracion'
import rutasAlmacenamiento from './modulos/rutasAlmacenamiento'
import rutasRecepcion from './modulos/rutasRecepcion'
import rutasAccesorio from './modulos/rutasAccesorio'
import rutasGerencia from './modulos/rutasGerencia'

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
  {
    path: '/app',
    component: () => import('layouts/AplicacionLayout.vue'), // Keep existing app layout
    children: [
      {
        path: 'dashboard/global',
        name: 'DashboardGlobal',
        component: () => import('pages/Dashboard/GlobalDashboard.vue'),
        meta: { requireAuth: true, roles: ['SuperAdmin'] }
      },
      {
        path: 'dashboard/departamento',
        name: 'DashboardDepartamento',
        component: () => import('pages/Dashboard/DepartmentDashboard.vue'),
        meta: { requireAuth: true, roles: ['Lider', 'Colaborador'] }
      },
      {
        path: 'tareas',
        name: 'ListadoTareas',
        component: () => import('pages/Tareas/ListadoTareas.vue'),
      },
      {
        path: 'departamentos',
        name: 'GestionDepartamentos',
        component: () => import('pages/Configuracion/Departamentos.vue'),
        meta: { requireAuth: true, roles: ['SuperAdmin'] }
      }
    ]
  },
  ...rutasAdministracion,
  ...rutasAlmacenamiento,
  ...rutasRecepcion,
  ...rutasAccesorio,
  ...rutasGerencia,
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/Error404.vue'),
  }
]

export default routes
