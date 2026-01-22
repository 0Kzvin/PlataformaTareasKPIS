import { idModuloAdmin } from 'src/core/modulos'
import permisosAD from 'src/core/permisos/moduloAdministracion'

const rutasAdministracion = [
  {
    path: '/Administracion',
    name: 'Administracion',
    component: () => import('src/layouts/AplicacionLayout.vue'),
    children: [
      {
        path: '/Administracion/Auditoria',
        name: 'Auditoria',
        meta: {
          idModulo: idModuloAdmin,
          permisos: permisosAD.pLogs.Todos(),
          sidebar: {
            icon: 'pi pi-list',
            visible: false,
          },
        },
        component: () => import('src/modules/administracion/pages/auditoria/Auditoria.vue'),
      },
      {
        path: '/Administracion/Roles',
        name: 'Roles',
        meta: {
          idModulo: idModuloAdmin,
          permisos: permisosAD.pIdentidad.TodosRoles(),
          sidebar: {
            icon: 'pi pi-users',
            visible: true,
          },
        },
        component: () => import('src/modules/administracion/pages/roles/Roles.vue'),
      },
      {
        path: '/Administracion/Usuarios',
        name: 'Usuarios',
        meta: {
          idModulo: idModuloAdmin,
          permisos: permisosAD.pIdentidad.TodosUsuarios(),
          sidebar: {
            icon: 'pi pi-user',
            visible: true,
          },
        },
        component: () => import('src/modules/administracion/pages/usuarios/Usuarios.vue'),
      },
      {
        path: '/Administracion/CorreosAutomaticos',
        name: 'CorreosAutomaticos',
        meta: {
          idModulo: idModuloAdmin,
          permisos: permisosAD.pCorreosAutomaticosAD.Todos(),
          sidebar: {
            icon: 'pi pi-envelope',
            visible: false,
          },
        },
        component: () =>
          import('src/modules/administracion/pages/correosAutomaticos/CorreosAutomaticos.vue'),
      },
      {
        path: '/Administracion/Modulos',
        name: 'Modulos',
        meta: {
          idModulo: idModuloAdmin,
          permisos: permisosAD.pIdentidad.TodosUsuarios(),
          sidebar: {
            icon: 'pi pi-box',
            visible: false,
          },
        },
        component: () => import('src/modules/administracion/pages/modulos/Modulos.vue'),
      },
      {
        path: '/Administracion/Mantenimiento',
        name: 'MantenimientoAdmin',
        meta: {
          idModulo: idModuloAdmin,
          sidebar: { visible: false },
        },
        component: () => import('pages/Mantenimiento.vue'),
      },
      {
        path: '/Administracion/NoAutorizado',
        name: 'NoAutorizadoAdmin',
        meta: {
          idModulo: idModuloAdmin,
          sidebar: { visible: false },
        },
        component: () => import('pages/NoAutorizado.vue'),
      },
    ],
  },
]

export default rutasAdministracion
