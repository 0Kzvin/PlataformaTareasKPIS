using API.Database.Administracion.Entidades.Identidad;
using API.Database.Core.Enums;

namespace API.Database.Core.Entidades
{
    public class Departamentos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string LiderId { get; set; }
        public int? ConfiguracionId { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Usuarios Lider { get; set; }
        public virtual ConfiguracionDepartamento Configuracion { get; set; }
        public virtual ICollection<DepartamentoUsuario> Usuarios { get; set; }
        public virtual ICollection<Usuarios> Miembros { get; set; }
        public virtual ICollection<Tareas> Tareas { get; set; }
        public virtual ICollection<KpiDepartamento> Kpis { get; set; }
    }
}
