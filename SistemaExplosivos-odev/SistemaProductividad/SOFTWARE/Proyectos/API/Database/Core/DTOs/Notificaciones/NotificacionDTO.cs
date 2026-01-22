namespace API.Database.Core.DTOs.Notificaciones
{
    public class NotificacionDTO
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public bool Leido { get; set; }
        public DateTime Fecha { get; set; }
    }
}
