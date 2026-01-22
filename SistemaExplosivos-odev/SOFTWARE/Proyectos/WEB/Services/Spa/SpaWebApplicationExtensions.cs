using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaFrontEnd.Services.Spa
{
    /// <summary>
    /// Extensiones para la aplicación Web
    /// </summary>
    public static class SpaWebApplicationExtensions
    {
        /// <summary>
        /// Inicia el servidor de desarrollo del SPA usando el script especificado en el directorio especificado.
        /// </summary>
        /// <param name="spa"></param>
        /// <param name="config"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        public static async Task UseDevServer(this ISpaBuilder spa, SpaServiceConfiguration config, WebApplication app)
        {
            // Registrar el evento de cierre de la aplicación
            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
                SpaProcessManager.KillProcessOnPort(config.DevelopmentPort);

            try
            {
                // 1. Limpieza inicial de procesos
                Console.WriteLine($"[SPA] Cleaning existing processes on port {config.DevelopmentPort}...");
                SpaProcessManager.KillProcessOnPort(config.DevelopmentPort);
                await Task.Delay(1000); // Espera sincrónica para liberar el puerto

                if (!SpaProcessManager.IsPortFree(config.DevelopmentPort))
                {
                    Console.WriteLine($"[WARNING] Port {config.DevelopmentPort} is still in use after cleanup!");
                    throw new InvalidOperationException($"Port {config.DevelopmentPort} is still in use after cleanup!");
                }

                // 2. Iniciar servidor de desarrollo
                Console.WriteLine($"[SPA] Starting development server with {config.ScriptRunner}...");

                var process = SpaScriptRunner.StartDevServer(
                    runnerType: config.ScriptRunner,
                    workingDirectory: Path.Combine(Directory.GetCurrentDirectory(), config.DevelopmentSpaFilePath),
                    port: config.DevelopmentPort,
                    scriptName: config.ScriptName,
                    useHttps: config.UseHttps,
                    openBrowser: config.OpenBrowser);

                // 3. Esperar inicialización
                Console.WriteLine($"[SPA] Waiting for server to be ready...");
                bool isServerReady = await SpaScriptRunner.WaitForServerReady(process, config.DevelopmentPort, config.UseHttps, TimeSpan.FromMinutes(2));
                if (!isServerReady)
                {
                    throw new TimeoutException("Server did not start in expected time");
                }

                // 4. Configurar proxy
                Console.WriteLine($"[SPA] Configuring proxy...");
                var protocol = config.UseHttps ? "https" : "http";
                var baseUri = new Uri($"{protocol}://localhost:{config.DevelopmentPort}");
                spa.UseProxyToSpaDevelopmentServer(baseUri);

                Console.WriteLine($"[SPA] Running in Development mode - Ready at {protocol}://localhost:{app.GetPort()}/ (proxied to {config.DevelopmentPort})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SPA ERROR] {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Obtiene el puerto de la aplicación Web
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static int GetPort(this WebApplication app)
        {
            return app.Urls
                .Select(url => new Uri(url))
                .FirstOrDefault()
                ?.Port ?? 0;
        }
    }
}
