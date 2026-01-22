using API.Database.Administracion.Entidades.Identidad;

namespace API.Database.Core.Entidades
{
    public class Notificacion
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public string Tipo { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public bool Leido { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public virtual Usuarios Usuario { get; set; }
    }
}
