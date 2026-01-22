using API.Database.Administracion;
using API.Database.Administracion.DTOs.Identidad;
using API.Database.Administracion.DTOs.Modulos;
using API.Database.Administracion.DTOs.Respuestas;
using API.Servicios.Preterminados.Autorizacion.PermisosAutorizacion.Controladores.Administracion;
using API.Servicios.Preterminados.Autorizacion.PermisosAutorizacion.Controladores;
using API.Servicios.Propios.Identidad;
using API.Utilidades.Constantes;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controladores.VehiculosPesados
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = ConstantesModulos.ADMINISTRACION)]
    [Route("administracion/[controller]")]
    [ApiController]
    public class ModulosController : ControllerBase
    {
        private readonly ModuloAdministracionExplosivosContext context;
        private readonly IMapper mapper;
        private readonly IServicioAutorizacion servicioIdentidad;
        private readonly IStringLocalizer<ModulosController> stringLocalizer;

        public ModulosController(IServicioAutorizacion servicioIdentidad,
            ModuloAdministracionExplosivosContext context,
            IMapper mapper, IStringLocalizer<ModulosController> stringLocalizer)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioIdentidad = servicioIdentidad;
            this.stringLocalizer = stringLocalizer;


        }

        [AutorizarPermisos(PermisosModulos.PERMISO_LISTAR_MODULOS)]
        [HttpGet("[action]")]
        public async Task<ActionResult<List<ModuloDTO>>> Listar(CancellationToken cancellationToken)
        {
            var modulos = await context.Modulos.ToListAsync(cancellationToken);

            var modulosDTO = mapper.Map<List<ModuloDTO>>(modulos);

            return Ok(modulosDTO);
        }

        [AutorizarPermisos(PermisosModulos.PERMISO_LISTAR_MODULOS)]
        [HttpGet("[action]")]
        public async Task<ActionResult<List<ModuloDTO>>> ListarPorAdministrador(CancellationToken cancellationToken, [FromQuery] bool agruparPermisosEnModulo = false)
        {
            var nombreUsuario = HttpContext.User.Claims
                .FirstOrDefault(x => x.Type.ToUpper() == ClaimTypes.Name.ToUpper());
            var usuario = await context.Users
                .FirstOrDefaultAsync(x => x.NormalizedUserName == (nombreUsuario != null ? nombreUsuario.Value : ""));
            var roles = await servicioIdentidad.ObtenerRolesPorUsuario(usuario);
            List<ModuloDTO> modulosDTO = new List<ModuloDTO>();
            if (agruparPermisosEnModulo)
            {
                var permisosGrupos = context.Permisos
                        .Include(x => x.GrupoPermiso)
                        .ThenInclude(x => x.Modulo)
                        .ThenInclude(x => x.RolesModulos)
                        .Include(x => x.RolesPermisos)
                        .AsEnumerable()
                        .Where(x => x.GrupoPermiso.Modulo.RolesModulos.Any(p => roles.Any(k => k.RoleId == p.IdRol) && p.EsAdministrador))
                        //.Where(x => !x.GrupoPermiso.Ocultar)
                        //.Where(x => !x.Ocultar)
                        .GroupBy(x => new { x.GrupoPermiso.GrupoNombre, x.GrupoPermiso.GrupoNombreNormalizado, x.GrupoPermiso.Modulo.Id, x.GrupoPermiso.Modulo.Nombre })
                        .Select(p => new
                        {
                            p.Key,
                            Permisos = p.ToList()
                        });
                Dictionary<Tuple<Tuple<string, string>, Tuple<string, string>>, List<PermisoDTO>> permisosDic = permisosGrupos
                    .ToDictionary(x => Tuple.Create(Tuple.Create(x.Key.GrupoNombre, x.Key.GrupoNombreNormalizado), Tuple.Create(x.Key.Id.ToString(), x.Key.Nombre)), x => mapper.Map<List<PermisoDTO>>(x.Permisos));
                var permisoDicDTO = mapper.Map<List<PermisosAgrupadosDTO>>(permisosDic);
                List<int> groupoPermisoPorModulo = permisoDicDTO
                    .GroupBy(x => x.IdModulo)
                    .Select(p => p.Key).ToList();
                var modulos = await context.Modulos.Where(x => groupoPermisoPorModulo.Contains(x.Id)).ToListAsync(cancellationToken);
                modulosDTO = mapper.Map<List<ModuloDTO>>(modulos);
                modulosDTO.ForEach(e =>
                {
                    e.PermmisosAgrupados = permisoDicDTO.Where(x => x.IdModulo == e.Id).ToList();
                    e.NumeroGruposPermisos = permisoDicDTO.Count(x => x.IdModulo == e.Id);
                });

            }
            else
            {
                modulosDTO = context.Modulos
                    .Include(x => x.RolesModulos)
                    .ThenInclude(x => x.Roles)
                    .ThenInclude(x => x.RolesPermisos)
                    .ThenInclude(x => x.Permisos)
                    .ThenInclude(x => x.GrupoPermiso)
                    .AsEnumerable()
                    .Where(x => x.RolesModulos.Any(p => roles.Any(k => k.RoleId == p.IdRol) && p.EsAdministrador == true) && x.Estado)
                    //.Where(x => x.RolesModulos.Any(p => p.Roles.RolesPermisos.Any(t => !t.Permisos.GrupoPermiso.Ocultar)))
                    //.Where(x => x.RolesModulos.Any(p => p.Roles.RolesPermisos.Any(t => !t.Permisos.Ocultar)))
                    .Select(p => new ModuloDTO
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        NombreNormalizado = p.NombreNormalizado,
                        Descripcion = p.Descripcion,
                        Estado = p.Estado,
                        EsAdministrador = true,
                    })
                    .ToList();
            }

            return Ok(modulosDTO);
        }

        [AutorizarPermisos(PermisosModulos.PERMISO_CAMBIAR_ESTADO_MODULOS)]
        [HttpPatch("[action]/{id}")]
        public async Task<ActionResult> CambiarEstado([FromRoute] int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["IdMenorACero"].Value }

                });
            }

            var modulo = await context.Modulos.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (modulo == null)
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["RegistroNoEncontrado"].Value }

                });
            }

            modulo.Estado = !modulo.Estado;
            context.Entry(modulo).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["ErrorAlCambiarEstadoModulo"].Value }

                });
            }

            var activadoDesactivado = stringLocalizer["RegistroDeModuloDesactivadoConExito"].Value;

            if (modulo.Estado)
            {
                activadoDesactivado = stringLocalizer["RegistroDeModuloActivadoConExito"].Value;
            }

            return Ok(new RespuestaExitosaGenerica
            {
                Mensaje = activadoDesactivado,
            });
        }
    }
}
