using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace API.Database.Administracion.DTOs.Identidad
{
    public class EditarUsuarioDTO
    {
        public string IdUsuario { get; set; }
        public List<string> AreaSeleccionada { get; set; }
        public string IdRol { get; set; }
        public string Username { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public IFormFile Foto { get; set; }
    }
}
