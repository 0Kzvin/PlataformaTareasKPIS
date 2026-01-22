using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Administracion.Entidades.Identidad
{
    public class RolesPermisos
    {
        public string IdRol { get; set; }

        [ForeignKey("IdRol")]
        public virtual Roles Roles { get; set; }
        public string IdPermiso { get; set; }

        [ForeignKey("IdPermiso")]
        public virtual Permisos Permisos { get; set; }
    }
}
