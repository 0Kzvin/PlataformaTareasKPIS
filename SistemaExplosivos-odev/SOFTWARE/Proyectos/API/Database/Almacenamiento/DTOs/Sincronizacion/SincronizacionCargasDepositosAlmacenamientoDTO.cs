using System;

namespace API.Database.Almacenamiento.DTOs.Sincronizacion
{
    public class SincronizacionCargasDepositosAlmacenamientoDTO
    {
        public string IdUnico { get; set; }
        public string Deposito { get; set; }
        public string Producto { get; set; }
        public string Ubicacion { get; set; }
        public decimal NivelInicial { get; set; }
        public decimal NivelFinal { get; set; }
        public decimal VolumenInicial { get; set; }
        public decimal VolumenFinal { get; set; }
        public decimal VolumenCargado { get; set; }
        public string Tipo { get; set; }
        public DateTime FechaHoraInicial { get; set; }
        public DateTime FechaHoraFinal { get; set; }
    }
}
