using API.Database.Almacenamiento.Entidades;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Almacenamiento.DTOs.Cargas
{
    public class CargasDepositoAlmacenamientoDTO
    {
        public int Id { get; set; }

        public string IdUnico { get; set; }

        public string Deposito { get; set; }

        public decimal CapacidadMaximaDeposito { get; set; }
        
        public decimal LimiteMaximo { get; set; }
        
        public decimal LimiteAlto { get; set; }
        
        public decimal LimiteBajo { get; set; }

        public decimal LimiteMinimo { get; set; }

        public string Producto { get; set; }
        
        public string ColorProducto { get; set; }

        public string? NumeroEconomico { get; set; }

        public string? Observaciones { get; set; }

        public string Ubicacion { get; set; }

        public decimal NivelInicial { get; set; }

        public decimal NivelFinal { get; set; }

        public decimal PorcentajeFinal { get; set; }

        public decimal PorcentajeInicial { get; set; }

        public decimal VolumenInicial { get; set; }

        public decimal VolumenFinal { get; set; }

        public decimal VolumenCargado { get; set; }

        public decimal VolumenCargadoReal { get; set; }

        public DateTime FechaHoraInicial { get; set; }

        public DateTime FechaHoraFinal { get; set; }
    }
}
