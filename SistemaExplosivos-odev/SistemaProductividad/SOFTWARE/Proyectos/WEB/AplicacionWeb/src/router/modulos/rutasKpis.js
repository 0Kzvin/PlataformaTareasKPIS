import { idModuloKpis } from 'src/core/modulos'

const rutasKpis = [
  {
    path: '/Kpis',
    name: 'Kpis',
    component: () => import('src/layouts/AplicacionLayout.vue'),
    meta: {
      sidebar: {
        icon: 'pi pi-chart-line',
        groupTitle: 'KpisAnalitica',
        visible: true,
      },
    },
    children: [
      {
        path: '/Kpis/Global',
        name: 'DashboardGlobal',
        meta: {
          idModulo: idModuloKpis,
          sidebar: {
            icon: 'pi pi-chart-bar',
            visible: true,
          },
        },
        component: () => import('pages/Dashboard/GlobalDashboard.vue'),
      },
      {
        path: '/Kpis/Departamento',
        name: 'DashboardDepartamento',
        meta: {
          idModulo: idModuloKpis,
          sidebar: {
            icon: 'pi pi-chart-pie',
            visible: true,
          },
        },
        component: () => import('pages/Dashboard/DepartmentDashboard.vue'),
      },
      {
        path: '/Kpis/Mantenimiento',
        name: 'MantenimientoKpis',
        meta: {
          idModulo: idModuloKpis,
          sidebar: { visible: false },
        },
        component: () => import('pages/Mantenimiento.vue'),
      },
      {
        path: '/Kpis/NoAutorizado',
        name: 'NoAutorizadoKpis',
        meta: {
          idModulo: idModuloKpis,
          sidebar: { visible: false },
        },
        component: () => import('pages/NoAutorizado.vue'),
      },
    ],
  },
]

export default rutasKpis
