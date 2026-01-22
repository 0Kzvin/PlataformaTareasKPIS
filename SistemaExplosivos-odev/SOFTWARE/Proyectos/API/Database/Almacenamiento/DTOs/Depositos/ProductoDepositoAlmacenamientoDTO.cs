using System;

namespace API.Database.Almacenamiento.DTOs.Depositos
{
    public class ProductoDepositoAlmacenamientoDTO
    {
        public string IdUnico { get; set; }

        public string Nombre { get; set; }

        public string CodigoColor { get; set; }

        public bool Estado { get; set; }

        public bool Borrado { get; set; }
    }
}
