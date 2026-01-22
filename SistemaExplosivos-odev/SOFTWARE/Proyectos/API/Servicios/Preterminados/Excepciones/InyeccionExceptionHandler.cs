using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Servicios.Preterminados.Excepciones
{
    public class InyeccionExceptionHandler : IInyeccionServicios
    {
        public void InstalarServicios(IServiceCollection servicios, IConfiguration configuracion)
        {
            servicios.AddExceptionHandler<GlobalExceptionHandler>();
            servicios.AddProblemDetails();
        }
    }
}
