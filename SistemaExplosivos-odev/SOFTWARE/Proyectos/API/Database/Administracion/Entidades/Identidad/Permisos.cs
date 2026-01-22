using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Administracion.Entidades.Identidad
{
    public class Permisos
    {
        public string Id { get; set; }
        public string IdUnico { get; set; }
        public int IdGrupoPermiso { get; set; }
        public string Nombre { get; set; }
        public string NombreNormalizado { get; set; }
        public string Descripcion { get; set; }
        public bool Ocultar { get; set; } = false;
        [ForeignKey("IdGrupoPermiso")]
        public GruposPermisos GrupoPermiso { get; set; }
        public virtual ICollection<RolesPermisos> RolesPermisos { get; set; } = new List<RolesPermisos>();

    }
}
