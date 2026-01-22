using API.Database.Administracion.Entidades.Identidad;
using API.Utilidades.Constantes;
using System.Collections.Generic;

namespace API.Servicios.Preterminados.Autorizacion.PermisosAutorizacion.Controladores.Almacenamiento
{
    public static class PermisosAlmacenamiento
    {
        // Secciones
        // Secciones
        public const string SECCION_CARGAS = "SeccionCargasDepositos";
        public const string SECCION_DEPOSITOS = "SeccionDepositos";
        public const string SECCION_EQUIPOS = "SeccionEquipos";
        public const string SECCION_ESTATUS = "SeccionEstatusDepositos";
        public const string SECCION_MOVIMIENTOS = "SeccionMovimientosSupersacos";
        public const string SECCION_ANALISIS = "SeccionAnalisis";
        public const string SECCION_DESCARGAS = "SeccionDescargasPipas";

        // Nombres de Permisos
        // Cargas
        public const string PERMISO_CREAR_CARGA = "CrearCargaDeposito";
        public const string PERMISO_EDITAR_CARGA = "EditarCargaDeposito";
        public const string PERMISO_ELIMINAR_CARGA = "EliminarCargaDeposito";
        public const string PERMISO_LISTAR_CARGAS = "ListarCargasDepositos";
        public const string PERMISO_DETALLES_CARGA = "DetallesCargaDeposito";

        // Depositos
        public const string PERMISO_CREAR_DEPOSITO = "CrearDeposito";
        public const string PERMISO_EDITAR_DEPOSITO = "EditarDeposito";
        public const string PERMISO_ELIMINAR_DEPOSITO = "EliminarDeposito";
        public const string PERMISO_LISTAR_DEPOSITOS = "ListarDepositos";
        public const string PERMISO_DETALLES_DEPOSITO = "DetallesDeposito";

        // Equipos
        public const string PERMISO_CREAR_EQUIPO = "CrearEquipoAlmacenamiento";
        public const string PERMISO_EDITAR_EQUIPO = "EditarEquipoAlmacenamiento";
        public const string PERMISO_ELIMINAR_EQUIPO = "EliminarEquipoAlmacenamiento";
        public const string PERMISO_LISTAR_EQUIPOS = "ListarEquiposAlmacenamiento";
        public const string PERMISO_DETALLES_EQUIPO = "DetallesEquipoAlmacenamiento";

        // Estatus
        public const string PERMISO_CREAR_ESTATUS = "CrearEstatusDeposito";
        public const string PERMISO_EDITAR_ESTATUS = "EditarEstatusDeposito";
        public const string PERMISO_ELIMINAR_ESTATUS = "EliminarEstatusDeposito";
        public const string PERMISO_LISTAR_ESTATUS = "ListarEstatusDepositos";
        public const string PERMISO_DETALLES_ESTATUS = "DetallesEstatusDeposito";

        // Movimientos
        public const string PERMISO_CREAR_MOVIMIENTO = "CrearMovimientoSupersaco";
        public const string PERMISO_EDITAR_MOVIMIENTO = "EditarMovimientoSupersaco";
        public const string PERMISO_ELIMINAR_MOVIMIENTO = "EliminarMovimientoSupersaco";
        public const string PERMISO_LISTAR_MOVIMIENTOS = "ListarMovimientosSupersacos";
        public const string PERMISO_DETALLES_MOVIMIENTO = "DetallesMovimientoSupersaco";

        // Analisis
        public const string PERMISO_VER_TABLERO = "VerTableroAlmacenamiento";
        public const string PERMISO_VER_MONITOREO = "VerMonitoreoAlmacenamiento";

        // Descargas
        public const string PERMISO_CREAR_DESCARGA = "CrearDescargaPipa";
        public const string PERMISO_EDITAR_DESCARGA = "EditarDescargaPipa";
        public const string PERMISO_ELIMINAR_DESCARGA = "EliminarDescargaPipa";
        public const string PERMISO_LISTAR_DESCARGAS = "ListarDescargasPipas";
        public const string PERMISO_DETALLES_DESCARGA = "DetallesDescargaPipa";

