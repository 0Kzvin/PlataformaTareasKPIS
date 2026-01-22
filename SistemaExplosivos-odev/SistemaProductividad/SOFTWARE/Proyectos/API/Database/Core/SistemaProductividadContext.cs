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

                b.HasMany(d => d.Miembros)
                    .WithOne(u => u.Departamento)
                    .HasForeignKey(u => u.DepartamentoId)
                    .OnDelete(DeleteBehavior.Restrict); 
            });

            modelBuilder.Entity<Tareas>(b =>
            {
                b.ToTable("Tareas");
                b.HasOne(t => t.Departamento)
                    .WithMany()
                    .HasForeignKey(t => t.DepartamentoId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(t => t.Asignado)
                    .WithMany()
                    .HasForeignKey(t => t.AsignadoId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(t => t.Creador)
                    .WithMany()
                    .HasForeignKey(t => t.CreadorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Comentarios>(b =>
            {
                b.ToTable("Comentarios");
                b.HasOne(c => c.Tarea)
                    .WithMany(t => t.Comentarios)
                    .HasForeignKey(c => c.TareaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Roles>(b =>
            {
                b.ToTable("Roles");
            });
        }
    }
}
