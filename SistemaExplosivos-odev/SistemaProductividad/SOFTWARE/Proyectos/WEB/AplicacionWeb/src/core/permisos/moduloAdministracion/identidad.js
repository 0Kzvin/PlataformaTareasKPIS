export const PermisoCrearRol = "CrearRol";
export const PermisoEditarRol = "EditarRol";
export const PermisoAsignarRol = "AsignarRol";
export const PermisoBorrarRol = "BorrarRol";
export const PermisoCambiarEstadoRol = "CambiarEstadoRol";

export const PermisoRegistrarUsuario = "RegistrarUsuario";
export const PermisoCambiarPassword = "CambiarPassword";
export const PermisoCambiarCorreo = "CambiarEmail";
export const PermisoCambiarNombre = "CambiarNombre";
export const PermisoEditarUsuario = "EditarUsuario";
export const PermisoBorrarUsuario = "BorrarUsuario";
export const PermisoCambiarEstadoUsuario = "CambiarEstadoUsuario";

export const PermisoListarUsuarios = "ListarUsuarios";
export const PermisoListarPermisos = "ListarPermisos";
export const PermisoListarRoles = "ListarRoles";

export const Todos = () => {
    return [
        PermisoCrearRol,
        PermisoEditarRol,
        PermisoAsignarRol,
        PermisoBorrarRol,
        PermisoCambiarEstadoRol,
        PermisoRegistrarUsuario,
        PermisoCambiarPassword,
        PermisoCambiarCorreo,
        PermisoCambiarNombre,
        PermisoEditarUsuario,
        PermisoBorrarUsuario,
        PermisoCambiarEstadoUsuario,
        PermisoListarUsuarios,
        PermisoListarPermisos,
        PermisoListarRoles,
    ]
}

export const TodosUsuarios = () => {
    return [
        PermisoRegistrarUsuario,
        PermisoCambiarPassword,
        PermisoCambiarCorreo,
        PermisoCambiarNombre,
        PermisoEditarUsuario,
        PermisoBorrarUsuario,
        PermisoCambiarEstadoUsuario,
        PermisoListarUsuarios,
    ]
}

export const TodosRoles = () => {
    return [
        PermisoCrearRol,
        PermisoEditarRol,
        PermisoAsignarRol,
        PermisoBorrarRol,
        PermisoCambiarEstadoRol,
        PermisoListarRoles,
    ]
}
