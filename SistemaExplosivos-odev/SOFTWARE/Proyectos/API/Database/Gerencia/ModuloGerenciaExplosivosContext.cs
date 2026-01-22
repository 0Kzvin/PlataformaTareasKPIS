using API.Database.Gerencia.Entidades;
using Microsoft.EntityFrameworkCore;

namespace API.Database.Gerencia
{
    public partial class ModuloGerenciaExplosivosContext : DbContext
    {
        public ModuloGerenciaExplosivosContext() { }

        public ModuloGerenciaExplosivosContext(DbContextOptions<ModuloGerenciaExplosivosContext> options) : base(options) { }

        public virtual DbSet<OperadoresGerencia> Operadores { get; set; }
        public virtual DbSet<EstacionesGerencia> Estaciones { get; set; }
        public virtual DbSet<ProductosGerencia> Productos { get; set; }
    }
}
