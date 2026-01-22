using System.Collections.Generic;

namespace API.Database.Administracion.DTOs.Identidad
{
    public class EditarRol
    {
        public string IdRol { get; set; }
        public string NombreRol { get; set; }
        public string Descripcion { get; set; }
        public List<string> PermisosOtorgados { get; set; }
    }
}
