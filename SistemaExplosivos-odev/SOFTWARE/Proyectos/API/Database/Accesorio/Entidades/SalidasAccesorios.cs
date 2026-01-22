using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Accesorio.Entidades
{
    public class SalidasAccesorios
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string IdUnico { get; set; }

        [ForeignKey(nameof(Consumo))]
        public int IdConsumo { get; set; }

        public string NumeroStock { get; set; }

        public int NumeroSalida { get; set; }

        public string Nombre { get; set; }

        public string UnidadDeMedida { get; set; }
        
        [Column(TypeName = "decimal(30,2)")]
        public decimal CantidadASacar { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal CantidadInicial { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal CantidadFinal { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal FactorCorrecion { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal CantidadCorregida { get; set; }

        public bool EsDevolucion { get; set; }

        public virtual ConsumosAccesorios Consumo { get; set; }
    }
}
