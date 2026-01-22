using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Accesorio.Entidades
{
    public class EntradasAccesorios
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string IdUnico { get; set; }

        public string NumeroStock { get; set; }

        public string Descripcion { get; set; }

        public string UnidadDeMedida { get; set; }

        public string NumeroPedido { get; set; }

        public string DMEntrada { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal CantidadInicial { get; set; }
        
        [Column(TypeName = "decimal(30,2)")]
        public decimal CantidadFinal { get; set; }

        public string Transportista { get; set; }

        public string Placas { get; set; }

        public string Chofer { get; set; }

        public string Proveedor { get; set; }

        public string NumeroFactura { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal CantidadFacturada { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal PrecioUnitario { get; set; }

        public string Observaciones { get; set; }

        public DateTime FechaHoraSalida { get; set; }

        public DateTime FechaHoraFactura { get; set; }

        public DateTime FechaHoraRecepcion { get; set; }
    }
}
