using System.Collections.Generic;
using System.Net;

namespace API.Database.Administracion.DTOs.Respuestas
{
    public class RespuestaGenerica
    {
        public string Mensaje { get; set; }
        public IEnumerable<string> Errores { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool OperacionExitosa { get; set; }
        public object Payload { get; set; }
    }
}
