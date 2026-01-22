namespace API.Database.Recepcion.DTOs.Transportistas
{
    public class ModificarTransportistaRecepcion
    {
        public int Id { get; set; }

        public int IdProveedor { get; set; }

        public string Nombre { get; set; }

        public string Apodo { get; set; }
    }
}
