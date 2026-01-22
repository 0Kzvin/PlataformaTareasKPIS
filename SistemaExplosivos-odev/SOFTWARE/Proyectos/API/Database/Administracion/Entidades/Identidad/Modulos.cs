using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Administracion.Entidades.Identidad
{
    public class Modulos
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string NombreNormalizado { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public virtual ICollection<UsuariosModulos> UsuarioModulos { get; set; } = new List<UsuariosModulos>();
        public virtual ICollection<RolesModulos> RolesModulos { get; set; } = new List<RolesModulos>();
    }
}
