using System;
using System.Diagnostics;
using System.Linq;

namespace SistemaFrontEnd.Services.Spa
{
    public static class SpaProcessManager
    {
        /// <summary>
        /// Elimina los procesos que esten escuchando en el puerto especificado
        /// </summary>
        /// <param name="port"></param>
        public static void KillProcessOnPort(int port)
        {
            try
            {
                Console.WriteLine($"[KILL PORT] Attempting to kill processes on port {port}");

                var output = GetOutput(port);
                var lines = output.Split('\n');
                foreach (var line in lines)
                {
                    if (line.Contains($":{port}") && (line.Contains("LISTENING") || line.Contains("ESTABLISHED")))
                    {
                        var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        var pid = parts.Last();

                        if (int.TryParse(pid, out var processId))
                        {
                            try
                            {
                                Console.WriteLine($"[KILL PORT] Found process {processId} on port {port}");

                                using var killer = new Process
                                {
                                    StartInfo = new ProcessStartInfo
                                    {
                                        FileName = "taskkill",
                                        Arguments = $"/PID {processId} /F /T",
                                        CreateNoWindow = true,
                                        UseShellExecute = false
                                    }
                                };

                                killer.Start();
                                killer.WaitForExit(2000);

                                Console.WriteLine($"[KILL PORT] Process {processId} terminated");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"[KILL PORT ERROR] Could not kill process {processId}: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PORT KILL ERROR] {ex.Message}");
            }
        }

        /// <summary>
        /// Verifica si el puerto especificado esta libre
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool IsPortFree(int port)
        {
            string outputProcess = GetOutput(port);

            return string.IsNullOrEmpty(outputProcess) || !outputProcess.Contains("LISTENING") && !outputProcess.Contains("ESTABLISHED");
        }

        /// <summary>
        /// Obtiene la salida del comando netstat
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        private static string GetOutput(int port)
        {
            string output = string.Empty;

            try
            {
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/c netstat -ano | findstr \":{port}\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
            catch { }

            return output;
        }
    }
}
