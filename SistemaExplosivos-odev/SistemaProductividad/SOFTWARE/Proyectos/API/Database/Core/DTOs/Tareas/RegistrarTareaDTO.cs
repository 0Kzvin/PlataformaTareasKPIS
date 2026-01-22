using API.Database.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.Database.Core.DTOs.Tareas
{
    public class RegistrarTareaDTO
    {
        [Required]
        public int DepartamentoId { get; set; }
        [Required]
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string AsignadoId { get; set; } // Optional at creation
        public DateTime? Deadline { get; set; }
        public PrioridadEnum Prioridad { get; set; }
    }
}
