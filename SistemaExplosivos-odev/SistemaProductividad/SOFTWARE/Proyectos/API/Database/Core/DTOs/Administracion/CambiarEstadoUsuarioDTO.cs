using System.ComponentModel.DataAnnotations;

namespace API.Database.Core.DTOs.Administracion
{
    public class CambiarEstadoUsuarioDTO
    {
        [Required]
        public string Id { get; set; }
        public bool Estado { get; set; }
    }
}
