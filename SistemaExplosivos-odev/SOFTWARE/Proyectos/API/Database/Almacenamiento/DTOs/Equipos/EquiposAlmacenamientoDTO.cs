using API.Database.Almacenamiento.DTOs.Depositos;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Almacenamiento.DTOs.Equipos
{
    public class EquiposAlmacenamientoDTO
    {
        public int Id { get; set; }
        
        public string IdUnico { get; set; }

        public string NumeroEconomico { get; set; }

        public string Apodo { get; set; }

        public string Producto { get; set; }

        public string ColorProducto { get; set; }

        public decimal CantidadActual { get; set; }

        public decimal Capacidad { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool EsExterno { get; set; }

        public ProductoDepositoAlmacenamientoDTO ProductoDTO { get; set; }

        public bool Estado { get; set; }

        public bool Borrado { get; set; }
    }
}
