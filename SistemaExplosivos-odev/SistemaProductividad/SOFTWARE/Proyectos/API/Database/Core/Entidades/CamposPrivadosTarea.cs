namespace API.Database.Core.Entidades
{
    public class CamposPrivadosTarea
    {
        public int TareaId { get; set; }
        public int? DificultadEstimada { get; set; }
        public double? TiempoEstimado { get; set; }
        public double? TiempoReal { get; set; }
        public string EvaluacionDesempeno { get; set; }
        public string NotasPrivadas { get; set; }
        public string ImpactoInterno { get; set; }
        public string ClasificacionInterna { get; set; }

        public virtual Tareas Tarea { get; set; }
    }
}
