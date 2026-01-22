using System;
using System.Collections.Generic;

namespace API.Database.Administracion.DTOs.Autentificacion
{
    public class AutentificacionRespuesta
    {

        public string Token { get; set; }

        public string ActualizarToken { get; set; }
        public DateTime Expiracion { get; set; }
        public bool OperacionExitosa { get; set; }
        public bool TokenExpirado { get; set; }
        public bool EsMantenimiento { get; set; }
        public IEnumerable<string> Errores { get; set; }
        //public List<ModuloDTO> ModulosHabilitados { get; set; }

    }
}
