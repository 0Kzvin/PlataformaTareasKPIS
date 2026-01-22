using API.Database.Gerencia.Entidades;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Almacenamiento.Entidades
{
    /// <summary>
    /// Representa un depósito o tanque de almacenamiento dentro del sistema.
    /// Contiene información sobre su capacidad, límites de operación y estado actual.
    /// </summary>
    public class DepositosAlmacenamiento
    {
        /// <summary>
        /// Identificador interno único del depósito. Es una clave primaria con incremento automático.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Identificador único asignado al depósito para su referencia externa.
        /// </summary>
        [Required]
        public string IdUnico { get; set; }

        /// <summary>
        /// Nombre descriptivo del depósito de almacenamiento.
        /// </summary>
        [Required]
        public string Nombre { get; set; }

        /// <summary>
        /// Nombre corto o apodo utilizado para identificar el depósito de manera informal.
        /// </summary>
        public string Apodo { get; set; }

        /// <summary>
        /// Id Único del producto en la db de gerencia almacenado en el depósito.
        /// </summary>
        public string IdProducto { get; set; }

        /// <summary>
        /// Capacidad máxima del depósito en unidades del producto
        /// </summary>
        [Column(TypeName = "decimal(30,2)")]
        public decimal CapacidadMaxima { get; set; }

        /// <summary>
        /// Altura máxima física del depósito, generalmente expresada en metros.
        /// </summary>
        [Column(TypeName = "decimal(30,2)")]
        public decimal AlturaMaxima { get; set; }

        /// <summary>
        /// Capacidad operativa máxima recomendada para el uso seguro del depósito.
        /// </summary>
        [Column(TypeName = "decimal(30,2)")]
        public decimal CapacidadOperativa { get; set; }

        /// <summary>
        /// Altura operativa asociada a la capacidad operativa máxima del depósito.
        /// </summary>
        [Column(TypeName = "decimal(30,2)")]
        public decimal AlturaOperativa { get; set; }

        /// <summary>
        /// Nivel superior de advertencia antes de alcanzar la capacidad máxima.
        /// </summary>
        [Column(TypeName = "decimal(30,2)")]
        public decimal LimiteAlto { get; set; }

        /// <summary>
        /// Nivel máximo permitido del depósito antes de considerarse sobrellenado.
        /// </summary>
        [Column(TypeName = "decimal(30,2)")]
        public decimal LimiteMaximo { get; set; }

        /// <summary>
        /// Nivel inferior de advertencia antes de llegar al mínimo operativo.
        /// </summary>
        [Column(TypeName = "decimal(30,2)")]
        public decimal LimiteBajo { get; set; }

        /// <summary>
        /// Nivel mínimo permitido antes de que el depósito se considere vacío o en condición crítica.
        /// </summary>
        [Column(TypeName = "decimal(30,2)")]
        public decimal LimiteMinimo { get; set; }

        /// <summary>
        /// Ubicación física o descripción del sitio donde se encuentra el depósito.
        /// </summary>
        [Required]
        public string Ubicacion { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaModificacion { get; set; }

        /// <summary>
        /// Indica si el depósito se encuentra activo (true) o inactivo (false) en el sistema.
        /// </summary>
        public bool Estado { get; set; }

        /// <summary>
        /// Indica si el registro del depósito fue marcado como eliminado lógicamente.
        /// </summary>
        public bool Borrado { get; set; }
    }
}
