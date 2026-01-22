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
        public string AsignadoId { get; set; } // UserId
        public string CreadorId { get; set; } // UserId
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? Deadline { get; set; }
        public PrioridadEnum Prioridad { get; set; }
        public EstadoEnum Estado { get; set; }
        
        // Private Fields (Admin/Leader only)
        public int? DificultadEstimada { get; set; } // 1-10
        public double? TiempoEstimadoHoras { get; set; }
        public double? TiempoRealHoras { get; set; }
        public string NotasPrivadas { get; set; }
        public string ClasificacionInterna { get; set; }

        public bool Eliminado { get; set; } = false;

        // Navigation
        public virtual Departamentos Departamento { get; set; }
        public virtual Usuarios Asignado { get; set; }
        public virtual Usuarios Creador { get; set; }
        public virtual ICollection<Comentarios> Comentarios { get; set; }
    }

    public class Comentarios
    {
        public int Id { get; set; }
        public int TareaId { get; set; }
        public string UsuarioId { get; set; }
        public string Mensaje { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public virtual Tareas Tarea { get; set; }
        public virtual Usuarios Usuario { get; set; }
    }
}
