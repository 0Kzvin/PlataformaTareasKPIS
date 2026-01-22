using System.ComponentModel.DataAnnotations;

namespace API.Database.Core.DTOs.Departamentos
{
    public class EditarDepartamentoDTO
    {
        [Required]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string LiderId { get; set; }
        public bool? Activo { get; set; }
    }
}
