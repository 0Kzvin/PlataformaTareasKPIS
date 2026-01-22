using System;

namespace API.Database.Almacenamiento.DTOs.Supersacos
{
    public class ModificarMovimientoSupersacosAlmacenamientoDTO
    {
        public int Id { get; set; }

        public string IdProducto { get; set; }

        public string Ubicacion { get; set; }

        public string Observaciones { get; set; }

        public decimal CantidadInicial { get; set; }

        public decimal CantidadFinal { get; set; }

        public decimal CantidadMovimiento { get; set; }

        public DateTime FechaHora { get; set; }
    }
}
