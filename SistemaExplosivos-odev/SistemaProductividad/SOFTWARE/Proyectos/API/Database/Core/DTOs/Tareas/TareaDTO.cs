using API.Database.Core.Enums;

namespace API.Database.Core.DTOs.Tareas
{
    public class TareaDTO
    {
        public int Id { get; set; }
        public int DepartamentoId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string ResponsablePrincipalNombre { get; set; }
        public string CreadorNombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? Deadline { get; set; }
        public PrioridadEnum Prioridad { get; set; }
        public string PrioridadTexto => Prioridad.ToString();
        public EstadoEnum Estado { get; set; }
        public string EstadoTexto => Estado.ToString();
        
        // Private fields (Nullable, null if not authorized)
        public int? DificultadEstimada { get; set; }
        public double? TiempoEstimado { get; set; }
        public double? TiempoReal { get; set; }
        public string EvaluacionDesempeno { get; set; }
        public string NotasPrivadas { get; set; }
        public string ImpactoInterno { get; set; }
        public string ClasificacionInterna { get; set; }
    }
}
