using API.Atributos;
using System.ComponentModel;
using System;

namespace API.Database.Administracion.DTOs.DLogs
{
    public class ReporteLogsDTO
    {
        [TituloRG("Momento del Evento")]
        [DisplayName("Momento del Evento")]
        public DateTime FechaHora { get; set; }

        [TituloRG("Mensaje")]
        [DisplayName("Mensaje")]
        public string Mensaje { get; set; }

        [TituloRG("Lugar del error")]
        [DisplayName("Lugar del error")]
        public string Accion { get; set; }

        [TituloRG("Equipo")]
        [DisplayName("Equipo")]
        public string Direccion { get; set; }

        [TituloRG("Cliente")]
        [DisplayName("Cliente")]
        public string Cliente { get; set; }
    }
}
