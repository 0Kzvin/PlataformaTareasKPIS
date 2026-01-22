using System.Collections.Generic;

namespace API.Database.Administracion.DTOs.Identidad
{
    public class PermisosAgrupadosDTO
    {
        public string GrupoNombre { get; set; }
        public string GrupoNombreNormalizado { get; set; }
        public int NumeroPermisos { get; set; }
        public int IdModulo { get; set; }
        public string NombreModulo { get; set; }
        public List<PermisoDTO> Permisos { get; set; }
    }
}
