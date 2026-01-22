using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Recepcion.Entidades
{
    public class ConductoresRecepcion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string IdUnico { get; set; }

        [Required]
        public string Nombre { get; set; }

        public bool Estado { get; set; }

        public bool Borrado { get; set; }
    }
}
