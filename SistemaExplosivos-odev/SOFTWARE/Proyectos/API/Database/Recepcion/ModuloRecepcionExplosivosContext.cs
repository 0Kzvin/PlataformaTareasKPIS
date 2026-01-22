using API.Database.Recepcion.Entidades;
using Microsoft.EntityFrameworkCore;

namespace API.Database.Recepcion
{
    public partial class ModuloRecepcionExplosivosContext : DbContext
    {
        public ModuloRecepcionExplosivosContext() { }

        public ModuloRecepcionExplosivosContext(DbContextOptions<ModuloRecepcionExplosivosContext> options) : base(options) { }

        public virtual DbSet<ConductoresRecepcion> ConductoresRecepcion { get; set; } 

        public virtual DbSet<EquiposRecepcion> EquiposRecepcion { get; set; }

        public virtual DbSet<OrigenesRecepcion> OrigenesRecepcion { get; set; }

        public virtual DbSet<Recepciones> Recepciones { get; set; }

        public virtual DbSet<TransaccionesRecepcion> TransaccionesRecepcion { get; set; }

        public virtual DbSet<ProveedoresRecepcion> ProveedoresRecepcion { get; set; }

        public virtual DbSet<TransportistasRecepcion> TransportistasRecepcion { get; set; }
    }
}
