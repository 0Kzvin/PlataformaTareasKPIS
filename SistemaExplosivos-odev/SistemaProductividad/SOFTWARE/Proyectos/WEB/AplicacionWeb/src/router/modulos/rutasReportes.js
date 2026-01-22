import { idModuloReportes } from 'src/core/modulos'

const rutasReportes = [
  {
    path: '/Reportes',
    name: 'Reportes',
    component: () => import('src/layouts/AplicacionLayout.vue'),
    meta: {
      sidebar: {
        title: 'Reportes',
        icon: 'pi pi-file',
      },
    },
    children: [
      {
        path: '/Reportes/Reportes',
        name: 'ReportesListado',
        meta: {
          idModulo: idModuloReportes,
          sidebar: {
            icon: 'pi pi-file',
            title: 'Reportes',
            visible: true,
          },
        },
        component: () => import('pages/Reportes/Reportes.vue'),
      },
    ],
  },
]

export default rutasReportes
