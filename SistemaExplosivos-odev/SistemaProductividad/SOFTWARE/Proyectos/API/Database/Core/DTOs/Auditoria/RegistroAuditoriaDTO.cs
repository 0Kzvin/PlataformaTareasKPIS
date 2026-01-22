namespace API.Database.Core.DTOs.Auditoria
{
    public class RegistroAuditoriaDTO
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public string UsuarioNombre { get; set; }
        public string Entidad { get; set; }
        public string Accion { get; set; }
        public DateTime Fecha { get; set; }
        public string Payload { get; set; }
    }
}
