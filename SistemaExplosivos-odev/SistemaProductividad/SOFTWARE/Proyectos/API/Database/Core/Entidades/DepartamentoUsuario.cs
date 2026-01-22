using API.Database.Administracion.Entidades.Identidad;

namespace API.Database.Core.Entidades
{
    public class DepartamentoUsuario
    {
        public int Id { get; set; }
        public int DepartamentoId { get; set; }
        public string UsuarioId { get; set; }
        public string RolDepartamento { get; set; }

        public virtual Departamentos Departamento { get; set; }
        public virtual Usuarios Usuario { get; set; }
    }
}
