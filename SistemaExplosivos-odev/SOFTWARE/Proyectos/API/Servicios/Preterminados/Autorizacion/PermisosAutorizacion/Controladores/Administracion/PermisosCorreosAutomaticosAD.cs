using API.Database.Administracion.Entidades.Identidad;
using API.Utilidades.Constantes;
using System.Collections.Generic;

namespace API.Servicios.Preterminados.Autorizacion.PermisosAutorizacion.Controladores.Administracion
{
    public static class PermisosCorreosAutomaticosAD
    {
        // Constantes para la sección
        public const string SECCION_NORMALIZADA = "SeccionCorreosAutomaticos";

        // Constantes para los nombres de los permisos
        public const string PERMISO_LISTAR_CORREOS_AUTOMATICOS_AD = "ListarCorreosAutomaticosAD";
        public const string PERMISO_EDITAR_CORREOS_AUTOMATICOS_AD = "EditarCorreosAutomaticosAD";
        public const string PERMISO_CAMBIAR_ESTADO_CORREOS_AUTOMATICOS_AD = "CambiarEstadoCorreosAutomaticosAD";

        // Constantes para los GUIDs de los permisos
        public const string GUID_LISTAR = "ED3599B4-C3AF-4078-B11B-CF6419E83894";
        public const string GUID_EDITAR = "6E560314-0CB2-40A8-B5C6-00B4FC435BE6";
        public const string GUID_CAMBIAR_ESTADO = "230E1800-E9E6-401A-A37A-27866333CE8F";

        // Descripciones de los permisos
        public const string DESCRIPCION_LISTAR = "DescListarCorreosAutomaticosAD";
        public const string DESCRIPCION_EDITAR = "DescEditarCorreosAutomaticosAD";
        public const string DESCRIPCION_CAMBIAR_ESTADO = "DescCambiarEstadoCorreosAutomaticosAD";

        // Builder de permisos (ahora usando las constantes de grupo)
        private static readonly PermisoSistemaBuilder Builder = new PermisoSistemaBuilder(
            new GruposPermisos
            {
                Id = ConstantesGruposPermisos.ID_GRUPOPERMISOS_CORREOSAUTOMATICOS,
                GrupoNombre = ConstantesGruposPermisos.NOMBRE_CORREOS_AUTOMATICOS,
                GrupoNombreNormalizado = ConstantesGruposPermisos.NOMBRE_NORMALIZADO_CORREOS_AUTOMATICOS
            });

        // Permisos como propiedades estáticas de solo lectura
        public static readonly Permisos ListarCorreosAutomaticos = Builder.CrearPermiso(
            PERMISO_LISTAR_CORREOS_AUTOMATICOS_AD,
            DESCRIPCION_LISTAR,
            GUID_LISTAR
        );

        public static readonly Permisos EditarCorreosAutomaticos = Builder.CrearPermiso(
            PERMISO_EDITAR_CORREOS_AUTOMATICOS_AD,
            DESCRIPCION_EDITAR,
            GUID_EDITAR
        );

        public static readonly Permisos CambiarEstadoCorreosAutomaticos = Builder.CrearPermiso(
            PERMISO_CAMBIAR_ESTADO_CORREOS_AUTOMATICOS_AD,
            DESCRIPCION_CAMBIAR_ESTADO,
            GUID_CAMBIAR_ESTADO
        );

        // Método para obtener todos los permisos como lista de solo lectura
        public static IReadOnlyList<Permisos> Todos() => new List<Permisos>
        {
            ListarCorreosAutomaticos,
            EditarCorreosAutomaticos,
            CambiarEstadoCorreosAutomaticos
        }.AsReadOnly();
    }
}