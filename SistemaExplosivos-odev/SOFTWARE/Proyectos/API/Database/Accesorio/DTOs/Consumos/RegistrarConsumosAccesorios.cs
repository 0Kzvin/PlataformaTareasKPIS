using API.Database.Accesorio.DTOs.Consumos.Salidas;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Database.Accesorio.DTOs.Consumos
{
    public class RegistrarConsumosAccesorios
    {
        [Required(ErrorMessage = "Folio es obligatorio")]
        public string Folio { get; set; }

        public string Destino { get; set; }

        public string Observaciones { get; set; }

        public string FirmaReciboHanka { get; set; }

        public string FirmaJefe { get; set; }

        public string FirmaAlmacen { get; set; }

        public bool EstaCerrado { get; set; }

        public DateTime FechaHora { get; set; }

        public List<RegistrarSalidaAccesorios> Salidas { get; set; } = new();
    }
}
