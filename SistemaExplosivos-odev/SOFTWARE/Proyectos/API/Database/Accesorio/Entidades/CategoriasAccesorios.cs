using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Accesorio.Entidades
{
    public class CategoriasAccesorios
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string IdUnico { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string UnidadDeMedida { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal LimiteCapacidadMaximaSedena { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal LimiteConsumoMaximaSedena { get; set; }

        public bool Estado { get; set; }

        public bool Borrado { get; set; }
    }
}
