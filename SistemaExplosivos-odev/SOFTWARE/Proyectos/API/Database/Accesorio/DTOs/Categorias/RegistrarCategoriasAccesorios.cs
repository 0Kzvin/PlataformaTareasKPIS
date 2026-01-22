using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Accesorio.DTOs.Categorias
{
    public class RegistrarCategoriasAccesorios
    {
        [Required(ErrorMessage = "Nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "UnidadDeMedida es obligatorio")]
        public string UnidadDeMedida { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El valor debe ser mayor o igual a 0")]
        public decimal LimiteCapacidadMaximaSedena { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El valor debe ser mayor o igual a 0")]
        public decimal LimiteConsumoMaximaSedena { get; set; }
    }
}
