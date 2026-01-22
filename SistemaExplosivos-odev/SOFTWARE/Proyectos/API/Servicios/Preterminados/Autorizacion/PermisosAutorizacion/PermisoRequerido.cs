using Microsoft.AspNetCore.Authorization;

namespace API.Servicios.Preterminados.Autorizacion.PermisosAutorizacion
{
    /// <summary>
    /// Clase que representa un requisito de autorización basado en permisos.
    /// Esta clase implementa la interfaz <see cref="IAuthorizationRequirement"/> y se utiliza para definir políticas de autorización
    /// que requieren que el usuario tenga uno o más permisos específicos.
    /// </summary>
    public class PermisoRequerido : IAuthorizationRequirement
    {
        /// <summary>
        /// Constructor que inicializa una nueva instancia de la clase <see cref="PermisoRequerido"/>.
        /// </summary>
        /// <param name="permiso">Arreglo de cadenas que representa los permisos requeridos.</param>
        public PermisoRequerido(string[] permiso)
        {
            Permiso = permiso;
        }

        /// <summary>
        /// Obtiene los permisos requeridos para satisfacer este requisito de autorización.
        /// </summary>
        public string[] Permiso { get; }
    }
}