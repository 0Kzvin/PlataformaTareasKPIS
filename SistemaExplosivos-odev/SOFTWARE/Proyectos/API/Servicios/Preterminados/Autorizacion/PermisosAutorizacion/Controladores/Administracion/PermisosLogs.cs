using API.Database.Administracion.Entidades.Identidad;
using API.Utilidades.Constantes;
using System.Collections.Generic;

namespace API.Servicios.Preterminados.Autorizacion.PermisosAutorizacion.Controladores.Administracion
{
    public static class PermisosLogs
    {
        // Constantes para la sección
        public const string SECCION_NORMALIZADA = "SeccionLogs";

        // Constantes para nombres de permisos
        public const string PERMISO_LISTAR_LOGS = "ListarLogs";

        // Constantes para GUIDs
        public const string GUID_LISTAR_LOGS = "8ECE13A6-CD88-46A6-B413-1B7533E07268";

        // Descripción del permiso
        public const string DESCRIPCION_LISTAR_LOGS = "DescListarLogs";

        // Builder de permisos
        private static readonly PermisoSistemaBuilder Builder = new PermisoSistemaBuilder(
            new GruposPermisos
            {
                Id = ConstantesGruposPermisos.ID_GRUPOPERMISOS_LOGS,
                GrupoNombre = ConstantesGruposPermisos.NOMBRE_LOGS,
                GrupoNombreNormalizado = ConstantesGruposPermisos.NOMBRE_NORMALIZADO_LOGS
            });

        // Permiso
        public static readonly Permisos ListarLogs = Builder.CrearPermiso(
            PERMISO_LISTAR_LOGS,
            DESCRIPCION_LISTAR_LOGS,
            GUID_LISTAR_LOGS
        );

        // Método para obtener todos los permisos
        public static IReadOnlyList<Permisos> Todos() => new List<Permisos>
        {
            ListarLogs
        }.AsReadOnly();
    }
}