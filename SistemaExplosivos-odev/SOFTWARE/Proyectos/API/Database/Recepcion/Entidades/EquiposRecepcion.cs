using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Recepcion.Entidades
{
    public class EquiposRecepcion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string IdUnico { get; set; }

        [ForeignKey(nameof(Transportista))]
        public int IdTransportista { get; set; }

        [Required]
        public string Placa { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Estado { get; set; }

        public bool Borrado { get; set; }

        public virtual TransportistasRecepcion Transportista { get; set; }
    }
}
