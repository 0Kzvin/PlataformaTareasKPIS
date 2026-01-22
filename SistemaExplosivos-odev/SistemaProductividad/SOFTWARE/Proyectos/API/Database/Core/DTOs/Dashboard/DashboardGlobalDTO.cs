namespace API.Database.Core.DTOs.Dashboard
{
    public class DashboardGlobalDTO
    {
        public int TotalDepartamentos { get; set; }
        public int TotalUsuarios { get; set; }
        public int TotalTareas { get; set; }
        public int TareasVencidas { get; set; }
        public int TareasCompletadas { get; set; }
    }
}
