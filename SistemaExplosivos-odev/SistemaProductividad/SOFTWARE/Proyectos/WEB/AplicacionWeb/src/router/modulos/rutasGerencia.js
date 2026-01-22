import { idModuloGerencia } from 'src/core/modulos'
import permisosAD from 'src/core/permisos/moduloAdministracion'

const rutasGerencia = [
  {
    path: '/Gerencia',
    name: 'Gerencia',
    redirect: { name: 'AlmacenamientoGerencia' },
    component: () => import('src/layouts/AplicacionLayout.vue'),
    children: [
      // Grupo: Analisis
      {
        path: '',
        name: 'AnalisisGerencia',
        meta: {
          sidebar: {
            icon: 'pi pi-chart-line',
            visible: true,
            active: true,
            enMantenimiento: true,
          },
        },
        component: () => import('src/layouts/EmptyRouterView.vue'),
        children: [
          {
            path: '/Gerencia/AlmacenamientoGerencia',
            name: 'AlmacenamientoGerencia',
            meta: {
              idModulo: idModuloGerencia,
              permisos: permisosAD.pIdentidad.TodosUsuarios(),
              enMantenimiento: true,
              sidebar: {
                icon: 'pi pi-box',
                visible: true,
              },
            },
            component: () =>
              import('src/modules/gerencia/analisis/almacenamiento/AlmacenamientoGerencia.vue'),
          },
          {
            path: '/Gerencia/RecepcionGerencia',
            name: 'RecepcionGerencia',
            meta: {
              idModulo: idModuloGerencia,
              permisos: permisosAD.pIdentidad.TodosUsuarios(),
              enMantenimiento: true,
              sidebar: {
                icon: 'pi pi-box',
                visible: true,
              },
            },
            component: () =>
              import('src/modules/gerencia/analisis/recepcion/RecepcionGerencia.vue'),
          },
          {
            path: '/Gerencia/AccesoriosGerencia',
            name: 'AccesoriosGerencia',
            meta: {
              idModulo: idModuloGerencia,
              permisos: permisosAD.pIdentidad.TodosUsuarios(),
              enMantenimiento: true,
              sidebar: {
                icon: 'pi pi-box',
                visible: true,
              },
            },
            component: () =>
              import('src/modules/gerencia/analisis/accesorios/AccesoriosGerencia.vue'),
          },
        ],
      },
      // Grupo: Gestión de Altas
      {
        path: '',
        name: 'GestionAltasGerencia',
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
            path: '/Gerencia/EstacionesGerencia',
            name: 'Estaciones',
            meta: {
              idModulo: idModuloGerencia,
              permisos: permisosAD.pIdentidad.TodosUsuarios(),
              sidebar: {
                icon: 'pi pi-server',
                visible: true,
              },
            },
            component: () => import('src/modules/gerencia/altas/estaciones/EstacionesGerencia.vue'),
          },
          {
            path: '/Gerencia/ProductosGerencia',
            name: 'Productos',
            meta: {
              idModulo: idModuloGerencia,
              permisos: permisosAD.pIdentidad.TodosUsuarios(),
              sidebar: {
                icon: 'pi pi-box',
                visible: true,
              },
            },
            component: () => import('src/modules/gerencia/altas/productos/ProductosGerencia.vue'),
          },
          {
            path: '/Gerencia/OperadoresGerencia',
            name: 'Operadores',
            meta: {
              idModulo: idModuloGerencia,
              permisos: permisosAD.pIdentidad.TodosUsuarios(),
              sidebar: {
                icon: 'pi pi-users',
                visible: true,
              },
            },
            component: () => import('src/modules/gerencia/altas/operadores/OperadoresGerencia.vue'),
          },
        ],
      },
      {
        path: '/Gerencia/Mantenimiento',
        name: 'MantenimientoGerencia',
        meta: {
          idModulo: idModuloGerencia,
          sidebar: { visible: false },
        },
        component: () => import('pages/Mantenimiento.vue'),
      },
      {
        path: '/Gerencia/NoAutorizado',
        name: 'NoAutorizadoGerencia',
        meta: {
          idModulo: idModuloGerencia,
          sidebar: { visible: false },
        },
        component: () => import('pages/NoAutorizado.vue'),
      },
    ],
  },
]

export default rutasGerencia
