using System.ComponentModel.DataAnnotations;

namespace API.Database.Core.DTOs.Administracion
{
    public class CrearUsuarioDTO
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellidos { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public int? DepartamentoId { get; set; }
    }
}
