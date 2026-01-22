using System;

namespace API.Database.Almacenamiento.DTOs.Supersacos.Grafico
{
    public class SupersacosMovimientosGraficaDTO
    {
        public string NombreTiempo { get; set; }

        public DateTime FechaHora { get; set; }

        public decimal ValorMovimiento { get; set; }
    }
}
