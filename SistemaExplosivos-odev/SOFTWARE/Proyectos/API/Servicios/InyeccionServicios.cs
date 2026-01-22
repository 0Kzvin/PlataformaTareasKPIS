using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace API.Servicios
{
    public static class InyeccionServicios
    {
        public static void InstalarServiciosEnEnsamblados(this IServiceCollection servicios, IConfiguration configuracion)
        {
            //DEFAULT API
            //Controladores
            servicios.AddControllers()
                .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            //SignalR
            servicios.AddSignalR(options =>
            {
                options.MaximumReceiveMessageSize = 1024 * 1024 * 5; // 5 MB
            });

            // Registrar TimeProvider para usar el tiempo del sistema
            servicios.AddSingleton(_ => TimeProvider.System);

            // Registrar IHttpContextAccessor para acceder al HttpContext dentro de los servicios
            servicios.AddHttpContextAccessor();

            //PERSONALIZADOS PREDETERMINADOS
            var serviciosAImplementar = typeof(Program).Assembly.ExportedTypes.Where(x =>
            typeof(IInyeccionServicios).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance).Cast<IInyeccionServicios>().ToList();

            serviciosAImplementar.ForEach(inyeccion => inyeccion.InstalarServicios(servicios, configuracion));
        }
    }
}
