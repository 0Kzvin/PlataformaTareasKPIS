using API.Database.Core.Enums;

namespace API.Database.Core.Entidades
{
    public class ConfiguracionDepartamento
    {
        public int Id { get; set; }
        public int DepartamentoId { get; set; }
        public ModoAsignacionEnum ModoAsignacion { get; set; }
        public bool PermiteAsignarOtros { get; set; }
        public bool PermiteCamposPrivados { get; set; }
        public string KpisActivos { get; set; } = "[]";

        public virtual Departamentos Departamento { get; set; }
    }
}
