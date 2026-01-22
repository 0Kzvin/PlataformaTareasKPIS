using API.Database.Administracion.DTOs.Modulos;
using System.Collections.Generic;

namespace API.Database.Administracion.DTOs.Identidad
{
    public class RolDTO
    {
        public string Id { get; set; }
        public string NombreRol { get; set; }
        public string NombreRolNormalizado { get; set; }
        public string Descripcion { get; set; }
        public int NumeroPermisos { get; set; }
        public List<ModuloDTO> ModulosActivados { get; set; } = new List<ModuloDTO>();
        public bool Estado { get; set; }
    }
}
