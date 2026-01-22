using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Almacenamiento.Entidades
{
    public class MovimientosSupersacosAlmacenamiento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string IdUnico { get; set; }

        public string Ubicacion { get; set; }

        public string IdProducto { get; set; }

        public string Observaciones { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal CantidadInicial { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal CantidadFinal { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal CantidadMovimiento { get; set; }

        public DateTime FechaHora { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Borrado { get; set; }
    }
}
