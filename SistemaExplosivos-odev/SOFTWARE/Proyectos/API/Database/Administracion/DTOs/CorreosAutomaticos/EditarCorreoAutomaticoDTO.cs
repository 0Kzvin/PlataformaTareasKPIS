using System.Collections.Generic;

namespace API.Database.Administracion.DTOs.CorreosAutomaticos
{
    public class EditarCorreoAutomaticoDTO
    {
        public int Id { get; set; }
        public int IdModulo { get; set; }
        public string Nombre { get; set; }
        public string NombreModulo { get; set; }
        public string NombreClave { get; set; }
        public string Descripcion { get; set; }
        public string ExpresionCron { get; set; }
        public List<string> ListaDestinatarios { get; set; }
    }
}
