using Microsoft.AspNetCore.Identity;

namespace API.Database.Administracion.Entidades.Identidad
{
    public class UsuariosRoles : IdentityUserRole<string>
    {

        public virtual Usuarios Usuario { get; set; }

        public virtual Roles Rol { get; set; }
    }
}
