namespace API.Database.Administracion.DTOs.Identidad
{
    public class CambiarPasswordDTO
    {
        public string Id { get; set; }
        public string PasswordActual { get; set; }
        public string PasswordNuevo { get; set; }
    }
}