        // GUIDs (Generated manually for uniqueness)
        // Cargas
        public const string GUID_CREAR_CARGA = "2AFFA6B3-5687-4B07-BD8A-0B37031A008B";
        public const string GUID_EDITAR_CARGA = "EF86B7F5-FEF2-41ED-AD59-DE793C85BD77";
        public const string GUID_ELIMINAR_CARGA = "2CF93F00-C7AB-4E43-AB0E-C6FEB9CC8CEB";
        public const string GUID_LISTAR_CARGAS = "B84AC0D8-ECB6-40D2-ABBD-1F70A68607BF";
        public const string GUID_DETALLES_CARGA = "D4251E28-1246-4F69-8249-3523AAA85320";

        // Depositos
        public const string GUID_CREAR_DEPOSITO = "0735AFB5-B628-41A5-BF54-2FF5207E4EED";
        public const string GUID_EDITAR_DEPOSITO = "A141FF34-513B-478C-930A-2198293FEF65";
        public const string GUID_ELIMINAR_DEPOSITO = "14F8C99D-2616-4FAD-BA19-4DA75E7B2CA1";
        public const string GUID_LISTAR_DEPOSITOS = "C138E17C-CD84-4006-8C41-2FF02EB36A25";
        public const string GUID_DETALLES_DEPOSITO = "0930AF81-55B0-4520-A2B5-C703F2C7A8F0";

        // Equipos
        public const string GUID_CREAR_EQUIPO = "4E27ACF4-99D9-4F1F-A7D0-FA8C975BAFA0";
        public const string GUID_EDITAR_EQUIPO = "6927E09A-E6D4-4F07-8BDB-1A7A40D27F0A";
        public const string GUID_ELIMINAR_EQUIPO = "A879327E-1156-488F-ACB6-85E695F1C52E";
        public const string GUID_LISTAR_EQUIPOS = "78832BAF-0A75-4788-ABAE-328DC463C5F0";
        public const string GUID_DETALLES_EQUIPO = "0DB9ED42-272F-4DB0-B33F-C0D79ECD44DF";

        // Estatus
        public const string GUID_CREAR_ESTATUS = "608FCC77-0B12-454A-AE36-64115C377423";
        public const string GUID_EDITAR_ESTATUS = "698A591D-338F-4F03-BD3C-E2967B2A7E86";
        public const string GUID_ELIMINAR_ESTATUS = "74609979-A892-4F62-AAC4-AB7960D4C764";
        public const string GUID_LISTAR_ESTATUS = "105FD673-0079-47C4-ADA6-6462832C4ECF";
        public const string GUID_DETALLES_ESTATUS = "8296AD44-541D-405E-A095-4447280D0706";

        // Movimientos
        public const string GUID_CREAR_MOVIMIENTO = "F423F97B-1814-41A8-AB67-7B525B3A086A";
        public const string GUID_EDITAR_MOVIMIENTO = "638243A9-A885-411E-B1A5-8CEAE1E6DAD6";
        public const string GUID_ELIMINAR_MOVIMIENTO = "45DFFCDF-984A-4696-9A97-FAB581888F43";
        public const string GUID_LISTAR_MOVIMIENTOS = "1C76E364-A93D-4376-897B-F22A72832698";
        public const string GUID_DETALLES_MOVIMIENTO = "9E026B12-BF95-4814-BB80-1FACDEC16127";

        // Analisis
        public const string GUID_VER_TABLERO = "A1B2C3D4-E5F6-4789-8011-121314151617";
        public const string GUID_VER_MONITOREO = "B2C3D4E5-F6A7-4890-9122-232425262728";

        // Descargas
        public const string GUID_CREAR_DESCARGA = "C3D4E5F6-A7B8-4901-A233-343536373839";
        public const string GUID_EDITAR_DESCARGA = "D4E5F6A7-B8C9-4012-B344-454647484940";
        public const string GUID_ELIMINAR_DESCARGA = "E5F6A7B8-C9D0-4123-C455-565758595051";
        public const string GUID_LISTAR_DESCARGAS = "F6A7B8C9-D0E1-4234-D566-676869606162";
        public const string GUID_DETALLES_DESCARGA = "A7B8C9D0-E1F2-4345-E677-787970717273";


