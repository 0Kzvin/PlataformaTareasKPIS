using System;
using System.Collections.Generic;

namespace API.Database.Administracion.DTOs.Identidad
{
    public class UsuarioDTO
    {
        public string Id { get; set; }
        public string IdRol { get; set; }
        public string NombreRol { get; set; }
        public string Usuario { get; set; }

        public string Email { get; set; }

        public string Nombre { get; set; }

        public string Apellidos { get; set; }

        public string NombreCompleto { get; set; }
        public string Foto { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaModificacion { get; set; }
        public string AreaSeleccionada { get; set; }
        public bool Estado { get; set; }
    }
}
