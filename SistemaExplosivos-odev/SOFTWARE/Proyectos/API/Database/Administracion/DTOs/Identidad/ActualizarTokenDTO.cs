using System;

namespace API.Database.Administracion.DTOs.Identidad
{
    public class ActualizarTokenDTO
    {
        public string Token { get; set; }

        public string ActualizarToken { get; set; }
        public DateTime Expiracion { get; set; }
    }
}