        // Builders
        private static readonly PermisoSistemaBuilder BuilderCargas = new PermisoSistemaBuilder(
            new GruposPermisos
            {
                Id = ConstantesGruposPermisos.ID_GRUPOPERMISOS_ALMACENAMIENTO_CARGAS,
                GrupoNombre = ConstantesGruposPermisos.NOMBRE_ALMACENAMIENTO_CARGAS,
                GrupoNombreNormalizado = ConstantesGruposPermisos.NOMBRE_NORMALIZADO_ALMACENAMIENTO_CARGAS,
                IdModulo = ConstantesModulos.ID_ALMACENAMIENTO
            });

        private static readonly PermisoSistemaBuilder BuilderDepositos = new PermisoSistemaBuilder(
            new GruposPermisos
            {
                Id = ConstantesGruposPermisos.ID_GRUPOPERMISOS_ALMACENAMIENTO_DEPOSITOS,
                GrupoNombre = ConstantesGruposPermisos.NOMBRE_ALMACENAMIENTO_DEPOSITOS,
                GrupoNombreNormalizado = ConstantesGruposPermisos.NOMBRE_NORMALIZADO_ALMACENAMIENTO_DEPOSITOS,
                IdModulo = ConstantesModulos.ID_ALMACENAMIENTO
            });

        private static readonly PermisoSistemaBuilder BuilderEquipos = new PermisoSistemaBuilder(
            new GruposPermisos
            {
                Id = ConstantesGruposPermisos.ID_GRUPOPERMISOS_ALMACENAMIENTO_EQUIPOS,
                GrupoNombre = ConstantesGruposPermisos.NOMBRE_ALMACENAMIENTO_EQUIPOS,
                GrupoNombreNormalizado = ConstantesGruposPermisos.NOMBRE_NORMALIZADO_ALMACENAMIENTO_EQUIPOS,
                IdModulo = ConstantesModulos.ID_ALMACENAMIENTO
            });

        private static readonly PermisoSistemaBuilder BuilderEstatus = new PermisoSistemaBuilder(
            new GruposPermisos
            {
                Id = ConstantesGruposPermisos.ID_GRUPOPERMISOS_ALMACENAMIENTO_ESTATUS,
                GrupoNombre = ConstantesGruposPermisos.NOMBRE_ALMACENAMIENTO_ESTATUS,
                GrupoNombreNormalizado = ConstantesGruposPermisos.NOMBRE_NORMALIZADO_ALMACENAMIENTO_ESTATUS,
                IdModulo = ConstantesModulos.ID_ALMACENAMIENTO
            });

        private static readonly PermisoSistemaBuilder BuilderMovimientos = new PermisoSistemaBuilder(
            new GruposPermisos
            {
                Id = ConstantesGruposPermisos.ID_GRUPOPERMISOS_ALMACENAMIENTO_MOVIMIENTOS,
                GrupoNombre = ConstantesGruposPermisos.NOMBRE_ALMACENAMIENTO_MOVIMIENTOS,
                GrupoNombreNormalizado = ConstantesGruposPermisos.NOMBRE_NORMALIZADO_ALMACENAMIENTO_MOVIMIENTOS,
                IdModulo = ConstantesModulos.ID_ALMACENAMIENTO
            });

        private static readonly PermisoSistemaBuilder BuilderAnalisis = new PermisoSistemaBuilder(
            new GruposPermisos
            {
                Id = ConstantesGruposPermisos.ID_GRUPOPERMISOS_ALMACENAMIENTO_ANALISIS,
                GrupoNombre = ConstantesGruposPermisos.NOMBRE_ALMACENAMIENTO_ANALISIS,
                GrupoNombreNormalizado = ConstantesGruposPermisos.NOMBRE_NORMALIZADO_ALMACENAMIENTO_ANALISIS,
                IdModulo = ConstantesModulos.ID_ALMACENAMIENTO
            });

        private static readonly PermisoSistemaBuilder BuilderDescargas = new PermisoSistemaBuilder(
            new GruposPermisos
            {
                Id = ConstantesGruposPermisos.ID_GRUPOPERMISOS_ALMACENAMIENTO_DESCARGAS,
                GrupoNombre = ConstantesGruposPermisos.NOMBRE_ALMACENAMIENTO_DESCARGAS,
                GrupoNombreNormalizado = ConstantesGruposPermisos.NOMBRE_NORMALIZADO_ALMACENAMIENTO_DESCARGAS,
                IdModulo = ConstantesModulos.ID_ALMACENAMIENTO
            });

