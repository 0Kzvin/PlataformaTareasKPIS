using API.Database.Core.Enums;

namespace API.Database.Core.DTOs.Departamentos
{
    public class ConfiguracionDepartamentoDTO
    {
        public int Id { get; set; }
        public ModoAsignacionEnum ModoAsignacion { get; set; }
        public bool PermiteAsignarOtros { get; set; }
        public bool PermiteCamposPrivados { get; set; }
        public string KpisActivos { get; set; }
    }
}
