using API.Database.Administracion.Entidades.Identidad;
using API.Utilidades.Constantes;
using System.Collections.Generic;

namespace API.Servicios.Preterminados.Autorizacion.PermisosAutorizacion.Controladores.Administracion
{
    public static class PermisosModulos
    {
        // Constantes para la sección
        public const string SECCION_NORMALIZADA = "SeccionModulos";

        // Constantes para nombres de permisos
        public const string PERMISO_LISTAR_MODULOS = "ListarModulos";
        public const string PERMISO_CAMBIAR_ESTADO_MODULOS = "CambiarEstadoModulos";

        // Constantes para GUIDs
        public const string GUID_LISTAR_MODULOS = "A855E684-62DA-4476-8972-B52906C34086";
        public const string GUID_CAMBIAR_ESTADO_MODULOS = "D66F9A4C-08D7-4D64-943B-3F13779044A2";

        // Descripciones de permisos
        public const string DESCRIPCION_LISTAR_MODULOS = "DescListarModulos";
        public const string DESCRIPCION_CAMBIAR_ESTADO_MODULOS = "DescCambiarEstadoModulos";

        // Builder de permisos
        private static readonly PermisoSistemaBuilder Builder = new PermisoSistemaBuilder(
            new GruposPermisos
            {
                Id = ConstantesGruposPermisos.ID_GRUPOPERMISOS_MODULOS,
                GrupoNombre = ConstantesGruposPermisos.NOMBRE_MODULOS,
                GrupoNombreNormalizado = ConstantesGruposPermisos.NOMBRE_NORMALIZADO_MODULOS
            });

        // Permisos
        public static readonly Permisos ListarModulos = Builder.CrearPermiso(
            PERMISO_LISTAR_MODULOS,
            DESCRIPCION_LISTAR_MODULOS,
            GUID_LISTAR_MODULOS
        );

        public static readonly Permisos CambiarEstadoModulos = Builder.CrearPermiso(
            PERMISO_CAMBIAR_ESTADO_MODULOS,
            DESCRIPCION_CAMBIAR_ESTADO_MODULOS,
            GUID_CAMBIAR_ESTADO_MODULOS
        );

        // Método para obtener todos los permisos
        public static IReadOnlyList<Permisos> Todos() => new List<Permisos>
        {
            ListarModulos,
            CambiarEstadoModulos
        }.AsReadOnly();
    }
}