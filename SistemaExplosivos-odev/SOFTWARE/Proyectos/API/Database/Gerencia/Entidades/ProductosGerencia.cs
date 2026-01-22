using API.Database.Almacenamiento.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Gerencia.Entidades
{
    public class ProductosGerencia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string IdUnico { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string Apodo { get; set; }

        [Required]
        public string CodigoColor { get; set; }

        public bool Estado { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Borrado { get; set; }
    }
}
