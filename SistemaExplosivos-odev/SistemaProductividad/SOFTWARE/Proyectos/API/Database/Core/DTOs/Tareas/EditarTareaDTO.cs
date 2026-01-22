using API.Database.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.Database.Core.DTOs.Tareas
{
    public class EditarTareaDTO
    {
        [Required]
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime? Deadline { get; set; }
        public PrioridadEnum? Prioridad { get; set; }
    }
}
