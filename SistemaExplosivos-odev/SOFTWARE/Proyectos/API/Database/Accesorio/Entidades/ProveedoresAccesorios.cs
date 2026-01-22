using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Accesorio.Entidades
{
    public class ProveedoresAccesorios
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

        public bool Estado { get; set; }

        public bool Borrado { get; set; }
    }
}
