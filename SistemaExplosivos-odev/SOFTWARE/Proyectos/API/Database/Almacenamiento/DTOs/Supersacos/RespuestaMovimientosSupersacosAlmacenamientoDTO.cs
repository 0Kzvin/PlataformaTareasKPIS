using iTextSharp.text;
using System;
using System.Collections.Generic;

namespace API.Database.Almacenamiento.DTOs.Supersacos
{
    public class RespuestaMovimientosSupersacosAlmacenamientoDTO
    {
        public decimal InventarioInicial { get; set; }

        public decimal Entradas { get; set; }

        public decimal Salidas { get; set; }

        public decimal InventarioFinal { get; set; }

        public DateTime FechaInicial { get; set; }

        public DateTime FechaFinal { get; set; }

        public List<MovimientosSupersacosAlmacenamientoDTO> Movimientos { get; set; }
    }
}
