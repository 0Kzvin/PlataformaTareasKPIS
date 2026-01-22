using System.Collections.Generic;

namespace API.Database.Administracion.DTOs.Autentificacion
{
    public class AuthFallidaRespuesta
    {
        public IEnumerable<string> Errores { get; set; }
        public bool OperacionExitosa { get; set; }
        public bool TokenExpirado { get; set; } = true;
    }
}
