import { idModuloAlmacenamiento } from 'src/core/modulos'
import permisosAlmacenamiento from 'src/core/permisos/moduloAlmacenamiento'

const rutasAlmacenamiento = [
  {
    path: '/Almacenamiento',
    name: 'Almacenamiento',
    component: () => import('src/layouts/AplicacionLayout.vue'),
    children: [
      {
        path: '',
        name: 'AnalisisAlmacenamiento',
        meta: {
          sidebar: {
            icon: 'pi pi-chart-line',
            visible: true,
            active: true,
          },
        },
        component: () => import('src/layouts/EmptyRouterView.vue'),
        children: [
          {
            path: '/Almacenamiento/TableroAlmacenamiento',
            name: 'TableroAlmacenamiento',
            meta: {
              idModulo: idModuloAlmacenamiento,
              enMantenimiento: false,
              permisos: permisosAlmacenamiento.analisis.Todos(),
              sidebar: {
                icon: 'pi pi-objects-column',
                visible: true,
              },
            },
            component: () =>
              import('src/modules/almacenamiento/analisis/tablero/TableroAlmacenamiento.vue'),
          },
          {
            path: '/Almacenamiento/MonitoreoAlmacenamiento',
            name: 'MonitoreoAlmacenamiento',
            meta: {
              idModulo: idModuloAlmacenamiento,
              enMantenimiento: false,
              permisos: permisosAlmacenamiento.analisis.Todos(),
              sidebar: {
                icon: 'pi pi-desktop',
                visible: true,
              },
            },
            component: () =>
              import('src/modules/almacenamiento/analisis/monitoreo/MonitoreoAlmacenamiento.vue'),
          },
        ],
      },
      {
        path: '',
        name: 'HistorialesAlmacenamiento',
        meta: {
          sidebar: {
            icon: 'pi pi-book',
            visible: true,
            active: true,
          },
        },
        component: () => import('src/layouts/EmptyRouterView.vue'),
        children: [
          {
            path: '/Almacenamiento/CargasAlmacenamiento',
            name: 'CargasAlmacenamiento',
            meta: {
              idModulo: idModuloAlmacenamiento,
              enMantenimiento: false,
              permisos: permisosAlmacenamiento.cargas.Todos(),
              sidebar: {
                icon: 'pi pi-chevron-circle-up',
                visible: true,
              },
            },
            component: () =>
              import('src/modules/almacenamiento/historiales/cargas/CargasAlmacenamiento.vue'),
          },
          {
            path: '/Almacenamiento/DescargasAlmacenamiento',
            name: 'DescargasAlmacenamiento',
            meta: {
              idModulo: idModuloAlmacenamiento,
              enMantenimiento: false,
              permisos: permisosAlmacenamiento.descargas.Todos(),
              sidebar: {
                icon: 'pi pi-chevron-circle-down',
                visible: true,
              },
            },
            component: () =>
              import('src/modules/almacenamiento/historiales/descargas/DescargasAlmacenamiento.vue'),
          },
          {
            path: '/Almacenamiento/SuperSacosAlmacenamiento',
            name: 'SuperSacosAlmacenamiento',
            meta: {
              idModulo: idModuloAlmacenamiento,
              enMantenimiento: false,
              permisos: permisosAlmacenamiento.movimientos.Todos(),
              sidebar: {
                icon: 'pi pi-table',
                visible: true,
              },
            },
            component: () =>
              import('src/modules/almacenamiento/historiales/supersacos/SuperSacosAlmacenamiento.vue'),
          },
        ],
      },
      {
        path: '',
        name: 'AltasAlmacenamiento',
        meta: {
          sidebar: {
            icon: 'pi pi-plus-circle',
            visible: true,
            active: true,
          },
        },
        component: () => import('src/layouts/EmptyRouterView.vue'),
        children: [
          {
            path: '/Almacenamiento/EquiposAlmacenamiento',
            name: 'EquiposAlmacenamiento',
            meta: {
              idModulo: idModuloAlmacenamiento,
              enMantenimiento: false,
              permisos: permisosAlmacenamiento.equipos.Todos(),
              sidebar: {
                icon: 'pi pi-cog',
                visible: true,
              },
            },
            component: () =>
              import('src/modules/almacenamiento/altas/equipos/EquiposAlmacenamiento.vue'),
          },
          {
            path: '/Almacenamiento/TanquesAlmacenamiento',
            name: 'TanquesAlmacenamiento',
            meta: {
              idModulo: idModuloAlmacenamiento,
              enMantenimiento: false,
              permisos: permisosAlmacenamiento.tanques.Todos(),
              sidebar: {
                icon: 'pi pi-box',
                visible: true,
              },
            },
            component: () =>
              import('src/modules/almacenamiento/altas/tanques/TanquesAlmacenamiento.vue'),
          },
        ],
      },
      {
        path: '/Almacenamiento/Mantenimiento',
        name: 'MantenimientoAlmacenamiento',
        meta: {
          idModulo: idModuloAlmacenamiento,
          enMantenimiento: false,
          sidebar: { visible: false },
        },
        component: () => import('pages/Mantenimiento.vue'),
      },
      {
        path: '/Almacenamiento/NoAutorizado',
        name: 'NoAutorizadoAlmacenamiento',
        meta: {
          idModulo: idModuloAlmacenamiento,
          enMantenimiento: false,
          sidebar: { visible: false },
        },
        component: () => import('pages/NoAutorizado.vue'),
      },
    ],
  },
]

export default rutasAlmacenamiento
