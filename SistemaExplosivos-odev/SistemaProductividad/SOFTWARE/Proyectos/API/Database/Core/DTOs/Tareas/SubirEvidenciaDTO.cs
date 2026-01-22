using System.ComponentModel.DataAnnotations;

namespace API.Database.Core.DTOs.Tareas
{
    public class SubirEvidenciaDTO
    {
        [Required]
        public int TareaId { get; set; }
        [Required]
        public string RutaArchivo { get; set; }
        public string Tipo { get; set; }
    }
}
