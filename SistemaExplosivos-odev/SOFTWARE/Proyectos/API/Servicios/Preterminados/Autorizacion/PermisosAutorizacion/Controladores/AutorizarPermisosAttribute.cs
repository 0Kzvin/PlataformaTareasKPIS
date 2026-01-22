using Microsoft.AspNetCore.Mvc;

namespace API.Servicios.Preterminados.Autorizacion.PermisosAutorizacion.Controladores
{
    /// <summary>
    /// Atributo personalizado que aplica el filtro de autorización <see cref="PermisoFiltroAttribute"/>.
    /// Este atributo se utiliza para restringir el acceso a un endpoint basado en permisos específicos.
    /// </summary>
    public class AutorizarPermisosAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Constructor que inicializa una nueva instancia de la clase <see cref="AutorizarPermisosAttribute"/>.
        /// </summary>
        /// <param name="permisos">Lista de permisos requeridos para acceder al endpoint.</param>
        public AutorizarPermisosAttribute(params string[] permisos)
            : base(typeof(PermisoFiltroAttribute))
        {
            // Configurar los argumentos para el filtro de autorización
            Arguments = [new PermisoRequerido(permisos)];

            // Establecer el orden de ejecución del filtro
            Order = int.MinValue;
        }
    }
}