namespace API.Database.Core.DTOs.Administracion
{
    public class UsuarioResumenDTO
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public bool Estado { get; set; }
        public string DepartamentoPrincipal { get; set; }
    }
}
