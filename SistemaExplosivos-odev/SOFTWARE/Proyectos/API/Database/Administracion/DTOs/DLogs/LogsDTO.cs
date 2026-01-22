using System;

namespace API.Database.Administracion.DTOs.DLogs
{
    public class LogsDTO
    {
        public long Id { get; set; }
        public string Mensaje { get; set; }
        public string Nivel { get; set; }
        public string Excepcion { get; set; }
        public string Usuario { get; set; }
        public string Accion { get; set; }
        public DateTime FechaHora { get; set; }
        public string DatosPeticion { get; set; }
    }

}
