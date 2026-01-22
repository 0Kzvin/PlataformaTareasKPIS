using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Almacenamiento.DTOs.Depositos
{
    public class DepositosAlmacenamientoDTO
    {
        public int Id { get; set; }

        public string IdUnico { get; set; }

        public string Nombre { get; set; }

        public string Apodo { get; set; }

        public string Producto { get; set; }

        public string ColorProducto { get; set; }

        public decimal CapacidadMaxima { get; set; }

        public decimal AlturaMaxima { get; set; }

        public decimal CapacidadOperativa { get; set; }

        public decimal AlturaOperativa { get; set; }

        public decimal LimiteAlto { get; set; }

        public decimal LimiteMaximo { get; set; }

        public decimal LimiteBajo { get; set; }

        public decimal LimiteMinimo { get; set; }

        public string Ubicacion { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaModificacion { get; set; }

        public ProductoDepositoAlmacenamientoDTO ProductoDTO { get; set; }

        public bool Estado { get; set; }

        public bool Borrado { get; set; }
    }
}
