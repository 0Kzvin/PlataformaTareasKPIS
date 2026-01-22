using API.Servicios.Preterminados.Autorizacion.PermisosAutorizacion;
using API.Servicios.Propios.Identidad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace API.Servicios.Preterminados.Autorizacion
{
    /// <summary>
    /// Clase que implementa la interfaz <see cref="IInyeccionServicios"/> para configurar y registrar los servicios de autorización en la aplicación.
    /// Esta clase se encarga de configurar las políticas de autorización basadas en permisos.
    /// </summary>
    public class InyeccionAutorizacion : IInyeccionServicios
    {
        /// <summary>
        /// Método que instala y configura los servicios de autorización en la colección de servicios.
        /// Este método configura las políticas de autorización basadas en permisos y registra el manejador de permisos.
        /// </summary>
        /// <param name="servicios">Colección de servicios donde se configurarán los servicios de autorización.</param>
        /// <param name="configuracion">Instancia de <see cref="IConfiguration"/> que proporciona acceso a la configuración de la aplicación.</param>
        public void InstalarServicios(IServiceCollection servicios, IConfiguration configuracion)
        {
            // Configurar políticas de autorización basadas en permisos
            servicios.AddAuthorization(options =>
            {
                // Agregar una política por cada permiso definido en PermisosDefault
                foreach (var nombrePermiso in PermisosDefault.Todos().Select(p => p.Nombre))
                {
                    options.AddPolicy(nombrePermiso, politica =>
                        politica.Requirements.Add(new PermisoRequerido([nombrePermiso])));
                }

                // Ejemplo de política personalizada (comentada)
                //options.AddPolicy("NombrePolicy", builder =>
                //{
                //    builder.RequireClaim("ClaimPersonalizado", "ValorPersonalizado");
                //});
            });

            // Registrar el manejador de permisos como un servicio singleton
            servicios.AddSingleton<IAuthorizationHandler, PermisosManejador>();
        }
    }
}