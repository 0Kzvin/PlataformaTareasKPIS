using API.Database.Administracion.Entidades.Identidad;

namespace API.Database.Core.Entidades
{
    public class TareaComentario
    {
        public int Id { get; set; }
        public int TareaId { get; set; }
        public string UsuarioId { get; set; }
        public string Comentario { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public virtual Tareas Tarea { get; set; }
        public virtual Usuarios Usuario { get; set; }
    }
}
