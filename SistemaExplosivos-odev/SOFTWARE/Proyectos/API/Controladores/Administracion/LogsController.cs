using API.Database.Administracion;
using API.Database.Administracion.DTOs.DLogs;
using API.Servicios.Preterminados.Autorizacion.PermisosAutorizacion.Controladores;
using API.Servicios.Preterminados.Autorizacion.PermisosAutorizacion.Controladores.Administracion;
using API.Utilidades.Constantes;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToolkitG3Core.Utilidades.DateTimeUtils;

namespace API.Controladores.Administracion
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = ConstantesModulos.ADMINISTRACION)]
    [Route("administracion/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        public readonly ModuloAdministracionExplosivosContext context;
        public readonly IMapper mapper;

        public LogsController(ModuloAdministracionExplosivosContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //// TODO: Despues de que se implemente el modulo de logs

        //[AutorizarPermisos(PermisosLogs.PermisoListarLogs)]
        //[HttpGet("[action]")]
        //public async Task<ActionResult<List<LogsDTO>>> Listar(CancellationToken cancellationToken, [FromQuery] DateTime? fecha1 = null, [FromQuery] DateTime? fecha2 = null)
        //{
        //    var (inicio, fin) = G3DateTimeUtils.ObtenerRangoFechas(fecha1, fecha2, tipoRango: TipoRangoFechas.Dia);

        //    var logs = await context.Logs
        //        .Where(x => x.FechaHora >= inicio && x.FechaHora <= fin)
        //        .OrderByDescending(x => x.FechaHora)
        //        .ToListAsync(cancellationToken);

        //    var logsDTO = mapper.Map<List<LogsDTO>>(logs);

        //    return Ok(logsDTO);
        //}
    }
}
