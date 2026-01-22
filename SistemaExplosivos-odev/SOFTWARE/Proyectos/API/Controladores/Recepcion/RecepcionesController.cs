using API.Database.Recepcion;
using API.Database.Recepcion.DTOs.Recepciones;
using API.Database.Recepcion.DTOs.Transportistas;
using API.Database.Recepcion.Entidades;
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

namespace API.Controladores.Recepcion
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = ConstantesModulos.RECEPCION)]
    [Route("recepcion/[controller]")]
    [ApiController]
    public class RecepcionesController : ControllerBase
    {
        private readonly ModuloRecepcionExplosivosContext context;
        private readonly IMapper mapper;

        public RecepcionesController(ModuloRecepcionExplosivosContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult> Listar()
        {
            var listaModelosDB = await context.Recepciones
                .Where(x => !x.Borrado)
                .ToListAsync();

            var respuestaValidada = RespuestaGenericaDTO<List<Recepciones>>.CrearRespuestaExitosa(listaModelosDB);

            return Ok(respuestaValidada);
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult> Obtener([FromQuery] int id)
        {
            var modeloDB = await context.Recepciones
                .FirstOrDefaultAsync(x => x.Id == id);

            var respuestaValidada = RespuestaGenericaDTO<Recepciones>.CrearRespuestaExitosa(modeloDB);

            return Ok(respuestaValidada);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult> Registrar([FromBody] RegistrarRecepciones modelo)
        {
            var (esValido, mensaje) = await ComprobarModeloAsync(modelo);

            if (!esValido)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores(mensaje);

                return BadRequest(respuesta);
            }

            var nuevoRegistro = mapper.Map<Recepciones>(modelo);

            nuevoRegistro.IdUnico = Guid.NewGuid().ToString();

            await context.Recepciones.AddAsync(nuevoRegistro);

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
        public async Task<ActionResult> Modificar([FromBody] ModificarRecepciones modelo)
        {
            if (modelo.Id <= 0)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("El Id de la recepción es inválido");

                return BadRequest(respuesta);
            }

            var modeloAValidar = mapper.Map<RegistrarRecepciones>(modelo);

            var (esValido, mensaje) = await ComprobarModeloAsync(modeloAValidar, modelo.Id);

            if (!esValido)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores(mensaje);

                return BadRequest(respuesta);
            }

            var existente = await context.Recepciones
                .FirstOrDefaultAsync(x => x.Id == modelo.Id);

            if (existente == null)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("No se encontró una recepción con el Id proporcionado");
                return BadRequest(respuesta);
            }

            mapper.Map(modelo, existente);

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
            var modeloDB = await context.Recepciones.FirstOrDefaultAsync(x => x.Id == id);

            if (modeloDB == null)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("No se encontró una recepción con el Id proporcionado");

                return BadRequest(respuesta);
            }

            modeloDB.Borrado = true;

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

        //TODO: Completar validaciones necesarias
        private async Task<(bool, string)> ComprobarModeloAsync(RegistrarRecepciones modelo, int? idModelo = null)
        {
            if (string.IsNullOrEmpty(modelo.Producto))
            {
                return (false, "El producto de la recepción es obligatorio");
            }

            bool existe;

            if (idModelo.HasValue)
            {
                existe = await context.Recepciones
                .AnyAsync(x => x.Folio.ToLower() == modelo.Folio.ToLower() && x.Id != idModelo.Value);
            }
            else
            {
                existe = await context.Recepciones
                .AnyAsync(x => x.Folio.ToLower() == modelo.Folio.ToLower());
            }

            if (existe)
                return (false, "Ya existe una recepción con ese mismo folio");

            return (true, "El modelo es óptimo");
        }
    }
}
