using API.Database.Administracion;
using API.Database.Administracion.DTOs.Identidad;
using API.Database.Administracion.DTOs.Modulos;
using API.Database.Administracion.DTOs.Respuestas;
using API.Database.Administracion.Entidades.Identidad;
using API.Utilidades.Constantes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;

namespace API.Servicios.Propios.Identidad
{
    public class ServicioAutorizacion : IServicioAutorizacion
    {
        private readonly UserManager<Usuarios> _userManager;
        private readonly RoleManager<Roles> _roleManager;
        private readonly ModuloAdministracionExplosivosContext _context;

        public ServicioAutorizacion(
            UserManager<Usuarios> userManager,
            RoleManager<Roles> roleManager,
            ModuloAdministracionExplosivosContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<RespuestaGenerica> AsignarRol(EditarRolUsuarioDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.UsuarioId);

            if (user == null)
            {
                return new RespuestaGenerica
                {
                    Mensaje = "Ocurrió un error",
                    Errores = new List<string> { "El Usuario no existe" },
                    OperacionExitosa = false,
                };
            }

            var rolEncontrado = await _roleManager.FindByIdAsync(model.RolId);

            if (rolEncontrado == null)
            {
                return new RespuestaGenerica
                {
                    Mensaje = "Ocurrió un error",
                    Errores = new List<string> { "El Rol especificado no existe" },
                    OperacionExitosa = false,
                };
            }

            // Eliminar roles actuales antes de asignar el nuevo
            var rolesActuales = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, rolesActuales);

