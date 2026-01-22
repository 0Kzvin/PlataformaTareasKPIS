using System.ComponentModel.DataAnnotations;

namespace API.Database.Core.DTOs.Tareas
{
    public class AsignarTareaDTO
    {
        [Required]
        public int TareaId { get; set; }
        [Required]
        public string UsuarioId { get; set; }
        public string RolAsignacion { get; set; } = "Colaborador";
        public bool EsResponsablePrincipal { get; set; }
    }
}
