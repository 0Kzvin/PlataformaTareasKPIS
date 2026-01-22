using System;
using System.Collections.Generic;

namespace API.Database.Administracion.DTOs.Autentificacion
{
    public class AutentificacionInformacion
    {
        public bool Estado { get; set; }
        public string AreaSeleccionada { get; set; }
        public string NumeroTelefonico { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public bool OperacionExitosa { get; set; }
        public string Foto { get; set; }
        public IEnumerable<string> Errores { get; set; }
        public ConfiguracionesModulos ConfiguracionesModulos { get; set; }
    }

    public class ConfiguracionesModulos
    {
        public ConfiguracionesVL ConfiguracionesVL { get; set; }
    }

    public class ConfiguracionesVL
    {
        public string ProductoEncargado { get; set; }
    }
}
