namespace API.Database.Administracion.Entidades.General
{
    public class CorreosAutomaticos
    {
        public int Id { get; set; }
        public int IdModulo { get; set; }
        public string Nombre { get; set; }
        public string NombreModulo { get; set; }
        public string NombreClave { get; set; }
        public string Descripcion { get; set; }
        public string ExpresionCron { get; set; }
        public string ListaDestinatarios { get; set; }
        public bool Activo { get; set; }
        public bool Ocultar { get; set; }
    }
}
