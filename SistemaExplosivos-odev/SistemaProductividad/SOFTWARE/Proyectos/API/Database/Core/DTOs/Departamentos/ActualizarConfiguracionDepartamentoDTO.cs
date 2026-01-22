using API.Database.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.Database.Core.DTOs.Departamentos
{
    public class ActualizarConfiguracionDepartamentoDTO
    {
        [Required]
        public int DepartamentoId { get; set; }
        public ModoAsignacionEnum ModoAsignacion { get; set; }
        public bool PermiteAsignarOtros { get; set; }
        public bool PermiteCamposPrivados { get; set; }
        public string KpisActivos { get; set; } = "[]";
    }
}
