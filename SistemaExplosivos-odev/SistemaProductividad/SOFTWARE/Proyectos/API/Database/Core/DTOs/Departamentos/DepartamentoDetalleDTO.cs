namespace API.Database.Core.DTOs.Departamentos
{
    public class DepartamentoDetalleDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string LiderId { get; set; }
        public string LiderNombre { get; set; }
        public bool Activo { get; set; }
        public ConfiguracionDepartamentoDTO Configuracion { get; set; }
    }
}
