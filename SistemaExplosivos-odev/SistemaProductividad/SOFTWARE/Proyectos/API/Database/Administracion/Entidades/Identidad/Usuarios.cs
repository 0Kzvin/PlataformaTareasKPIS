using Microsoft.AspNetCore.Identity;

namespace API.Database.Administracion.Entidades.Identidad
{
    public class Usuarios : IdentityUser
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string NombreCompleto => $"{Nombre} {Apellidos}";
        public string Foto { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        public bool Estado { get; set; } = true;

        // Multi-Department logic
        public int? DepartamentoId { get; set; }
        
        // Navigation properties
        public virtual API.Database.Core.Entidades.Departamentos Departamento { get; set; }
        public virtual ICollection<API.Database.Core.Entidades.DepartamentoUsuario> Departamentos { get; set; }
    }
}
