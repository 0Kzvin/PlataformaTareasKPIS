using API.Database.Core.Enums;

namespace API.Database.Core.DTOs.Tareas
{
    public class TareaDTO
    {
        public int Id { get; set; }
        public int DepartamentoId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string AsignadoNombre { get; set; }
        public string CreadorNombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? Deadline { get; set; }
        public PrioridadEnum Prioridad { get; set; }
        public string PrioridadTexto => Prioridad.ToString();
        public EstadoEnum Estado { get; set; }
        public string EstadoTexto => Estado.ToString();
        
        // Private fields (Nullable, null if not authorized)
        public int? DificultadEstimada { get; set; }
        public double? TiempoEstimadoHoras { get; set; }
        public double? TiempoRealHoras { get; set; }
        public string NotasPrivadas { get; set; }
        public string ClasificacionInterna { get; set; }
    }
}
