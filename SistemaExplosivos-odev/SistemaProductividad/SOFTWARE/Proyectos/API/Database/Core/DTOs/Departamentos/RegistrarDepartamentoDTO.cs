using System.ComponentModel.DataAnnotations;

namespace API.Database.Core.DTOs.Departamentos
{
    public class RegistrarDepartamentoDTO
    {
        [Required]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string LiderId { get; set; }
    }
}
