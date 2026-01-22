using API.Database.Almacenamiento;
using API.Database.Almacenamiento.DTOs.Depositos;
using API.Database.Almacenamiento.DTOs.Supersacos;
using API.Database.Almacenamiento.DTOs.Supersacos.Filtros;
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
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controladores.Almacenamiento
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = ConstantesModulos.ALMACENAMIENTO)]
    [Route("almacenamiento/[controller]")]
    [ApiController]
    public class MovimientosSupersacosAlmacenamientoController : ControllerBase
    {
        private readonly ModuloAlmacenamientoExplosivosContext context;
        private readonly ModuloGerenciaExplosivosContext contextGerencia;
        private readonly IMapper mapper;

        public MovimientosSupersacosAlmacenamientoController(ModuloAlmacenamientoExplosivosContext context, ModuloGerenciaExplosivosContext contextGerencia, IMapper mapper)
        {
            this.context = context;
            this.contextGerencia = contextGerencia;
            this.mapper = mapper;
        }

        [AutorizarPermisos(PermisosAlmacenamiento.PERMISO_LISTAR_MOVIMIENTOS)]
        [HttpGet("[action]")]
        public async Task<ActionResult> Listar([FromQuery] FiltroMovimientosSupersacosDTO filtro)
        {
            var modelos = context.MovimientosSupersacosAlmacenamiento
                .Where(x => !x.Borrado)
                .OrderByDescending(x => x.FechaHora)
                .ThenByDescending(x => x.FechaRegistro)
                .ThenByDescending(x => x.Id)
                .AsQueryable();

            if (!String.IsNullOrWhiteSpace(filtro.IdProducto))
            {
                modelos = modelos.Where(c => c.IdProducto == filtro.IdProducto);
            }

            if (filtro.FechaInicial.HasValue)
            {
                if (filtro.FechaFinal.HasValue)
                {
                    var fechaFinalMasUnDia = filtro.FechaFinal.Value.AddDays(1);
                    modelos = modelos.Where(c => c.FechaHora >= filtro.FechaInicial.Value && c.FechaHora < fechaFinalMasUnDia);
                }
                else
                {
                    modelos = modelos.Where(c => c.FechaHora.Date == filtro.FechaInicial.Value.Date && c.FechaHora.Year == filtro.FechaInicial.Value.Year);
                }
            }

            var listaModelos = await modelos.ToListAsync();

            var modelosDTO = mapper.Map<List<MovimientosSupersacosAlmacenamientoDTO>>(listaModelos);

            var productosId = listaModelos
                .Select(d => d.IdProducto)
                .Distinct()
                .ToList();

            var productos = await contextGerencia.Productos
                .Where(p => productosId.Contains(p.IdUnico))
                .ToListAsync();

            var productosDTO = mapper.Map<List<ProductoDepositoAlmacenamientoDTO>>(productos);

            var productosPorId = productosDTO
                .ToDictionary(p => p.IdUnico);

            var resultado = listaModelos
                .Select(d =>
                {
                    productosPorId.TryGetValue(d.IdProducto, out var producto);

                    return new MovimientosSupersacosAlmacenamientoDTO
                    {
                        Id = d.Id,
                        IdUnico = d.IdUnico,

                        Producto = producto != null ? producto.Nombre : string.Empty,
                        ColorProducto = producto?.CodigoColor,

                        CantidadInicial = d.CantidadInicial,
                        CantidadFinal = d.CantidadFinal,
                        CantidadMovimiento = d.CantidadMovimiento,

                        FechaModificacion = d.FechaModificacion,
                        FechaRegistro = d.FechaRegistro,

                        Ubicacion = d.Ubicacion,
                        Observaciones = d.Observaciones,

                        FechaHora = d.FechaHora,

                        ProductoDTO = producto
                    };
                })
                .ToList();


            if (filtro.AgruparPorDia)
            {
                modelosDTO = [.. resultado
                    .GroupBy(m => m.FechaHora.Date)
                    .Select(grupo =>
                    {

                        var movimientosDelDia = grupo.OrderBy(m => m.FechaHora).ToList();
                        var primero = movimientosDelDia.First();
                        var ultimo = movimientosDelDia.Last();

                        return new MovimientosSupersacosAlmacenamientoDTO
                        {
                            Id = primero.Id,
                            IdUnico = primero.IdUnico,
                            Ubicacion = primero.Ubicacion,
                            ColorProducto = primero.ColorProducto,
                            Producto = primero.Producto,
                            Observaciones = primero.Observaciones,

                            CantidadInicial = primero.CantidadInicial,
                            CantidadFinal = ultimo.CantidadFinal,
                            CantidadMovimiento = movimientosDelDia.Sum(m => m.CantidadMovimiento),

                            FechaModificacion = ultimo.FechaModificacion,
                            FechaRegistro = ultimo.FechaRegistro,
                            FechaHora = grupo.Key,

                            Movimientos = movimientosDelDia
                        };
                    })
                    .OrderBy(r => r.FechaHora)];
            }

            var respuestaMovimiento = new RespuestaMovimientosSupersacosAlmacenamientoDTO
            {
                InventarioInicial = resultado.Any() ? resultado.Last().CantidadInicial : 0,

                Salidas = resultado
                    .Where(m => m.CantidadMovimiento < 0)
                    .Sum(m => Math.Abs(m.CantidadMovimiento)),

                Entradas = resultado
                    .Where(m => m.CantidadMovimiento > 0)
                    .Sum(m => m.CantidadMovimiento),

                InventarioFinal = resultado.Any() ? resultado.First().CantidadFinal : 0,

                FechaInicial = resultado.Any() ? resultado.Last().FechaHora : (filtro.FechaInicial ?? DateTime.Now),

                FechaFinal = resultado.Any() ? resultado.First().FechaHora : (filtro.FechaFinal ?? DateTime.Now),

                Movimientos = resultado
            };

            var respuestaValidada = RespuestaGenericaDTO<RespuestaMovimientosSupersacosAlmacenamientoDTO>.CrearRespuestaExitosa(respuestaMovimiento);

            return Ok(respuestaValidada);
        }

        [AutorizarPermisos(PermisosAlmacenamiento.PERMISO_CREAR_MOVIMIENTO)]
        [HttpPost("[action]")]
        public async Task<ActionResult> Registrar([FromBody] RegistrarMovimientoSupersacoAlmacenamientoDTO modelo)
        {
             var (esValido, mensaje) = await ValidarMovimientoSupersacoAlmacenamiento(modelo);

            if (!esValido)
            {
                var respuestaError = RespuestaGenericaDTO<string>
                    .CrearRespuestaConErrores(mensaje);

                return BadRequest(respuestaError);
            }

            if (!DateTime.TryParse(
                modelo.FechaHora,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeLocal,
                out DateTime fechaHora))
            {
                var respuestaError = RespuestaGenericaDTO<string>
                    .CrearRespuestaConErrores("Formato de fecha inválido");

                return BadRequest(respuestaError);
            }

            var nuevoModelo = mapper.Map<MovimientosSupersacosAlmacenamiento>(modelo);

            nuevoModelo.IdUnico = Guid.NewGuid().ToString();
            nuevoModelo.FechaHora = fechaHora;

            await context.MovimientosSupersacosAlmacenamiento.AddAsync(nuevoModelo);

            try
            {
                await context.SaveChangesAsync();

                var respuesta = RespuestaGenericaDTO<string>
                    .CrearRespuestaExitosa("Registro exitoso");

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                var respuesta = RespuestaGenericaDTO<string>
                    .CrearRespuestaConErrores(
                        "Ocurrió un error inesperado",
                        ["Error en la base de datos", ex.Message]
                    );

                return BadRequest(respuesta);
            }
        }

        [AllowAnonymous]
        [HttpPut("[action]")]
        public async Task<ActionResult> Modificar([FromBody] ModificarMovimientoSupersacosAlmacenamientoDTO modelo)
        {
            if (modelo.Id <= 0)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("El Id del movimiento es inválido");

                return BadRequest(respuesta);
            }

            var modeloAValidar = mapper.Map<RegistrarMovimientoSupersacoAlmacenamientoDTO>(modelo);

            var (esValido, mensaje) = await ValidarMovimientoSupersacoAlmacenamiento(modeloAValidar);

            if (!esValido)
            {
                var respuestaError = RespuestaGenericaDTO<string>.CrearRespuestaConErrores(mensaje);

                return BadRequest(respuestaError);
            }

            var modeloExistente = await context.MovimientosSupersacosAlmacenamiento
                .FirstOrDefaultAsync(m => m.Id == modelo.Id);

            if (modeloExistente == null)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("El movimiento especificado no existe");

                return BadRequest(respuesta);
            }

            mapper.Map(modelo, modeloExistente);

            context.Entry(modeloExistente).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();

                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaExitosa("Actualización exitosa");

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("Ocurrió un error inesperado al actualizar el registro en la base de datos", ["Error en la base de datos", ex.Message]);
                
                return BadRequest(respuesta);
            }
        }

        [AllowAnonymous]
        [HttpPut("[action]")]
        public async Task<ActionResult> Borrar([FromQuery] int id)
        {
            var modelo = await context.MovimientosSupersacosAlmacenamiento.FirstOrDefaultAsync(x => x.Id == id);

            if (modelo == null)
            {
                var respuesta = RespuestaGenericaDTO<string>.CrearRespuestaConErrores("No se encontró un movimiento con el Id proporcionado");

                return BadRequest(respuesta);
            }

            modelo.Borrado = true;

            modelo.FechaModificacion = DateTime.Now;

            context.Entry(modelo).State = EntityState.Modified;

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

        private async Task<(bool EsValido, string Mensaje)> ValidarMovimientoSupersacoAlmacenamiento(RegistrarMovimientoSupersacoAlmacenamientoDTO modelo)
        {
            var producto = await contextGerencia.Productos
                .FirstOrDefaultAsync(p => p.IdUnico == modelo.IdProducto);

            if (producto == null)
            {
                return (false, "El producto especificado no existe.");
            }

            if (modelo.CantidadMovimiento == 0)
            {
                return (false, "La cantidad de movimiento no puede ser cero.");
            }

            var ultimoMovimiento = await context.MovimientosSupersacosAlmacenamiento
                .Where(m => m.IdProducto == modelo.IdProducto)
                .OrderByDescending(m => m.FechaHora)
                .FirstOrDefaultAsync();

            var cantidadInicial = ultimoMovimiento != null ? ultimoMovimiento.CantidadFinal : 0;
            
            var cantidadFinal = cantidadInicial + modelo.CantidadMovimiento;
            
            if (cantidadFinal < 0)
            {
                return (false, "El movimiento resultaría en una cantidad negativa de supersacos.");
            }

            return (true, string.Empty);
        }
    }
}
