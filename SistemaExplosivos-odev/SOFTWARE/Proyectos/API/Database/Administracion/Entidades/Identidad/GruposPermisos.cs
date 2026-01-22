using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Administracion.Entidades.Identidad
{
    public class GruposPermisos
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int IdModulo { get; set; }
        public string GrupoNombre { get; set; }
        public string GrupoNombreNormalizado { get; set; }
        public bool Ocultar { get; set; } = false;
        [ForeignKey("IdModulo")]
        public Modulos Modulo { get; set; }
    }
}
