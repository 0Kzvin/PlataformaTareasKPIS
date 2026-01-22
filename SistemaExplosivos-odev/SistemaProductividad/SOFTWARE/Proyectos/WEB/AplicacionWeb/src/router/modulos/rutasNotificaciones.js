import { idModuloNotificaciones } from 'src/core/modulos'

const rutasNotificaciones = [
  {
    path: '/Notificaciones',
    name: 'Notificaciones',
    component: () => import('src/layouts/AplicacionLayout.vue'),
    meta: {
      sidebar: {
        title: 'Notificaciones',
        icon: 'pi pi-bell',
      },
    },
    children: [
      {
        path: '/Notificaciones/Inbox',
        name: 'NotificacionesInbox',
        meta: {
          idModulo: idModuloNotificaciones,
          sidebar: {
            icon: 'pi pi-bell',
            title: 'Notificaciones',
            visible: true,
          },
        },
        component: () => import('pages/Notificaciones/Notificaciones.vue'),
      },
    ],
  },
]

export default rutasNotificaciones
