using API.Database.Administracion.DTOs.Respuestas;
using API.Database.Almacenamiento;
using API.Database.Almacenamiento.DTOs.Sincronizacion;
using API.Database.Almacenamiento.Entidades;
using API.Modelos;
using API.Utilidades.Constantes;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Renci.SshNet.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controladores.Almacenamiento
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = ConstantesModulos.ALMACENAMIENTO)]
    [Route("almacenamiento/[controller]")]
    [ApiController]
    public class SincronizacionAlmacenamientoController : ControllerBase
    {
        private readonly ModuloAlmacenamientoExplosivosContext context;
        private readonly IMapper mapper;

        public SincronizacionAlmacenamientoController(ModuloAlmacenamientoExplosivosContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult> SincronizarCargasDepositos (SincronizacionCargasDepositosAlmacenamientoDTO modelo)
        {
            var cargaDeposito = mapper.Map<CargasDepositosAlmacenamiento>(modelo);
            
            var cargaExistenteDB = await context.CargasDepositosAlmacenamientos
                .FirstOrDefaultAsync(x => x.IdUnico == cargaDeposito.IdUnico);
            
            if (cargaExistenteDB != null)
            {
                mapper.Map(cargaDeposito, cargaExistenteDB);
                context.Entry(cargaExistenteDB).State = EntityState.Modified;
            }
            else
            {
                context.CargasDepositosAlmacenamientos.Add(cargaDeposito);
            }
            
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var respuestaError = RespuestaGenericaDTO<string>.CrearRespuestaConErrores($"Error al guardar en la base de datos: {e.Message}");
                return BadRequest(respuestaError);
            }
            
            var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaExitosa("Sincronizacion exitosa");
            
            return Ok(respuesta);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult> SincronizarCargasDepositosLista(List<SincronizacionCargasDepositosAlmacenamientoDTO> modelos)
        {
            if (modelos.Count == 0)
            {
                var respuestaError = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("Se ha enviado una lista vacía");

                return BadRequest(respuestaError);
            }

            var listaCargasDepositos = mapper.Map<List<CargasDepositosAlmacenamiento>>(modelos);

            var listaIdsUnicos = listaCargasDepositos.Select(x => new { x.IdUnico, x.FechaHoraInicial }).ToList();

            var fechaCargasMasAntigua = listaIdsUnicos.Min(x => x.FechaHoraInicial);

            var existeAlgunCargas = await context.CargasDepositosAlmacenamientos
                .Where(x => x.FechaHoraInicial >= fechaCargasMasAntigua)
                .AnyAsync(x => listaIdsUnicos.Select(x => x.IdUnico).Contains(x.IdUnico));

            if (existeAlgunCargas)
            {
                var cargasExistentesDB = await context.CargasDepositosAlmacenamientos
                    .Where(x => listaIdsUnicos.Select(x => x.IdUnico).Contains(x.IdUnico))
                    .ToListAsync();

                var listaIdsUnicosExistentesDB = cargasExistentesDB.Select(x => x.IdUnico);

                var cargasNoExistentes = listaCargasDepositos.Where(x => !listaIdsUnicosExistentesDB.Contains(x.IdUnico));

                foreach (var cargasDB in cargasExistentesDB)
                {
                    var cargasSincronizacion = listaCargasDepositos
                        .FirstOrDefault(x => x.IdUnico == cargasDB.IdUnico);

                    mapper.Map(cargasSincronizacion, cargasDB);
                    context.Entry(cargasDB).State = EntityState.Modified;
                }
            }
            else
            {
                context.CargasDepositosAlmacenamientos.AddRange(listaCargasDepositos);
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var respuestaError = RespuestaGenericaDTO<string>.CrearRespuestaConErrores($"Error al guardar en la base de datos: {e.Message}");

                return BadRequest(respuestaError);
            }

            var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaExitosa("Sincronizacion exitosa");

            return Ok(respuesta);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult> SincronizarEstatusDepositos (SincronizacionEstatusAlmacenamientoDTO modelo)
        {
            var estatusDeposito = mapper.Map<EstatusDepositosAlmacenamiento>(modelo);
            
            var estatusExistenteDB = await context.EstatusDepositosAlmacenamiento
                .FirstOrDefaultAsync(x => x.IdUnico == estatusDeposito.IdUnico);
            
            if (estatusExistenteDB != null)
            {
                mapper.Map(estatusDeposito, estatusExistenteDB);
                context.Entry(estatusExistenteDB).State = EntityState.Modified;
            }
            else
            {
                context.EstatusDepositosAlmacenamiento.Add(estatusDeposito);
            }
            
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var respuestaError = RespuestaGenericaDTO<string>.CrearRespuestaConErrores($"Error al guardar en la base de datos: {e.Message}");
                return BadRequest(respuestaError);
            }
            
            var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaExitosa("Sincronizacion exitosa");
            
            return Ok(respuesta);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult> SincronizarEstatusDepositosLista (List<SincronizacionEstatusAlmacenamientoDTO> modelos)
        {
            if (modelos.Count == 0)
            {
                var respuestaError = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("Se ha enviado una lista vacía");

                return BadRequest(respuestaError);
            }

            var listaEstatusDepositos = mapper.Map<List<EstatusDepositosAlmacenamiento>>(modelos);

            var listaIdsUnicos = listaEstatusDepositos.Select(x => new { x.IdUnico, x.FechaHora }).ToList();

            var fechaEstatusMasAntigua = listaIdsUnicos.Min(x => x.FechaHora);

            var existeAlgunEstatus = await context.EstatusDepositosAlmacenamiento
                .Where(x => x.FechaHora >= fechaEstatusMasAntigua)
                .AnyAsync(x => listaIdsUnicos.Select(x => x.IdUnico).Contains(x.IdUnico));

            if (existeAlgunEstatus)
            {
                var estatusExistentesDB = await context.EstatusDepositosAlmacenamiento
                    .Where(x => listaIdsUnicos.Select(x => x.IdUnico).Contains(x.IdUnico))
                    .ToListAsync();

                var listaIdsUnicosExistentesDB = estatusExistentesDB.Select(x => x.IdUnico);

                var estatusNoExistentes = listaEstatusDepositos.Where(x => !listaIdsUnicosExistentesDB.Contains(x.IdUnico));

                foreach (var estatusDB in estatusExistentesDB)
                {
                    var estatusSincronizacion = listaEstatusDepositos
                        .FirstOrDefault(x => x.IdUnico == estatusDB.IdUnico);

                    mapper.Map(estatusSincronizacion, estatusDB);
                    context.Entry(estatusDB).State = EntityState.Modified;
                }
            }
            else
            {
                context.EstatusDepositosAlmacenamiento.AddRange(listaEstatusDepositos);
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var respuestaError = RespuestaGenericaDTO<string>.CrearRespuestaConErrores($"Error al guardar en la base de datos: {e.Message}");

                return BadRequest(respuestaError);
            }

            var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaExitosa("Sincronizacion exitosa");

            return Ok(respuesta);
        }
    }
}
