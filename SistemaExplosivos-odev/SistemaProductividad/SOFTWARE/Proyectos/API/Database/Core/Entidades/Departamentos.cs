using API.Database.Administracion.Entidades.Identidad;

namespace API.Database.Core.Entidades
{
    public class Departamentos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string LiderId { get; set; }
        public string ConfiguracionJson { get; set; } = "{}";
        public bool Estado { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Usuarios Lider { get; set; }
        public virtual ICollection<Usuarios> Miembros { get; set; }
    }
}
