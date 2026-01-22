using System.ComponentModel.DataAnnotations;

namespace API.Database.Core.DTOs.Tareas
{
    public class AgregarComentarioDTO
    {
        [Required]
        public int TareaId { get; set; }
        [Required]
        public string Comentario { get; set; }
    }
}
