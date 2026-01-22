using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Recepcion.Entidades
{
    public class Recepciones
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string IdUnico { get; set; }

        public string Folio { get; set; }

        //DATOS RECEPCION

        public string NumeroPedido { get; set; }

        public string OrdenEmbarque { get; set; }

        public string Producto { get; set; }

        public string DMEntrada { get; set; }

        public DateTime FechaHoraSalida { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal DiferenciaEntreFechas { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal PesoBruto { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal PesoTara { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal PesoNeto { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal Diferencia { get; set; }
        
        public DateTime FechaHoraRecepcion { get; set; }

        public string Ubicacion { get; set; }

        //DATOS EQUIPO
        public string Transportista { get; set; }

        public string Placas { get; set; }

        public string Chofer { get; set; }

        public string Proveedor { get; set; }

        public string RFCProveedor { get; set; }

        //DATOS FACTURA
        public string NumeroFactura { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal CantidadFactura { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal PrecioUnitario { get; set; }

        public string NumeroBoletaFactura { get; set; }

        public DateTime FechaHoraFactura { get; set; }

        public bool Borrado { get; set; }

        public virtual ICollection<TransaccionesRecepcion> TransaccionesRecepcion { get; set; }
    }
}
