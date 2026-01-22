using System;

namespace API.Database.Gerencia.DTOs.Productos
{
    public class ProductosGerenciaDTO
    {
        public int Id { get; set; }

        public string IdUnico { get; set; }

        public string Nombre { get; set; }

        public string Apodo { get; set; }

        public string CodigoColor { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Estado { get; set; }

        public bool Borrado { get; set; }
    }
}
