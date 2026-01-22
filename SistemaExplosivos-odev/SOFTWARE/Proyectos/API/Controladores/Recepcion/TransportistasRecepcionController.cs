using API.Database.Recepcion;
using API.Database.Recepcion.DTOs.Proveedores;
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
    public class TransportistasRecepcionController : ControllerBase
    {
        private readonly ModuloRecepcionExplosivosContext context;
        private readonly IMapper mapper;

        public TransportistasRecepcionController(ModuloRecepcionExplosivosContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult> Listar()
        {
            var listaModelosDB = await context.TransportistasRecepcion
                .Where(d => !d.Borrado)
                .ToListAsync();

            var respuestaValidada = RespuestaGenericaDTO<List<TransportistasRecepcion>>.CrearRespuestaExitosa(listaModelosDB);

            return Ok(respuestaValidada);
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult> Obtener([FromQuery] int id)
        {
            var modeloDB = await context.TransportistasRecepcion
                .FirstOrDefaultAsync(x => x.Id == id);

            var respuestaValidada = RespuestaGenericaDTO<TransportistasRecepcion>.CrearRespuestaExitosa(modeloDB);

            return Ok(respuestaValidada);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult> Registrar([FromBody] RegistrarTransportistaRecepcion modelo)
        {
            var (esValido, mensaje) = await ComprobarModeloAsync(modelo);

            if (!esValido)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores(mensaje);

                return BadRequest(respuesta);
            }

            var nuevoRegistro = mapper.Map<TransportistasRecepcion>(modelo);

            nuevoRegistro.IdUnico = Guid.NewGuid().ToString();

            await context.TransportistasRecepcion.AddAsync(nuevoRegistro);

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
        public async Task<ActionResult> Modificar([FromBody] ModificarTransportistaRecepcion modelo)
        {
            if (modelo.Id <= 0)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("El Id del transportista es inválido");

                return BadRequest(respuesta);
            }

            var modeloAValidar = mapper.Map<RegistrarTransportistaRecepcion>(modelo);

            var (esValido, mensaje) = await ComprobarModeloAsync(modeloAValidar, modelo.Id);

            if (!esValido)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores(mensaje);

                return BadRequest(respuesta);
            }

            var existente = await context.TransportistasRecepcion
                .FirstOrDefaultAsync(x => x.Id == modelo.Id && !x.Borrado);

            if (existente == null)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("No se encontró un transportista con el Id proporcionado");
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
        public async Task<ActionResult> CambiarEstado([FromQuery] int id)
        {
            var modeloDB = await context.TransportistasRecepcion.FirstOrDefaultAsync(x => x.Id == id);

            if (modeloDB == null)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("No se encontró un transportistas con el Id proporcionado");

                return BadRequest(respuesta);
            }

            modeloDB.Estado = !modeloDB.Estado;
            modeloDB.FechaModificacion = DateTime.Now;

            context.Entry(modeloDB).State = EntityState.Modified;

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
            var modeloDB = await context.TransportistasRecepcion.FirstOrDefaultAsync(x => x.Id == id);

            if (modeloDB == null)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("No se encontró un transportistas con el Id proporcionado");

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

        private async Task<(bool, string)> ComprobarModeloAsync(RegistrarTransportistaRecepcion modelo, int? idModelo = null)
        {
            if (string.IsNullOrEmpty(modelo.Nombre))
            {
                return (false, "El nombre del transportista es obligatorio");
            }

            bool existe;

            if (idModelo.HasValue)
            {
                existe = await context.TransportistasRecepcion
                .AnyAsync(x => x.Nombre.ToLower() == modelo.Nombre.ToLower() && x.Id != idModelo.Value && !x.Borrado);
            }
            else
            {
                existe = await context.TransportistasRecepcion
                .AnyAsync(x => x.Nombre.ToLower() == modelo.Nombre.ToLower() && !x.Borrado);
            }

            if (existe)
                return (false, "Ya existe un transportista con ese mismo nombre");

            return (true, "El modelo es óptimo");
        }
    }
}
