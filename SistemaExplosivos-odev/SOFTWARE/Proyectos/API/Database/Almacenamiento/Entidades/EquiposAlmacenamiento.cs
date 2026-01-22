using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Almacenamiento.Entidades
{
    public class EquiposAlmacenamiento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string IdUnico { get; set; }

        public string NumeroEconomico { get; set; }

        public string Apodo { get; set; }

        public string IdProducto { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal CantidadActual { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal Capacidad { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaModificacion { get; set; }

        /// <summary>
        /// Esta propiedad se requiere porque hay equipos que no son parte de mina
        /// </summary>
        public bool EsExterno { get; set; }

        public bool Estado { get; set; }

        public bool Borrado { get; set; }
    }
}
