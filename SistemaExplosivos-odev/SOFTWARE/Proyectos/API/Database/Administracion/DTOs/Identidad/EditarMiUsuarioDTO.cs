namespace API.Database.Administracion.DTOs.Identidad
{
    public class EditarMiUsuarioDTO
    {
        public string IdUsuario { get; set; }
        public string Username { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string NombreCompleto { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }


    }
}
