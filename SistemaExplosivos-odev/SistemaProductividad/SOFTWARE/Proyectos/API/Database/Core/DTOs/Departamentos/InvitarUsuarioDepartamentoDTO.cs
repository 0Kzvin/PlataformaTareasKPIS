using System.ComponentModel.DataAnnotations;

namespace API.Database.Core.DTOs.Departamentos
{
    public class InvitarUsuarioDepartamentoDTO
    {
        [Required]
        public int DepartamentoId { get; set; }
        [Required]
        public string UsuarioId { get; set; }
        public string RolDepartamento { get; set; } = "Colaborador";
    }
}
