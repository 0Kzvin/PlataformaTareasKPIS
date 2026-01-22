import { idModuloAdministracion } from 'src/core/modulos'

const rutasAdministracion = [
  {
    path: '/Administracion',
    name: 'Administracion',
    component: () => import('src/layouts/AplicacionLayout.vue'),
    meta: {
      sidebar: {
        title: 'Administracion',
        icon: 'pi pi-shield',
      },
    },
    children: [
      {
        path: '/Administracion/Usuarios',
        name: 'AdministracionUsuarios',
        meta: {
          idModulo: idModuloAdministracion,
          sidebar: {
            icon: 'pi pi-users',
            title: 'Usuarios',
            visible: true,
          },
        },
        component: () => import('pages/Administracion/Usuarios.vue'),
      },
    ],
  },
]

export default rutasAdministracion
