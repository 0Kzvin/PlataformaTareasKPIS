namespace API.Database.Core.DTOs.Kpis
{
    public class KpiDepartamentoDTO
    {
        public int Id { get; set; }
        public int DepartamentoId { get; set; }
        public string Nombre { get; set; }
        public string Formula { get; set; }
        public string Meta { get; set; }
        public bool Activo { get; set; }
    }
}
