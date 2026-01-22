import { idModuloAccesorios } from 'src/core/modulos'
import permisosAD from 'src/core/permisos/moduloAdministracion'

const rutasAccesorios = [
  {
    path: '/Accesorios',
    name: 'Accesorios',
    component: () => import('src/layouts/AplicacionLayout.vue'),
    children: [
      // Grupo: Analisis
      {
        path: '',
        name: 'AnalisisAccesorios',
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
            path: '/Accesorios/TableroAccesorios',
            name: 'TableroAccesorios',
            meta: {
              idModulo: idModuloAccesorios,
              permisos: permisosAD.pIdentidad.TodosUsuarios(),
              sidebar: {
                icon: 'pi pi-objects-column',
                visible: true,
              },
            },
            component: () =>
              import('src/modules/accesorios/analisis/tablero/TableroAccesorios.vue'),
          },
          {
            path: '/Accesorios/MonitoreoAccesorios',
            name: 'MonitoreoAccesorios',
            meta: {
              idModulo: idModuloAccesorios,
              permisos: permisosAD.pIdentidad.TodosUsuarios(),
              sidebar: {
                icon: 'pi pi-desktop',
                visible: true,
              },
            },
            component: () =>
              import('src/modules/accesorios/analisis/monitoreo/MonitoreoAccesorios.vue'),
          },
        ],
      },
      // Grupo: Almacen
      {
        path: '',
        name: 'AlmacenAccesorios',
        meta: {
          sidebar: {
            icon: 'pi pi-box',
            visible: true,
            active: true,
          },
        },
        component: () => import('src/layouts/EmptyRouterView.vue'),
        children: [
          {
            path: '/Accesorios/InventarioAccesorios',
            name: 'InventarioAccesorios',
            meta: {
              idModulo: idModuloAccesorios,
              permisos: permisosAD.pIdentidad.TodosUsuarios(),
              sidebar: {
                icon: 'pi pi-tag',
                visible: true,
              },
            },
            component: () =>
              import('src/modules/accesorios/almacen/inventario/InventarioAccesorios.vue'),
          },
          {
            path: '/Accesorios/MovimientosAccesorios',
            name: 'MovimientosAccesorios',
            meta: {
              idModulo: idModuloAccesorios,
              permisos: permisosAD.pIdentidad.TodosUsuarios(),
              sidebar: {
                icon: 'pi pi-arrow-right-arrow-left',
                visible: true,
              },
            },
            component: () =>
              import('src/modules/accesorios/almacen/movimientos/MovimientosAccesorios.vue'),
          },
        ],
      },
      // Grupo: Altas
      {
        path: '',
        name: 'AltasAccesoriosGrupo', // Nombre unico
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
            path: '/Accesorios/CategoriasAccesorios',
            name: 'CategoriasAccesorios',
            meta: {
              idModulo: idModuloAccesorios,
              permisos: permisosAD.pIdentidad.TodosUsuarios(),
              sidebar: {
                icon: 'pi pi-tag',
                visible: true,
              },
            },
            component: () =>
              import('src/modules/accesorios/altas/categorias/CategoriasAccesorios.vue'),
          },
          {
            path: '/Accesorios/AltasAccesorios',
            name: 'AltasAccesorios',
            meta: {
              idModulo: idModuloAccesorios,
              permisos: permisosAD.pIdentidad.TodosUsuarios(),
              sidebar: {
                icon: 'pi pi-tag',
                visible: true,
              },
            },
            component: () => import('src/modules/accesorios/altas/accesorios/Accesorios.vue'),
          },
          {
            path: '/Accesorios/ProveedoresAccesorios',
            name: 'ProveedoresAccesorios',
            meta: {
              idModulo: idModuloAccesorios,
              permisos: permisosAD.pIdentidad.TodosUsuarios(),
              sidebar: {
                icon: 'pi pi-check-square',
                visible: true,
              },
            },
            component: () =>
              import('src/modules/accesorios/altas/proveedores/ProveedoresAccesorios.vue'),
          },
        ],
      },
      // Grupo: Configuraciones
      {
        path: '',
        name: 'ConfiguracionesAccesorios',
        meta: {
          sidebar: {
            icon: 'pi pi-cog',
            visible: true,
            active: true,
          },
        },
        component: () => import('src/layouts/EmptyRouterView.vue'),
        children: [
          {
            path: '/Accesorios/LimitesSedenaAccesorios',
            name: 'LimitesSedenaAccesorios',
            meta: {
              idModulo: idModuloAccesorios,
              permisos: permisosAD.pIdentidad.TodosUsuarios(),
              sidebar: {
                icon: 'pi pi-clipboard',
                visible: true,
              },
            },
            component: () =>
              import('src/modules/accesorios/configuraciones/limitesSedena/LimitesSedenaAccesorios.vue'),
          },
        ],
      },
      {
        path: '/Accesorios/Mantenimiento',
        name: 'MantenimientoAccesorios',
        meta: {
          idModulo: idModuloAccesorios,
          sidebar: { visible: false },
        },
        component: () => import('pages/Mantenimiento.vue'),
      },
      {
        path: '/Accesorios/NoAutorizado',
        name: 'NoAutorizadoAccesorios',
        meta: {
          idModulo: idModuloAccesorios,
          sidebar: { visible: false },
        },
        component: () => import('pages/NoAutorizado.vue'),
      },
    ],
  },
]

export default rutasAccesorios
