// SPAScriptRunner.cs
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFrontEnd.Services.Spa
{
    /// <summary>
    /// Clase para iniciar el servidor de desarrollo del SPA.
    /// </summary>
    public static class SpaScriptRunner
    {
        /// <summary>
        /// Inicia el servidor de desarrollo del SPA usando el script especificado en el directorio especificado.
        /// </summary>
        /// <param name="runnerType"></param>
        /// <param name="workingDirectory"></param>
        /// <param name="port"></param>
        /// <param name="scriptName"></param>
        /// <param name="useHttps"></param>
        /// <param name="openBrowser"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static Process StartDevServer(
            ScriptRunnerType runnerType,
            string workingDirectory,
            int port,
            string scriptName = "dev",
            bool useHttps = false,
            bool openBrowser = true)
        {
            var args = BuildDevServerCommand(runnerType, scriptName, port, useHttps);

            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {args}",
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            // Configurar variables de entorno
            startInfo.Environment["BROWSER"] = openBrowser ? "true" : "false";
            startInfo.Environment["PORT"] = port.ToString();
            startInfo.Environment["HTTPS"] = useHttps ? "true" : "false";

            var process = new Process { StartInfo = startInfo };

            process.OutputDataReceived += (_, e) => Console.WriteLine($"[{runnerType.ToString().ToUpper()}] {e.Data}");
            process.ErrorDataReceived += (_, e) => Console.WriteLine($"[{runnerType.ToString().ToUpper()} ERROR] {e.Data}");

            if (!process.Start())
            {
                throw new InvalidOperationException($"Failed to start {runnerType} process");
            }

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return process;
        }

        /// <summary>
        /// Construye el comando para iniciar el servidor de desarrollo del SPA.
        /// </summary>
        /// <param name="runnerType"></param>
        /// <param name="scriptName"></param>
        /// <param name="port"></param>
        /// <param name="useHttps"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        private static string BuildDevServerCommand(
            ScriptRunnerType runnerType,
            string scriptName,
            int port,
            bool useHttps)
        {
            string protocolArg = useHttps ? "--https" : "";
            string portArg = $"--port={port}";

            return runnerType switch
            {
                // yarn dev --port 3000 --https
                ScriptRunnerType.yarn => $"yarn {scriptName} {portArg} {protocolArg}",

                // pnpm run dev --port 3000 --https
                ScriptRunnerType.pnpm => $"pnpm run {scriptName} {portArg}  {protocolArg}",

                // bun run dev --port 3000 --https
                ScriptRunnerType.bun => $"bun run {scriptName} {portArg}  {protocolArg}",

                _ => throw new NotSupportedException($"Script runner {runnerType} is not supported")
            };
        }

        /// <summary>
        /// Espera a que el servidor de desarrollo del SPA se encuentre listo.
        /// </summary>
        /// <param name="process"></param>
        /// <param name="port"></param>
        /// <param name="useHttps"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static async Task<bool> WaitForServerReady(Process process, int port, bool useHttps, TimeSpan timeout)
        {
            var startTime = DateTime.UtcNow;
            var httpClient = new HttpClient();
            var baseUrl = $"{(useHttps ? "https" : "http")}://localhost:{port}";

            // Timeout para las peticiones HTTP (no confundir con el timeout total)
            httpClient.Timeout = TimeSpan.FromSeconds(2);

            while (!process.HasExited && DateTime.UtcNow - startTime < timeout)
            {
                try
                {
                    // Intentamos hacer una petición al servidor
                    var response = await httpClient.GetAsync(baseUrl);

                    // Consideramos que el servidor está listo si responde con cualquier código 2xx o 3xx
                    if (response.IsSuccessStatusCode || (int)response.StatusCode >= 300 && (int)response.StatusCode < 400)
                    {
                        Console.WriteLine($"[SPA] Server is ready and responding at {baseUrl}");
                        return true;
                    }
                }
                catch (HttpRequestException)
                {
                    // El servidor no está listo aún, continuamos esperando
                }
                catch (TaskCanceledException)
                {
                    // Timeout de la petición HTTP, continuamos esperando
                }

                await Task.Delay(500);
            }

            // Si llegamos aquí es porque se agotó el tiempo de espera
            Console.WriteLine($"[SPA] Timeout waiting for server at {baseUrl}");
            return false;
        }
    }
}
