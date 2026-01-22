using System.Collections.Generic;

namespace API.Database.Administracion.DTOs.Identidad
{
    public class PermisoDTO
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string NombreNormalizado { get; set; }
        public string GrupoNombre { get; set; }
        public string GrupoNombreNormalizado { get; set; }
        public List<string> IdRolesAsignados { get; set; }
        public string Descripcion { get; set; }
        public int IdModulo { get; set; }
        public string NombreModulo { get; set; }
        public string NombreNormalizadoModulo { get; set; }
        public string DescripcionModulo { get; set; }
    }
}
