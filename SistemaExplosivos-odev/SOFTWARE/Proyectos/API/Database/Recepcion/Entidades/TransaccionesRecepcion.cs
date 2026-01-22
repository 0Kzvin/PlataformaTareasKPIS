using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Recepcion.Entidades
{
    public class TransaccionesRecepcion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string IdUnico { get; set; }

        [ForeignKey(nameof(Recepcion))]
        public int IdRecepcion { get; set; }

        public string Ubicacion { get; set; }

        public string Producto { get; set; }

        public string NumeroFactura { get; set; }

        public string Folio { get; set; }

        public string Remision { get; set; }

        public string Observaciones { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal KilosTransaccion { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal KilosFactura { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal PesoBruto { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal PesoTara { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal PesoNeto { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal Diferencia { get; set; }
        
        [Column(TypeName = "decimal(30,2)")]        
        public decimal PrecioUnitario { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal DiferenciaPesoContraFacturado { get; set; }

        public string Remanente { get; set; }

        [Column(TypeName = "decimal(30,2)")]        
        public decimal Tolerancia { get; set; }

        public DateTime FechaHora { get; set; }

        public virtual Recepciones Recepcion { get; set; }
    }
}
