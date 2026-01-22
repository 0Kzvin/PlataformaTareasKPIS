using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Recepcion.Entidades
{
    public class TransportistasRecepcion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string IdUnico { get; set; }

        [ForeignKey(nameof(Proveedor))]
        public int IdProveedor { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string Apodo { get; set; }

        public bool Estado { get; set; }

        public bool Borrado { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaModificacion { get; set; }

        public virtual ProveedoresRecepcion Proveedor { get; set; }

        public virtual ICollection<EquiposRecepcion> Equipos { get; set; }
    }
}
