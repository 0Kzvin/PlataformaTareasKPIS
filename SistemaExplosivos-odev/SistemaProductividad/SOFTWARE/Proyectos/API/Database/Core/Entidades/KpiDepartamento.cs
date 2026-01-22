namespace API.Database.Core.Entidades
{
    public class KpiDepartamento
    {
        public int Id { get; set; }
        public int DepartamentoId { get; set; }
        public string Nombre { get; set; }
        public string Formula { get; set; }
        public string Meta { get; set; }
        public bool Activo { get; set; } = true;

        public virtual Departamentos Departamento { get; set; }
    }
}
