using API.Database.Administracion;
using API.Database.Administracion.DTOs.CorreosAutomaticos;
using API.Database.Administracion.DTOs.Identidad;
using API.Database.Administracion.DTOs.Respuestas;
using API.Database.Administracion.Entidades.General;
using API.Utilidades.Constantes;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controladores.Administracion
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = ConstantesModulos.ADMINISTRACION)]
    [Route("administracion/[controller]")]
    [ApiController]
    public class CorreosAutomaticosController : ControllerBase
    {
        public ModuloAdministracionExplosivosContext context { get; }
        public IMapper mapper { get; }
        public string SPLIT_MARKER = ";";
        private readonly IStringLocalizer<CorreosAutomaticosController> stringLocalizer;

        public CorreosAutomaticosController(ModuloAdministracionExplosivosContext context, IMapper mapper, IStringLocalizer<CorreosAutomaticosController> stringLocalizer)
        {
            this.context = context;
            this.mapper = mapper;
            this.stringLocalizer = stringLocalizer;

        }

        [HttpGet("[action]")]
        public async Task<ActionResult> Listar(CancellationToken cancellationToken)
        {
            var correosAutomaticos = await context.CorreosAutomaticos
                .AsNoTracking()
                .Where(x => !x.Ocultar)
                .OrderByDescending(x => x.NombreModulo)
                .ToListAsync(cancellationToken);

            var correosAutomaticosDTO = mapper.Map<List<CorreoAutomaticoDTO>>(correosAutomaticos);

            return Ok(correosAutomaticosDTO);
        }

        [HttpGet("ListarDestinatarios")]
        public async Task<ActionResult> ListarDestinatarios(CancellationToken cancellationToken)
        {
            var usuarios = await context.Users
                .AsNoTracking()
                .Include(x => x.UsuariosRoles)
                .ThenInclude(x => x.Rol)
                .ThenInclude(x => x.RolesModulos)
                .ThenInclude(x => x.Modulos)
                .Where(x => !x.UsuariosRoles.Any(p => p.Rol.EstaOculto))
                .ToListAsync(cancellationToken);

            var usuariosDTO = mapper.Map<List<UsuarioDTO>>(usuarios);

            return Ok(usuariosDTO);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearCorreoAutomaticoDTO model, CancellationToken cancellationToken)
        {
            if (model.IdModulo <= 0)
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["IdMenorACero"].Value }
                });
            }

            if (string.IsNullOrWhiteSpace(model.NombreModulo))
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["NombreModuloVacio"].Value }

                });
            }

            if (string.IsNullOrWhiteSpace(model.Nombre))
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["NombreCorreoAutomaticoVacio"].Value }

                });
            }

            if (String.IsNullOrWhiteSpace(model.NombreClave))
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["NombreClaveCorreoAutomaticoVacio"].Value }

                });
            }

            if (String.IsNullOrWhiteSpace(model.ExpresionCron))
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["TiempoejecucionNoDefinido"].Value }

                });
            }

            var correoAutomatico = mapper.Map<CorreosAutomaticos>(model);

            await context.CorreosAutomaticos.AddAsync(correoAutomatico);
            await context.SaveChangesAsync(cancellationToken);

            //try
            //{
            //}
            //catch (Exception e)
            //{
            //    return BadRequest(new RespuestaFallidaGenerica
            //    {
            //        Errores = new List<string> { stringLocalizer["ErrorAlCrearCorreo"].Value }

            //    });
            //}

            return Ok(new RespuestaExitosaGenerica
            {
                Mensaje = stringLocalizer["CorreoCreadoExito"].Value,
            });
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] EditarCorreoAutomaticoDTO model, CancellationToken cancellationToken)
        {
            var correoAutomatico = await context.CorreosAutomaticos.FirstOrDefaultAsync(x => x.Id == model.Id, cancellationToken);

            if (correoAutomatico == null)
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["RegistroNoEncontradoEnBaseDatos"].Value }

                });
            }

            if (model.IdModulo <= 0)
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["IdMenorACero"].Value }

                });
            }

            if (String.IsNullOrWhiteSpace(model.NombreModulo))
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["NombreModuloVacio"].Value }

                });
            }

            if (String.IsNullOrWhiteSpace(model.Nombre))
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["NombreCorreoAutomaticoVacio"].Value }

                });
            }

            if (String.IsNullOrWhiteSpace(model.NombreClave))
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["NombreClaveCorreoAutomaticoVacio"].Value }

                });
            }

            if (String.IsNullOrWhiteSpace(model.ExpresionCron))
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["TiempoejecucionNoDefinido"].Value }

                });
            }

            var esDiferenteCron = false;

            if (model.ExpresionCron != correoAutomatico.ExpresionCron)
            {
                esDiferenteCron = true;
            }

            bool existeCronIgual = true;

            while (existeCronIgual)
            {
                var existeAlgunOtroCronIgual = await context.CorreosAutomaticos.AnyAsync(x => x.ExpresionCron == model.ExpresionCron && x.NombreClave != model.NombreClave, cancellationToken);

                if (existeAlgunOtroCronIgual)
                {
                    var expresionCron = model.ExpresionCron.Split(" ");

                    int minutoDeExpresion = Convert.ToInt32(expresionCron[1]);
                    expresionCron[1] = (minutoDeExpresion + 1).ToString();

                    model.ExpresionCron = string.Join(" ", expresionCron);
                }
                else
                {
                    existeCronIgual = false;
                }
            }

            mapper.Map(model, correoAutomatico);

            context.Entry(correoAutomatico).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync(cancellationToken);

            }
            catch (Exception e)
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["ErrorActualizarCorreoAutomatico"].Value }

                });
            }

            return Ok(new RespuestaExitosaGenerica
            {
                Mensaje = stringLocalizer["CorreoAutomaticoProgramadoExito"].Value
            });
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> CambiarEstado([FromRoute] int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["IdMenorACero"].Value }

                });
            }

            var correoAutomatico = await context.CorreosAutomaticos.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (correoAutomatico == null)
            {
                return NotFound(stringLocalizer["CronNoencontrado"].Value);
            }

            correoAutomatico.Activo = !correoAutomatico.Activo;

            context.Entry(correoAutomatico).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = new List<string> { stringLocalizer["ErrorCambiarEstadoCorreoAutomatico"].Value }

                });
            }

            return Ok(new RespuestaExitosaGenerica
            {
                Mensaje = stringLocalizer["EstadoCambiadoExito"].Value
            });
        }
    }
}
