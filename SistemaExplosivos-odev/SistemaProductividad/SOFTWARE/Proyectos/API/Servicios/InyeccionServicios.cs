using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace API.Servicios
{
    public static class InyeccionServicios
    {
        public static void InstalarServiciosEnEnsamblados(this IServiceCollection servicios, IConfiguration configuracion)
        {
            servicios.AddControllers()
                .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            
            // Add JWT Auth
            servicios.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuracion["Jwt:Issuer"],
                        ValidAudience = configuracion["Jwt:Audience"],
                        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                            System.Text.Encoding.UTF8.GetBytes(configuracion["Jwt:Key"]))
                    };
                    
                    // SignalR Auth Hook
                    options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            
            servicios.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.SetIsOriginAllowed(x => _ = true)
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials();
                    });
            });

            servicios.AddEndpointsApiExplorer();
            servicios.AddSwaggerGen();
            
            // AutoMapper
            servicios.AddAutoMapper(typeof(Program));

            // Register IHttpContextAccessor
            servicios.AddHttpContextAccessor();
            servicios.AddSignalR();

            // Auto-discover and register services implementing IInyeccionServicios
            var serviciosAImplementar = typeof(Program).Assembly.ExportedTypes
                .Where(x => typeof(IInyeccionServicios).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance).Cast<IInyeccionServicios>().ToList();

            serviciosAImplementar.ForEach(inyeccion => inyeccion.InstalarServicios(servicios, configuracion));
        }
    }
}
