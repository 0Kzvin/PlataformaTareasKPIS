import { idModuloAuditoria } from 'src/core/modulos'

const rutasAuditoria = [
  {
    path: '/Auditoria',
    name: 'Auditoria',
    component: () => import('src/layouts/AplicacionLayout.vue'),
    meta: {
      sidebar: {
        title: 'Auditoria',
        icon: 'pi pi-history',
        icon: 'pi pi-eye',
        groupTitle: 'Auditoria',
        visible: true,
      },
    },
    children: [
      {
        path: '/Auditoria/Registros',
        name: 'AuditoriaRegistros',
        meta: {
          idModulo: idModuloAuditoria,
          sidebar: {
            icon: 'pi pi-history',
            title: 'Auditoria',
            visible: true,
          },
        },
        component: () => import('pages/Auditoria/Auditoria.vue'),
        path: '/Auditoria/Trazabilidad',
        name: 'AuditoriaTrazabilidad',
        meta: {
          idModulo: idModuloAuditoria,
          sidebar: {
            icon: 'pi pi-book',
            visible: true,
          },
        },
        component: () => import('pages/Auditoria/Trazabilidad.vue'),
      },
      {
        path: '/Auditoria/Mantenimiento',
        name: 'MantenimientoAuditoria',
        meta: {
          idModulo: idModuloAuditoria,
          sidebar: { visible: false },
        },
        component: () => import('pages/Mantenimiento.vue'),
      },
      {
        path: '/Auditoria/NoAutorizado',
        name: 'NoAutorizadoAuditoria',
        meta: {
          idModulo: idModuloAuditoria,
          sidebar: { visible: false },
        },
        component: () => import('pages/NoAutorizado.vue'),
      },
    ],
  },
]

export default rutasAuditoria
