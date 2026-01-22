using API.Database.Almacenamiento;
using API.Database.Almacenamiento.DTOs.Depositos;
using API.Database.Almacenamiento.DTOs.Depositos.Filtros;
using API.Database.Almacenamiento.Entidades;
using API.Database.Gerencia;
using API.Modelos;
using API.Utilidades.Constantes;
using API.Servicios.Preterminados.Autorizacion.PermisosAutorizacion.Controladores.Almacenamiento;
using API.Servicios.Preterminados.Autorizacion.PermisosAutorizacion.Controladores;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class DepositosAlmacenamientoController : ControllerBase
    {
        private readonly ModuloAlmacenamientoExplosivosContext context;
        private readonly ModuloGerenciaExplosivosContext contextGerencia;
        private readonly IMapper mapper;

        public DepositosAlmacenamientoController(ModuloAlmacenamientoExplosivosContext context, ModuloGerenciaExplosivosContext contextGerencia, IMapper mapper)
        {
            this.context = context;
            this.contextGerencia = contextGerencia;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult> Listar([FromQuery] FiltroDepositosAlmacenamientoDTO filtro)
        {
            var depositos = context.DepositosAlmacenamiento
                .OrderByDescending(x => x.FechaModificacion)
                .AsQueryable();

            if (filtro.Estado.HasValue)
            {
                depositos = depositos.Where(x => x.Estado == filtro.Estado.Value);
            }

            if (filtro.Borrado.HasValue)
            {
                depositos = depositos.Where(x => x.Borrado == filtro.Borrado.Value);
            }

            var listaDepositos = await depositos.ToListAsync();

            var productosId = listaDepositos
                .Select(d => d.IdProducto)
                .Distinct()
                .ToList();
            
            var productos = await contextGerencia.Productos
                .Where(p => productosId.Contains(p.IdUnico))
                .ToListAsync();

            var productosDTO = mapper.Map<List<ProductoDepositoAlmacenamientoDTO>>(productos);

            var productosPorId = productosDTO
                .ToDictionary(p => p.IdUnico);

            var resultado = listaDepositos
                .Select(d =>
                {
                    productosPorId.TryGetValue(d.IdProducto, out var producto);

                    return new DepositosAlmacenamientoDTO
                    {
                        Id = d.Id,
                        IdUnico = d.IdUnico,
                        Nombre = d.Nombre,
                        Apodo = d.Apodo,

                        Producto = producto != null ? producto.Nombre : "Producto no encontrado",
                        ColorProducto = producto?.CodigoColor,

                        CapacidadMaxima = d.CapacidadMaxima,
                        AlturaMaxima = d.AlturaMaxima,
                        CapacidadOperativa = d.CapacidadOperativa,
                        AlturaOperativa = d.AlturaOperativa,

                        LimiteAlto = d.LimiteAlto,
                        LimiteMaximo = d.LimiteMaximo,
                        LimiteBajo = d.LimiteBajo,
                        LimiteMinimo = d.LimiteMinimo,

                        Ubicacion = d.Ubicacion,

                        FechaRegistro = d.FechaRegistro,
                        FechaModificacion = d.FechaModificacion,

                        Estado = d.Estado,
                        Borrado = d.Borrado,

                        ProductoDTO = producto
                    };
                })
                .ToList();


            var respuestaValidada = RespuestaGenericaDTO<List<DepositosAlmacenamientoDTO>>.CrearRespuestaExitosa(resultado);

            return Ok(respuestaValidada);
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult> Obtener([FromQuery] int id)
        {
            var modelo = await context.DepositosAlmacenamiento
                .Where(d => !d.Borrado && d.Id == id)
                .FirstOrDefaultAsync();

            if (modelo == null)
            {
                return NotFound(
                    RespuestaGenericaDTO<string>.CrearRespuestaConErrores("Depósito no encontrado")
                );
            }

            ProductoDepositoAlmacenamientoDTO productoDTO = null;

            if (!string.IsNullOrWhiteSpace(modelo.IdProducto))
            {
                var producto = await contextGerencia.Productos
                    .Where(p => p.IdUnico == modelo.IdProducto)
                    .FirstOrDefaultAsync();

                if (producto != null)
                {
                    productoDTO = mapper.Map<ProductoDepositoAlmacenamientoDTO>(producto);
                }
            }

            var resultado = new DepositosAlmacenamientoDTO
            {
                Id = modelo.Id,
                IdUnico = modelo.IdUnico,
                Nombre = modelo.Nombre,
                Apodo = modelo.Apodo,

                Producto = productoDTO != null ? productoDTO.Nombre : "Producto no encontrado",
                ColorProducto = productoDTO != null ? productoDTO.CodigoColor : "Color no encontrado",

                CapacidadMaxima = modelo.CapacidadMaxima,
                AlturaMaxima = modelo.AlturaMaxima,
                CapacidadOperativa = modelo.CapacidadOperativa,
                AlturaOperativa = modelo.AlturaOperativa,

                LimiteAlto = modelo.LimiteAlto,
                LimiteMaximo = modelo.LimiteMaximo,
                LimiteBajo = modelo.LimiteBajo,
                LimiteMinimo = modelo.LimiteMinimo,

                Ubicacion = modelo.Ubicacion,

                FechaRegistro = modelo.FechaRegistro,
                FechaModificacion = modelo.FechaModificacion,

                Estado = modelo.Estado,
                Borrado = modelo.Borrado,

                ProductoDTO = productoDTO
            };

            var respuestaValidada =
                RespuestaGenericaDTO<DepositosAlmacenamientoDTO>.CrearRespuestaExitosa(resultado);

            return Ok(respuestaValidada);
        }

        [AutorizarPermisos(PermisosAlmacenamiento.PERMISO_CREAR_DEPOSITO)]
        [HttpPost("[action]")]
        public async Task<ActionResult> Registrar([FromBody] RegistrarDepositoAlmacenamiento modelo)
        {
            var (esValido, mensaje) = await ComprobarModeloDepositoAsync(modelo);

            if (!esValido)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores(mensaje);

                return BadRequest(respuesta);
            }
           
            var nuevoDeposito = mapper.Map<DepositosAlmacenamiento>(modelo);

            nuevoDeposito.IdUnico = Guid.NewGuid().ToString();

            await context.DepositosAlmacenamiento.AddAsync(nuevoDeposito);

            try
            {
                await context.SaveChangesAsync();

                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaExitosa("Registro exitoso");

                return Ok(respuesta);
            }
            catch(Exception ex)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("Ocurrió un error inesperado al insertar el registro en la base de datos", [ "Error en la base de datos", ex.Message]);

                return BadRequest(respuesta);
            }
        }

        [AllowAnonymous]
        [HttpPut("[action]")]
        public async Task<ActionResult> Modificar([FromBody] ModificarDepositoAlmacenamiento modelo)
        {
            if (modelo.Id <= 0)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("El Id del depósito es inválido");

                return BadRequest(respuesta);
            }

            var modeloAValidar = mapper.Map<RegistrarDepositoAlmacenamiento>(modelo);

            var (esValido, mensaje) = await ComprobarModeloDepositoAsync(modeloAValidar, modelo.Id);

            if (!esValido)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores(mensaje);

                return BadRequest(respuesta);
            }

            var depositoExistente = await context.DepositosAlmacenamiento
                .FirstOrDefaultAsync(x => x.Id == modelo.Id && !x.Borrado);

            if (depositoExistente == null)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("No se encontró un depósito con el Id proporcionado");
                return BadRequest(respuesta);
            }

            mapper.Map(modelo, depositoExistente);

            context.Entry(depositoExistente).State = EntityState.Modified;

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
            var depositoAlmacenamiento = await context.DepositosAlmacenamiento.FirstOrDefaultAsync(x => x.Id == id);

            if(depositoAlmacenamiento == null)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("No se encontró un depósito con el Id proporcionado");

                return BadRequest(respuesta);
            }

            depositoAlmacenamiento.Estado = !depositoAlmacenamiento.Estado;
            depositoAlmacenamiento.FechaModificacion = DateTime.Now;

            context.Entry(depositoAlmacenamiento).State = EntityState.Modified;

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
            var depositoAlmacenamiento = await context.DepositosAlmacenamiento.FirstOrDefaultAsync(x => x.Id == id);

            if (depositoAlmacenamiento == null)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("No se encontró un depósito con el Id proporcionado");

                return BadRequest(respuesta);
            }

            depositoAlmacenamiento.Borrado = true;
            depositoAlmacenamiento.FechaModificacion = DateTime.Now;

            context.Entry(depositoAlmacenamiento).State = EntityState.Modified;

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

        /// <summary>
        /// Revisa que el modelo de registro cumpla con las reglas de negocio establecidas.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="idModelo"></param>
        /// <returns>Un booleano indicando si es válido y un mensaje humanizado para poder enviarlo en respuestas</returns>
        private async Task<(bool EsValido, string Mensaje)> ComprobarModeloDepositoAsync(RegistrarDepositoAlmacenamiento modelo, int? idModelo = null)
        {
            if (String.IsNullOrEmpty(modelo.Nombre))
                return (false, "El nombre del tanque no puede ser vacío");

            if (String.IsNullOrEmpty(modelo.Ubicacion))
                return (false, "La ubicación del tanque no puede ser vacío");

            if (modelo.AlturaOperativa > modelo.AlturaMaxima)
                return (false, "La altura operativa no puede ser mayor que la altura máxima");

            if (modelo.CapacidadOperativa > modelo.CapacidadMaxima)
                return (false, "La capacidad operativa no puede ser mayor que la capacidad máxima");

            if (modelo.LimiteMinimo > modelo.LimiteBajo)
                return (false, "El límite mínimo no puede ser mayor que el límite bajo");

            if (modelo.LimiteBajo > modelo.LimiteAlto)
                return (false, "El límite bajo no puede ser mayor que el límite alto");

            if (modelo.LimiteAlto > modelo.LimiteMaximo)
                return (false, "El límite alto no puede ser mayor que el límite máximo");

            bool existe;

            if(idModelo.HasValue)
            {
                existe = await context.DepositosAlmacenamiento
                .AnyAsync(x => x.Nombre.ToLower() == modelo.Nombre.ToLower() && x.Id != idModelo.Value && !x.Borrado);
            }
            else
            {
                existe = await context.DepositosAlmacenamiento
                .AnyAsync(x => x.Nombre.ToLower() == modelo.Nombre.ToLower() && !x.Borrado);
            }   

            if (existe)
                return (false, "Ya existe un depósito con ese mismo nombre");

            if (String.IsNullOrWhiteSpace(modelo.IdProducto))
            {
                return (false, "Se requiere un producto asociado a un depósito");
            }

            var existeProducto = await contextGerencia.Productos
                .AnyAsync(x => x.IdUnico == modelo.IdProducto && x.Estado && !x.Borrado);

            if (!existeProducto)
            {
                return (false, "El producto que seleccionó no existe o se encuentra desactivado");
            }

            return (true, "El modelo es óptimo");
        }
    }
}
