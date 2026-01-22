namespace API.Controladores.Administracion
{
    using API.Database.Administracion;
    using API.Database.Administracion.DTOs.Autentificacion;
    using API.Database.Administracion.DTOs.Identidad;
    using API.Database.Administracion.DTOs.Modulos;
    using API.Database.Administracion.DTOs.Respuestas;
    using API.Database.Administracion.Entidades.Identidad;
    using API.Modelos.Configuraciones;
    using API.Servicios.Archivos;
    using API.Servicios.Preterminados.Autorizacion.PermisosAutorizacion.Controladores;
    using API.Servicios.Preterminados.Autorizacion.PermisosAutorizacion.Controladores.Administracion;
    using API.Servicios.Preterminados.Identidad;
    using API.Servicios.Propios.Identidad;
    using API.Servicios.Reportes.EXCEL;
    using API.Utilidades.Constantes;
    using AutoMapper;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Localization;
    using SkiaSharp;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IdentidadControlador" />
    /// </summary>
    [ApiExplorerSettings(GroupName = ConstantesModulos.ADMINISTRACION)]
    [Route("administracion/Identidad")]
    [ApiController]
    public class IdentidadControlador : Controller
    {
        /// <summary>
        /// Defines the servicioAutenticacion
        /// </summary>
        private readonly IServicioAutenticacion servicioAutenticacion;

        /// <summary>
        /// Defines the servicioAutorizacion
        /// </summary>
        private readonly IServicioAutorizacion servicioAutorizacion;

        /// <summary>
        /// Defines the context
        /// </summary>
        private readonly ModuloAdministracionExplosivosContext context;

        /// <summary>
        /// Defines the mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Defines the almacenador
        /// </summary>
        private readonly IAlmacenadorArchivos almacenador;

        /// <summary>
        /// Defines the CONTENEDOR_CARPETA
        /// </summary>
        public string CONTENEDOR_CARPETA = "Usuarios";

        /// <summary>
        /// Defines the stringLocalizer
        /// </summary>
        private readonly IStringLocalizer<IdentidadControlador> stringLocalizer;

        /// <summary>
        /// Defines the datosCliente
        /// </summary>
        private readonly DatosClienteDTO datosCliente;

        //private readonly HttpContext httpContext;

        /// <summary>
        /// Defines the splitMarker
        /// </summary>
        private readonly string splitMarker = ";";

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentidadControlador"/> class.
        /// </summary>
        /// <param name="servicioAutenticacion">The servicioAutenticacion<see cref="IServicioAutenticacion"/></param>
        /// <param name="servicioAutorizacion">The servicioAutorizacion<see cref="IServicioAutorizacion"/></param>
        /// <param name="context">The context<see cref="ModuloAdministracionExplosivosContext"/></param>
        /// <param name="mapper">The mapper<see cref="IMapper"/></param>
        /// <param name="almacenador">The almacenador<see cref="IAlmacenadorArchivos"/></param>
        /// <param name="stringLocalizer">The stringLocalizer<see cref="IStringLocalizer{IdentidadControlador}"/></param>
        /// <param name="datosCliente">The datosCliente<see cref="DatosClienteDTO"/></param>
        public IdentidadControlador(IServicioAutenticacion servicioAutenticacion, IServicioAutorizacion servicioAutorizacion,
            ModuloAdministracionExplosivosContext context, IMapper mapper, IAlmacenadorArchivos almacenador, IStringLocalizer<IdentidadControlador> stringLocalizer,
            DatosClienteDTO datosCliente)
        {
            this.servicioAutenticacion = servicioAutenticacion;
            this.servicioAutorizacion = servicioAutorizacion;
            this.context = context;
            this.mapper = mapper;
            this.almacenador = almacenador;
            this.stringLocalizer = stringLocalizer;
            this.datosCliente = datosCliente;
        }

        /// <summary>
        /// The ListarUsuarios
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [AutorizarPermisos(
            [
                PermisosIdentidad.PERMISO_LISTAR_USUARIOS,
                PermisosIdentidad.PERMISO_EDITAR_USUARIO,
                PermisosIdentidad.PERMISO_REGISTRAR_USUARIO,
                PermisosIdentidad.PERMISO_CAMBIAR_ESTADO_USUARIO,
                PermisosIdentidad.PERMISO_BORRAR_USUARIO,
            ]
        )]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("ListarUsuarios")]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> ListarUsuarios(CancellationToken cancellationToken)
        {
            var usuarios = await context.Users
                .AsNoTracking()
                .Where(u => !u.UsuariosRoles.Any(ur => ur.Rol.EstaOculto))
                .Select(u => new UsuarioDTO
                {
                    Id = u.Id,
                    IdRol = u.UsuariosRoles.FirstOrDefault().RoleId,
                    NombreRol = u.UsuariosRoles.FirstOrDefault().Rol.Name,
                    Usuario = u.UserName,
                    Email = u.Email,
                    Nombre = u.Nombre,
                    Apellidos = u.Apellidos,
                    NombreCompleto = u.NombreCompleto,
                    Foto = u.Foto,
                    FechaRegistro = u.FechaRegistro,
                    FechaModificacion = u.FechaModificacion,
                    AreaSeleccionada = string.Empty,
                    Estado = u.Estado
                })
                .ToListAsync(cancellationToken);

            return Ok(usuarios);
        }

        /// <summary>
        /// The ListarPermisos
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <param name="grupo">The grupo<see cref="bool"/></param>
        /// <param name="idModulos">The idModulos<see cref="int[]"/></param>
        /// <param name="incluirOcultados">The incluirOcultados<see cref="bool"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [AutorizarPermisos(PermisosIdentidad.PERMISO_LISTAR_PERMISOS)]
        [HttpGet("ListarPermisos")]
        public async Task<ActionResult> ListarPermisos(
    CancellationToken cancellationToken,
    [FromQuery] bool grupo = false,
    [FromQuery] int[] idModulos = null,
    [FromQuery] bool incluirOcultados = true)
        {
            // Consulta base optimizada
            var query = context.Permisos
                .AsNoTracking()
                .Include(p => p.GrupoPermiso)
                    .ThenInclude(g => g.Modulo)
                .Include(p => p.RolesPermisos)
                .Where(p => (idModulos == null || idModulos.Length == 0 || idModulos.Contains(p.GrupoPermiso.Modulo.Id)))
                .Where(p => incluirOcultados || (!p.GrupoPermiso.Ocultar && !p.Ocultar));

            if (grupo)
            {
                // Versión agrupada optimizada
                var permisosAgrupados = await query
                    .GroupBy(p => new
                    {
                        p.GrupoPermiso.GrupoNombre,
                        p.GrupoPermiso.GrupoNombreNormalizado,
                        p.GrupoPermiso.Modulo.Id,
                        p.GrupoPermiso.Modulo.Nombre,
                        p.GrupoPermiso.Modulo.NombreNormalizado,
                        p.GrupoPermiso.Modulo.Descripcion
                    })
                    .Select(g => new PermisosAgrupadosDTO
                    {
                        GrupoNombre = g.Key.GrupoNombre,
                        GrupoNombreNormalizado = g.Key.GrupoNombreNormalizado,
                        NumeroPermisos = g.Count(),
                        IdModulo = g.Key.Id,
                        NombreModulo = g.Key.Nombre,
                        Permisos = g.Select(p => new PermisoDTO
                        {
                            Id = p.Id,
                            Nombre = p.Nombre,
                            NombreNormalizado = p.NombreNormalizado,
                            GrupoNombre = p.GrupoPermiso.GrupoNombre,
                            GrupoNombreNormalizado = p.GrupoPermiso.GrupoNombreNormalizado,
                            IdRolesAsignados = p.RolesPermisos.Select(rp => rp.IdRol).ToList(),
                            Descripcion = p.Descripcion,
                            IdModulo = p.GrupoPermiso.Modulo.Id,
                            NombreModulo = p.GrupoPermiso.Modulo.Nombre,
                            NombreNormalizadoModulo = p.GrupoPermiso.Modulo.NombreNormalizado,
                            DescripcionModulo = p.GrupoPermiso.Modulo.Descripcion
                        }).ToList()
                    })
                    .ToListAsync(cancellationToken);

                return Ok(permisosAgrupados);
            }
            else
            {
                // Versión no agrupada optimizada
                var permisos = await query
                    .Select(p => new PermisoDTO
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        NombreNormalizado = p.NombreNormalizado,
                        GrupoNombre = p.GrupoPermiso.GrupoNombre,
                        GrupoNombreNormalizado = p.GrupoPermiso.GrupoNombreNormalizado,
                        IdRolesAsignados = p.RolesPermisos.Select(rp => rp.IdRol).ToList(),
                        Descripcion = p.Descripcion,
                        IdModulo = p.GrupoPermiso.Modulo.Id,
                        NombreModulo = p.GrupoPermiso.Modulo.Nombre,
                        NombreNormalizadoModulo = p.GrupoPermiso.Modulo.NombreNormalizado,
                        DescripcionModulo = p.GrupoPermiso.Modulo.Descripcion
                    })
                    .ToListAsync(cancellationToken);

                return Ok(permisos);
            }
        }

        /// <summary>
        /// The ListarRoles
        /// </summary>
        /// <param name="soloActivos">The soloActivos<see cref="bool"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [AutorizarPermisos(
    PermisosIdentidad.PERMISO_LISTAR_ROLES,
    PermisosIdentidad.PERMISO_CREAR_ROL,
    PermisosIdentidad.PERMISO_EDITAR_ROL,
    PermisosIdentidad.PERMISO_CAMBIAR_ESTADO_ROL,
    PermisosIdentidad.PERMISO_ASIGNAR_ROL,
    PermisosIdentidad.GUID_BORRAR_ROL
)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("ListarRoles/{soloActivos}")]
        public async Task<ActionResult<IEnumerable<RolDTO>>> ListarRoles(bool soloActivos = false)
        {
            // Obtener información del usuario actual de manera eficiente
            var nombreUsuario = HttpContext.User.Claims
                .FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name, StringComparison.OrdinalIgnoreCase))?.Value;

            var usuario = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.NormalizedUserName == (nombreUsuario ?? "").ToUpper());

            if (usuario == null)
                return Ok(new List<RolDTO>());

            // Obtener roles del usuario con la información necesaria
            var rolesDeUsuarioActual = await context.UserRoles
                .AsNoTracking()
                .Include(ur => ur.Rol)
                .Where(ur => ur.UserId == usuario.Id)
                .ToListAsync();

            bool esSuperUsuarioOAdmin = rolesDeUsuarioActual.Exists(x =>
                x.Rol.SuperUsuario ||
                x.Rol.NormalizedName.Equals(ConstantesRoles.ADMINISTRADOR_PREDETERMINADO, StringComparison.OrdinalIgnoreCase));

            // Consulta principal optimizada con carga de relaciones necesarias
            var query = context.Roles
                .AsNoTracking()
                .Include(r => r.RolesModulos)
                    .ThenInclude(rm => rm.Modulos)
                .Include(r => r.RolesPermisos)
                .Where(r => !r.SuperUsuario && (esSuperUsuarioOAdmin || !r.EstaOculto));

            if (soloActivos)
            {
                query = query.Where(r => r.Estado);
            }

            // Filtrado para usuarios no administradores
            if (!esSuperUsuarioOAdmin)
            {
                var modulosAdministrables = await context.UserRoles
                    .AsNoTracking()
                    .Where(ur => ur.UserId == usuario.Id)
                    .Join(context.RolesModulos,
                        ur => ur.RoleId,
                        rm => rm.IdRol,
                        (ur, rm) => new { rm.IdModulo, rm.EsAdministrador })
                    .Where(x => x.EsAdministrador)
                    .Select(x => x.IdModulo)
                    .Distinct()
                    .ToListAsync();

                if (modulosAdministrables.Count == 0)
                {
                    return Ok(new List<RolDTO>());
                }

                query = query.Where(r => r.RolesModulos.All(rm => modulosAdministrables.Contains(rm.IdModulo)));
            }

            // Proyección directa a DTO con toda la información necesaria
            var rolesDTO = await query
                .Select(r => new RolDTO
                {
                    Id = r.Id,
                    NombreRol = r.Name,
                    NombreRolNormalizado = r.NormalizedName,
                    Descripcion = r.Descripcion,
                    Estado = r.Estado,
                    NumeroPermisos = r.RolesPermisos.Count,
                    ModulosActivados = r.RolesModulos.Select(rm => new ModuloDTO
                    {
                        Id = rm.Modulos.Id,
                        Nombre = rm.Modulos.Nombre,
                        NombreNormalizado = rm.Modulos.NombreNormalizado,
                        Descripcion = rm.Modulos.Descripcion,
                        Estado = rm.Modulos.Estado,
                        EsAdministrador = rm.EsAdministrador,
                        NumeroGruposPermisos = context.GruposPermisos
                            .Count(gp => gp.IdModulo == rm.Modulos.Id)
                    }).ToList()
                })
                .ToListAsync();

            return Ok(rolesDTO);
        }

        /// <summary>
        /// The ObtenerPermisosOtorgados
        /// </summary>
        /// <param name="usuarioOCorreo">The usuarioOCorreo<see cref="string"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("[action]/{usuarioOCorreo}")]
        public async Task<ActionResult<IEnumerable<PermisoOtorgadoDTO>>> ObtenerPermisosOtorgados([FromRoute] string usuarioOCorreo)
        {
            // Consulta optimizada que proyecta directamente al DTO
            var permisosDTO = await context.Users
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

            return Ok(permisosDTO);
        }

        /// <summary>
        /// The ObtenerModulosOtorgados
        /// </summary>
        /// <param name="usuarioOCorreo">The usuarioOCorreo<see cref="string"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("[action]/{usuarioOCorreo}")]
        public async Task<ActionResult> ObtenerModulosOtorgados([FromRoute] string usuarioOCorreo)
        {
            var modulosOtorgadosDTO = await servicioAutorizacion.ObtenerModulosOtorgados(usuarioOCorreo);

            return Ok(modulosOtorgadosDTO);
        }

        /// <summary>
        /// The Registrar
        /// </summary>
        /// <param name="registroUsuarioDTO">The registroUsuarioDTO<see cref="RegistroUsuarioDTO"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [AutorizarPermisos(PermisosIdentidad.PERMISO_REGISTRAR_USUARIO)]
        [HttpPost("Registrar")]
        public async Task<ActionResult> Registrar([FromBody] RegistroUsuarioDTO registroUsuarioDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFallidaRespuesta
                {
                    Errores = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage))
                });
            }

            var autRespuesta = await servicioAutenticacion.RegistrarAsync(registroUsuarioDTO);

            if (!autRespuesta.OperacionExitosa)
            {
                return BadRequest(new AuthFallidaRespuesta
                {
                    Errores = autRespuesta.Errores
                });
            }

            return Ok(new RespuestaExitosaGenerica
            {
                Mensaje = stringLocalizer["UsuarioRegistrado"].Value,
            });
        }

        /// <summary>
        /// The EditarUsuario
        /// </summary>
        /// <param name="model">The model<see cref="EditarUsuarioDTO"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [AutorizarPermisos(PermisosIdentidad.PERMISO_EDITAR_USUARIO)]
        [HttpPut("[action]")]
        public async Task<ActionResult> EditarUsuario([FromBody] EditarUsuarioDTO model)
        {
            var usuario = await context.Users.FirstOrDefaultAsync(x => x.Id == model.IdUsuario);
            if (usuario == null)
            {
                return BadRequest(new AuthFallidaRespuesta
                {
                    Errores = new List<string> { stringLocalizer["UsuarioNoEncontrado"].Value }

                });
            }

            var roles = await servicioAutorizacion.ObtenerRolesPorUsuario(usuario);

            if (roles.Any(x => x.Rol.EstaOculto))
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["AccionDenegada"].Value }

                });
            }

            mapper.Map(model, usuario);

            if (!String.IsNullOrWhiteSpace(model.Password))
            {
                var respuestaCambiarPassword = await servicioAutenticacion.CambiarPassword(new CambiarPasswordDTO
                {
                    Id = model.IdUsuario,
                    PasswordNuevo = model.Password,
                });

                if (!respuestaCambiarPassword.OperacionExitosa)
                {
                    return BadRequest(new RespuestaFallidaGenerica
                    {
                        Errores = respuestaCambiarPassword.Errores
                    });
                }

            }

            context.Entry(usuario).State = EntityState.Modified;

            if (!String.IsNullOrWhiteSpace(model.IdRol))
            {
                var usuariosRoles = await context.UserRoles.FirstOrDefaultAsync(x => x.UserId == model.IdUsuario);
                if (usuariosRoles != null)
                {
                    context.UserRoles.Remove(usuariosRoles);
                }
                context.UserRoles.Add(new UsuariosRoles()
                {
                    UserId = model.IdUsuario,
                    RoleId = model.IdRol,
                });
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["ErrorAlEditarUsuario"].Value }

                });
            }

            return Ok(new RespuestaExitosaGenerica
            {
                Mensaje = stringLocalizer["RegistroEditadoConExito"].Value
            });
        }

        /// <summary>
        /// The EditarMiUsuario
        /// </summary>
        /// <param name="model">The model<see cref="EditarMiUsuarioDTO"/></param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("[action]")]
        public async Task<ActionResult> EditarMiUsuario([FromBody] EditarMiUsuarioDTO model, CancellationToken cancellationToken)
        {
            var usuario = await context.Users.FirstOrDefaultAsync(x => x.Id == model.IdUsuario, cancellationToken);
            if (usuario == null)
            {
                return BadRequest(new AuthFallidaRespuesta
                {
                    Errores = new List<string> { stringLocalizer["UsuarioNoEncontrado"].Value }

                });
            }

            var roles = await servicioAutorizacion.ObtenerRolesPorUsuario(usuario);

            if (roles.Any(x => x.Rol.EstaOculto))
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["AccionDenegada"].Value }

                });
            }

            mapper.Map(model, usuario);

            if (!String.IsNullOrWhiteSpace(model.Password))
            {
                var respuestaCambiarPassword = await servicioAutenticacion.CambiarPassword(new CambiarPasswordDTO
                {
                    Id = model.IdUsuario,
                    PasswordNuevo = model.Password,
                });

                if (!respuestaCambiarPassword.OperacionExitosa)
                {
                    return BadRequest(new RespuestaFallidaGenerica
                    {
                        Errores = respuestaCambiarPassword.Errores
                    });
                }

            }

            context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["ErrorAlEditarUsuario"].Value }

                });
            }

            return Ok(new RespuestaExitosaGenerica
            {
                Mensaje = stringLocalizer["RegistroEditadoConExito"].Value
            });
        }

        //[AutorizarPermisos(PermisosTiposEquiposVP.PermisoEditarTipoEquipo)]

        /// <summary>
        /// The CambiarFotoPerfil
        /// </summary>
        /// <param name="model">The model<see cref="CambiarFotoPerfilDTO"/></param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("[action]")]
        public async Task<ActionResult> CambiarFotoPerfil([FromForm] CambiarFotoPerfilDTO model, CancellationToken cancellationToken)
        {
            if (model.IdUsuario == null)
            {

                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["IdVacio"].Value }

                });
            }

            if (model.Foto == null)
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["FotoVacia"].Value }

                });
            }

            var usuario = await context.Users.FirstOrDefaultAsync(x => x.Id == model.IdUsuario, cancellationToken);

            if (usuario == null)
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["UsuarioNoEncontrado"].Value }

                });
            }
            var fotoAnterior = usuario.Foto;

            mapper.Map(model, usuario);

            if (model.Foto != null)
            {
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.Foto.CopyToAsync(memoryStream);
                        var contenido = memoryStream.ToArray();
                        var extension = Path.GetExtension(model.Foto.FileName);
                        usuario.Foto = await almacenador.GuardarArchivo(contenido, extension, CONTENEDOR_CARPETA, model.Foto.ContentType);
                    }
                }
                catch (Exception e)
                {
                    var xd = e.Message;
                }
            }
            else
            {
                usuario.Foto = fotoAnterior;
            }

            context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync(cancellationToken);
                if (!String.IsNullOrWhiteSpace(fotoAnterior))
                {
                    await almacenador.BorrarArchivo(fotoAnterior, CONTENEDOR_CARPETA);
                }
            }
            catch
            {
                if (!String.IsNullOrWhiteSpace(usuario.Foto))
                {
                    await almacenador.BorrarArchivo(usuario.Foto, CONTENEDOR_CARPETA);
                }
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["ErrorAlEditarUsuario"].Value }

                });
            }

            return Ok(new RespuestaCambioFotoPerfil
            {
                Mensaje = stringLocalizer["RegistroEditadoConExito"].Value,
                Foto = usuario.Foto,
            });
        }

        /// <summary>
        /// The SolicitarRecuperacionDeCuenta
        /// </summary>
        /// <param name="model">The model<see cref="UsuarioOCorreoModel"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [AllowAnonymous]
        [HttpPost("SolicitarRecuperacionDeCuenta")]
        public async Task<ActionResult> SolicitarRecuperacionDeCuenta([FromBody] UsuarioOCorreoModel model)
        {

            var autRespuesta = await servicioAutenticacion.SolicitarRecuperacionDeCuenta(model.UsuarioOCorreo);

            if (!autRespuesta.OperacionExitosa)
            {
                return BadRequest(autRespuesta);
            }

            return Ok(autRespuesta);
        }

        /// <summary>
        /// The VerificarCodigoRecuperacion
        /// </summary>
        /// <param name="codigoDTO">The codigoDTO<see cref="CodigoRecuperacionDTO"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [AllowAnonymous]
        [HttpPost("VerificarCodigoRecuperacion")]
        public async Task<ActionResult> VerificarCodigoRecuperacion([FromBody] CodigoRecuperacionDTO codigoDTO)
        {
            var autRespuesta = await servicioAutenticacion.VerificarCodigoRecuperacion(codigoDTO.CodigoRecuperacion);

            if (!autRespuesta.OperacionExitosa)
            {
                return BadRequest(autRespuesta);
            }

            return Ok(autRespuesta);
        }

        /// <summary>
        /// The RecuperarCuenta
        /// </summary>
        /// <param name="cambiarPasswordRecuperadoDTO">The cambiarPasswordRecuperadoDTO<see cref="CambiarPasswordRecuperadoDTO"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [AllowAnonymous]
        [HttpPost("RecuperarCuenta")]
        public async Task<ActionResult> RecuperarCuenta([FromBody] CambiarPasswordRecuperadoDTO cambiarPasswordRecuperadoDTO)
        {
            var autRespuesta = await servicioAutenticacion.RecuperarCuenta(cambiarPasswordRecuperadoDTO);

            if (!autRespuesta.OperacionExitosa)
            {
                return BadRequest(autRespuesta);
            }

            return Ok(autRespuesta);
        }

        /// <summary>
        /// The IniciarSesion
        /// </summary>
        /// <param name="inicioSesionDTO">The inicioSesionDTO<see cref="InicioSesionDTO"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [AllowAnonymous]
        [HttpPost("IniciarSesion")]
        public async Task<ActionResult> IniciarSesion([FromBody] InicioSesionDTO inicioSesionDTO)
        {

            var autRespuesta = await servicioAutenticacion.IniciarSesionAsync(inicioSesionDTO.UsuarioOCorreo, inicioSesionDTO.Password);

            if (!autRespuesta.OperacionExitosa)
            {
                return BadRequest(new AuthFallidaRespuesta
                {
                    Errores = autRespuesta.Errores
                });
            }

            var usuario = await servicioAutenticacion.ObtenerInformacionUsuarioAsync(inicioSesionDTO.UsuarioOCorreo);

            if (!usuario.OperacionExitosa)
            {
                return BadRequest(new AuthFallidaRespuesta
                {
                    Errores = usuario.Errores
                });
            }

            return Ok(new AutExitosaRespuesta
            {
                Token = autRespuesta.Token,
                ActualizarToken = autRespuesta.ActualizarToken,
                Expiracion = autRespuesta.Expiracion,
                DatosUsuario = usuario,
                EsMantenimiento = autRespuesta.EsMantenimiento,
            });
        }

        /// <summary>
        /// The ActualizarToken
        /// </summary>
        /// <param name="actualizarTokenDTO">The actualizarTokenDTO<see cref="ActualizarTokenDTO"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("ActualizarToken")]
        public async Task<ActionResult> ActualizarToken([FromBody] ActualizarTokenDTO actualizarTokenDTO)
        {

            var autRespuesta = await servicioAutenticacion.ActualizarToken(actualizarTokenDTO.Token, actualizarTokenDTO.ActualizarToken, actualizarTokenDTO.Expiracion);

            // ! Si la operación no es exitosa envia un error de token sin refrescarlo
            // ! En un dado caso de que el token no haya expirado, la variable tokenExpirado es falsa
            // ! Esto evita reasignar el token desde el front end y saber cual error debe de realizar logout y cual no
            if (!autRespuesta.OperacionExitosa)
            {
                return Ok(new AuthFallidaRespuesta
                {
                    OperacionExitosa = autRespuesta.OperacionExitosa,
                    TokenExpirado = autRespuesta.TokenExpirado,
                    Errores = autRespuesta.Errores,
                });
            }

            var nombreUsuario = IdentidadExtensiones.ObtenerUsernameUsuario(HttpContext);
            var usuario = await servicioAutenticacion.ObtenerInformacionUsuarioAsync(nombreUsuario);

            if (!usuario.OperacionExitosa)
            {
                return BadRequest(new AuthFallidaRespuesta
                {
                    Errores = usuario.Errores
                });
            }

            return Ok(new AutExitosaRespuesta
            {
                OperacionExitosa = autRespuesta.OperacionExitosa,
                Token = autRespuesta.Token,
                ActualizarToken = autRespuesta.ActualizarToken,
                Expiracion = autRespuesta.Expiracion,
                DatosUsuario = usuario,
            });
        }

        /// <summary>
        /// The AsignarRol
        /// </summary>
        /// <param name="model">The model<see cref="EditarRolUsuarioDTO"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [AutorizarPermisos(PermisosIdentidad.PERMISO_ASIGNAR_ROL)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("[action]")]
        public async Task<ActionResult> AsignarRol([FromBody] EditarRolUsuarioDTO model)
        {
            var respuestaAsignarRol = await servicioAutorizacion.AsignarRol(model);

            if (!respuestaAsignarRol.OperacionExitosa)
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = respuestaAsignarRol.Errores
                });
            }

            return Ok(new RespuestaExitosaGenerica
            {
                Mensaje = stringLocalizer["RolAsignadoExito"].Value
            });
        }

        /// <summary>
        /// The CambiarPassword
        /// </summary>
        /// <param name="model">The model<see cref="CambiarPasswordDTO"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [AutorizarPermisos(PermisosIdentidad.PERMISO_CAMBIAR_PASSWORD)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPatch("[action]")]
        public async Task<ActionResult> CambiarPassword([FromBody] CambiarPasswordDTO model)
        {
            var respuestaCambiarPassword = await servicioAutenticacion.CambiarPassword(model);

            if (!respuestaCambiarPassword.OperacionExitosa)
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = respuestaCambiarPassword.Errores
                });
            }

            return Ok(new RespuestaExitosaGenerica
            {
                Mensaje = stringLocalizer["ContrasenaActualizadaExito"].Value

            });
        }

        /// <summary>
        /// The CambiarCorreo
        /// </summary>
        /// <param name="model">The model<see cref="CambiarCorreoDTO"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [AutorizarPermisos(PermisosIdentidad.PERMISO_CAMBIAR_CORREO)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPatch("[action]")]
        public async Task<ActionResult> CambiarCorreo([FromBody] CambiarCorreoDTO model)
        {
            var respuestaCambiarPassword = await servicioAutenticacion.CambiarCorreo(model);

            if (!respuestaCambiarPassword.OperacionExitosa)
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = respuestaCambiarPassword.Errores
                });
            }

            return Ok(new RespuestaExitosaGenerica
            {
                Mensaje = stringLocalizer["CorreoActualizadoExito"].Value

            });
        }

        /// <summary>
        /// The CambiarNombre
        /// </summary>
        /// <param name="model">The model<see cref="CambiarNombreDTO"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [AutorizarPermisos(PermisosIdentidad.PERMISO_CAMBIAR_NOMBRE)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPatch("[action]")]
        public async Task<ActionResult> CambiarNombre([FromBody] CambiarNombreDTO model)
        {
            var respuestaCambiarPassword = await servicioAutenticacion.CambiarNombre(model);

            if (!respuestaCambiarPassword.OperacionExitosa)
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = respuestaCambiarPassword.Errores
                });
            }

            return Ok(new RespuestaExitosaGenerica
            {
                Mensaje = stringLocalizer["NombreActualizadoExito"].Value

            });
        }

        /// <summary>
        /// The CrearRol
        /// </summary>
        /// <param name="crearRolDTO">The crearRolDTO<see cref="CrearRolDTO"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [AutorizarPermisos(PermisosIdentidad.PERMISO_CREAR_ROL)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("[action]")]
        public async Task<ActionResult> CrearRol([FromBody] CrearRolDTO crearRolDTO)
        {
            var respuestaCrearRol = await servicioAutorizacion.CrearRol(crearRolDTO);

            if (!respuestaCrearRol.OperacionExitosa)
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = respuestaCrearRol.Errores,
                });
            }

            return Ok(new RespuestaExitosaGenerica
            {
                Mensaje = stringLocalizer["RolCreadoExito"].Value

            });
        }

        /// <summary>
        /// The EditarRol
        /// </summary>
        /// <param name="model">The model<see cref="EditarRol"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [AutorizarPermisos(PermisosIdentidad.PERMISO_EDITAR_ROL)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("[action]")]
        public async Task<ActionResult> EditarRol([FromBody] EditarRol model)
        {
            var nombreUsuario = HttpContext.User.Claims.FirstOrDefault(x => x.Type.ToUpper() == ClaimTypes.Name.ToUpper());
            var usuario = await context.Users.FirstOrDefaultAsync(x => x.NormalizedUserName == (nombreUsuario != null ? nombreUsuario.Value : ""));
            var rolesDeUsuarioActual = usuario != null ? await servicioAutorizacion.ObtenerRolesPorUsuario(usuario) : new List<UsuariosRoles>();
            var esSuperUsuario = rolesDeUsuarioActual.Exists(x => x.Rol.SuperUsuario);

            var respuesta = await servicioAutorizacion.EditarRol(model, esSuperUsuario);

            if (!respuesta.OperacionExitosa)
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = respuesta.Errores
                });
            }

            return Ok(new RespuestaExitosaGenerica
            {
                Mensaje = respuesta.Mensaje
            });
        }

        /// <summary>
        /// The BorrarRol
        /// </summary>
        /// <param name="idRol">The idRol<see cref="string"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [AutorizarPermisos(PermisosIdentidad.PERMISO_BORRAR_ROL)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[AllowAnonymous]
        [HttpDelete("[action]/{idRol}")]
        public async Task<ActionResult> BorrarRol([FromRoute] string idRol)
        {
            var resultado = await servicioAutorizacion.BorrarRol(idRol);

            if (!resultado.OperacionExitosa)
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = resultado.Errores
                });
            }

            return Ok(new RespuestaExitosaGenerica
            {
                Mensaje = resultado.Mensaje
            });
        }

        /// <summary>
        /// The BorrarUsuario
        /// </summary>
        /// <param name="idUsuario">The idUsuario<see cref="string"/></param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [AutorizarPermisos(PermisosIdentidad.PERMISO_BORRAR_USUARIO)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("[action]/{idUsuario}")]
        public async Task<ActionResult> BorrarUsuario([FromRoute] string idUsuario, CancellationToken cancellationToken)
        {
            var usuario = await context.Users.FirstOrDefaultAsync(x => x.Id == idUsuario, cancellationToken);

            if (usuario == null)
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["UsuarioNoEncontrado"].Value }
                });
            }

            var roles = await servicioAutorizacion.ObtenerRolesPorUsuario(usuario);

            if (roles.Any(x => x.Rol.EstaOculto))
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["AccionDenegadaAlBorrarUsuario"].Value + ConstantesRoles.SUPER_USUARIO_G3 }

                    //Errores = new List<string> { "Acción denegada al intentar borrar el usuario de " + ConstantesRoles.SUPER_USUARIO_G3 }
                });
            }

            context.Users.Remove(usuario);

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                var xd = e.Message;
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["ErrorAlBorrarUsuario"].Value }
                });
            }

            return Ok(new RespuestaExitosaGenerica
            {
                Mensaje = stringLocalizer["RegistroUsuarioEliminado"].Value,
            });
        }

        /// <summary>
        /// The CambiarEstadoUsuario
        /// </summary>
        /// <param name="model">The model<see cref="CambiarEstadoUsuarioDTO"/></param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [AutorizarPermisos(PermisosIdentidad.PERMISO_CAMBIAR_ESTADO_USUARIO)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPatch("[action]")]
        public async Task<ActionResult> CambiarEstadoUsuario([FromBody] CambiarEstadoUsuarioDTO model, CancellationToken cancellationToken)
        {
            var usuario = await context.Users.FirstOrDefaultAsync(x => x.Id == model.Id, cancellationToken);
            if (usuario == null)
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["UsuarioNoEncontrado"].Value }

                });
            }

            var roles = await servicioAutorizacion.ObtenerRolesPorUsuario(usuario);

            if (roles.Any(x => x.Rol.EstaOculto))
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["AccionDenegadaAlCambiarEstadoUsuario"].Value + ConstantesRoles.SUPER_USUARIO_G3 }

                    //Errores = new List<string> { "Acción denegada al cambiar el estado del usuario de " + ConstantesRoles.SUPER_USUARIO_G3 }
                });
            }

            usuario.Estado = !usuario.Estado;

            context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["ErrorCambiarEstadoUsuario"].Value }

                });
            }

            var activadoDesactivado = stringLocalizer["RegistroUsuarioDesactivado"].Value;

            if (usuario.Estado)
            {
                activadoDesactivado = stringLocalizer["RegistroUsuarioActivado"].Value;
            }

            return Ok(new RespuestaExitosaGenerica
            {
                Mensaje = activadoDesactivado,
            });
        }

        /// <summary>
        /// The CambiarEstadoRol
        /// </summary>
        /// <param name="model">The model<see cref="CambiarEstadoRolDTO"/></param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [AutorizarPermisos(PermisosIdentidad.PERMISO_CAMBIAR_ESTADO_ROL)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPatch("[action]")]
        public async Task<ActionResult> CambiarEstadoRol([FromBody] CambiarEstadoRolDTO model, CancellationToken cancellationToken)
        {
            var rol = await context.Roles.FirstOrDefaultAsync(x => x.Id == model.IdRol, cancellationToken);
            var usuario = await context.Users.FirstOrDefaultAsync(x => x.Id == model.IdUsuario, cancellationToken);

            if (rol == null)
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["RolNoEncontrado"].Value }

                });
            }

            if (usuario == null)
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["UsuarioNoEncontrado"].Value }

                });
            }

            if (rol.EstaOculto)
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["NoPosibleCambiarEstadoRol"].Value + ConstantesRoles.SUPER_USUARIO_G3 }

                    //Errores = new List<string> { "No es posible cambiar el estado del rol de " + ConstantesRoles.SUPER_USUARIO_G3 }
                });
            }

            var rolesUsuario = await servicioAutorizacion.ObtenerRolesPorUsuario(usuario);
            //string rolPorProhibir = "Administrador".ToUpper();
            if (!rolesUsuario.Any(x => x.Rol.EstaOculto))
            {
                if (
                    rolesUsuario.Any(x => x.Rol.NormalizedName == ConstantesRoles.ADMINISTRADOR_PREDETERMINADO.ToUpper())
                    &&
                    rol.NormalizedName == ConstantesRoles.ADMINISTRADOR_PREDETERMINADO.ToUpper()
                    )
                {
                    return BadRequest(new RespuestaFallidaGenerica
                    {
                        Errores = new List<string> { stringLocalizer["NoPosibleCambiarEstadoRolAdministrador"].Value }

                        //Errores = new List<string> { "No es posible cambiar el estado del rol de un administrador siendo un administrador" }
                    });
                }

                if (rol.NormalizedName == ConstantesRoles.LIMITADO.ToUpper())
                {
                    return BadRequest(new RespuestaFallidaGenerica
                    {
                        Errores = new List<string> { stringLocalizer["NoPosibleCambiarEstadoRolLimitado"].Value }

                        //Errores = new List<string> { "No es posible cambiar el estado del rol Limitado" }
                    });
                }
            }

            rol.Estado = !rol.Estado;
            context.Entry(rol).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["ErrorAlCambiarEstadoRol"].Value }

                });
            }

            var activadoDesactivado = stringLocalizer["RolDesactivadoExito"].Value;
            if (rol.Estado) activadoDesactivado = stringLocalizer["RolActivadoExito"].Value;

            return Ok(new RespuestaExitosaGenerica
            {
                Mensaje = activadoDesactivado
            });
        }

        /// <summary>
        /// The ReporteUsuarios
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("[action]")]
        public async Task<ActionResult> ReporteUsuarios(CancellationToken cancellationToken)
        {
            var usuarios = await context.Users
                .Include(x => x.UsuariosRoles)
                .ThenInclude(x => x.Rol)
                .ThenInclude(x => x.RolesModulos)
                .ThenInclude(x => x.Modulos)
                .Where(x => !x.UsuariosRoles.Any(p => p.Rol.EstaOculto))
                .ToListAsync(cancellationToken);

            var usuariosDTO = mapper.Map<List<UsuarioDTO>>(usuarios);

            var roles = await context.Roles
                .Include(x => x.RolesPermisos)
                .ThenInclude(x => x.Permisos)
                .ThenInclude(x => x.GrupoPermiso)
                .ThenInclude(x => x.Modulo)
                .Include(x => x.RolesModulos)
                .ThenInclude(x => x.Modulos)
                .Where(x => !x.EstaOculto)
                .ToListAsync(cancellationToken);

            var reporteoUsuariosDTO = new ReporteUsuariosDTO
            {
                Usuarios = usuarios.Select(x =>
            {
                var rol = x.UsuariosRoles
                    .FirstOrDefault();
                var modulosHabilitados = x.UsuariosModulos
                    .Select(x => x.Modulos.Nombre)
                    .ToList();

                var usuarioReporteo = new UsuariosReporteoDTO
                {
                    NombreCompleto = x.NombreCompleto,
                    Usuario = x.UserName,
                    Correo = String.IsNullOrWhiteSpace(x.Email) ? "-" : x.Email,
                };

                if (rol != null)
                {
                    usuarioReporteo.NombreRol = rol.Rol.Name;
                }

                return usuarioReporteo;
            })
            .ToList(),

                Roles = roles.Select(x =>
                {
                    var asd = x.RolesPermisos.Select(x => x.Permisos).ToList();

                    var rolReporteoDTO = new RolesReporteoDTO
                    {
                        NombreRol = x.Name,
                        Descripcion = x.Descripcion,
                        Permisos = mapper.Map<List<PermisoDTO>>(x.RolesPermisos.Select(x => x.Permisos).ToList())
                    };

                    return rolReporteoDTO;
                })
            .ToList()
            };

            var reporteBytes = new ReporteEXCELUsuarios().Generar(reporteoUsuariosDTO, datosCliente.RazonSocial);

            return File(reporteBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte_Usuarios_GFuel.xlsx");
        }
    }
}
