namespace API.Database.Almacenamiento.DTOs.Equipos
{
    public class ModificarEquiposAlmacenamiento
    {
        public int Id { get; set; }

        public string NumeroEconomico { get; set; }

        public string Apodo { get; set; }

        public string IdProducto { get; set; }

        public decimal CantidadActual { get; set; }

        public decimal Capacidad { get; set; }

        public bool EsExterno { get; set; }
    }
}
