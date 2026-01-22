using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Servicios.Preterminados
{
    /// <summary>
    /// Clase que implementa la interfaz IInyeccionServicios para configurar las políticas de CORS (Cross-Origin Resource Sharing) en la aplicación.
    /// Esta clase define políticas de CORS que permiten o restringen el acceso a recursos desde diferentes orígenes.
    /// </summary>
    public class InyeccionCors : IInyeccionServicios
    {
        /// <summary>
        /// Método que instala y configura los servicios de CORS en la colección de servicios.
        /// Este método define dos políticas de CORS: una para uso general y otra específica para SignalR.
        /// </summary>
        /// <param name="servicios">Colección de servicios donde se configurarán las políticas de CORS.</param>
        /// <param name="configuracion">Instancia de IConfiguration que proporciona acceso a la configuración de la aplicación.</param>
        public void InstalarServicios(IServiceCollection servicios, IConfiguration configuracion)
        {
            // Configuración de políticas de CORS
            servicios.AddCors(options =>
            {
                // Política de CORS para uso general (SinSignalR)
                options.AddPolicy("SinSignalR",
                    builder => builder
                        .WithOrigins("*") // Permite solicitudes desde cualquier origen
                        .WithHeaders("*") // Permite cualquier cabecera
                        .WithMethods("*") // Permite cualquier método HTTP
                    );

                // Política de CORS específica para SignalR (ConSignalR)
                options.AddPolicy("ConSignalR",
                    builder => builder
                        .AllowAnyMethod() // Permite cualquier método HTTP
                        .AllowAnyHeader() // Permite cualquier cabecera
                        .AllowCredentials() // Permite credenciales (cookies, encabezados de autenticación, etc.)
                        .SetIsOriginAllowed((hosts) => true) // Permite cualquier origen
                    );
            });
        }
    }
}