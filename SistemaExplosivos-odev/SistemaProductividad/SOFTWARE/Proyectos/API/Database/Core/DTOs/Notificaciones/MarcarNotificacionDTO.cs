using System.ComponentModel.DataAnnotations;

namespace API.Database.Core.DTOs.Notificaciones
{
    public class MarcarNotificacionDTO
    {
        [Required]
        public int Id { get; set; }
        public bool Leido { get; set; } = true;
    }
}
