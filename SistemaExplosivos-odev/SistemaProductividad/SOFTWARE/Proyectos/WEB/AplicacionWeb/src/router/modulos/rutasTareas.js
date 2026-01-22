import { idModuloTareas } from 'src/core/modulos'

const rutasTareas = [
  {
    path: '/Tareas',
    name: 'Tareas',
    component: () => import('src/layouts/AplicacionLayout.vue'),
    meta: {
      sidebar: {
        title: 'Tareas',
        icon: 'pi pi-check-square',
      },
    },
    children: [
      {
        path: '/Tareas/MisTareas',
        name: 'MisTareas',
        meta: {
          idModulo: idModuloTareas,
          sidebar: {
            icon: 'pi pi-check-square',
            title: 'MisTareas',
            visible: true,
          },
        },
        component: () => import('pages/Tareas/ListadoTareas.vue'),
      },
    ],
  },
]

export default rutasTareas
