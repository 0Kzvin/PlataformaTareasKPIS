using API.Database.Accesorio;
using API.Database.Accesorio.DTOs.Categorias;
using API.Database.Accesorio.Entidades;
using API.Database.Administracion.DTOs.Respuestas;
using API.Database.Recepcion;
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

namespace API.Controladores.Accesorios
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = ConstantesModulos.ACCESORIOS)]
    [Route("accesorios/[controller]")]
    [ApiController]
    public class CategoriasAccesoriosController : ControllerBase
    {
        private readonly ModuloAccesoriosExplosivosContext context;
        private readonly IMapper mapper;

        public CategoriasAccesoriosController(ModuloAccesoriosExplosivosContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult> Listar()
        {
            var listaModelosDB = await context.CategoriasAccesorios
                .Where(d => !d.Borrado)
                .ToListAsync();

            var respuestaValidada = RespuestaGenericaDTO<List<CategoriasAccesorios>>.CrearRespuestaExitosa(listaModelosDB);

            return Ok(respuestaValidada);
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult> Obtener([FromQuery] int id)
        {
            var modeloDB = await context.CategoriasAccesorios
                .FirstOrDefaultAsync(x => x.Id == id);

            var respuestaValidada = RespuestaGenericaDTO<CategoriasAccesorios>.CrearRespuestaExitosa(modeloDB);

            return Ok(respuestaValidada);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult> Registrar([FromBody] RegistrarCategoriasAccesorios modelo)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("El modelo ingresado no cumple con los requisitos", [.. errores]);

                return BadRequest(respuesta);
            }

            var (esValido, mensaje) = await ComprobarModeloAsync(modelo);

            if (!esValido)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores(mensaje);

                return BadRequest(respuesta);
            }

            var nuevoRegistro = mapper.Map<CategoriasAccesorios>(modelo);

            nuevoRegistro.IdUnico = Guid.NewGuid().ToString();

            await context.CategoriasAccesorios.AddAsync(nuevoRegistro);

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
        public async Task<ActionResult> Modificar([FromBody] ModificarCategoriasAccesorios modelo)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                return BadRequest(new RespuestaFallidaGenerica
                {
                    Errores = errores
                });
            }

            var modeloAValidar = mapper.Map<RegistrarCategoriasAccesorios>(modelo);

            var (esValido, mensaje) = await ComprobarModeloAsync(modeloAValidar, modelo.Id);

            if (!esValido)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores(mensaje);

                return BadRequest(respuesta);
            }

            var existente = await context.CategoriasAccesorios
                .FirstOrDefaultAsync(x => x.Id == modelo.Id && !x.Borrado);

            if (existente == null)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("No se encontró una categoría con el Id proporcionado");
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
            var modeloDB = await context.CategoriasAccesorios.FirstOrDefaultAsync(x => x.Id == id);

            if (modeloDB == null)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("No se encontró una categoría con el Id proporcionado");

                return BadRequest(respuesta);
            }

            modeloDB.Estado = !modeloDB.Estado;

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
            var modeloDB = await context.CategoriasAccesorios.FirstOrDefaultAsync(x => x.Id == id);

            if (modeloDB == null)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("No se encontró una categoría con el Id proporcionado");

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

        private async Task<(bool, string)> ComprobarModeloAsync(RegistrarCategoriasAccesorios modelo, int? idModelo = null)
        {
            bool existe;

            if (idModelo.HasValue)
            {
                existe = await context.CategoriasAccesorios
                .AnyAsync(x => x.Nombre.ToLower() == modelo.Nombre.ToLower() && x.Id != idModelo.Value && !x.Borrado);
            }
            else
            {
                existe = await context.CategoriasAccesorios
                .AnyAsync(x => x.Nombre.ToLower() == modelo.Nombre.ToLower() && !x.Borrado);
            }

            if (existe)
                return (false, "Ya existe una categoría con ese mismo nombre");

            return (true, "El modelo es óptimo");
        }
    }
}
