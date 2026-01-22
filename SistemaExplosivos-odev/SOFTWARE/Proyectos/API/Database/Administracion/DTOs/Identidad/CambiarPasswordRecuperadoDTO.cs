namespace API.Database.Administracion.DTOs.Identidad
{
    public class CambiarPasswordRecuperadoDTO
    {
        public string Password { get; set; }

        public string ConfirmarPassword { get; set; }

        public string Codigo { get; set; }
    }
}
