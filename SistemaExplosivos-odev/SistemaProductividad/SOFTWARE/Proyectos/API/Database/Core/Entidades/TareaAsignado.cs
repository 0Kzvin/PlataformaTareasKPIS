using API.Database.Administracion.Entidades.Identidad;

namespace API.Database.Core.Entidades
{
    public class TareaAsignado
    {
        public int Id { get; set; }
        public int TareaId { get; set; }
        public string UsuarioId { get; set; }
        public string RolAsignacion { get; set; }

        public virtual Tareas Tarea { get; set; }
        public virtual Usuarios Usuario { get; set; }
    }
}
