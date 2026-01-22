using API.Database.Administracion.Entidades.Identidad;
using API.Servicios.Preterminados.Autorizacion.PermisosAutorizacion.Controladores.Administracion;
using API.Servicios.Preterminados.Autorizacion.PermisosAutorizacion.Controladores.Almacenamiento;
using System.Collections.Generic;

namespace API.Servicios.Preterminados.Autorizacion
{
    /// <summary>
    /// Clase utilitaria que centraliza la definición de todos los permisos disponibles en la aplicación.
    /// Estos permisos están organizados por módulos y sub-módulos.
    /// </summary>
    public static class PermisosDefault
    {
        /// <summary>
        /// Obtiene una lista de todos los permisos disponibles en la aplicación.
        /// </summary>
        /// <returns>Lista de permisos.</returns>
        public static List<Permisos> Todos()
        {
            var permisosDefault = new List<Permisos>();

            // Agregar permisos de todos los módulos
            permisosDefault.AddRange(PermisosAdministracion());
            permisosDefault.AddRange(PermisosDeAlmacenamiento());

            return permisosDefault;
        }

        /// <summary>
        /// Obtiene los permisos relacionados con el módulo de Administración.
        /// </summary>
        /// <returns>Lista de permisos de Administración.</returns>
        private static List<Permisos> PermisosAdministracion()
        {
            var permisosDefault = new List<Permisos>();
            permisosDefault.AddRange(PermisosIdentidad.Todos());
            permisosDefault.AddRange(PermisosLogs.Todos());
            permisosDefault.AddRange(PermisosCorreosAutomaticosAD.Todos());
            permisosDefault.AddRange(PermisosModulos.Todos());
            return permisosDefault;
        }

        /// <summary>
        /// Obtiene los permisos relacionados con el módulo de Almacenamiento.
        /// </summary>
        /// <returns>Lista de permisos de Almacenamiento.</returns>
        private static List<Permisos> PermisosDeAlmacenamiento()
        {
            var permisosDefault = new List<Permisos>();
            permisosDefault.AddRange(PermisosAlmacenamiento.Todos());
            return permisosDefault;
        }
    }
}