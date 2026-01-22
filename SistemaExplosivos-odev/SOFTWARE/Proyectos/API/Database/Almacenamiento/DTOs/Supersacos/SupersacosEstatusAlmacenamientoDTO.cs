using API.Database.Almacenamiento.DTOs.Supersacos.Grafico;
using System.Collections.Generic;

namespace API.Database.Almacenamiento.DTOs.Supersacos
{
    public class SupersacosEstatusAlmacenamientoDTO
    {
        public List<MovimientosSupersacosAlmacenamientoDTO> Entradas { get; set; }

        public List<MovimientosSupersacosAlmacenamientoDTO> Salidas { get; set; }

        public SupersacosGraficoDTO PeriodoActual { get; set; }

        public SupersacosGraficoDTO PeriodoAnterior { get; set; }
    }
}
