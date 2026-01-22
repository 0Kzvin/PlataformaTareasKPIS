namespace API.Hubs.DTOs
{
    public class EnvioDatosSignalRDTO
    {
        public string UserId { get; set; }
        public object Datos { get; set; } = new object();
    }
}
