import { idModuloRecepcion } from 'src/core/modulos'
import permisosAD from 'src/core/permisos/moduloAdministracion'

const rutasRecepcion = [
  {
    path: '/Recepcion',
    name: 'Recepcion',
    component: () => import('src/layouts/AplicacionLayout.vue'),
    children: [
      // Grupo: Analisis
      {
        path: '',
        name: 'AnalisisRecepcion',
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
            path: '/Recepcion/TableroRecepcion',
            name: 'TableroRecepcion',
            meta: {
              idModulo: idModuloRecepcion,
              permisos: permisosAD.pIdentidad.TodosUsuarios(),
              sidebar: {
                icon: 'pi pi-objects-column',
                visible: true,
              },
            },
            component: () => import('src/modules/recepcion/analisis/tablero/TableroRecepcion.vue'),
          },
          {
            path: '/Recepcion/MonitoreoRecepcion',
            name: 'MonitoreoRecepcion',
            meta: {
              idModulo: idModuloRecepcion,
              permisos: permisosAD.pIdentidad.TodosUsuarios(),
              sidebar: {
                icon: 'pi pi-desktop',
                visible: true,
              },
            },
            component: () =>
              import('src/modules/recepcion/analisis/monitoreo/MonitoreoRecepcion.vue'),
          },
        ],
      },
      // Grupo: Historiales
      {
        path: '',
        name: 'HistorialesRecepciones',
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
            path: '/Recepcion/HistorialesRecepcion',
            name: 'HistorialesRecepcion',
            meta: {
              idModulo: idModuloRecepcion,
              permisos: permisosAD.pIdentidad.TodosUsuarios(),
              sidebar: {
                icon: 'pi pi-table',
                visible: true,
              },
            },
            component: () =>
              import('src/modules/recepcion/historiales/recepciones/HistorialRecepciones.vue'),
          },
        ],
      },
      // Grupo: Altas
      {
        path: '',
        name: 'AltasRecepcion',
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
            path: '/Recepcion/ProveedoresRecepcion',
            name: 'ProveedoresRecepcion',
            meta: {
              idModulo: idModuloRecepcion,
              permisos: permisosAD.pIdentidad.TodosUsuarios(),
              sidebar: {
                icon: 'pi pi-users',
                visible: true,
              },
            },
            component: () =>
              import('src/modules/recepcion/altas/proveedores/ProveedoresRecepcion.vue'),
          },
          {
            path: '/Recepcion/ConductoresRecepcion',
            name: 'ConductoresRecepcion',
            meta: {
              idModulo: idModuloRecepcion,
              permisos: permisosAD.pIdentidad.TodosUsuarios(),
              sidebar: {
                icon: 'pi pi-id-card',
                visible: true,
              },
            },
            component: () =>
              import('src/modules/recepcion/altas/conductores/ConductoresRecepcion.vue'),
          },
          {
            path: '/Recepcion/TransportistaRecepcion',
            name: 'TransportistaRecepcion',
            meta: {
              idModulo: idModuloRecepcion,
              permisos: permisosAD.pIdentidad.TodosUsuarios(),
              sidebar: {
                icon: 'pi pi-truck',
                visible: true,
              },
            },
            component: () =>
              import('src/modules/recepcion/altas/transportista/TransportistaRecepcion.vue'),
          },
          {
            path: '/Recepcion/OrigenesRecepcion',
            name: 'OrigenesRecepcion',
            meta: {
              idModulo: idModuloRecepcion,
              permisos: permisosAD.pIdentidad.TodosUsuarios(),
              sidebar: {
                icon: 'pi pi-map-marker',
                visible: true,
              },
            },
            component: () => import('src/modules/recepcion/altas/origenes/OrigenesRecepcion.vue'),
          },
          {
            path: '/Recepcion/EquiposRecepcion',
            name: 'EquiposRecepcion',
            meta: {
              idModulo: idModuloRecepcion,
              permisos: permisosAD.pIdentidad.TodosUsuarios(),
              sidebar: {
                icon: 'pi pi-cog',
                visible: true,
              },
            },
            component: () => import('src/modules/recepcion/altas/equipos/EquiposRecepcion.vue'),
          },
        ],
      },
      {
        path: '/Recepcion/Mantenimiento',
        name: 'MantenimientoRecepcion',
        meta: {
          idModulo: idModuloRecepcion,
          sidebar: { visible: false },
        },
        component: () => import('pages/Mantenimiento.vue'),
      },
      {
        path: '/Recepcion/NoAutorizado',
        name: 'NoAutorizadoRecepcion',
        meta: {
          idModulo: idModuloRecepcion,
          sidebar: { visible: false },
        },
        component: () => import('pages/NoAutorizado.vue'),
      },
    ],
  },
]

export default rutasRecepcion
