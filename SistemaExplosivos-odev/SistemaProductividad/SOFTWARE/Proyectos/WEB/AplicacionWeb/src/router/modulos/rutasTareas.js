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
        groupTitle: 'Tareas',
        visible: true,
      },
    },
    children: [
      {
        path: '/Tareas/Listado',
        name: 'ListadoTareas',
        meta: {
          idModulo: idModuloTareas,
          sidebar: {
            icon: 'pi pi-list',
            title: 'ListadoTareas',
            visible: true,
          },
        },
        component: () => import('pages/Tareas/ListadoTareas.vue'),
      },
      {
        path: '/Tareas/Mantenimiento',
        name: 'MantenimientoTareas',
        meta: {
          idModulo: idModuloTareas,
          sidebar: { visible: false },
        },
        component: () => import('pages/Mantenimiento.vue'),
      },
      {
        path: '/Tareas/NoAutorizado',
        name: 'NoAutorizadoTareas',
        meta: {
          idModulo: idModuloTareas,
          sidebar: { visible: false },
        },
        component: () => import('pages/NoAutorizado.vue'),
      },
    ],
  },
]

export default rutasTareas
