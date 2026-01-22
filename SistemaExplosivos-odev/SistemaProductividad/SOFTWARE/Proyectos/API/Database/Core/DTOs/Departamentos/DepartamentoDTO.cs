namespace API.Database.Core.DTOs.Departamentos
{
    public class DepartamentoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string LiderNombre { get; set; }
        public string LiderId { get; set; }
        public bool Estado { get; set; }
        public int NumeroMiembros { get; set; }
    }
}
