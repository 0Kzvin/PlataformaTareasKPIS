using API.Database.Almacenamiento;
using API.Database.Almacenamiento.DTOs.Depositos;
using API.Database.Almacenamiento.DTOs.Equipos;
using API.Database.Almacenamiento.DTOs.Equipos.Filtros;
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
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
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
    public class EquiposAlmacenamientoController : ControllerBase
    {
        private readonly ModuloAlmacenamientoExplosivosContext context;
        private readonly ModuloGerenciaExplosivosContext contextGerencia;
        private readonly IMapper mapper;

        public EquiposAlmacenamientoController(ModuloAlmacenamientoExplosivosContext context, ModuloGerenciaExplosivosContext contextGerencia,IMapper mapper)
        {
            this.context = context;
            this.contextGerencia = contextGerencia;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult> Listar([FromQuery]FiltroEquiposAlmacenamientoDTO filtro)
        {
            var equipos = context.EquiposAlmacenamiento
                .OrderByDescending(x => x.FechaModificacion)
                .AsQueryable();

            if(filtro.Borrado.HasValue)
            {
                equipos = equipos.Where(x => x.Borrado == filtro.Borrado.Value);
            }   

            if(filtro.Estado.HasValue)
            {
                equipos = equipos.Where(x => x.Estado == filtro.Estado.Value);
            }

            var listaEquipos = await equipos.ToListAsync();

            var productosId = listaEquipos
                         .Select(d => d.IdProducto)
                         .Distinct()
                         .ToList();

            var productos = await contextGerencia.Productos
                .Where(p => productosId.Contains(p.IdUnico))
                .ToListAsync();

            var productosDTO = mapper.Map<List<ProductoDepositoAlmacenamientoDTO>>(productos);

            var productosPorId = productosDTO
                .ToDictionary(p => p.IdUnico);

            var resultado = listaEquipos.Select(equipo =>
                {
                    productosPorId.TryGetValue(equipo.IdProducto, out var producto);

                    return new EquiposAlmacenamientoDTO
                    {
                        Id = equipo.Id,
                        IdUnico = equipo.IdUnico,
                        NumeroEconomico = equipo.NumeroEconomico,
                        Apodo = equipo.Apodo,
                        
                        Producto = producto != null ? producto.Nombre : "Producto no encontrado",
                        ColorProducto = producto != null ? producto.CodigoColor : "Color no encontrado",

                        CantidadActual = equipo.CantidadActual, 
                        Capacidad = equipo.Capacidad,   
                        EsExterno = equipo.EsExterno,
                        
                        Borrado = equipo.Borrado,
                        Estado = equipo.Estado,

                        FechaModificacion = equipo.FechaModificacion,
                        FechaRegistro = equipo.FechaRegistro,

                        ProductoDTO = producto,
                    };
                })
                .ToList();

            var respuestaValidada = RespuestaGenericaDTO<List<EquiposAlmacenamientoDTO>>.CrearRespuestaExitosa(resultado);

            return Ok(respuestaValidada);
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult> Obtener([FromQuery] int id)
        {
            var modelo = await context.EquiposAlmacenamiento
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

            var resultado = new EquiposAlmacenamientoDTO
            {
                Id = modelo.Id,
                IdUnico = modelo.IdUnico,
                NumeroEconomico = modelo.NumeroEconomico,
                Apodo = modelo.Apodo,

                Producto = productoDTO != null ? productoDTO.Nombre : "Producto no encontrado",
                ColorProducto = productoDTO != null ? productoDTO.CodigoColor : "Color no encontrado",

                CantidadActual = modelo.CantidadActual,
                Capacidad = modelo.Capacidad,
                EsExterno = modelo.EsExterno,

                Borrado = modelo.Borrado,
                Estado = modelo.Estado,

                FechaModificacion = modelo.FechaModificacion,
                FechaRegistro = modelo.FechaRegistro,

                ProductoDTO = productoDTO,
            };

            var respuestaValidada = RespuestaGenericaDTO<EquiposAlmacenamientoDTO>.CrearRespuestaExitosa(resultado);

            return Ok(respuestaValidada);
        }

        [AutorizarPermisos(PermisosAlmacenamiento.PERMISO_CREAR_EQUIPO)]
        [HttpPost("[action]")]
        public async Task<ActionResult> Registrar([FromBody] RegistrarEquiposAlmacenamiento modelo)
        {
            var (esValido, mensaje) = await ComprobarModeloEquipoAsync(modelo);

            if (!esValido)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores(mensaje);

                return BadRequest(respuesta);
            }

            if (String.IsNullOrWhiteSpace(modelo.IdProducto))
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("Se requiere un producto asociado a un equipo");

                return BadRequest(respuesta);
            }

            var existeProducto = await contextGerencia.Productos
                .AnyAsync(x => x.IdUnico == modelo.IdProducto && x.Estado && !x.Borrado);

            if (!existeProducto)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("El producto que seleccionó no existe o se encuentra desactivado");

                return BadRequest(respuesta);
            }

            var nuevoEquipo = mapper.Map<EquiposAlmacenamiento>(modelo);

            nuevoEquipo.IdUnico = Guid.NewGuid().ToString();

            await context.EquiposAlmacenamiento.AddAsync(nuevoEquipo);

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
        public async Task<ActionResult> Modificar([FromBody] ModificarEquiposAlmacenamiento modelo)
        {
            if (modelo.Id <= 0)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("El Id del equipo es inválido");

                return BadRequest(respuesta);
            }

            var modeloAValidar = mapper.Map<RegistrarEquiposAlmacenamiento>(modelo);

            var (esValido, mensaje) = await ComprobarModeloEquipoAsync(modeloAValidar, modelo.Id);

            if (!esValido)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores(mensaje);

                return BadRequest(respuesta);
            }
            if (String.IsNullOrWhiteSpace(modelo.IdProducto))
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("Se requiere un producto asociado a un equipo");

                return BadRequest(respuesta);
            }

            var existeProducto = await contextGerencia.Productos
                .AnyAsync(x => x.IdUnico == modelo.IdProducto && x.Estado && !x.Borrado);

            if (!existeProducto)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("El producto que seleccionó no existe o se encuentra desactivado");

                return BadRequest(respuesta);
            }

            var equipoExistente = await context.EquiposAlmacenamiento
                .FirstOrDefaultAsync(x => x.Id == modelo.Id && !x.Borrado);

            if (equipoExistente == null)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("No se encontró un depósito con el Id proporcionado");
                return BadRequest(respuesta);
            }

            mapper.Map(modelo, equipoExistente);

            context.Entry(equipoExistente).State = EntityState.Modified;

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
            var equipoAlmacenamiento = await context.EquiposAlmacenamiento.FirstOrDefaultAsync(x => x.Id == id);

            if (equipoAlmacenamiento == null)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("No se encontró un equipo con el Id proporcionado");

                return BadRequest(respuesta);
            }

            equipoAlmacenamiento.Estado = !equipoAlmacenamiento.Estado;
            equipoAlmacenamiento.FechaModificacion = DateTime.Now;

            context.Entry(equipoAlmacenamiento).State = EntityState.Modified;

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
            var equipoAlmacenamiento = await context.EquiposAlmacenamiento.FirstOrDefaultAsync(x => x.Id == id);

            if (equipoAlmacenamiento == null)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("No se encontró un depósito con el Id proporcionado");

                return BadRequest(respuesta);
            }

            equipoAlmacenamiento.Borrado = true;
            equipoAlmacenamiento.FechaModificacion = DateTime.Now;

            context.Entry(equipoAlmacenamiento).State = EntityState.Modified;

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
        private async Task<(bool EsValido, string Mensaje)> ComprobarModeloEquipoAsync(RegistrarEquiposAlmacenamiento modelo, int? idModelo = null)
        {
            bool existe;

            if(idModelo.HasValue)
            {
                existe = await context.EquiposAlmacenamiento
                .AnyAsync(x => x.NumeroEconomico.ToLower() == modelo.NumeroEconomico.ToLower() && x.Id != idModelo.Value && !x.Borrado);
            }
            else
            {
                existe = await context.EquiposAlmacenamiento
                .AnyAsync(x => x.NumeroEconomico.ToLower() == modelo.NumeroEconomico.ToLower() && !x.Borrado);
            }

            if (existe)
                return (false, "Ya existe un equipo con ese mismo número de económico");

            return (true, "El modelo es óptimo");
        }
    }
}
