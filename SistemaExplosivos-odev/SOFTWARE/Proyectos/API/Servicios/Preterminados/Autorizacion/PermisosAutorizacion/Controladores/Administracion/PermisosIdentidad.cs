using API.Database.Administracion.Entidades.Identidad;
using API.Utilidades.Constantes;
using System.Collections.Generic;

namespace API.Servicios.Preterminados.Autorizacion.PermisosAutorizacion.Controladores.Administracion
{
    public static class PermisosIdentidad
    {
        // Constantes para secciones
        public const string SECCION_ROLES = "SeccionRoles";
        public const string SECCION_PERMISOS = "SeccionPermisos";
        public const string SECCION_USUARIOS = "SeccionUsuarios";

        // Constantes para nombres de permisos
        public const string PERMISO_CREAR_ROL = "CrearRol";
        public const string PERMISO_ASIGNAR_ROL = "AsignarRol";
        public const string PERMISO_REGISTRAR_USUARIO = "RegistrarUsuario";
        public const string PERMISO_CAMBIAR_PASSWORD = "CambiarPassword";
        public const string PERMISO_CAMBIAR_CORREO = "CambiarEmail";
        public const string PERMISO_CAMBIAR_NOMBRE = "CambiarNombre";
        public const string PERMISO_LISTAR_USUARIOS = "ListarUsuarios";
        public const string PERMISO_LISTAR_PERMISOS = "ListarPermisos";
        public const string PERMISO_LISTAR_ROLES = "ListarRoles";
        public const string PERMISO_EDITAR_ROL = "EditarRol";
        public const string PERMISO_CAMBIAR_ESTADO_USUARIO = "CambiarEstadoUsuario";
        public const string PERMISO_CAMBIAR_ESTADO_ROL = "CambiarEstadoRol";
        public const string PERMISO_EDITAR_USUARIO = "EditarUsuario";
        public const string PERMISO_BORRAR_ROL = "BorrarRol";
        public const string PERMISO_BORRAR_USUARIO = "BorrarUsuario";

        // Constantes para GUIDs
        public const string GUID_CREAR_ROL = "C7A8387B-9A43-4F42-A463-FCD2B3ED82CB";
        public const string GUID_ASIGNAR_ROL = "72A1D08C-8913-4834-BBA2-18AEF94E72CF";
        public const string GUID_REGISTRAR_USUARIO = "9A4F19D2-9604-47CA-B656-3834F256976E";
        public const string GUID_CAMBIAR_PASSWORD = "2D5196AE-38BE-4E01-BB6F-93EB2E4F1BFD";
        public const string GUID_CAMBIAR_CORREO = "3BB8E04B-15EF-4F13-AB1C-8FB9A7754123";
        public const string GUID_CAMBIAR_NOMBRE = "8D9DDB73-578C-4295-BD80-7F495F170C3D";
        public const string GUID_LISTAR_USUARIOS = "34AEC870-7909-4671-BFCF-F5F24B03F5DB";
        public const string GUID_LISTAR_PERMISOS = "DC47CE4E-BFA0-4EC4-8D0F-9FD286E5FF21";
        public const string GUID_LISTAR_ROLES = "083BA68C-9160-4A20-94CB-ACCACD5872D7";
        public const string GUID_EDITAR_ROL = "0FFC404F-4346-4CF0-B503-F64EB439B0F2";
        public const string GUID_CAMBIAR_ESTADO_USUARIO = "99840914-D86B-4514-9AC7-CEC0BD018F47";
        public const string GUID_CAMBIAR_ESTADO_ROL = "B2D2B748-22E3-438D-BA5A-CD2FE10B9BC6";
        public const string GUID_EDITAR_USUARIO = "34C5566B-FFFA-4ABA-AA46-FA32B7C76BBE";
        public const string GUID_BORRAR_ROL = "A0D9856F-7922-440D-AB6F-B51483ED4A08";
        public const string GUID_BORRAR_USUARIO = "A3BCD7C8-BA8B-4BA4-A3FE-375DD164E922";

        // Builders específicos por sección
        private static readonly PermisoSistemaBuilder BuilderRoles = new PermisoSistemaBuilder(
            new GruposPermisos
            {
                Id = ConstantesGruposPermisos.ID_GRUPOPERMISOS_IDENTIDAD,
                GrupoNombre = ConstantesGruposPermisos.NOMBRE_IDENTIDAD,
                GrupoNombreNormalizado = ConstantesGruposPermisos.NOMBRE_NORMALIZADO_IDENTIDAD
            });

