using API.Database.Almacenamiento.Entidades;
using Microsoft.EntityFrameworkCore;

namespace API.Database.Almacenamiento
{
    public partial class ModuloAlmacenamientoExplosivosContext : DbContext
    {
        public ModuloAlmacenamientoExplosivosContext() { }

        public ModuloAlmacenamientoExplosivosContext(DbContextOptions<ModuloAlmacenamientoExplosivosContext> options) : base(options) { }

        public virtual DbSet<DepositosAlmacenamiento> DepositosAlmacenamiento { get; set; }
        public virtual DbSet<CargasDepositosAlmacenamiento> CargasDepositosAlmacenamientos { get; set; }
        public virtual DbSet<EstatusDepositosAlmacenamiento> EstatusDepositosAlmacenamiento { get; set; }
        public virtual DbSet<EquiposAlmacenamiento> EquiposAlmacenamiento { get; set; }
        public virtual DbSet<MovimientosSupersacosAlmacenamiento> MovimientosSupersacosAlmacenamiento { get; set; }
    }
}
