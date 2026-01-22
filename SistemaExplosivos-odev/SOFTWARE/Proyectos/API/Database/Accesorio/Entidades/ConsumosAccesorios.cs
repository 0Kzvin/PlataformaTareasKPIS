using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Accesorio.Entidades
{
    public class ConsumosAccesorios
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string IdUnico { get; set; }

        public string Folio { get; set; }

        public string Destino { get; set; }

        public string Observaciones { get; set; }

        public string FirmaReciboHanka { get; set; }

        public string FirmaJefe { get; set; }

        public string FirmaAlmacen { get; set; }

        public bool EstaCerrado { get; set; }

        public DateTime FechaHora { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Borrado { get; set; }

        public virtual ICollection<SalidasAccesorios> Salidas { get; set; } = new List<SalidasAccesorios>();
    }
}
