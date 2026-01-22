using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace API.Database.Administracion.DTOs.Identidad
{
    public class RegistroUsuarioDTO
    {
        public string Username { get; set; }

        //[EmailAddress(ErrorMessage = "El campo correo no es un correo valido")]
        public string Email { get; set; }

        public string Nombre { get; set; }

        public string Apellidos { get; set; }

        public string Password { get; set; }

        public string RolId { get; set; }

        public IFormFile Foto { get; set; }
        public List<ModulosPorActivarDTO> ModulosPorActivar { get; set; }
        public List<string> AreaSeleccionada { get; set; }
    }
}
