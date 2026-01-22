using API.Database.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.Database.Core.DTOs.Tareas
{
    public class CambiarEstadoTareaDTO
    {
        [Required]
        public int Id { get; set; }
        public EstadoEnum Estado { get; set; }
    }
}
