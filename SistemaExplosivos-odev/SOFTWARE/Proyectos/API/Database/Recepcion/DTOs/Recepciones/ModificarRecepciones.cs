using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Recepcion.DTOs.Recepciones
{
    public class ModificarRecepciones
    {
        public int Id { get; set; }

        public string Folio { get; set; }

        //DATOS RECEPCION
        public string NumeroPedido { get; set; }

        public string OrdenEmbarque { get; set; }

        public string Producto { get; set; }

        public string DMEntrada { get; set; }

        public DateTime FechaHoraSalida { get; set; }

        public decimal DiferenciaEntreFechas { get; set; }

        public decimal PesoBruto { get; set; }

        public decimal PesoTara { get; set; }

        public decimal PesoNeto { get; set; }

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

        public decimal CantidadFactura { get; set; }

        public decimal PrecioUnitario { get; set; }

        public string NumeroBoletaFactura { get; set; }

        public DateTime FechaHoraFactura { get; set; }
    }
}
