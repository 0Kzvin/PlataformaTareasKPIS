using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Accesorio.Entidades
{
    public class FacturasAccesorios
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string IdUnico { get; set; }

        public string Ubicacion { get; set; }

        public string Producto { get; set; }

        public string NumeroFactura { get; set; }

        public string Folio { get; set; }

        public string Remision { get; set; }

        public string Proveedor { get; set; }

        public string Transportista { get; set; }

        public string Equipo { get; set; }

        public string Conductor { get; set; }

        public string Origen { get; set; }

        public string Observaciones { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal CantidadFacturada { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal PrecioUnitario { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal DiferenciaPesoContraFacturado { get; set; }

        public string Remanente { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal Tolerancia { get; set; }

        public DateTime FechaHora { get; set; }
    }
}
