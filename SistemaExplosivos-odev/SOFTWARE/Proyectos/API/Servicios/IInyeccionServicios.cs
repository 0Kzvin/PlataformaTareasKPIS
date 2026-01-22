using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Servicios
{
    public interface IInyeccionServicios
    {
        void InstalarServicios(IServiceCollection servicios, IConfiguration configuracion);
    }
}
