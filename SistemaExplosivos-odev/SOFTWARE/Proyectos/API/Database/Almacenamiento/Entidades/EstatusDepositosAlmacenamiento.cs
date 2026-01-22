using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Almacenamiento.Entidades
{
    /// <summary>
    /// Representa el estado actual de un depósito de almacenamiento sólido o líquido.
    /// Incluye información sobre el material contenido, su nivel, masa y límites operativos.
    /// </summary>
    public class EstatusDepositosAlmacenamiento
    {
        /// <summary>
        /// Identificador interno único del estatus del depósito.
        /// Es una clave primaria con incremento automático.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Identificador único asociado al depósito al que pertenece este estatus.
        /// Puede ser un GUID o un código interno que relacione con la entidad principal <see cref="DepositosAlmacenamiento"/>.
        /// </summary>
        [Required]
        public string IdUnico { get; set; }

        /// <summary>
        /// Nombre descriptivo del depósito al que corresponde este estatus.
        /// </summary>
        public string Deposito { get; set; }

        /// <summary>
        /// Nombre o tipo del producto actualmente contenido en el depósito.
        /// </summary>
        public string Producto { get; set; }

        /// <summary>
        /// Ubicación física o referencia geográfica donde se encuentra el depósito.
        /// </summary>
        public string Ubicacion { get; set; }

        /// <summary>
        /// Altura actual del material dentro del depósito, normalmente expresada en metros (m).
        /// </summary>
        [Column(TypeName = "decimal(30,2)")]
        public decimal Altura { get; set; }

        /// <summary>
        /// Masa actual del material almacenado en el depósito, expresada en kilogramos (kg).
        /// </summary>
        [Column(TypeName = "decimal(30,2)")]
        public decimal Volumen { get; set; }

        /// <summary>
        /// Porcentaje actual de llenado del depósito respecto a su capacidad total (%).
        /// </summary>
        [Column(TypeName = "decimal(30,2)")]
        public decimal PorcentajeNivel { get; set; }

        /// <summary>
        /// Nivel máximo permitido antes de alcanzar el límite de seguridad o sobrellenado del depósito.
        /// </summary>
        [Column(TypeName = "decimal(30,2)")]
        public decimal LimiteMaximo { get; set; }

        /// <summary>
        /// Nivel alto de advertencia previo al máximo permitido, usado para control preventivo de sobrellenado.
        /// </summary>
        [Column(TypeName = "decimal(30,2)")]
        public decimal LimiteAlto { get; set; }

        /// <summary>
        /// Nivel bajo de advertencia previo al límite mínimo operativo.
        /// </summary>
        [Column(TypeName = "decimal(30,2)")]
        public decimal LimiteBajo { get; set; }

        /// <summary>
        /// Nivel mínimo permitido antes de que el depósito se considere vacío o en condición crítica.
        /// </summary>
        [Column(TypeName = "decimal(30,2)")]
        public decimal LimiteMinimo { get; set; }

        public bool HayAlarma { get; set; }

        /// <summary>
        /// Sirve para identificar el dispositivo o sistema de medición utilizado para obtener los datos del depósito.
        /// </summary>
        public string DispositivoDeMedicion { get; set; }
        
        public DateTime FechaHora { get; set; }
    }
}
