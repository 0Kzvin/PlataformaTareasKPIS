using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace API.Database.Administracion.Entidades.Identidad
{
    public class Usuarios : IdentityUser
    {
        public string Nombre { get; set; }

        public string Apellidos { get; set; }

        public string NombreCompleto { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Estado { get; set; } = true;

        public string Foto { get; set; }

        public virtual ICollection<UsuariosRoles> UsuariosRoles { get; set; } = new List<UsuariosRoles>();
        public virtual ICollection<UsuariosModulos> UsuariosModulos { get; set; } = new List<UsuariosModulos>();
    }
}
