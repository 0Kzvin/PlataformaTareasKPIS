using System;

namespace API.Database.Administracion.DTOs.Autentificacion
{
    public class AutExitosaRespuesta
    {
        public bool OperacionExitosa { get; set; }
        public AutentificacionInformacion DatosUsuario { get; set; }
        public string Token { get; set; }
        public string ActualizarToken { get; set; }
        public DateTime Expiracion { get; set; }
        public bool EsMantenimiento { get; set; }
        //public List<ModuloDTO> ModulosHabilitados { get; set; }
    }
}
