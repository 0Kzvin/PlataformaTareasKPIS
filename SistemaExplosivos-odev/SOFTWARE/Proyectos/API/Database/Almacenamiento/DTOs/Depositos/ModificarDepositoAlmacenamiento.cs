namespace API.Database.Almacenamiento.DTOs.Depositos
{
    public class ModificarDepositoAlmacenamiento
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Apodo { get; set; }

        public string IdProducto { get; set; }

        public decimal CapacidadMaxima { get; set; }

        public decimal AlturaMaxima { get; set; }

        public decimal CapacidadOperativa { get; set; }

        public decimal AlturaOperativa { get; set; }

        public decimal LimiteAlto { get; set; }

        public decimal LimiteMaximo { get; set; }

        public decimal LimiteBajo { get; set; }

        public decimal LimiteMinimo { get; set; }

        public string Ubicacion { get; set; }
    }
}
