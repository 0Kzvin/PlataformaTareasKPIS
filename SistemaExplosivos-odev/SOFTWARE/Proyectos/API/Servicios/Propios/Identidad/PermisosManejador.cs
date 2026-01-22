using API.Database.Administracion;
using API.Database.Administracion.Entidades.Identidad;
using API.Servicios.Preterminados.Autorizacion.PermisosAutorizacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Servicios.Propios.Identidad
{
    /// <summary>
    /// Manejador de autorización que verifica si un usuario tiene los permisos requeridos para acceder a un recurso.
    /// </summary>
    /// <remarks>
    /// Este manejador implementa la lógica de autorización basada en permisos personalizados:
    /// 1. Se integra con el sistema de autenticación de ASP.NET Core Identity
    /// 2. Verifica múltiples condiciones antes de conceder acceso:
    ///    - Usuario autenticado
    ///    - Usuario existente en la base de datos
    ///    - Usuario activo (no deshabilitado)
    ///    - Permisos suficientes a través de roles
    /// 3. Utiliza políticas de autorización mediante <see cref="PermisoRequerido"/>
    /// 4. Optimiza las consultas a la base de datos mediante Entity Framework Core
    /// </remarks>
    public class PermisosManejador : AuthorizationHandler<PermisoRequerido>
    {
        private readonly IServiceProvider _servicios;

        /// <summary>
        /// Inicializa una nueva instancia del manejador de permisos.
        /// </summary>
        /// <param name="servicios">Proveedor de servicios para resolver dependencias.</param>
        /// <remarks>
        /// El proveedor de servicios se utiliza para:
        /// - Obtener instancias de UserManager y DbContext de forma segura
        /// - Evitar problemas de ciclo de vida (scoped services en singleton)
        /// - Crear nuevos ámbitos de servicio cuando sea necesario
        /// </remarks>
        public PermisosManejador(IServiceProvider servicios)
        {
            _servicios = servicios ?? throw new ArgumentNullException(nameof(servicios));
        }

        /// <summary>
        /// Evalúa si el usuario actual cumple con los requisitos de permiso especificados.
        /// </summary>
        /// <param name="context">Contexto de autorización que contiene información del usuario.</param>
        /// <param name="requisito">Requisito de permiso que debe ser satisfecho.</param>
        /// <returns>Tarea asincrónica que representa la operación de evaluación.</returns>
        /// <remarks>
        /// El flujo de evaluación sigue estos pasos:
        /// 1. Verifica autenticación del usuario
        /// 2. Obtiene el usuario desde la base de datos
        /// 3. Valida que el usuario esté activo
        /// 4. Recupera todos los permisos del usuario a través de sus roles
        /// 5. Compara con los permisos requeridos
        /// 6. Marca éxito o falla en el contexto de autorización
        /// </remarks>
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermisoRequerido requisito)
        {
            // Crear un nuevo ámbito de servicio para resolver dependencias con el ciclo de vida correcto
            using (var alcance = _servicios.CreateScope())
            {
                // Resolver servicios necesarios
                var administradorUsuarios = alcance.ServiceProvider.GetRequiredService<UserManager<Usuarios>>();
                var contextoBD = alcance.ServiceProvider.GetRequiredService<ModuloAdministracionExplosivosContext>();

                // Paso 1: Verificar autenticación
                if (context.User == null || !context.User.Identity.IsAuthenticated)
                {
                    context.Fail();
                    return;
                }

                // Paso 2: Obtener usuario desde la base de datos
                var usuario = await administradorUsuarios.Users
                    .FirstOrDefaultAsync(x =>
                        x.UserName == context.User.Identity.Name ||
                        x.Email == context.User.Identity.Name);

                if (usuario == null)
                {
                    context.Fail();
                    return;
                }

                // Paso 3: Verificar estado del usuario
                if (!usuario.Estado)
                {
                    context.Fail();
                    return;
                }

                // Paso 4: Obtener permisos del usuario
                var permisosUsuario = await ObtenerPermisosUsuarioAsync(contextoBD, usuario.Id);

                // Paso 5: Verificar permisos requeridos
                bool tienePermiso = permisosUsuario.Exists(x => requisito.Permiso.Contains(x.Nombre));

                if (!tienePermiso)
                {
                    context.Fail();
                    return;
                }

                // Paso 6: Marcar requisito como cumplido
                context.Succeed(requisito);
            }
        }

        /// <summary>
        /// Obtiene todos los permisos asociados a los roles activos de un usuario mediante una consulta optimizada.
        /// </summary>
        private async Task<List<Permisos>> ObtenerPermisosUsuarioAsync(
            ModuloAdministracionExplosivosContext contextoBD,
            string usuarioId)
        {
            try
            {
                return await contextoBD.UserRoles
                    // Filtra por el usuario actual
                    .Where(ur => ur.UserId == usuarioId)

                    // Une con Roles que estén activos
                    .Join(contextoBD.Roles.Where(r => r.Estado),
                        ur => ur.RoleId,
                        r => r.Id,
                        (ur, r) => r)

                    // Une con la relación Roles-Permisos
                    .Join(contextoBD.RolesPermisos,
                        r => r.Id,
                        rp => rp.IdRol,
                        (r, rp) => rp.IdPermiso)

                    // Elimina permisos duplicados
                    .Distinct()

                    // Obtiene los objetos Permiso completos
                    .Join(contextoBD.Permisos,
                        idPermiso => idPermiso,
                        p => p.Id,
                        (idPermiso, p) => p)
                    .AsNoTracking()
                    // Ejecuta la consulta
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Loggear error adecuadamente
                Console.WriteLine($"Error al obtener permisos: {ex.Message}");
                return new List<Permisos>();
            }
        }



        ///// <summary>
        ///// Obtiene todos los permisos asociados a los roles activos de un usuario.
        ///// </summary>
        ///// <param name="contextoBD">Contexto de base de datos para acceder a las entidades.</param>
        ///// <param name="usuarioId">Identificador único del usuario.</param>
        ///// <returns>
        ///// Lista de permisos asociados a los roles del usuario.
        ///// Retorna lista vacía si no hay permisos o el usuario no tiene roles.
        ///// </returns>
        ///// <remarks>
        ///// Este método realiza las siguientes operaciones:
        ///// 1. Obtiene los IDs de todos los roles asignados al usuario
        ///// 2. Filtra solo los roles que están activos
        ///// 3. Recupera las relaciones entre roles y permisos
        ///// 4. Obtiene los detalles completos de los permisos
        ///// 5. Retorna una lista consolidada de permisos
        ///// </remarks>
        //private async Task<List<Permisos>> ObtenerPermisosUsuarioAsync(
        //    ModuloAdministracionCalidadAceitesContext contextoBD,
        //    string usuarioId)
        //{
        //    var permisosUsuario = new List<Permisos>();

        //    // Paso 1: Obtener IDs de roles del usuario
        //    var idsRoles = await contextoBD.UserRoles
        //        .Where(x => x.UserId == usuarioId)
        //        .Select(x => x.RoleId)
        //        .ToListAsync();

        //    if (!idsRoles.Any())
        //    {
        //        return permisosUsuario;
        //    }

        //    // Paso 2 y 3: Obtener relaciones rol-permiso para roles activos
        //    var rolesPermisos = await contextoBD.RolesPermisos
        //        .Where(x => idsRoles.Contains(x.IdRol) &&
        //               contextoBD.Roles.Any(r => r.Id == x.IdRol && r.Estado))
        //        .ToListAsync();

        //    if (!rolesPermisos.Any())
        //    {
        //        return permisosUsuario;
        //    }

        //    // Paso 4: Obtener detalles de permisos
        //    var idsPermisos = rolesPermisos.Select(x => x.IdPermiso).Distinct();
        //    permisosUsuario = await contextoBD.Permisos
        //        .Where(x => idsPermisos.Contains(x.Id))
        //        .ToListAsync();

        //    return permisosUsuario;
        //}
    }
}