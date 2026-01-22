using API.Database.Administracion;
using API.Database.Administracion.Entidades.Identidad;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Servicios.Preterminados.Identidad
{
    /// <summary>
    /// Clase que implementa la interfaz IInyeccionServicios para configurar y registrar los servicios de identidad en la aplicación.
    /// Esta clase se encarga de configurar la autenticación y autorización basada en roles y usuarios.
    /// </summary>
    public class InyeccionIdentidad : IInyeccionServicios
    {
        /// <summary>
        /// Método que instala y configura los servicios de identidad en la colección de servicios.
        /// Este método configura las opciones de identidad, como las políticas de contraseñas, bloqueo de usuarios y proveedores de tokens.
        /// </summary>
        /// <param name="servicios">Colección de servicios donde se configurarán los servicios de identidad.</param>
        /// <param name="configuracion">Instancia de IConfiguration que proporciona acceso a la configuración de la aplicación.</param>
        public void InstalarServicios(IServiceCollection servicios, IConfiguration configuracion)
        {
            servicios.AddIdentity<Usuarios, Roles>(options =>
            {
                // Opciones de bloqueo de usuarios
                options.Lockout.AllowedForNewUsers = false;

                // Opciones de usuario
                options.User.RequireUniqueEmail = false;

                // Opciones de contraseña
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                // Otras opciones disponibles:
                // Opciones de Claims
                // Opciones de SignIn
                // Opciones de Stores
                // Opciones de Token
            })
            .AddRoles<Roles>() // Agrega soporte para roles
            .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Usuarios, Roles>>() // Agrega fábrica de claims personalizada
            .AddEntityFrameworkStores<ModuloAdministracionExplosivosContext>() // Configura el almacenamiento en la base de datos
            .AddSignInManager<SignInManager<Usuarios>>() // Agrega el administrador de inicio de sesión
            .AddRoleManager<RoleManager<Roles>>() // Agrega el administrador de roles
            .AddDefaultTokenProviders(); // Agrega proveedores de tokens predeterminados
        }
    }
}