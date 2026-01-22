using API.Database.Administracion.DTOs.Respuestas;
using API.Database.Administracion.Entidades.General;
using API.Utilidades.Constantes;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Serilog.Events;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using API.Attributes;
using Serilog;
using API.Database.Administracion;
using API.Database.Administracion.DTOs.DLogs;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API.Controladores.Administracion
{
    [ApiKey]
    [ApiExplorerSettings(GroupName = ConstantesModulos.ADMINISTRACION)]
    [Route("administracion/[controller]")]
    [ApiController]
    public class SincronizacionLogsController : ControllerBase
    {
        private readonly ILogger seriLog;
        private readonly IMapper mapper;
        private readonly ModuloAdministracionExplosivosContext context;

        public SincronizacionLogsController(ModuloAdministracionExplosivosContext context, ILogger seriLog, IMapper mapper)
        {
            this.seriLog = seriLog;
            this.mapper = mapper;
            this.context = context;
        }

        //// TODO : Revisar funcionalidad de sincronizacion de Logs

        //[HttpPost("[action]")]
        //public async Task<ActionResult> SincronizarLog([FromBody] SincronizarLogDTO model)
        //{
        //    if (model == null)
        //    {
        //        return BadRequest(new RespuestaFallidaGenerica
        //        {
        //            Errores = new List<string> { "El objeto log no puede ser nulo" }
        //        });
        //    }

        //    var logParaSincronizar = mapper.Map<Logs>(model);

        //    // Margen de tolerancia de 1 segundo para la comparación de fechas
        //    var fechaMinima = logParaSincronizar.FechaHora.AddSeconds(-1);
        //    var fechaMaxima = logParaSincronizar.FechaHora.AddSeconds(1);

        //    // Buscar log existente con criterios más robustos
        //    var logExistente = await context.Logs
        //        .FirstOrDefaultAsync(x =>
        //            x.FechaHora >= fechaMinima &&
        //            x.FechaHora <= fechaMaxima &&
        //            x.Mensaje == logParaSincronizar.Mensaje &&
        //            x.Usuario == logParaSincronizar.Usuario);

        //    if (logExistente == null)
        //    {
        //        // Agregar nuevo log
        //        bool existeLogLevel = Enum.TryParse<LogEventLevel>(model.NivelLog, true, out var result);

        //        seriLog.ForContext("Usuario", model.Origen)
        //                       .ForContext("Accion", model.Metodo)
        //                       .ForContext("Direccion", model.Cliente)
        //                       .ForContext("FechaHora", model.FechaHora)
        //                       .ForContext("DatosPeticion", model.DescripcionCliente)
        //                       .Write(existeLogLevel ? result : LogEventLevel.Error, model.Mensaje);
        //    }

        //    try
        //    {
        //        await context.SaveChangesAsync();
        //        return Ok(new RespuestaExitosaGenerica
        //        {
        //            Mensaje = "Log sincronizado exitosamente"
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(new RespuestaFallidaGenerica
        //        {
        //            Errores = new List<string> { $"Error al sincronizar el log: {e.Message}" }
        //        });
        //    }
        //}

        //[HttpPost("[action]")]
        //public async Task<ActionResult> SincronizarListaLog([FromBody] List<SincronizarLogDTO> logsClient)
        //{
        //    if (logsClient?.Count == 0)
        //    {
        //        return BadRequest(new RespuestaFallidaGenerica
        //        {
        //            Errores = new List<string> { "La lista de logs está vacía" },
        //        });
        //    }

        //    // 1. Crear un rango de fechas para la consulta
        //    var minFecha = logsClient.Min(x => x.FechaHora);
        //    var maxFecha = logsClient.Max(x => x.FechaHora);

        //    // 2. Buscar logs existentes en ese rango
        //    var logsExistentes = await context.Logs
        //        .Where(x => x.FechaHora >= minFecha && x.FechaHora <= maxFecha)
        //        .ToListAsync();

        //    // 3. Agrupar por criterio único (fecha + mensaje + usuario por ejemplo)
        //    foreach (var logNuevo in logsClient)
        //    {
        //        // Buscar log existente con margen de tolerancia para la fecha
        //        var logExistente = logsExistentes.Find(x =>
        //            Math.Abs((x.FechaHora - logNuevo.FechaHora).TotalSeconds) < 1 && // 1 segundo de tolerancia
        //            x.Mensaje == logNuevo.Mensaje &&
        //            x.Usuario == logNuevo.Origen &&
        //            x.Accion == logNuevo.Metodo &&
        //            x.Direccion == logNuevo.Cliente);

        //        if (logExistente == null)
        //        {
        //            // Agregar nuevo log
        //            bool existeLogLevel = Enum.TryParse<LogEventLevel>(logNuevo.NivelLog, true, out var result);

        //            seriLog.ForContext("Usuario", logNuevo.Origen)
        //                           .ForContext("Accion", logNuevo.Metodo)
        //                           .ForContext("Direccion", logNuevo.Cliente)
        //                           .ForContext("FechaHora", logNuevo.FechaHora)
        //                           .ForContext("DatosPeticion", logNuevo.DescripcionCliente)
        //                           .Write(existeLogLevel ? result : LogEventLevel.Error, logNuevo.Mensaje);
        //        }
        //    }

        //    try
        //    {
        //        await context.SaveChangesAsync();
        //        return Ok(new RespuestaExitosaGenerica
        //        {
        //            Mensaje = $"Logs sincronizados exitosamente. Total: {logsClient.Count}"
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(new RespuestaFallidaGenerica
        //        {
        //            Errores = new List<string> { $"Error al sincronizar los logs: {e.Message}" }
        //        });
        //    }
        //}
    }
}
