using API.Database.Administracion.Entidades.Identidad;
using API.Database.Core.Enums;

namespace API.Database.Core.Entidades
{
    public class Tareas
    {
        // Public Fields
        public int Id { get; set; }
        public int DepartamentoId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string ResponsablePrincipalId { get; set; } // UserId
        public string CreadorId { get; set; } // UserId
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? Deadline { get; set; }
        public PrioridadEnum Prioridad { get; set; }
        public EstadoEnum Estado { get; set; }
        public bool Eliminado { get; set; } = false;

        // Navigation
        public virtual Departamentos Departamento { get; set; }
        public virtual Usuarios ResponsablePrincipal { get; set; }
        public virtual Usuarios Creador { get; set; }
        public virtual CamposPrivadosTarea CamposPrivados { get; set; }
        public virtual ICollection<TareaAsignado> Asignados { get; set; }
        public virtual ICollection<TareaComentario> Comentarios { get; set; }
        public virtual ICollection<TareaHistorial> Historial { get; set; }
        public virtual ICollection<TareaEvidencia> Evidencias { get; set; }
    }
}
