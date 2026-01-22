using System.Collections.Generic;

namespace API.Database.Administracion.DTOs.Respuestas
{
    public class RespuestaFallidaGenerica
    {
        public string Mensaje { get; set; }
        public IEnumerable<string> Errores { get; set; }
        public object Payload { get; set; }
    }
}
