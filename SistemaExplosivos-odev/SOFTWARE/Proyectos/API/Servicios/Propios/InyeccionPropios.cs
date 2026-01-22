using API.Database.Accesorio;
using API.Database.Administracion;
using API.Database.Almacenamiento;
using API.Database.Gerencia;
using API.Database.Recepcion;
using API.Modelos.Configuraciones;
using API.Servicios.Archivos;
using API.Servicios.Propios.Identidad;
using API.Trabajos;
using API.Utilidades.Constantes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using ServiciosAPIG3;
using ServiciosAPIG3.ServiciosAPI.Correo.DTOs;
using ServiciosAPIG3.ServiciosAPI.Logger.Base;
using ServiciosAPIG3.ServiciosAPI.Quartz;
using ServiciosAPIG3.ServiciosAPI.Salud.DTOs;
using System.IO;

namespace API.Servicios.Propios
{
    public class InyeccionPropios : IInyeccionServicios
    {
        public void InstalarServicios(IServiceCollection servicios, IConfiguration configuracion)
        {
            #region DatosConfiguracion
            var datosCliente = new DatosClienteDTO();
            var clienteActual = configuracion.GetValue<string>("Cliente:Actual");
            configuracion.Bind($"Cliente:{clienteActual}", datosCliente);
            datosCliente.ClienteActual = clienteActual;
            ConstsGenerales.CambiarHoraCorte(datosCliente.HoraCorte);
            servicios.AddSingleton(datosCliente);

            var opcionesAlmacenador = new OpcionesAlmacenadorDTO();
            configuracion.Bind("OpcionesAlmacenador", opcionesAlmacenador);
            servicios.AddSingleton(opcionesAlmacenador);

            CrearCarpetasPorDefecto(opcionesAlmacenador);
            #endregion

            // Extraer la configuración del cliente actual
            var datosClientes = configuracion.GetSection("Cliente");
            var datoClienteActual = datosClientes.GetSection($"{datosClientes.GetValue<string>("Actual")}");

            // Configuración del contexto de base de datos para el módulo de administración
            servicios.AddDbContext<ModuloAdministracionExplosivosContext>(options =>
                options.UseSqlServer(datoClienteActual.GetConnectionString("ModuloIdentidad")));

            // Configuración del contexto de base de datos para el módulo de accesorios
            servicios.AddDbContext<ModuloAccesoriosExplosivosContext>(options =>
                options.UseSqlServer(datoClienteActual.GetConnectionString("ModuloAccesorios")));

            // Configuración del contexto de base de datos para el módulo de almacenamiento
            servicios.AddDbContext<ModuloAlmacenamientoExplosivosContext>(options =>
                options.UseSqlServer(datoClienteActual.GetConnectionString("ModuloAlmacenamiento")));

            // Configuración del contexto de base de datos para el módulo de recepción
            servicios.AddDbContext<ModuloRecepcionExplosivosContext>(options =>
                options.UseSqlServer(datoClienteActual.GetConnectionString("ModuloRecepcion")));

            // Configuración del contexto de base de datos para el módulo de gerencia
            servicios.AddDbContext<ModuloGerenciaExplosivosContext>(options =>
                options.UseSqlServer(datoClienteActual.GetConnectionString("ModuloGerencia")));

            ////MQTT
            //servicios.AddSingleton<ServicioServidorMQTT>();
            //servicios.AddHostedService<ServidorMQTTHostedService>();

            //Servicios Personalizados
            servicios.AddScoped<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
            servicios.AddScoped<IServicioAutenticacion, ServicioAutenticacion>();
            servicios.AddScoped<IServicioAutorizacion, ServicioAutorizacion>();

            //Servicios de ServiciosAPIG3
            servicios.AddServiciosAPIG3(
                new ServiciosAPIG3Options()
                {
                    DefaultConnectionString = datoClienteActual.GetConnectionString("ModuloIdentidad"),
                    AppDetails = configuracion.GetSection("AppDetails").Get<AppDetails>(),
                    ConnectionStrings = [
                        datoClienteActual.GetConnectionString("ModuloIdentidad"),
                        datoClienteActual.GetConnectionString("ModuloAlmacenamiento"),
                        datoClienteActual.GetConnectionString("ModuloAccesorios"),
                        datoClienteActual.GetConnectionString("ModuloRecepcion"),
                    ],
                    MailkitConfig = datoClienteActual.GetSection("MailKit").Get<MailkitDTO>(),
                    HealthCheckConfig = new HealthCheckConfig()
                    {
                        ApplicationHealthUrl = "http://0.0.0.0:8082/health"
                    },
                    LoggerConfig = new LoggerConfig(),
                    QuartzJobsConfigurator = quartzJobsConfigs =>
                    {
                        quartzJobsConfigs.ScheduleJobFactory<ReportarErroresLogsJob>("Reporte de errores y logs diarios", "0 0 0 * * ?");
                    }
                }
            );
        }

        public void CrearCarpetasPorDefecto(OpcionesAlmacenadorDTO opcionesAlmacenador)
        {
            if (!Directory.Exists(opcionesAlmacenador.RutaBase))
            {
                Directory.CreateDirectory(opcionesAlmacenador.RutaBase);
            }

            //TODO DIRECTORIO HARDCODEADO
            string directorioAPKs = Path.Combine(opcionesAlmacenador.RutaBase, "Versiones");
            if (!Directory.Exists(directorioAPKs))
            {
                Directory.CreateDirectory(directorioAPKs);
            }
        }
    }
}
