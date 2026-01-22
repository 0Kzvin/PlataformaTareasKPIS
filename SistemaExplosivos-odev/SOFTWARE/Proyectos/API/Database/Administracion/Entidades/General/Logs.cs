using System;

namespace API.Database.Administracion.Entidades.General
{
    public class Logs
    {
        public long Id { get; set; }
        public string Mensaje { get; set; }
        public string Nivel { get; set; }
        public DateTime FechaHora { get; set; }
        public string Usuario { get; set; }
        public string Direccion { get; set; }
        public string Accion { get; set; }
        public string DatosPeticion { get; set; }
    }
}
