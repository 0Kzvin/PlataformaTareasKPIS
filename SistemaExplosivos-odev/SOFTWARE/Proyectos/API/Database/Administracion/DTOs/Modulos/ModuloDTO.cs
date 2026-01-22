using API.Database.Administracion.DTOs.Identidad;
using System.Collections.Generic;

namespace API.Database.Administracion.DTOs.Modulos
{
    public class ModuloDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string NombreNormalizado { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public int NumeroGruposPermisos { get; set; }
        public bool EsAdministrador { get; set; }
        public List<PermisosAgrupadosDTO> PermmisosAgrupados { get; set; }
    }
}
