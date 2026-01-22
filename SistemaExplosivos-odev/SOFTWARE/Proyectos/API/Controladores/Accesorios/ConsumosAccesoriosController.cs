using API.Database.Accesorio;
using API.Database.Accesorio.DTOs.Consumos;
using API.Database.Accesorio.Entidades;
using API.Database.Administracion.DTOs.Respuestas;
using API.Modelos;
using API.Utilidades.Constantes;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controladores.Accesorios
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = ConstantesModulos.ACCESORIOS)]
    [Route("accesorios/[controller]")]
    [ApiController]
    public class ConsumosAccesoriosController : ControllerBase
    {
        private readonly ModuloAccesoriosExplosivosContext context;
        private readonly IMapper mapper;

        public ConsumosAccesoriosController(ModuloAccesoriosExplosivosContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult> Listar()
        {
            var listaModelosDB = await context.ConsumosAccesorios
                .Include(c => c.Salidas)
                .Where(d => !d.Borrado)
                .ToListAsync();

            var respuestaValidada = RespuestaGenericaDTO<List<ConsumosAccesorios>>.CrearRespuestaExitosa(listaModelosDB);

            return Ok(respuestaValidada);
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult> Obtener([FromQuery] int id)
        {
            var modeloDB = await context.ConsumosAccesorios
                .FirstOrDefaultAsync(x => x.Id == id);

            var respuestaValidada = RespuestaGenericaDTO<ConsumosAccesorios>.CrearRespuestaExitosa(modeloDB);

            return Ok(respuestaValidada);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult> Registrar([FromBody] RegistrarConsumosAccesorios modelo)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("El modelo ingresado no cumple con los requisitos", [..errores]);

                return BadRequest(respuesta);
            }

            var (esValido, mensaje) = await ComprobarModeloAsync(modelo);

            if (!esValido)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores(mensaje);

                return BadRequest(respuesta);
            }

            var nuevoRegistro = mapper.Map<ConsumosAccesorios>(modelo);

            nuevoRegistro.IdUnico = Guid.NewGuid().ToString();

            await context.ConsumosAccesorios.AddAsync(nuevoRegistro);

            try
            {
                await context.SaveChangesAsync();

                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaExitosa("Registro exitoso");

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("Ocurrió un error inesperado al insertar el registro en la base de datos", ["Error en la base de datos", ex.Message]);

                return BadRequest(respuesta);
            }
        }

        [AllowAnonymous]
        [HttpPut("[action]")]
        public async Task<ActionResult> Modificar([FromBody] ModificarConsumosAccesorios modelo)
        {
            if (modelo.Id <= 0)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("El Id del consumo es inválido");

                return BadRequest(respuesta);
            }

            var modeloAValidar = mapper.Map<RegistrarConsumosAccesorios>(modelo);

            var (esValido, mensaje) = await ComprobarModeloAsync(modeloAValidar, modelo.Id);

            if (!esValido)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores(mensaje);

                return BadRequest(respuesta);
            }

            var existente = await context.ConsumosAccesorios
                .Include(x => x.Salidas)
                .FirstOrDefaultAsync(x => x.Id == modelo.Id && !x.Borrado);

            if (existente == null)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("No se encontró un consumo con el Id proporcionado");
                return BadRequest(respuesta);
            }

            var salidasActuales = existente.Salidas.ToList();

            mapper.Map(modelo, existente);

            var idsDto = modelo.Salidas
                .Where(s => s.Id.HasValue)
                .Select(s => s.Id.Value)
                .ToList();

            foreach (var salidaExistente in salidasActuales)
            {
                if (!idsDto.Contains(salidaExistente.Id))
                    context.SalidasAccesorios.Remove(salidaExistente);
            }

            existente.Salidas.Clear();

            foreach (var salidaDto in modelo.Salidas)
            {
                if (salidaDto.Id.HasValue && salidaDto.Id != 0)
                {
                    var salida = salidasActuales.FirstOrDefault(s => s.Id == salidaDto.Id.Value);

                    if (salida == null)
                    {
                        var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("La salida que intenta editar no existe");
                        return BadRequest(respuesta);
                    }

                    mapper.Map(salidaDto, salida);
                    existente.Salidas.Add(salida);
                }
                else
                {
                    var nuevaSalida = mapper.Map<SalidasAccesorios>(salidaDto);
                    nuevaSalida.IdUnico = Guid.NewGuid().ToString();
                    existente.Salidas.Add(nuevaSalida);
                }
            }

            context.Entry(existente).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();

                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaExitosa("Modificación exitosa");

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("Ocurrió un error inesperado al modificar el registro en la base de datos", ["Error en la base de datos", ex.Message]);

                return BadRequest(respuesta);
            }
        }

        [AllowAnonymous]
        [HttpPut("[action]")]
        public async Task<ActionResult> Borrar([FromQuery] int id)
        {
            var modeloDB = await context.ConsumosAccesorios.FirstOrDefaultAsync(x => x.Id == id);

            if (modeloDB == null)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("No se encontró un consumo con el Id proporcionado");

                return BadRequest(respuesta);
            }

            modeloDB.Borrado = true;
            modeloDB.FechaModificacion = DateTime.Now;

            context.Entry(modeloDB).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();

                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaExitosa("Borrado exitosamente");

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("Ocurrió un error inesperado al borrar el registro en la base de datos", ["Error en la base de datos", ex.Message]);

                return BadRequest(respuesta);
            }
        }

        private async Task<(bool, string)> ComprobarModeloAsync(RegistrarConsumosAccesorios modelo, int? idModelo = null)
        {
            bool existe;

            if (idModelo.HasValue)
            {
                existe = await context.ConsumosAccesorios
                .AnyAsync(x => x.Folio.ToLower() == modelo.Folio.ToLower() && x.Id != idModelo.Value && !x.Borrado);
            }
            else
            {
                existe = await context.ConsumosAccesorios
                .AnyAsync(x => x.Folio.ToLower() == modelo.Folio.ToLower() && !x.Borrado);
            }

            if (existe)
                return (false, "Ya existe un consumo con ese mismo folio");

            return (true, "El modelo es óptimo");
        }
    }
}
