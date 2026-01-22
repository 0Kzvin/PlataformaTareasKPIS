using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace API.Database.Administracion.Entidades.Identidad
{
    public class Roles : IdentityRole
    {
        public virtual ICollection<RolesPermisos> RolesPermisos { get; set; } = new List<RolesPermisos>();
        public virtual ICollection<RolesModulos> RolesModulos { get; set; } = new List<RolesModulos>();
        public virtual ICollection<UsuariosRoles> UsuariosRoles { get; set; } = new List<UsuariosRoles>();
        public string Descripcion { get; set; }
        public bool Estado { get; set; } = true;
        public bool EstaOculto { get; set; }
        public bool SuperUsuario { get; set; }
    }
}
