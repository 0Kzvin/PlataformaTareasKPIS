namespace SistemaFrontEnd.Services.Spa
{
    /// <summary>
    /// Configuración del servicio de SPA
    /// </summary>
    public class SpaServiceConfiguration
    {
        /// <summary>
        /// Ruta del archivo de SPA en el directorio de producción
        /// </summary>
        public string ProductionSpaFilePath { get; set; } = "wwwroot";

        /// <summary>
        /// Ruta del archivo de SPA en el directorio de desarrollo
        /// </summary>
        public string DevelopmentSpaFilePath { get; set; } = "wwwroot";

        /// <summary>
        /// Puerto de desarrollo del SPA para iniciar el servidor
        /// </summary>
        public int DevelopmentPort { get; set; } = 9000;

        /// <summary>
        /// Tipo de script runner para iniciar el servidor
        /// </summary>
        public ScriptRunnerType ScriptRunner { get; set; } = ScriptRunnerType.bun;

        /// <summary>
        /// Nombre del script del package.json para iniciar el servidor
        /// </summary>
        public string ScriptName { get; set; } = "dev";

        /// <summary>
        /// Indica si se debe usar HTTPS para iniciar el servidor
        /// </summary>
        public bool UseHttps { get; set; } = false;

        /// <summary>
        /// Indica si se debe abrir el navegador al iniciar el servidor
        /// </summary>
        public bool OpenBrowser { get; set; } = true;
    }
}
