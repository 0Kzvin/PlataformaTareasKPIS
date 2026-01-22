using System;

namespace API.Servicios.Preterminados.Autenticacion
{
    /// <summary>
    /// Clase que contiene la configuración necesaria para la generación y validación de JWT (JSON Web Tokens).
    /// </summary>
    public class JwtConfiguraciones
    {
        /// <summary>
        /// Clave secreta utilizada para firmar y verificar los tokens JWT.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Tiempo de vida del token JWT. Define cuánto tiempo será válido el token después de su emisión.
        /// </summary>
        public TimeSpan VidaToken { get; set; }

        /// <summary>
        /// Emisor (Issuer) del token JWT. Identifica la entidad que emitió el token.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Audiencia (Audience) del token JWT. Identifica a los destinatarios para los que está destinado el token.
        /// </summary>
        public string Audience { get; set; }
    }
}