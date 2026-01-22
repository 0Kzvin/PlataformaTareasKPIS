import { idModuloDepartamentos } from 'src/core/modulos'

const rutasDepartamentos = [
  {
    path: '/Departamentos',
    name: 'Departamentos',
    component: () => import('src/layouts/AplicacionLayout.vue'),
    meta: {
      sidebar: {
        title: 'Departamentos',
        icon: 'pi pi-sitemap',
        groupTitle: 'Departamentos',
        visible: true,
      },
    },
    children: [
      {
        path: '/Departamentos/GestionDepartamentos',
        name: 'GestionDepartamentos',
        meta: {
          idModulo: idModuloDepartamentos,
          sidebar: {
            icon: 'pi pi-sitemap',
            title: 'GestionDepartamentos',
            visible: true,
          },
        },
        component: () => import('pages/Configuracion/Departamentos.vue'),
      },
      {
        path: '/Departamentos/Mantenimiento',
        name: 'MantenimientoDepartamentos',
        meta: {
          idModulo: idModuloDepartamentos,
          sidebar: { visible: false },
        },
        component: () => import('pages/Mantenimiento.vue'),
      },
      {
        path: '/Departamentos/NoAutorizado',
        name: 'NoAutorizadoDepartamentos',
        meta: {
          idModulo: idModuloDepartamentos,
          sidebar: { visible: false },
        },
        component: () => import('pages/NoAutorizado.vue'),
      },
    ],
  },
]

export default rutasDepartamentos
