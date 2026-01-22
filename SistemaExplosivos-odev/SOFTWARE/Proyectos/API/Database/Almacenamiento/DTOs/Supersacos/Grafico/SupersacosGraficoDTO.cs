using System;
using System.Collections.Generic;

namespace API.Database.Almacenamiento.DTOs.Supersacos.Grafico
{
    public class SupersacosGraficoDTO
    {
        public string NombrePeriodo { get; set; }

        public DateTime FechaInicial { get; set; }

        public DateTime FechaFinal { get; set; }

        public decimal SumaPeriodo { get; set; }

        public string ColorProducto { get; set; }

        public List<SupersacosMovimientosGraficaDTO> MovimientosPeriodo { get; set; }
    }
}
