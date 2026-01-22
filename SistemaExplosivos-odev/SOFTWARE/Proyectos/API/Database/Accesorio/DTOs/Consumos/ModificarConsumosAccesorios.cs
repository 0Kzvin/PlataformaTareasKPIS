using API.Database.Accesorio.DTOs.Consumos.Salidas;
using System;
using System.Collections.Generic;

namespace API.Database.Accesorio.DTOs.Consumos
{
    public class ModificarConsumosAccesorios
    {
        public int Id { get; set; }

        public string Folio { get; set; }

        public string Destino { get; set; }

        public string Observaciones { get; set; }

        public string FirmaReciboHanka { get; set; }

        public string FirmaJefe { get; set; }

        public string FirmaAlmacen { get; set; }

        public bool EstaCerrado { get; set; }

        public DateTime FechaHora { get; set; }

        public List<ModificarSalidaAccesorios> Salidas { get; set; } = new();
    }
}
