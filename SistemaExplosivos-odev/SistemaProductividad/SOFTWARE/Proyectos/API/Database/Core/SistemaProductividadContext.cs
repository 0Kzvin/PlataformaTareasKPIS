using API.Database.Administracion.Entidades.Identidad;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using API.Database.Core.Entidades;

namespace API.Database.Core
{
    public class SistemaProductividadContext : IdentityDbContext<Usuarios, Roles, string>
    {
        public SistemaProductividadContext(DbContextOptions<SistemaProductividadContext> options) : base(options)
        {
        }

        public DbSet<Departamentos> Departamentos { get; set; }
        public DbSet<ConfiguracionDepartamento> ConfiguracionesDepartamentos { get; set; }
        public DbSet<DepartamentoUsuario> DepartamentosUsuarios { get; set; }
        public DbSet<Tareas> Tareas { get; set; }
        public DbSet<CamposPrivadosTarea> CamposPrivadosTareas { get; set; }
        public DbSet<TareaAsignado> TareasAsignados { get; set; }
        public DbSet<TareaComentario> TareasComentarios { get; set; }
        public DbSet<TareaHistorial> TareasHistorial { get; set; }
        public DbSet<TareaEvidencia> TareasEvidencias { get; set; }
        public DbSet<KpiDepartamento> KpisDepartamentos { get; set; }
        public DbSet<RegistroAuditoria> RegistrosAuditoria { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Custom configuration for Usuarios
            modelBuilder.Entity<Usuarios>(b =>
            {
                b.ToTable("Usuarios");
                b.Property(e => e.Nombre).HasMaxLength(100);
                b.Property(e => e.Apellidos).HasMaxLength(100);
            });
            
            modelBuilder.Entity<Departamentos>(b =>
            {
                b.ToTable("Departamentos");
                b.HasOne(d => d.Lider)
                    .WithMany()
                    .HasForeignKey(d => d.LiderId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(d => d.Configuracion)
                    .WithOne(c => c.Departamento)
                    .HasForeignKey<ConfiguracionDepartamento>(c => c.DepartamentoId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasMany(d => d.Miembros)
                    .WithOne(u => u.Departamento)
                    .HasForeignKey(u => u.DepartamentoId)
                    .OnDelete(DeleteBehavior.Restrict); 

                b.HasMany(d => d.Usuarios)
                    .WithOne(du => du.Departamento)
                    .HasForeignKey(du => du.DepartamentoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Tareas>(b =>
            {
                b.ToTable("Tareas");
                b.HasOne(t => t.Departamento)
                    .WithMany(d => d.Tareas)
                    .HasForeignKey(t => t.DepartamentoId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(t => t.ResponsablePrincipal)
                    .WithMany()
                    .HasForeignKey(t => t.ResponsablePrincipalId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(t => t.Creador)
                    .WithMany()
                    .HasForeignKey(t => t.CreadorId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(t => t.CamposPrivados)
                    .WithOne(c => c.Tarea)
                    .HasForeignKey<CamposPrivadosTarea>(c => c.TareaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TareaComentario>(b =>
            {
                b.ToTable("TareaComentarios");
                b.HasOne(c => c.Tarea)
                    .WithMany(t => t.Comentarios)
                    .HasForeignKey(c => c.TareaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TareaHistorial>(b =>
            {
                b.ToTable("TareaHistorial");
                b.HasOne(h => h.Tarea)
                    .WithMany(t => t.Historial)
                    .HasForeignKey(h => h.TareaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TareaEvidencia>(b =>
            {
                b.ToTable("TareaEvidencias");
                b.HasOne(e => e.Tarea)
                    .WithMany(t => t.Evidencias)
                    .HasForeignKey(e => e.TareaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TareaAsignado>(b =>
            {
                b.ToTable("TareaAsignados");
                b.HasOne(a => a.Tarea)
                    .WithMany(t => t.Asignados)
                    .HasForeignKey(a => a.TareaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DepartamentoUsuario>(b =>
            {
                b.ToTable("DepartamentoUsuarios");
                b.HasOne(du => du.Usuario)
                    .WithMany(u => u.Departamentos)
                    .HasForeignKey(du => du.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<KpiDepartamento>(b =>
            {
                b.ToTable("KpisDepartamentos");
                b.HasOne(k => k.Departamento)
                    .WithMany(d => d.Kpis)
                    .HasForeignKey(k => k.DepartamentoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RegistroAuditoria>(b =>
            {
                b.ToTable("RegistrosAuditoria");
                b.HasOne(r => r.Usuario)
                    .WithMany()
                    .HasForeignKey(r => r.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Notificacion>(b =>
            {
                b.ToTable("Notificaciones");
                b.HasOne(n => n.Usuario)
                    .WithMany()
                    .HasForeignKey(n => n.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Roles>(b =>
            {
                b.ToTable("Roles");
            });
        }
    }
}
