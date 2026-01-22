namespace API.Database.Administracion.Entidades.Identidad
{
    public class RolesModulos
    {
        public int IdModulo { get; set; }
        public string IdRol { get; set; }
        public bool EsAdministrador { get; set; }
        public virtual Modulos Modulos { get; set; }
        public virtual Roles Roles { get; set; }
    }
}
