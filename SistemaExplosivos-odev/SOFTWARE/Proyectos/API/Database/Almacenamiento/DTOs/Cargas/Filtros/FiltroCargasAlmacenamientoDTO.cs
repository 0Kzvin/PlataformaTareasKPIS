using System;

namespace API.Database.Almacenamiento.DTOs.Cargas.Filtros
{
    public class FiltroCargasAlmacenamientoDTO
    {
        public bool? TraerCargas { get; set; }

        public bool? TraerDescargas { get; set; }

        public string? Producto { get; set; }

        public string? Deposito { get; set; }

        public string? NumeroEconomico { get; set; }

        public DateTime? FechaInicial { get; set; }

        public DateTime? FechaFinal { get; set; }
    }
}
