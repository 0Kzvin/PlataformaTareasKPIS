using API.Database.Administracion.Entidades.Identidad;
using API.Database.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Servicios.Infrastructure
{
    public class InyeccionBaseDatos : IInyeccionServicios
    {
        public void InstalarServicios(IServiceCollection servicios, IConfiguration configuracion)
        {
            servicios.AddDbContext<SistemaProductividadContext>(options =>
                options.UseSqlServer(configuracion.GetConnectionString("DefaultConnection")));

            servicios.AddIdentity<Usuarios, Roles>()
                .AddEntityFrameworkStores<SistemaProductividadContext>()
                .AddDefaultTokenProviders();
        }
    }
}
