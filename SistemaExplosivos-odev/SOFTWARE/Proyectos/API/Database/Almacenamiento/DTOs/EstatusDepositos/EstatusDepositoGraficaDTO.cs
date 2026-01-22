using System;

namespace API.Database.Almacenamiento.DTOs.EstatusDepositos
{
    public class EstatusDepositoGraficaDTO
    {
        public string IdUnico { get; set; }

        public string Deposito { get; set; }

        public string Producto { get; set; }

        public string Ubicacion { get; set; }

        public decimal Altura { get; set; }

        public decimal Volumen { get; set; }

        public decimal PorcentajeNivel { get; set; }

        public decimal LimiteAlto { get; set; }

        public decimal LimiteMaximo { get; set; }

        public decimal LimiteBajo { get; set; }

        public decimal LimiteMinimo { get; set; }

        public bool HayAlarma { get; set; }

        public string DispositivoDeMedicion { get; set; }

        public DateTime? FechaHora { get; set; }
    }
}