        // Definicion de Permisos
        // Cargas
        public static readonly Permisos CrearCarga = BuilderCargas.CrearPermiso(PERMISO_CREAR_CARGA, "DescCrearCargaDeposito", GUID_CREAR_CARGA);
        public static readonly Permisos EditarCarga = BuilderCargas.CrearPermiso(PERMISO_EDITAR_CARGA, "DescEditarCargaDeposito", GUID_EDITAR_CARGA);
        public static readonly Permisos EliminarCarga = BuilderCargas.CrearPermiso(PERMISO_ELIMINAR_CARGA, "DescEliminarCargaDeposito", GUID_ELIMINAR_CARGA);
        public static readonly Permisos ListarCargas = BuilderCargas.CrearPermiso(PERMISO_LISTAR_CARGAS, "DescListarCargasDepositos", GUID_LISTAR_CARGAS);
        public static readonly Permisos DetallesCarga = BuilderCargas.CrearPermiso(PERMISO_DETALLES_CARGA, "DescDetallesCargaDeposito", GUID_DETALLES_CARGA);

        // Depositos
        public static readonly Permisos CrearDeposito = BuilderDepositos.CrearPermiso(PERMISO_CREAR_DEPOSITO, "DescCrearDeposito", GUID_CREAR_DEPOSITO);
        public static readonly Permisos EditarDeposito = BuilderDepositos.CrearPermiso(PERMISO_EDITAR_DEPOSITO, "DescEditarDeposito", GUID_EDITAR_DEPOSITO);
        public static readonly Permisos EliminarDeposito = BuilderDepositos.CrearPermiso(PERMISO_ELIMINAR_DEPOSITO, "DescEliminarDeposito", GUID_ELIMINAR_DEPOSITO);
        public static readonly Permisos ListarDepositos = BuilderDepositos.CrearPermiso(PERMISO_LISTAR_DEPOSITOS, "DescListarDepositos", GUID_LISTAR_DEPOSITOS);
        public static readonly Permisos DetallesDeposito = BuilderDepositos.CrearPermiso(PERMISO_DETALLES_DEPOSITO, "DescDetallesDeposito", GUID_DETALLES_DEPOSITO);

        // Equipos
        public static readonly Permisos CrearEquipo = BuilderEquipos.CrearPermiso(PERMISO_CREAR_EQUIPO, "DescCrearEquipoAlmacenamiento", GUID_CREAR_EQUIPO);
        public static readonly Permisos EditarEquipo = BuilderEquipos.CrearPermiso(PERMISO_EDITAR_EQUIPO, "DescEditarEquipoAlmacenamiento", GUID_EDITAR_EQUIPO);
        public static readonly Permisos EliminarEquipo = BuilderEquipos.CrearPermiso(PERMISO_ELIMINAR_EQUIPO, "DescEliminarEquipoAlmacenamiento", GUID_ELIMINAR_EQUIPO);
        public static readonly Permisos ListarEquipos = BuilderEquipos.CrearPermiso(PERMISO_LISTAR_EQUIPOS, "DescListarEquiposAlmacenamiento", GUID_LISTAR_EQUIPOS);
        public static readonly Permisos DetallesEquipo = BuilderEquipos.CrearPermiso(PERMISO_DETALLES_EQUIPO, "DescDetallesEquipoAlmacenamiento", GUID_DETALLES_EQUIPO);

        // Estatus
        public static readonly Permisos CrearEstatus = BuilderEstatus.CrearPermiso(PERMISO_CREAR_ESTATUS, "DescCrearEstatusDeposito", GUID_CREAR_ESTATUS);
        public static readonly Permisos EditarEstatus = BuilderEstatus.CrearPermiso(PERMISO_EDITAR_ESTATUS, "DescEditarEstatusDeposito", GUID_EDITAR_ESTATUS);
        public static readonly Permisos EliminarEstatus = BuilderEstatus.CrearPermiso(PERMISO_ELIMINAR_ESTATUS, "DescEliminarEstatusDeposito", GUID_ELIMINAR_ESTATUS);
        public static readonly Permisos ListarEstatus = BuilderEstatus.CrearPermiso(PERMISO_LISTAR_ESTATUS, "DescListarEstatusDepositos", GUID_LISTAR_ESTATUS);
        public static readonly Permisos DetallesEstatus = BuilderEstatus.CrearPermiso(PERMISO_DETALLES_ESTATUS, "DescDetallesEstatusDeposito", GUID_DETALLES_ESTATUS);

