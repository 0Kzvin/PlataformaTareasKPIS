using System.ComponentModel.DataAnnotations;

namespace API.Database.Core.DTOs.Kpis
{
    public class RegistrarKpiDTO
    {
        [Required]
        public int DepartamentoId { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Formula { get; set; }
        public string Meta { get; set; }
        public bool Activo { get; set; } = true;
    }
}
