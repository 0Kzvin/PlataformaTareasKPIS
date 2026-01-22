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
    ],
  },
]

export default rutasDepartamentos
