using API.Database.Almacenamiento.DTOs.Depositos;
using API.Database.Almacenamiento.Entidades;
using API.Database.Recepcion;
using API.Database.Recepcion.DTOs.Proveedores;
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
    public class ProveedoresRecepcionController : ControllerBase
    {
        private readonly ModuloRecepcionExplosivosContext context;
        private readonly IMapper mapper;

        public ProveedoresRecepcionController(ModuloRecepcionExplosivosContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult> Listar()
        {
            var proveedores = await context.ProveedoresRecepcion
                .Where(d => !d.Borrado)
                .ToListAsync();

            var respuestaValidada = RespuestaGenericaDTO<List<ProveedoresRecepcion>>.CrearRespuestaExitosa(proveedores);

            return Ok(respuestaValidada);
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult> Obtener([FromQuery] int id)
        {
            var proveedores = await context.ProveedoresRecepcion
                .FirstOrDefaultAsync(x => x.Id == id);

            var respuestaValidada = RespuestaGenericaDTO<ProveedoresRecepcion>.CrearRespuestaExitosa(proveedores);

            return Ok(respuestaValidada);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult> Registrar([FromBody] RegistrarProveedorRecepcion modelo)
        {
            var (esValido, mensaje) = await ComprobarModeloAsync(modelo);

            if (!esValido)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores(mensaje);

                return BadRequest(respuesta);
            }

            var nuevoProveedor = mapper.Map<ProveedoresRecepcion>(modelo);

            nuevoProveedor.IdUnico = Guid.NewGuid().ToString();

            await context.ProveedoresRecepcion.AddAsync(nuevoProveedor);

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
        public async Task<ActionResult> Modificar([FromBody] ModificarProveedorRecepcion modelo)
        {
            if (modelo.Id <= 0)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("El Id del proveedor es inválido");

                return BadRequest(respuesta);
            }

            var modeloAValidar = mapper.Map<RegistrarProveedorRecepcion>(modelo);

            var (esValido, mensaje) = await ComprobarModeloAsync(modeloAValidar, modelo.Id);

            if (!esValido)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores(mensaje);

                return BadRequest(respuesta);
            }

            var existente = await context.ProveedoresRecepcion
                .FirstOrDefaultAsync(x => x.Id == modelo.Id && !x.Borrado);

            if (existente == null)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("No se encontró un depósito con el Id proporcionado");
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
            var modeloDB = await context.ProveedoresRecepcion.FirstOrDefaultAsync(x => x.Id == id);

            if (modeloDB == null)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("No se encontró un proveedor con el Id proporcionado");

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
            var modeloDB = await context.ProveedoresRecepcion.FirstOrDefaultAsync(x => x.Id == id);

            if (modeloDB == null)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("No se encontró un proveedor con el Id proporcionado");

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

        private async Task<(bool, string)> ComprobarModeloAsync(RegistrarProveedorRecepcion modelo, int? idModelo = null)
        {
            if (string.IsNullOrEmpty(modelo.Nombre))
            {
                return (false, "El nombre del proveedor es obligatorio");
            }

            bool existe;

            if(idModelo.HasValue)
            {
                existe = await context.ProveedoresRecepcion
                .AnyAsync(x => x.Nombre.ToLower() == modelo.Nombre.ToLower() && x.Id != idModelo.Value && !x.Borrado);
            }
            else
            {
                existe = await context.ProveedoresRecepcion
                .AnyAsync(x => x.Nombre.ToLower() == modelo.Nombre.ToLower() && !x.Borrado);
            }

            if (existe)
                return (false, "Ya existe un proveedor con ese mismo nombre");

            return (true, "El modelo es óptimo");
        }
    }
}
