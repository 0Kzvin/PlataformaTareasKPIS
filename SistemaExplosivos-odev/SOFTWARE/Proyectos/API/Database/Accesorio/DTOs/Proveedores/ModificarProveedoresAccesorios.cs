using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Accesorio.DTOs.Proveedores
{
    public class ModificarProveedoresAccesorios
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Apodo { get; set; }

        public string RFC { get; set; }

        public string Foto { get; set; }

        public decimal Tolerancia { get; set; }
    }
}
