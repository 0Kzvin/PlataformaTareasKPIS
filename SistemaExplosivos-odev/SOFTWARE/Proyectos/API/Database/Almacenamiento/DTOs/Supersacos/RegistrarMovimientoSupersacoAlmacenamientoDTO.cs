using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Almacenamiento.DTOs.Supersacos
{
    public class RegistrarMovimientoSupersacoAlmacenamientoDTO
    {
        public string IdProducto { get; set; }

        public string Ubicacion { get; set; }

        public string Observaciones { get; set; }

        public decimal CantidadInicial { get; set; }

        public decimal CantidadFinal { get; set; }

        public decimal CantidadMovimiento { get; set; }

        public string FechaHora { get; set; }
    }
}