        // Movimientos
        public static readonly Permisos CrearMovimiento = BuilderMovimientos.CrearPermiso(PERMISO_CREAR_MOVIMIENTO, "DescCrearMovimientoSupersaco", GUID_CREAR_MOVIMIENTO);
        public static readonly Permisos EditarMovimiento = BuilderMovimientos.CrearPermiso(PERMISO_EDITAR_MOVIMIENTO, "DescEditarMovimientoSupersaco", GUID_EDITAR_MOVIMIENTO);
        public static readonly Permisos EliminarMovimiento = BuilderMovimientos.CrearPermiso(PERMISO_ELIMINAR_MOVIMIENTO, "DescEliminarMovimientoSupersaco", GUID_ELIMINAR_MOVIMIENTO);
        public static readonly Permisos ListarMovimientos = BuilderMovimientos.CrearPermiso(PERMISO_LISTAR_MOVIMIENTOS, "DescListarMovimientosSupersacos", GUID_LISTAR_MOVIMIENTOS);
        public static readonly Permisos DetallesMovimiento = BuilderMovimientos.CrearPermiso(PERMISO_DETALLES_MOVIMIENTO, "DescDetallesMovimientoSupersaco", GUID_DETALLES_MOVIMIENTO);

        // Analisis
        public static readonly Permisos VerTablero = BuilderAnalisis.CrearPermiso(PERMISO_VER_TABLERO, "DescVerTableroAlmacenamiento", GUID_VER_TABLERO);
        public static readonly Permisos VerMonitoreo = BuilderAnalisis.CrearPermiso(PERMISO_VER_MONITOREO, "DescVerMonitoreoAlmacenamiento", GUID_VER_MONITOREO);

        // Descargas
        public static readonly Permisos CrearDescarga = BuilderDescargas.CrearPermiso(PERMISO_CREAR_DESCARGA, "DescCrearDescargaPipa", GUID_CREAR_DESCARGA);
        public static readonly Permisos EditarDescarga = BuilderDescargas.CrearPermiso(PERMISO_EDITAR_DESCARGA, "DescEditarDescargaPipa", GUID_EDITAR_DESCARGA);
        public static readonly Permisos EliminarDescarga = BuilderDescargas.CrearPermiso(PERMISO_ELIMINAR_DESCARGA, "DescEliminarDescargaPipa", GUID_ELIMINAR_DESCARGA);
        public static readonly Permisos ListarDescargas = BuilderDescargas.CrearPermiso(PERMISO_LISTAR_DESCARGAS, "DescListarDescargasPipas", GUID_LISTAR_DESCARGAS);
        public static readonly Permisos DetallesDescarga = BuilderDescargas.CrearPermiso(PERMISO_DETALLES_DESCARGA, "DescDetallesDescargaPipa", GUID_DETALLES_DESCARGA);


        public static IReadOnlyList<Permisos> Todos() => new List<Permisos>
        {
            // Cargas
            CrearCarga, EditarCarga, EliminarCarga, ListarCargas, DetallesCarga,
            // Depositos
            CrearDeposito, EditarDeposito, EliminarDeposito, ListarDepositos, DetallesDeposito,
            // Equipos
            CrearEquipo, EditarEquipo, EliminarEquipo, ListarEquipos, DetallesEquipo,
            // Estatus
            CrearEstatus, EditarEstatus, EliminarEstatus, ListarEstatus, DetallesEstatus,
            // Movimientos
            // Movimientos
            CrearMovimiento, EditarMovimiento, EliminarMovimiento, ListarMovimientos, DetallesMovimiento,
            // Analisis
            VerTablero, VerMonitoreo,
            // Descargas
            CrearDescarga, EditarDescarga, EliminarDescarga, ListarDescargas, DetallesDescarga
        }.AsReadOnly();
    }
}
