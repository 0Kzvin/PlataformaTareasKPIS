using API.Database.Administracion.DTOs.Identidad;
using System.Collections.Generic;

namespace API.Database.Administracion.DTOs.GrupoPermisos
{
    public class GrupoPermisoDTO
    {
        public int Id { get; set; }
        public int IdModulo { get; set; }
        public string GrupoNombre { get; set; }
        public string GrupoNombreNormalizado { get; set; }
        public int NumeroPermisos { get; set; }
        public List<PermisoDTO> Permisos { get; set; }

    }
}
