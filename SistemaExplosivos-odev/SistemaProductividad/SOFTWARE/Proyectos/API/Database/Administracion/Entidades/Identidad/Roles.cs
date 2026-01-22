using Microsoft.AspNetCore.Identity;

namespace API.Database.Administracion.Entidades.Identidad
{
    public class Roles : IdentityRole
    {
        public string Descripcion { get; set; }
        public bool Estado { get; set; } = true;
        
        // Additional fields from reference if any
    }
}
