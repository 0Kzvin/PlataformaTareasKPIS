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
        icon: 'pi pi-chart-line',
        groupTitle: 'KpisAnalitica',
        visible: true,
      },
    },
    children: [
      {
        path: '/Kpis/DashboardGlobal',
        path: '/Kpis/Global',
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
        path: '/Kpis/Departamento',
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
