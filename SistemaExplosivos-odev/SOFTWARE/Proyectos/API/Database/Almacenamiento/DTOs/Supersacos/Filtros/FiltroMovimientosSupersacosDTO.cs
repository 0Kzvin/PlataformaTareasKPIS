using System;

namespace API.Database.Almacenamiento.DTOs.Supersacos.Filtros
{
    public class FiltroMovimientosSupersacosDTO
    {
        public string? IdProducto { get; set; }

        public DateTime? FechaInicial { get; set; }

        public DateTime? FechaFinal { get; set; }

        public bool AgruparPorDia { get; set; }
    }
}
