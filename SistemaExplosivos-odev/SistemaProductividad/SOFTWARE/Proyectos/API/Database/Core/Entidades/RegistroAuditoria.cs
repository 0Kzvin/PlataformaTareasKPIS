using API.Database.Administracion.Entidades.Identidad;

namespace API.Database.Core.Entidades
{
    public class RegistroAuditoria
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public string Entidad { get; set; }
        public string Accion { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public string Payload { get; set; }

        public virtual Usuarios Usuario { get; set; }
    }
}
