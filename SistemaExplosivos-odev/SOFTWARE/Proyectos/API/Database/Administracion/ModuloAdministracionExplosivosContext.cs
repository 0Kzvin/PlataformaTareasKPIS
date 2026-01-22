using API.Database.Administracion.Entidades.General;
using API.Database.Administracion.Entidades.Identidad;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Database.Administracion
{
    public class ModuloAdministracionExplosivosContext : IdentityDbContext<
            Usuarios, Roles, string, IdentityUserClaim<string>, UsuariosRoles, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>
        >
    {
        public ModuloAdministracionExplosivosContext(DbContextOptions<ModuloAdministracionExplosivosContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuarios>(b =>
            {
                b.ToTable("Usuarios");
                b.Property(e => e.UserName).HasColumnName("Usuario");
                b.Property(e => e.NormalizedUserName).HasColumnName("UsuarioNormalizado");
                b.Property(e => e.Email).HasColumnName("Correo");
                b.Property(e => e.NormalizedEmail).HasColumnName("CorreoNormalizado");
                b.Property(e => e.PhoneNumber).HasColumnName("NumeroTelefonico");
                b.Property(e => e.ConcurrencyStamp).HasColumnName("SelloConcurrencia");
                b.Property(e => e.SecurityStamp).HasColumnName("SelloSeguridad");
                b.Property(e => e.LockoutEnabled).HasColumnName("BloqueoUsuario");
                b.Property(e => e.LockoutEnd).HasColumnName("BloqueadoHasta");
                b.Property(e => e.AccessFailedCount).HasColumnName("ConteoAccesosFallidos");
                b.Property(e => e.TwoFactorEnabled).HasColumnName("DobleFactorHabilitado");
                b.Property(e => e.PhoneNumberConfirmed).HasColumnName("NumeroConfirmado");
                b.Property(e => e.EmailConfirmed).HasColumnName("CorreoConfirmado");
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.ToTable("UsuariosClaims");
                b.Property(e => e.ClaimType).HasColumnName("ClaimTipo");
                b.Property(e => e.ClaimValue).HasColumnName("ClaimValor");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.ToTable("UsuariosLogins");
                b.Property(e => e.LoginProvider).HasColumnName("ProveedorLogin");
                b.Property(e => e.ProviderDisplayName).HasColumnName("ProveedorNombre");
                b.Property(e => e.ProviderKey).HasColumnName("ProveedorLlave");
            });

            modelBuilder.Entity<IdentityUserToken<string>>(b =>
            {
                b.ToTable("UsuariosToken");
                b.Property(e => e.LoginProvider).HasColumnName("ProveedorLogin");
                b.Property(e => e.Name).HasColumnName("Nombre");
                b.Property(e => e.Value).HasColumnName("Valor");
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.ToTable("RolesClaims");
                b.Property(e => e.ClaimValue).HasColumnName("ClaimValor");
                b.Property(e => e.ClaimType).HasColumnName("ClaimTipo");
            });

            modelBuilder.Entity<Roles>(b =>
            {
                b.ToTable("Roles");
                b.Property(e => e.Name).HasColumnName("Rol");
                b.Property(e => e.NormalizedName).HasColumnName("RolNormalizado");
                b.Property(e => e.ConcurrencyStamp).HasColumnName("SelloConcurrencia");
            });

            modelBuilder.Entity<UsuariosRoles>(b =>
            {
                b.ToTable("UsuariosRoles");
                b.HasKey(e => new { e.UserId, e.RoleId });

                b.HasOne(e => e.Rol)
                    .WithMany(e => e.UsuariosRoles)
                    .HasForeignKey(e => e.RoleId)
                    .IsRequired();

                b.HasOne(e => e.Usuario)
                    .WithMany(e => e.UsuariosRoles)
                    .HasForeignKey(e => e.UserId)
                    .IsRequired();

                b.Property(e => e.UserId).HasColumnName("IdUsuario");
                b.Property(e => e.RoleId).HasColumnName("IdRol");
            });

            modelBuilder.Entity<RolesPermisos>(b =>
            {
                b.HasKey(e => new { e.IdRol, e.IdPermiso });
            });

            modelBuilder.Entity<UsuariosModulos>(b =>
            {
                b.ToTable("UsuariosModulos");
                b.HasKey(e => new { e.IdModulo, e.IdUsuario });

                b.HasOne(e => e.Modulos)
                    .WithMany(e => e.UsuarioModulos)
                    .HasForeignKey(e => e.IdModulo)
                    .IsRequired();

                b.HasOne(e => e.Usuarios)
                    .WithMany(e => e.UsuariosModulos)
                    .HasForeignKey(e => e.IdUsuario)
                    .IsRequired();
            });

            modelBuilder.Entity<RolesModulos>(b =>
            {
                b.ToTable("RolesModulos");
                b.HasKey(e => new { e.IdModulo, e.IdRol });

                b.HasOne(e => e.Modulos)
                    .WithMany(e => e.RolesModulos)
                    .HasForeignKey(e => e.IdModulo)
                    .IsRequired();

                b.HasOne(e => e.Roles)
                    .WithMany(e => e.RolesModulos)
                    .HasForeignKey(e => e.IdRol)
                    .IsRequired();
            });
        }

        //Control de usuarios y roles
        public DbSet<CorreosAutomaticos> CorreosAutomaticos { get; set; }
        public DbSet<Modulos> Modulos { get; set; }
        public DbSet<Permisos> Permisos { get; set; }
        public DbSet<GruposPermisos> GruposPermisos { get; set; }
        public DbSet<ActualizarToken> ActualizarTokens { get; set; }
        public DbSet<RolesPermisos> RolesPermisos { get; set; }
        public DbSet<UsuariosModulos> UsuariosModulos { get; set; }
        public DbSet<RolesModulos> RolesModulos { get; set; }

        public DbSet<UsuariosRecuperacion> UsuariosRecuperacion { get; set; }

    }
}
