using API.Database.Almacenamiento.DTOs.Depositos;
using API.Database.Almacenamiento.Entidades;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Almacenamiento.DTOs.Supersacos
{
    public class MovimientosSupersacosAlmacenamientoDTO
    {
        public int Id { get; set; }

        public string IdUnico { get; set; }

        public string Ubicacion { get; set; }

        public string ColorProducto { get; set; }

        public string Producto { get; set; }

        public string Observaciones { get; set; }

        public decimal CantidadInicial { get; set; }

        public decimal CantidadFinal { get; set; }

        public decimal CantidadMovimiento { get; set; }

        public DateTime FechaHora { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaModificacion { get; set; }

        public List<MovimientosSupersacosAlmacenamientoDTO> Movimientos { get; set; }

        public ProductoDepositoAlmacenamientoDTO ProductoDTO { get; set; }
    }
}
