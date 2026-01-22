using System;

namespace API.Database.Administracion.Entidades.Identidad
{
    public class UsuariosRecuperacion
    {
        public int Id { get; set; }

        public string IdUsuario { get; set; }

        public string CodigoRecuperacion { get; set; }

        public string MetodoRecuperacion { get; set; }

        public DateTime FechaDeCreacion { get; set; }

        public DateTime FechaDeExpiracion { get; set; }

        public bool CodigoUsado { get; set; }

        public DateTime? FechaDeUtilizacion { get; set; }
    }
}
