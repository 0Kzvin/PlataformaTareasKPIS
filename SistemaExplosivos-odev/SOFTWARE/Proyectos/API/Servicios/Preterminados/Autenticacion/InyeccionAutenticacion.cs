using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

namespace API.Servicios.Preterminados.Autenticacion
{
    /// <summary>
    /// Clase que implementa la interfaz <see cref="IInyeccionServicios"/> para configurar y registrar los servicios de autenticación JWT en la aplicación.
    /// Esta clase se encarga de configurar la autenticación basada en tokens JWT.
    /// </summary>
    public class InyeccionAutenticacion : IInyeccionServicios
    {
        /// <summary>
        /// Método que instala y configura los servicios de autenticación JWT en la colección de servicios.
        /// Este método configura la autenticación JWT, incluyendo la validación de tokens y la configuración de esquemas de autenticación.
        /// </summary>
        /// <param name="servicios">Colección de servicios donde se configurarán los servicios de autenticación.</param>
        /// <param name="configuracion">Instancia de <see cref="IConfiguration"/> que proporciona acceso a la configuración de la aplicación.</param>
        public void InstalarServicios(IServiceCollection servicios, IConfiguration configuracion)
        {
            // Obtener y registrar la configuración de JWT desde el archivo de configuración
            var jwtConfiguraciones = new JwtConfiguraciones();
            configuracion.Bind("JWT", jwtConfiguraciones);
            servicios.AddSingleton(jwtConfiguraciones);

            // Configurar los parámetros de validación del token JWT
            var tokenValidacionParametros = new TokenValidationParameters
            {
                ValidateIssuer = true, // Validar el emisor del token
                ValidIssuer = jwtConfiguraciones.Issuer, // Emisor válido (debe coincidir con el valor en el token)
                ValidateAudience = true, // Validar la audiencia del token
                ValidAudience = jwtConfiguraciones.Audience, // Audiencia válida (debe coincidir con el valor en el token)
                RequireExpirationTime = false, // No requerir que el token tenga un tiempo de expiración
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfiguraciones.Key)), // Clave de firma para validar el token
                ValidateLifetime = true, // Validar el tiempo de vida del token
                ValidateIssuerSigningKey = true, // Validar la clave de firma del token
                //ClockSkew = TimeSpan.Zero, // Tolerancia de tiempo entre el servidor y el cliente (opcional)
            };

            // Registrar los parámetros de validación del token como un servicio singleton
            servicios.AddSingleton(tokenValidacionParametros);

            // Configurar la autenticación JWT como el esquema predeterminado
            servicios.AddAuthentication(auth =>
            {
                auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; // Esquema predeterminado
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Esquema para autenticación
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Esquema para desafíos de autenticación
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true; // Guardar el token en las propiedades de autenticación después de una autenticación exitosa
                options.TokenValidationParameters = tokenValidacionParametros; // Usar los parámetros de validación configurados

                // 👇 Permite el token en la cadena de consulta (query string) para SignalR
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // Verificar si la solicitud proviene de SignalR
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/EstacionHub"))
                        {
                            context.Token = accessToken; // Establecer el token para la validación
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}