using System.Collections.Generic;
using System.ComponentModel;

namespace API.Database.Administracion.DTOs.Identidad
{
    public class ReporteUsuariosDTO
    {
        public List<UsuariosReporteoDTO> Usuarios { get; set; }
        public List<RolesReporteoDTO> Roles { get; set; }
    }

    public class UsuariosReporteoDTO
    {
        [DisplayName("Nombre Completo")]
        public string NombreCompleto { get; set; }

        [DisplayName("Usuario")]
        public string Usuario { get; set; }

        [DisplayName("Perfil")]
        public string NombreRol { get; set; }
        [DisplayName("Correo")]
        public string Correo { get; set; }
        //public List<string> ModulosHabilitados { get; set; } = new List<string>();
    }

    public class RolesReporteoDTO
    {
        public string NombreRol { get; set; }
        public string Descripcion { get; set; }
        public List<PermisoDTO> Permisos { get; set; } = new List<PermisoDTO>();
    }
}
