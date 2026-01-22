using API.Database.Accesorio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace API.Database.Accesorio
{
    public class ModuloAccesoriosExplosivosContext : DbContext
    {
        public ModuloAccesoriosExplosivosContext() { }

        public ModuloAccesoriosExplosivosContext(DbContextOptions<ModuloAccesoriosExplosivosContext> options) : base(options) { }

        public virtual DbSet<ProductosAccesorios> Accesorios { get; set; }

        public virtual DbSet<ConsumosAccesorios> ConsumosAccesorios { get; set; }

        public virtual DbSet<SalidasAccesorios> SalidasAccesorios { get; set; }

        public virtual DbSet<ProveedoresAccesorios> ProveedoresAccesorios { get; set; }

        public virtual DbSet<CategoriasAccesorios> CategoriasAccesorios { get; set; } 

        public virtual DbSet<DestinosAccesorios> DestinosAccesorios { get; set; }

        public virtual DbSet<EntradasAccesorios> EntradasAccesorios { get; set; }

        public virtual DbSet<FacturasAccesorios> FacturasAccesorios { get; set; }
    }
}
