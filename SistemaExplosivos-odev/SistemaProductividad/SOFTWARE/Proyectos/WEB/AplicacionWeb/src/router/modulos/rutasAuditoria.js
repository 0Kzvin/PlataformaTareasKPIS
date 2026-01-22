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
      },
    ],
  },
]

export default rutasAuditoria
