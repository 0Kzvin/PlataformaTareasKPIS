using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Almacenamiento.Entidades
{
    /// <summary>
    /// Representa el registro de un consumo o despacho de material desde un depósito de almacenamiento.
    /// Contiene la información de las condiciones iniciales y finales del proceso, así como los datos del producto y la ubicación.
    /// </summary>
    public class CargasDepositosAlmacenamiento
    {
        /// <summary>
        /// Identificador interno único del consumo registrado.
        /// Es una clave primaria con incremento automático.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Identificador único asociado al depósito de almacenamiento al que pertenece este registro de consumo.
        /// Puede ser un GUID o un código interno que relacione con la entidad principal <see cref="DepositosAlmacenamiento"/>.
        /// </summary>
        [Required]
        public string IdUnico { get; set; }

        /// <summary>
        /// Nombre descriptivo del depósito desde el cual se realizó el consumo o despacho.
        /// </summary>
        public string Deposito { get; set; }

        /// <summary>
        /// Nombre o tipo del producto almacenado o despachado.
        /// </summary>
        public string Producto { get; set; }

        /// <summary>
        /// Número económico o identificador del equipo, vehículo o proceso que realizó el consumo.
        /// </summary>
        public string? NumeroEconomico { get; set; }

        /// <summary>
        /// Comentarios adicionales o notas relacionadas con el consumo o la operación.
        /// </summary>
        public string? Observaciones { get; set; }

        /// <summary>
        /// Ubicación física o referencia geográfica del depósito donde ocurrió el consumo.
        /// </summary>
        public string Ubicacion { get; set; }


        [Column(TypeName = "decimal(30,2)")]
        public decimal NivelInicial { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal NivelFinal { get; set; }


        [Column(TypeName = "decimal(30,2)")]
        public decimal VolumenInicial { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal VolumenFinal { get; set; }


        [Column(TypeName = "decimal(30,2)")]
        public decimal VolumenCargado { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal VolumenCargadoReal { get; set; }

        /// <summary>
        /// Fecha y hora en que inició el proceso de despacho o consumo.
        /// </summary>
        public DateTime FechaHoraInicial { get; set; }

        /// <summary>
        /// Fecha y hora en que finalizó el proceso de despacho o consumo.
        /// </summary>
        public DateTime FechaHoraFinal { get; set; }
    }
}
