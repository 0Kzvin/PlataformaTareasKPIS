using Microsoft.AspNetCore.Http;

namespace API.Database.Administracion.DTOs.Identidad
{
    public class CambiarFotoPerfilDTO
    {
        public string IdUsuario { get; set; }
        public IFormFile Foto { get; set; }
    }
}
