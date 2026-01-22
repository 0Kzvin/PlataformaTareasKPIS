using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Recepcion.Entidades
{
    public class ProveedoresRecepcion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string IdUnico { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string Apodo { get; set; }

        public string RFC { get; set; }

        public string Foto { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal Tolerancia { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Estado { get; set; }

        public bool Borrado { get; set; }

        public virtual ICollection<TransportistasRecepcion> Transportistas { get; set; }
    }
}
