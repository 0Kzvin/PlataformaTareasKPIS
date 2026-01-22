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
        icon: 'pi pi-file',
        groupTitle: 'Reportes',
        visible: true,
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
        path: '/Reportes/Resumen',
        name: 'ReportesPrincipal',
        meta: {
          idModulo: idModuloReportes,
          sidebar: {
            icon: 'pi pi-file-excel',
            visible: true,
          },
        },
        component: () => import('pages/Reportes/Reportes.vue'),
      },
      {
        path: '/Reportes/Mantenimiento',
        name: 'MantenimientoReportes',
        meta: {
          idModulo: idModuloReportes,
          sidebar: { visible: false },
        },
        component: () => import('pages/Mantenimiento.vue'),
      },
      {
        path: '/Reportes/NoAutorizado',
        name: 'NoAutorizadoReportes',
        meta: {
          idModulo: idModuloReportes,
          sidebar: { visible: false },
        },
        component: () => import('pages/NoAutorizado.vue'),
      },
    ],
  },
]

export default rutasReportes
