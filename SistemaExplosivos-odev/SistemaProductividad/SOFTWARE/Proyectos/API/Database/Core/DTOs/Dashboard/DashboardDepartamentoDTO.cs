namespace API.Database.Core.DTOs.Dashboard
{
    public class DashboardDepartamentoDTO
    {
        public string NombreDepartamento { get; set; }
        public int TotalTareas { get; set; }
        public int Pendientes { get; set; }
        public int EnProceso { get; set; }
        public int Terminadas { get; set; }
        public int Vencidas { get; set; }
        public double Eficiencia { get; set; } // Example metric
    }
}
