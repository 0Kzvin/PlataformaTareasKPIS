using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace API.Database.Accesorio.Entidades
{
    public class ProductosAccesorios
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string IdUnico { get; set; }

        [ForeignKey(nameof(Categoria))]
        public int IdCategoria { get; set; }

        [ForeignKey(nameof(Proveedor))]
        public int IdProveedor { get; set; }

        public int? IdConjunto { get; set; }

        public string NumeroStock { get; set; }

        public string Nombre { get; set; }

        public string Apodo { get; set; }

        public string UnidadDeMedida { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal LimiteMaximo { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal LimiteAlto { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal LimiteBajo { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal LimiteMinimo { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal LimiteConsumoMaximo { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal LimiteConsumoAlto { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal CantidadActual { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal ConsumoPromedioDiario { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal ConsumoPromedioDiarioCalculado { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal FactorCorreccionKg { get; set; }

        public bool Estado { get; set; }

        public bool Borrado { get; set; }

        [ForeignKey(nameof(IdConjunto))]
        public virtual ProductosAccesorios Conjunto { get; set; }

        public virtual ICollection<ProductosAccesorios> AccesoriosAsociados { get; set; }

        public virtual CategoriasAccesorios Categoria { get; set; }

        public virtual ProveedoresAccesorios Proveedor { get; set; }
    }
}
