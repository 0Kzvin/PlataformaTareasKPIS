using API.Localizacion;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Globalization;

namespace API.Servicios.Preterminados
{
    /// <summary>
    /// Clase que implementa la interfaz IInyeccionServicios para configurar y registrar los servicios de localización en la aplicación.
    /// Esta clase se encarga de configurar la localización (idiomas y formatos) para la aplicación.
    /// </summary>
    public class InyeccionLocalizacion : IInyeccionServicios
    {
        // Constantes para nombres de culturas
        private const string CulturaEspaniol = "es";
        private const string CulturaIngles = "en";

        /// <summary>
        /// Método que instala y configura los servicios de localización en la colección de servicios.
        /// Este método configura los recursos de localización, las culturas soportadas y la cultura predeterminada.
        /// </summary>
        /// <param name="servicios">Colección de servicios donde se configurarán los servicios de localización.</param>
        /// <param name="configuracion">Instancia de IConfiguration que proporciona acceso a la configuración de la aplicación.</param>
        public void InstalarServicios(IServiceCollection servicios, IConfiguration configuracion)
        {
            // Configuración para usar archivos de recursos de localización
            servicios.AddLocalization(options => options.ResourcesPath = "Localizacion");
            servicios.AddSingleton<ISharedLocalizer, SharedLocalizer>();

            // Configuración general de localización
            servicios.Configure<RequestLocalizationOptions>(options =>
            {
                // Obtener culturas soportadas
                var culturasSoportadas = ObtenerCulturasSoportadas();

                // !! IMPORTANTE !!
                // DEJARLO EN CULTURA EN PARA PODER PARSEAR LAS RESPUESTAS DE MANERA CORRECTA
                // DE LO CONTRARIO LOS DECIMALES SALEN DE MANERA INCORRECTA EN LA DB
                options.DefaultRequestCulture = new RequestCulture(culture: CulturaIngles, uiCulture: CulturaIngles);

                // Culturas soportadas para la aplicación
                options.SupportedCultures = culturasSoportadas;
                options.SupportedUICultures = culturasSoportadas;

                // Aplicar la cultura actual a los encabezados de respuesta
                options.ApplyCurrentCultureToResponseHeaders = true;
            });
        }

        /// <summary>
        /// Obtiene las culturas soportadas (español e inglés) y las personaliza.
        /// </summary>
        /// <returns>Lista de culturas soportadas.</returns>
        private List<CultureInfo> ObtenerCulturasSoportadas()
        {
            var culturasSoportadas = new List<CultureInfo>();

            // Configuración de la cultura en español
            var culturaEspaniol = new CultureInfo(CulturaEspaniol);
            PersonalizarFormatosNumericos(culturaEspaniol);
            culturasSoportadas.Add(culturaEspaniol);

            // Configuración de la cultura en inglés
            var culturaIngles = new CultureInfo(CulturaIngles);
            PersonalizarFormatosNumericos(culturaIngles);
            culturasSoportadas.Add(culturaIngles);

            return culturasSoportadas;
        }

        /// <summary>
        /// Personaliza los formatos numéricos de una cultura para evitar problemas con decimales.
        /// </summary>
        /// <param name="cultura">Cultura a personalizar.</param>
        private static void PersonalizarFormatosNumericos(CultureInfo cultura)
        {
            cultura.NumberFormat.CurrencyDecimalSeparator = ".";
            cultura.NumberFormat.NumberDecimalSeparator = ".";
            cultura.NumberFormat.NumberGroupSeparator = ",";
            cultura.NumberFormat.CurrencyGroupSeparator = ",";
        }
    }
}