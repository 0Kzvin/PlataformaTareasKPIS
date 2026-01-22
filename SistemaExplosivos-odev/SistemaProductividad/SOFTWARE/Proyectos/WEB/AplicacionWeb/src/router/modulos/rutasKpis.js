import { idModuloKpis } from 'src/core/modulos'

const rutasKpis = [
  {
    path: '/Kpis',
    name: 'Kpis',
    component: () => import('src/layouts/AplicacionLayout.vue'),
    meta: {
      sidebar: {
        title: 'Kpis',
        icon: 'pi pi-chart-line',
      },
    },
    children: [
      {
        path: '/Kpis/DashboardGlobal',
        name: 'DashboardGlobal',
        meta: {
          idModulo: idModuloKpis,
          sidebar: {
            icon: 'pi pi-chart-bar',
            title: 'DashboardGlobal',
            visible: true,
          },
        },
        component: () => import('pages/Dashboard/GlobalDashboard.vue'),
      },
      {
        path: '/Kpis/DashboardDepartamento',
        name: 'DashboardDepartamento',
        meta: {
          idModulo: idModuloKpis,
          sidebar: {
            icon: 'pi pi-chart-pie',
            title: 'DashboardDepartamento',
            visible: true,
          },
        },
        component: () => import('pages/Dashboard/DepartmentDashboard.vue'),
      },
      {
        path: '/Kpis/GestionKpis',
        name: 'GestionKpis',
        meta: {
          idModulo: idModuloKpis,
          sidebar: {
            icon: 'pi pi-sliders-h',
            title: 'GestionKpis',
            visible: true,
          },
        },
        component: () => import('pages/Kpis/GestionKpis.vue'),
      },
    ],
  },
]

export default rutasKpis
