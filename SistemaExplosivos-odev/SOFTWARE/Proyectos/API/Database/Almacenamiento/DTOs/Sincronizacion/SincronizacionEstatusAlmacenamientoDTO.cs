using System;

namespace API.Database.Almacenamiento.DTOs.Sincronizacion
{
    public class SincronizacionEstatusAlmacenamientoDTO
    {
        public string IdUnico { get; set; }
        public string Deposito { get; set; }
        public string Producto { get; set; }
        public string Ubicacion { get; set; }
        public decimal Altura { get; set; }
        public decimal Volumen { get; set; }
        public decimal PorcentajeNivel { get; set; }
        public DateTime FechaHora { get; set; }
    }
}
