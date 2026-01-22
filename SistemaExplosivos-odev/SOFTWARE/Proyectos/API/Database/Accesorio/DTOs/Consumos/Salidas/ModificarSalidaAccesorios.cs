namespace API.Database.Accesorio.DTOs.Consumos.Salidas
{
    public class ModificarSalidaAccesorios
    {
        public int? Id { get; set; }

        public string NumeroStock { get; set; }

        public int NumeroSalida { get; set; }

        public string Nombre { get; set; }

        public string UnidadDeMedida { get; set; }

        public decimal CantidadASacar { get; set; }

        public decimal CantidadInicial { get; set; }

        public decimal CantidadFinal { get; set; }

        public decimal FactorCorrecion { get; set; }

        public decimal CantidadCorregida { get; set; }

        public bool EsDevolucion { get; set; }
    }
}