            var asignarRol = await _userManager.AddToRoleAsync(user, rolEncontrado.Name);
            var rolClaim = await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, rolEncontrado.Name));

            if (!asignarRol.Succeeded || !rolClaim.Succeeded)
            {
                return new RespuestaGenerica
                {
                    Mensaje = "Ocurrió un error al asignar el rol",
                    Errores = asignarRol.Errors.Select(e => e.Description).ToList(),
                    OperacionExitosa = false,
                };
            }

            return new RespuestaGenerica
            {
                Mensaje = "Rol asignado con éxito",
                OperacionExitosa = true,
            };
        }

        public async Task<RespuestaGenerica> CrearRol(CrearRolDTO model)
        {
            var rolDuplicado = await _roleManager.FindByNameAsync(model.NombreRol);
            if (rolDuplicado != null)
            {
                return new RespuestaGenerica
                {
                    Errores = new List<string> { "No es posible crear un rol con nombre duplicado" },
                    OperacionExitosa = false,
                };
            }

            var rol = new Roles
            {
                Id = Guid.CreateVersion7().ToString(),
                Name = model.NombreRol,
                Descripcion = model.Descripcion,
                NormalizedName = model.NombreRol.ToUpper(),
                Estado = true,
            };

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var result = await _roleManager.CreateAsync(rol);
                    if (result.Succeeded)
                    {
                        await OtorgarPermisos(model.PermisosOtorgados, rol);
                        scope.Complete();
                        return new RespuestaGenerica
                        {
                            Mensaje = "Rol creado con éxito",
                            OperacionExitosa = true,
                        };
                    }
                    else
                    {
                        return new RespuestaGenerica
                        {
                            Errores = result.Errors.Select(e => e.Description).ToList(),
                            OperacionExitosa = false,
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new RespuestaGenerica
                    {
                        Errores = new List<string> { $"Error al crear el rol: {ex.Message}" },
                        OperacionExitosa = false,
                    };
                }
            }
        }

        public async Task<RespuestaGenerica> EditarRol(EditarRol model, bool esSuperUsuario)
        {
            var rol = await _roleManager.FindByIdAsync(model.IdRol);
            if (rol == null)
            {
                return new RespuestaGenerica
                {
                    Mensaje = "Rol especificado no existe",
                    OperacionExitosa = false,
                };
            }

            if (rol.EstaOculto)
            {
                return new RespuestaGenerica
                {
                    Errores = new List<string> { "No es posible editar roles ocultos" },
                    OperacionExitosa = false,
                };
            }

            // Validaciones para roles protegidos
            if ((rol.Name == ConstantesRoles.ADMINISTRADOR_PREDETERMINADO ||
                 rol.NormalizedName == ConstantesRoles.ADMINISTRADOR_PREDETERMINADO.ToUpper()) && !esSuperUsuario)
            {
                return new RespuestaGenerica
                {
                    Errores = new List<string> { $"No es posible editar el Rol {ConstantesRoles.ADMINISTRADOR_PREDETERMINADO}" },
                    OperacionExitosa = false,
                };
            }

            if ((rol.Name == ConstantesRoles.LIMITADO ||
                 rol.NormalizedName == ConstantesRoles.LIMITADO.ToUpper()) && !esSuperUsuario)
            {
                return new RespuestaGenerica
                {
                    Errores = new List<string> { $"No es posible editar el Rol {ConstantesRoles.LIMITADO}" },
                    OperacionExitosa = false,
                };
            }

            // Verificar permisos existentes
            var permisosExistentes = await _context.Permisos
                .Where(p => model.PermisosOtorgados.Contains(p.Id))
                .CountAsync();

            if (permisosExistentes != model.PermisosOtorgados.Count)
            {
                return new RespuestaGenerica
                {
                    Mensaje = "Uno o varios permisos especificados no existen",
                    OperacionExitosa = false,
                };
            }

            // Actualizar permisos
            var permisosActuales = await _context.RolesPermisos
                .Where(rp => rp.IdRol == model.IdRol)
                .ToListAsync();

            var permisosPorAgregar = model.PermisosOtorgados
                .Except(permisosActuales.Select(pa => pa.IdPermiso))
                .Select(permisoId => new RolesPermisos
                {
                    IdPermiso = permisoId,
                    IdRol = model.IdRol
                }).ToList();

            var permisosPorEliminar = permisosActuales
                .Where(pa => !model.PermisosOtorgados.Contains(pa.IdPermiso))
                .ToList();

            _context.RolesPermisos.AddRange(permisosPorAgregar);
            _context.RolesPermisos.RemoveRange(permisosPorEliminar);

            // Actualizar información del rol
            rol.Name = model.NombreRol;
            rol.NormalizedName = model.NombreRol.ToUpper();
            rol.Descripcion = model.Descripcion;

            try
            {
                await _context.SaveChangesAsync();
                await ActualizarModulosAsociados(model.IdRol);

                return new RespuestaGenerica
                {
                    Mensaje = "Rol editado con éxito",
                    OperacionExitosa = true,
                };
            }
            catch (Exception ex)
            {
                return new RespuestaGenerica
                {
                    Errores = new List<string> { $"Ocurrió un error al guardar los cambios: {ex.Message}" },
                    OperacionExitosa = false,
                };
            }
        }

        public async Task<RespuestaGenerica> BorrarRol(string idRol)
        {
            var rol = await _roleManager.FindByIdAsync(idRol);
            if (rol == null)
            {
                return new RespuestaGenerica
                {
                    Errores = new List<string> { "El Rol especificado no existe" },
                    OperacionExitosa = false,
                };
            }

            if (rol.EstaOculto)
            {
                return new RespuestaGenerica
                {
                    Errores = new List<string> { "No es posible borrar roles ocultos" },
                    OperacionExitosa = false,
                };
            }

            if (rol.Name == ConstantesRoles.ADMINISTRADOR_PREDETERMINADO ||
                rol.NormalizedName == ConstantesRoles.ADMINISTRADOR_PREDETERMINADO.ToUpper())
            {
                return new RespuestaGenerica
                {
                    Errores = new List<string> { $"No es posible borrar el Rol {ConstantesRoles.ADMINISTRADOR_PREDETERMINADO}" },
                    OperacionExitosa = false,
                };
            }

            if (rol.Name == ConstantesRoles.LIMITADO ||
                rol.NormalizedName == ConstantesRoles.LIMITADO.ToUpper())
            {
                return new RespuestaGenerica
                {
                    Errores = new List<string> { $"No es posible borrar el Rol {ConstantesRoles.LIMITADO}" },
                    OperacionExitosa = false,
                };
            }

            var result = await _roleManager.DeleteAsync(rol);
            if (!result.Succeeded)
            {
                return new RespuestaGenerica
                {
                    Errores = result.Errors.Select(e => e.Description).ToList(),
                    OperacionExitosa = false,
                };
            }

            // Asignar rol "Limitado" a usuarios que quedaron sin rol
            var usuariosSinRol = await _context.Users
                .Where(u => !_context.UserRoles.Any(ur => ur.UserId == u.Id))
                .ToListAsync();

            var rolLimitado = await _roleManager.FindByNameAsync(ConstantesRoles.LIMITADO);
            if (rolLimitado != null && usuariosSinRol.Any())
            {
                foreach (var usuario in usuariosSinRol)
                {
                    await _userManager.AddToRoleAsync(usuario, rolLimitado.Name);
                }
            }

            return new RespuestaGenerica
            {
                Mensaje = "Rol eliminado con éxito",
                OperacionExitosa = true,
            };
        }

        public async Task<List<PermisoOtorgadoDTO>> ObtenerPermisosOtorgados(string usuarioOCorreo)
        {
            // Consulta optimizada que proyecta directamente al DTO
            var permisos = await _context.Users
                .AsNoTracking()
                .Where(u => u.UserName == usuarioOCorreo || u.Email == usuarioOCorreo)
                .SelectMany(u => u.UsuariosRoles
                    .Select(ur => ur.Rol)
                    .SelectMany(r => r.RolesPermisos
                        .Select(rp => rp.Permisos)
                    )
                )
                .Distinct()
                .Select(p => new PermisoOtorgadoDTO
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    GrupoNombre = p.GrupoPermiso.GrupoNombre
                })
                .ToListAsync();

            return permisos;
        }

        public async Task<List<ModuloDTO>> ObtenerModulosOtorgados(string usuarioOCorreo)
        {
            var esSuperUsuario = await _context.Users
                .Where(u => u.UserName == usuarioOCorreo || u.Email == usuarioOCorreo)
                .SelectMany(u => u.UsuariosRoles
                    .Where(ur => ur.Rol.Name == ConstantesRoles.SUPER_USUARIO_G3)
                )
                .AnyAsync();

            var modulos = await _context.Users
                .AsNoTracking()
                .Where(u => u.UserName == usuarioOCorreo || u.Email == usuarioOCorreo)
                .SelectMany(u => u.UsuariosRoles
                    .Select(ur => ur.Rol)
                    .SelectMany(r => r.RolesModulos
                        .Select(rm => rm.Modulos)
                        .Where(m => esSuperUsuario || m.Estado) // Filtro para superusuario
                )).Distinct().ToListAsync();

            return modulos
                .GroupBy(m => new { m.Id, m.Nombre, m.NombreNormalizado, m.Estado, m.Descripcion })
                .Select(g => new ModuloDTO
                {
                    Id = g.Key.Id,
                    Nombre = g.Key.Nombre,
                    NombreNormalizado = g.Key.NombreNormalizado,
                    Descripcion = g.Key.Descripcion,
                    Estado = g.Key.Estado
                })
                .ToList();
        }

        public async Task<List<UsuariosRoles>> ObtenerRolesPorUsuario(Usuarios usuario)
        {
            return await _context.UserRoles
                .AsNoTracking()  // Mejor rendimiento para solo lectura
                .Where(ur => ur.UserId == usuario.Id)
                .Include(ur => ur.Rol)
                .ThenInclude(r => r.RolesModulos)
                .ToListAsync();
        }

        #region Métodos Privados

        private async Task OtorgarPermisos(List<string> permisosOtorgadosIds, Roles rol)
        {
            var modulosPorActivar = new Dictionary<int, bool>();

            foreach (var permisoId in permisosOtorgadosIds)
            {
                // Agregar permiso al rol
                await _context.RolesPermisos.AddAsync(new RolesPermisos
                {
                    IdPermiso = permisoId,
                    IdRol = rol.Id,
                });

                // Obtener módulo asociado al permiso
                var permiso = await _context.Permisos
                    .Include(p => p.GrupoPermiso)
                    .ThenInclude(gp => gp.Modulo)
                    .FirstOrDefaultAsync(p => p.Id == permisoId);

                if (permiso?.GrupoPermiso?.Modulo != null &&
                    !modulosPorActivar.ContainsKey(permiso.GrupoPermiso.Modulo.Id))
                {
                    modulosPorActivar[permiso.GrupoPermiso.Modulo.Id] = true;
                }
            }

            // Agregar módulos al rol
            foreach (var moduloId in modulosPorActivar.Keys)
            {
                await _context.RolesModulos.AddAsync(new RolesModulos
                {
                    IdModulo = moduloId,
                    IdRol = rol.Id,
                });
            }

            await _context.SaveChangesAsync();
        }

        private async Task ActualizarModulosAsociados(string rolId)
        {
            // Obtener módulos actuales basados en los permisos
            var modulosConPermisos = await _context.RolesPermisos
                .Include(rp => rp.Permisos)
                .ThenInclude(p => p.GrupoPermiso)
                .ThenInclude(gp => gp.Modulo)
                .Where(rp => rp.IdRol == rolId)
                .Select(rp => rp.Permisos.GrupoPermiso.Modulo.Id)
                .Distinct()
                .ToListAsync();

            // Obtener módulos actualmente asignados al rol
            var modulosAsignados = await _context.RolesModulos
                .Where(rm => rm.IdRol == rolId)
                .Select(rm => rm.IdModulo)
                .ToListAsync();

            // Módulos por agregar
            var modulosPorAgregar = modulosConPermisos
                .Except(modulosAsignados)
                .Select(moduloId => new RolesModulos
                {
                    IdModulo = moduloId,
                    IdRol = rolId
                });

            // Módulos por eliminar
            var modulosPorEliminar = _context.RolesModulos
                .Where(rm => rm.IdRol == rolId &&
                            !modulosConPermisos.Contains(rm.IdModulo));

            await _context.RolesModulos.AddRangeAsync(modulosPorAgregar);
            _context.RolesModulos.RemoveRange(modulosPorEliminar);

            await _context.SaveChangesAsync();
        }

        #endregion
    }
}