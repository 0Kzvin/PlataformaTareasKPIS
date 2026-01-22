using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Recepcion.DTOs.Proveedores
{
    public class RegistrarProveedorRecepcion
    {
        public string Nombre { get; set; }

        public string Apodo { get; set; }

        public string RFC { get; set; }

        public string Foto { get; set; }

        public decimal Tolerancia { get; set; }
    }
}
