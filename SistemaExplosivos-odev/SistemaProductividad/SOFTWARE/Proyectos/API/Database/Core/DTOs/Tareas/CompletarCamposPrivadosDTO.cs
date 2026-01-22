using System.ComponentModel.DataAnnotations;

namespace API.Database.Core.DTOs.Tareas
{
    public class CompletarCamposPrivadosDTO
    {
        [Required]
        public int TareaId { get; set; }
        public int? DificultadEstimada { get; set; }
        public double? TiempoEstimado { get; set; }
        public double? TiempoReal { get; set; }
        public string EvaluacionDesempeno { get; set; }
        public string NotasPrivadas { get; set; }
        public string ImpactoInterno { get; set; }
        public string ClasificacionInterna { get; set; }
    }
}
