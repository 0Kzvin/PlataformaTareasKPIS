using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Almacenamiento.DTOs.Equipos
{
    public class RegistrarEquiposAlmacenamiento
    {
        public string NumeroEconomico { get; set; }

        public string Apodo { get; set; }

        public string IdProducto { get; set; }

        public decimal CantidadActual { get; set; }

        public decimal Capacidad { get; set; }

        public bool EsExterno { get; set; }
    }
}