        private static readonly PermisoSistemaBuilder BuilderPermisos = new PermisoSistemaBuilder(
            new GruposPermisos
            {
                Id = ConstantesGruposPermisos.ID_GRUPOPERMISOS_IDENTIDAD,
                GrupoNombre = ConstantesGruposPermisos.NOMBRE_IDENTIDAD,
                GrupoNombreNormalizado = ConstantesGruposPermisos.NOMBRE_NORMALIZADO_IDENTIDAD
            });

        private static readonly PermisoSistemaBuilder BuilderUsuarios = new PermisoSistemaBuilder(
            new GruposPermisos
            {
                Id = ConstantesGruposPermisos.ID_GRUPOPERMISOS_IDENTIDAD,
                GrupoNombre = ConstantesGruposPermisos.NOMBRE_IDENTIDAD,
                GrupoNombreNormalizado = ConstantesGruposPermisos.NOMBRE_NORMALIZADO_IDENTIDAD
            });

        // Permisos
        public static readonly Permisos CrearRol = BuilderRoles.CrearPermiso(
            PERMISO_CREAR_ROL,
            "DescCrearRol",
            GUID_CREAR_ROL
        );

        public static readonly Permisos AsignarRol = BuilderRoles.CrearPermiso(
            PERMISO_ASIGNAR_ROL,
            "DescAsignarRol",
            GUID_ASIGNAR_ROL
        );

        public static readonly Permisos RegistrarUsuario = BuilderUsuarios.CrearPermiso(
            PERMISO_REGISTRAR_USUARIO,
            "DescRegistrarUsuario",
            GUID_REGISTRAR_USUARIO
        );

        public static readonly Permisos CambiarPassword = BuilderUsuarios.CrearPermiso(
            PERMISO_CAMBIAR_PASSWORD,
            "DescCambiarPassword",
            GUID_CAMBIAR_PASSWORD
        );

        public static readonly Permisos CambiarCorreo = BuilderUsuarios.CrearPermiso(
            PERMISO_CAMBIAR_CORREO,
            "DescCambiarEmail",
            GUID_CAMBIAR_CORREO
        );

        public static readonly Permisos CambiarNombre = BuilderUsuarios.CrearPermiso(
            PERMISO_CAMBIAR_NOMBRE,
            "DescCambiarNombre",
            GUID_CAMBIAR_NOMBRE
        );

        public static readonly Permisos ListarUsuarios = BuilderUsuarios.CrearPermiso(
            PERMISO_LISTAR_USUARIOS,
            "DescListarUsuarios",
            GUID_LISTAR_USUARIOS
        );

        public static readonly Permisos ListarPermisos = BuilderPermisos.CrearPermiso(
            PERMISO_LISTAR_PERMISOS,
            "DescListarPermisos",
            GUID_LISTAR_PERMISOS
        );

        public static readonly Permisos ListarRoles = BuilderRoles.CrearPermiso(
            PERMISO_LISTAR_ROLES,
            "DescListarRoles",
            GUID_LISTAR_ROLES
        );

        public static readonly Permisos EditarRol = BuilderRoles.CrearPermiso(
            PERMISO_EDITAR_ROL,
            "DescEditarRol",
            GUID_EDITAR_ROL
        );

        public static readonly Permisos CambiarEstadoUsuario = BuilderUsuarios.CrearPermiso(
            PERMISO_CAMBIAR_ESTADO_USUARIO,
            "DescCambiarEstadoUsuario",
            GUID_CAMBIAR_ESTADO_USUARIO
        );

        public static readonly Permisos CambiarEstadoRol = BuilderRoles.CrearPermiso(
            PERMISO_CAMBIAR_ESTADO_ROL,
            "DescCambiarEstadoRol",
            GUID_CAMBIAR_ESTADO_ROL
        );

        public static readonly Permisos EditarUsuario = BuilderUsuarios.CrearPermiso(
            PERMISO_EDITAR_USUARIO,
            "DescEditarUsuario",
            GUID_EDITAR_USUARIO
        );

        public static readonly Permisos BorrarRol = BuilderRoles.CrearPermiso(
            PERMISO_BORRAR_ROL,
            "DescBorrarRol",
            GUID_BORRAR_ROL
        );

        public static readonly Permisos BorrarUsuario = BuilderUsuarios.CrearPermiso(
            PERMISO_BORRAR_USUARIO,
            "DescBorrarUsuario",
            GUID_BORRAR_USUARIO
        );

        // Método para obtener todos los permisos
        public static IReadOnlyList<Permisos> Todos() => new List<Permisos>
        {
            CrearRol,
            AsignarRol,
            RegistrarUsuario,
            CambiarPassword,
            CambiarCorreo,
            CambiarNombre,
            ListarUsuarios,
            ListarPermisos,
            ListarRoles,
            EditarRol,
            CambiarEstadoUsuario,
            CambiarEstadoRol,
            EditarUsuario,
            BorrarRol,
            BorrarUsuario
        }.AsReadOnly();
    }
}