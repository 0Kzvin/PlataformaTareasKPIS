using API.Database.Accesorio;
using API.Database.Administracion;
using API.Database.Administracion.Entidades.Identidad;
using API.Database.Almacenamiento;
using API.Database.Gerencia;
using API.Database.Recepcion;
using API.Modelos.Configuraciones;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    /// <summary>
    /// Clase estática que contiene métodos para configurar y ejecutar tareas relacionadas con el servidor.
    /// Esta clase es responsable de ejecutar migraciones automáticas y configuraciones iniciales del servidor.
    /// </summary>
    public static class ServerConfig
    {
        /// <summary>
        /// Configura y ejecuta tareas iniciales del servidor, como la creación de directorios y la ejecución de migraciones automáticas.
        /// </summary>
        /// <param name="host">Instancia de IHost que representa el host de la aplicación.</param>
        /// <returns>Una tarea asincrónica que representa la ejecución de las configuraciones del servidor.</returns>
        public static async Task ConfigurarServer(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    CrearDirectorioAlmacenadorBase(scope);

                    // Ejecutar todas las migraciones automáticas en paralelo
                    await Task.WhenAll(
                        EjecutarMigracionesAdministracion(scope),
                        EjecutarMigracionesAlmacenamiento(scope),
                        EjecutarMigracionesAccesorios(scope),
                        EjecutarMigracionesRecepciones(scope),
                        EjecutarMigracionesGerencias(scope)
                    );
                }
                catch (Exception e)
                {
                    // Capturar y mostrar cualquier excepción que ocurra durante la configuración del servidor
                    var xd = e.Message;
                    Console.WriteLine(Environment.NewLine + Environment.NewLine + Environment.NewLine + "!ERROR DETECTADO>>>! " + xd + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                }
            }
        }

        private static void CrearDirectorioAlmacenadorBase(IServiceScope scope)
        {
            // Crear directorio si no existe
            var opcionesAlmacenador = scope.ServiceProvider.GetRequiredService<OpcionesAlmacenadorDTO>();

            if (!Directory.Exists(opcionesAlmacenador.RutaBase))
            {
                Directory.CreateDirectory(opcionesAlmacenador.RutaBase);
            }
        }

        /// <summary>
        /// Ejecuta las migraciones automáticas para el módulo de Administración.
        /// Además, inserta datos por defecto en la base de datos.
        /// </summary>
        /// <param name="scope">Instancia de IServiceScope que proporciona acceso a los servicios.</param>
        /// <returns>Una tarea asincrónica que representa la ejecución de las migraciones.</returns>
        private static async Task EjecutarMigracionesAdministracion(IServiceScope scope)
        {
            using var applicationDbContext = scope.ServiceProvider.GetRequiredService<ModuloAdministracionExplosivosContext>();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Usuarios>>();
            using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Roles>>();

            // Verificar si hay migraciones pendientes
            var pendingMigrations = await applicationDbContext.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                await applicationDbContext.Database.MigrateAsync();
            }

            await DatosPorDefectoAdministracion.InsertarDatos(applicationDbContext, userManager, roleManager);
        }

        /// <summary>
        /// Ejecuta las migraciones automáticas para el módulo de Gerencia.
        /// </summary>
        /// <param name="scope">Instancia de IServiceScope que proporciona acceso a los servicios.</param>
        /// <returns>Una tarea asincrónica que representa la ejecución de las migraciones.</returns>
        private static async Task EjecutarMigracionesAccesorios(IServiceScope scope)
        {
            using var moduloAccesorios = scope.ServiceProvider.GetRequiredService<ModuloAccesoriosExplosivosContext>();

            // Verificar si hay migraciones pendientes
            var pendingMigrations = await moduloAccesorios.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                await moduloAccesorios.Database.MigrateAsync();
            }
        }

        /// <summary>
        /// Ejecuta las migraciones automáticas para el módulo de Gerencia.
        /// </summary>
        /// <param name="scope">Instancia de IServiceScope que proporciona acceso a los servicios.</param>
        /// <returns>Una tarea asincrónica que representa la ejecución de las migraciones.</returns>
        private static async Task EjecutarMigracionesRecepciones(IServiceScope scope)
        {
            using var moduloRecepciones = scope.ServiceProvider.GetRequiredService<ModuloRecepcionExplosivosContext>();

            // Verificar si hay migraciones pendientes
            var pendingMigrations = await moduloRecepciones.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                await moduloRecepciones.Database.MigrateAsync();
            }
        }

        /// <summary>
        /// Ejecuta las migraciones automáticas para el módulo de Gerencia.
        /// </summary>
        /// <param name="scope">Instancia de IServiceScope que proporciona acceso a los servicios.</param>
        /// <returns>Una tarea asincrónica que representa la ejecución de las migraciones.</returns>
        private static async Task EjecutarMigracionesAlmacenamiento(IServiceScope scope)
        {
            using var moduloAlmacenamiento = scope.ServiceProvider.GetRequiredService<ModuloAlmacenamientoExplosivosContext>();

            // Verificar si hay migraciones pendientes
            var pendingMigrations = await moduloAlmacenamiento.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                await moduloAlmacenamiento.Database.MigrateAsync();
            }
        }

        /// <summary>
        /// Ejecuta las migraciones automáticas para el módulo de Gerencia.
        /// </summary>
        /// <param name="scope">Instancia de IServiceScope que proporciona acceso a los servicios.</param>
        /// <returns>Una tarea asincrónica que representa la ejecución de las migraciones.</returns>
        private static async Task EjecutarMigracionesGerencias(IServiceScope scope)
        {
            using var moduloGerencias = scope.ServiceProvider.GetRequiredService<ModuloGerenciaExplosivosContext>();

            // Verificar si hay migraciones pendientes
            var pendingMigrations = await moduloGerencias.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                await moduloGerencias.Database.MigrateAsync();
            }
        }
    }
}