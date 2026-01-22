import { idModuloNotificaciones } from 'src/core/modulos'

const rutasNotificaciones = [
  {
    path: '/Notificaciones',
    name: 'Notificaciones',
    component: () => import('src/layouts/AplicacionLayout.vue'),
    meta: {
      sidebar: {
        icon: 'pi pi-bell',
        groupTitle: 'Notificaciones',
        visible: true,
      },
    },
    children: [
      {
        path: '/Notificaciones/Resumen',
        name: 'NotificacionesPrincipal',
        meta: {
          idModulo: idModuloNotificaciones,
          sidebar: {
            icon: 'pi pi-bell',
            visible: true,
          },
        },
        component: () => import('pages/Notificaciones/Notificaciones.vue'),
      },
      {
        path: '/Notificaciones/Mantenimiento',
        name: 'MantenimientoNotificaciones',
        meta: {
          idModulo: idModuloNotificaciones,
          sidebar: { visible: false },
        },
        component: () => import('pages/Mantenimiento.vue'),
      },
      {
        path: '/Notificaciones/NoAutorizado',
        name: 'NoAutorizadoNotificaciones',
        meta: {
          idModulo: idModuloNotificaciones,
          sidebar: { visible: false },
        },
        component: () => import('pages/NoAutorizado.vue'),
      },
    ],
  },
]

export default rutasNotificaciones
