namespace API.Database.Core.Entidades
{
    public class TareaEvidencia
    {
        public int Id { get; set; }
        public int TareaId { get; set; }
        public string RutaArchivo { get; set; }
        public string Tipo { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public virtual Tareas Tarea { get; set; }
    }
}
