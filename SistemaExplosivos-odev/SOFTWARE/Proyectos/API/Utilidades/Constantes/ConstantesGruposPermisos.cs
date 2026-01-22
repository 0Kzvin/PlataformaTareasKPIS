namespace API.Utilidades.Constantes
{
    using API.Database.Administracion.Entidades.Identidad;
    using System.Collections.Generic;

    /// <summary>
    /// Constantes para grupos de permisos
    /// </summary>
    public static class ConstantesGruposPermisos
    {

        //ADMINISTRACIÓN
        // Constantes para nombres de grupos de identidad
        public const int ID_GRUPOPERMISOS_IDENTIDAD = 1;
        public const string NOMBRE_IDENTIDAD = "GrupoIdentidad";
        public const string NOMBRE_NORMALIZADO_IDENTIDAD = "GrupoIdentidadNorm";

        // Constantes para nombres de grupos de logs
        public const int ID_GRUPOPERMISOS_LOGS = 2;
        public const string NOMBRE_LOGS = "GrupoLogsAdmin";
        public const string NOMBRE_NORMALIZADO_LOGS = "GrupoLogsAdminNorm";

        // Constantes para nombres de grupos de correos automáticos
        public const int ID_GRUPOPERMISOS_CORREOSAUTOMATICOS = 3;
        public const string NOMBRE_CORREOS_AUTOMATICOS = "GrupoCorreosAutomaticosAdmin";
        public const string NOMBRE_NORMALIZADO_CORREOS_AUTOMATICOS = "GrupoCorreosAutomaticosAdminNorm";

        // Constantes para nombres de grupos de modulos
        public const int ID_GRUPOPERMISOS_MODULOS = 4;
        public const string NOMBRE_MODULOS = "GrupoModulosAdmin";
        public const string NOMBRE_NORMALIZADO_MODULOS = "GrupoModulosAdminNorm";      

        //ALMACENAMIENTO
        public const int ID_GRUPOPERMISOS_ALMACENAMIENTO_CARGAS = 10;
        public const string NOMBRE_ALMACENAMIENTO_CARGAS = "GrupoCargasAlmacenamiento";
        public const string NOMBRE_NORMALIZADO_ALMACENAMIENTO_CARGAS = "GrupoCargasAlmacenamientoNorm";

        public const int ID_GRUPOPERMISOS_ALMACENAMIENTO_DEPOSITOS = 11;
        public const string NOMBRE_ALMACENAMIENTO_DEPOSITOS = "GrupoTanquesAlmacenamiento";
        public const string NOMBRE_NORMALIZADO_ALMACENAMIENTO_DEPOSITOS = "GrupoTanquesAlmacenamientoNorm";

        public const int ID_GRUPOPERMISOS_ALMACENAMIENTO_EQUIPOS = 12;
        public const string NOMBRE_ALMACENAMIENTO_EQUIPOS = "GrupoEquiposAlmacenamiento";
        public const string NOMBRE_NORMALIZADO_ALMACENAMIENTO_EQUIPOS = "GrupoEquiposAlmacenamientoNorm";

        public const int ID_GRUPOPERMISOS_ALMACENAMIENTO_ESTATUS = 13;
        public const string NOMBRE_ALMACENAMIENTO_ESTATUS = "GrupoEstatusAlmacenamiento";
        public const string NOMBRE_NORMALIZADO_ALMACENAMIENTO_ESTATUS = "GrupoEstatusAlmacenamientoNorm";

        public const int ID_GRUPOPERMISOS_ALMACENAMIENTO_MOVIMIENTOS = 14;
        public const string NOMBRE_ALMACENAMIENTO_MOVIMIENTOS = "GrupoMovimientosAlmacenamiento";
        public const string NOMBRE_NORMALIZADO_ALMACENAMIENTO_MOVIMIENTOS = "GrupoMovimientosAlmacenamientoNorm";

        public const int ID_GRUPOPERMISOS_ALMACENAMIENTO_ANALISIS = 15;
        public const string NOMBRE_ALMACENAMIENTO_ANALISIS = "GrupoAnalisisAlmacenamiento";
        public const string NOMBRE_NORMALIZADO_ALMACENAMIENTO_ANALISIS = "GrupoAnalisisAlmacenamientoNorm";

        public const int ID_GRUPOPERMISOS_ALMACENAMIENTO_DESCARGAS = 16;
        public const string NOMBRE_ALMACENAMIENTO_DESCARGAS = "GrupoDescargasAlmacenamiento";
        public const string NOMBRE_NORMALIZADO_ALMACENAMIENTO_DESCARGAS = "GrupoDescargasAlmacenamientoNorm";

        // Lista de administración como constante
        public static readonly IReadOnlyList<GruposPermisos> GRUPOPERMISOS_ADMINISTRACION = new List<GruposPermisos>
        {
            new GruposPermisos
            {
                Id = ID_GRUPOPERMISOS_IDENTIDAD,
                GrupoNombre = NOMBRE_IDENTIDAD,
                GrupoNombreNormalizado = NOMBRE_NORMALIZADO_IDENTIDAD,
                IdModulo = ConstantesModulos.ID_ADMINISTRACION,
            },
            new GruposPermisos
            {
                Id = ID_GRUPOPERMISOS_LOGS,
                GrupoNombre = NOMBRE_LOGS,
                GrupoNombreNormalizado = NOMBRE_NORMALIZADO_LOGS,
                IdModulo = ConstantesModulos.ID_ADMINISTRACION,
            },
            new GruposPermisos
            {
                Id = ID_GRUPOPERMISOS_CORREOSAUTOMATICOS,
                GrupoNombre = NOMBRE_CORREOS_AUTOMATICOS,
                GrupoNombreNormalizado = NOMBRE_NORMALIZADO_CORREOS_AUTOMATICOS,
                IdModulo = ConstantesModulos.ID_ADMINISTRACION,
            },
            new GruposPermisos
            {
                Id = ID_GRUPOPERMISOS_MODULOS,
                GrupoNombre = NOMBRE_MODULOS,
                GrupoNombreNormalizado = NOMBRE_NORMALIZADO_MODULOS,
                IdModulo = ConstantesModulos.ID_ADMINISTRACION,
            }
        }.AsReadOnly();

        public static readonly IReadOnlyList<GruposPermisos> GRUPOPERMISOS_ALMACENAMIENTO = new List<GruposPermisos>
        {
            new GruposPermisos
            {
                Id = ID_GRUPOPERMISOS_ALMACENAMIENTO_CARGAS,
                GrupoNombre = NOMBRE_ALMACENAMIENTO_CARGAS,
                GrupoNombreNormalizado = NOMBRE_NORMALIZADO_ALMACENAMIENTO_CARGAS,
                IdModulo = ConstantesModulos.ID_ALMACENAMIENTO,
            },
            new GruposPermisos
            {
                Id = ID_GRUPOPERMISOS_ALMACENAMIENTO_DEPOSITOS,
                GrupoNombre = NOMBRE_ALMACENAMIENTO_DEPOSITOS,
                GrupoNombreNormalizado = NOMBRE_NORMALIZADO_ALMACENAMIENTO_DEPOSITOS,
                IdModulo = ConstantesModulos.ID_ALMACENAMIENTO,
            },
            new GruposPermisos
            {
                Id = ID_GRUPOPERMISOS_ALMACENAMIENTO_EQUIPOS,
                GrupoNombre = NOMBRE_ALMACENAMIENTO_EQUIPOS,
                GrupoNombreNormalizado = NOMBRE_NORMALIZADO_ALMACENAMIENTO_EQUIPOS,
                IdModulo = ConstantesModulos.ID_ALMACENAMIENTO,
            },
            new GruposPermisos
            {
                Id = ID_GRUPOPERMISOS_ALMACENAMIENTO_ESTATUS,
                GrupoNombre = NOMBRE_ALMACENAMIENTO_ESTATUS,
                GrupoNombreNormalizado = NOMBRE_NORMALIZADO_ALMACENAMIENTO_ESTATUS,
                IdModulo = ConstantesModulos.ID_ALMACENAMIENTO,
            },
            new GruposPermisos
            {
                Id = ID_GRUPOPERMISOS_ALMACENAMIENTO_MOVIMIENTOS,
                GrupoNombre = NOMBRE_ALMACENAMIENTO_MOVIMIENTOS,
                GrupoNombreNormalizado = NOMBRE_NORMALIZADO_ALMACENAMIENTO_MOVIMIENTOS,
                IdModulo = ConstantesModulos.ID_ALMACENAMIENTO,
            },
            new GruposPermisos
            {
                Id = ID_GRUPOPERMISOS_ALMACENAMIENTO_ANALISIS,
                GrupoNombre = NOMBRE_ALMACENAMIENTO_ANALISIS,
                GrupoNombreNormalizado = NOMBRE_NORMALIZADO_ALMACENAMIENTO_ANALISIS,
                IdModulo = ConstantesModulos.ID_ALMACENAMIENTO,
            },
            new GruposPermisos
            {
                Id = ID_GRUPOPERMISOS_ALMACENAMIENTO_DESCARGAS,
                GrupoNombre = NOMBRE_ALMACENAMIENTO_DESCARGAS,
                GrupoNombreNormalizado = NOMBRE_NORMALIZADO_ALMACENAMIENTO_DESCARGAS,
                IdModulo = ConstantesModulos.ID_ALMACENAMIENTO,
            }
        }.AsReadOnly();

        public static IReadOnlyList<GruposPermisos> ObtenerGruposPermisosPredefinidos()
        {
            var grupoPermisosLista = new List<GruposPermisos>();
            grupoPermisosLista.AddRange(GRUPOPERMISOS_ADMINISTRACION);
            grupoPermisosLista.AddRange(GRUPOPERMISOS_ALMACENAMIENTO);
            return grupoPermisosLista.AsReadOnly();
        }
    }
}