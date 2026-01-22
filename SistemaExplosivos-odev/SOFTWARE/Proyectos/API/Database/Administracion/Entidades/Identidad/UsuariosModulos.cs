namespace API.Database.Administracion.Entidades.Identidad
{
    public class UsuariosModulos
    {
        public int IdModulo { get; set; }
        public string IdUsuario { get; set; }
        public bool EsAdministrador { get; set; }
        public virtual Usuarios Usuarios { get; set; }
        public virtual Modulos Modulos { get; set; }
    }
}
