using API.Database.Almacenamiento;
using API.Database.Almacenamiento.DTOs.Depositos;
using API.Database.Almacenamiento.DTOs.EstatusDepositos;
using API.Database.Almacenamiento.DTOs.EstatusDepositos.Filtros;
using API.Database.Almacenamiento.DTOs.Supersacos;
using API.Database.Almacenamiento.DTOs.Supersacos.Grafico;
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
    public class EstatusDepositosAlmacenamientoController : ControllerBase
    {
        private readonly ModuloAlmacenamientoExplosivosContext context;
        private readonly ModuloGerenciaExplosivosContext contextGerencia;
        private readonly IMapper mapper;

        public EstatusDepositosAlmacenamientoController(ModuloAlmacenamientoExplosivosContext context, ModuloGerenciaExplosivosContext contextGerencia, IMapper mapper)
        {
            this.context = context;
            this.contextGerencia = contextGerencia;
            this.mapper = mapper;
        }

        [AutorizarPermisos(PermisosAlmacenamiento.PERMISO_LISTAR_ESTATUS)]
        [HttpGet("[action]")]
        public async Task<ActionResult> Listar()
        {
            var modelos = await context.EstatusDepositosAlmacenamiento
                .OrderByDescending(c => c.FechaHora)
                .ToListAsync();

            var respuestaValidada = RespuestaGenericaDTO<List<EstatusDepositosAlmacenamiento>>.CrearRespuestaExitosa(modelos);

            return Ok(respuestaValidada);
        }

        [AutorizarPermisos(PermisosAlmacenamiento.PERMISO_LISTAR_ESTATUS)]
        [HttpGet("[action]")]
        public async Task<ActionResult> ListarTanquesEstatus([FromQuery] FiltroEstatusTanquesDTO filtro) 
        {
            var query = context.DepositosAlmacenamiento
                .Where(x => x.Estado && !x.Borrado)
                .OrderBy(x => x.Nombre)
                .AsQueryable();

            if(!String.IsNullOrEmpty(filtro.Estacion))
            {
                query = query.Where(x => x.Ubicacion.ToUpper().Trim() == filtro.Estacion.ToUpper().Trim());
            }

            var listaDepositos = await query.ToListAsync();
 
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

            var estacionesAlmacenamiento = await contextGerencia.Estaciones
                .Where(x => x.Estado && !x.Borrado)
                .ToListAsync();

            var listaDepositosDTO = new List<DepositosEstatusAlmacenamientoDTO>();

            var hoy = DateTime.Now;

            foreach (var deposito in listaDepositos)
            {
                productosPorId.TryGetValue(deposito.IdProducto, out var producto);

                var depositoDTO = new DepositosEstatusAlmacenamientoDTO
                {
                    IdUnico = deposito.IdUnico,
                    Nombre = deposito.Nombre,
                    Apodo = deposito.Apodo,
                    
                    Producto = producto.Nombre ?? "Producto no encontrado",
                    ColorProducto = producto.CodigoColor ?? string.Empty,

                    CapacidadMaxima = deposito.CapacidadMaxima,
                    AlturaMaxima = deposito.AlturaMaxima,
                    CapacidadOperativa = deposito.CapacidadOperativa,
                    AlturaOperativa = deposito.AlturaOperativa,

                    LimiteAlto = deposito.LimiteAlto,
                    LimiteMaximo = deposito.LimiteMaximo,
                    LimiteBajo = deposito.LimiteBajo,
                    LimiteMinimo = deposito.LimiteMinimo,

                    Ubicacion = deposito.Ubicacion,

                    ProductoDTO = producto
                };

                var estatusTanque = await context.EstatusDepositosAlmacenamiento
                    .Where(x =>
                        x.Deposito.ToUpper().Trim() == deposito.Nombre.ToUpper().Trim() &&
                        x.FechaHora >= hoy.AddMonths(-3)
                    )
                    .OrderByDescending(x => x.FechaHora)
                    .Select(x => new EstatusDepositoGraficaDTO
                    {
                        IdUnico = deposito.IdUnico,
                        Deposito = deposito.Nombre,
                        Producto = producto.Nombre ?? "Producto no encontrado",
                        Ubicacion = deposito.Ubicacion,
                        Altura = x.Altura,
                        Volumen = x.Volumen,
                        PorcentajeNivel = x.PorcentajeNivel,
                        LimiteAlto = x.LimiteAlto,
                        LimiteMaximo = x.LimiteMaximo,
                        LimiteBajo = x.LimiteBajo,
                        LimiteMinimo = x.LimiteMinimo,
                        HayAlarma = x.HayAlarma,
                        DispositivoDeMedicion = x.DispositivoDeMedicion,
                        FechaHora = x.FechaHora,
                    })
                    .FirstOrDefaultAsync();

                if (estatusTanque != null)
                {
                    depositoDTO.EstatusDTO = estatusTanque;
                }
                else
                {
                    var estatusDTO = new EstatusDepositoGraficaDTO
                    {
                        IdUnico = deposito.IdUnico,
                        Deposito = deposito.Nombre,
                        Producto = producto.Nombre ?? "Producto no encontrado",
                        Ubicacion = deposito.Ubicacion,
                        Altura = 0,
                        Volumen = 0,
                        PorcentajeNivel = 0,
                        LimiteAlto = 0,
                        LimiteMaximo = 0,
                        LimiteBajo = 0,
                        LimiteMinimo = 0,
                        HayAlarma = false,
                        DispositivoDeMedicion = "Sin datos",
                        FechaHora = null
                    };

                    depositoDTO.EstatusDTO = estatusDTO;
                }

                listaDepositosDTO.Add(depositoDTO);
            }

            var respuestaValidada = RespuestaGenericaDTO<List<DepositosEstatusAlmacenamientoDTO>>.CrearRespuestaExitosa(listaDepositosDTO);

            return Ok(respuestaValidada);
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult> ListarSuperSacosEstatus([FromQuery] FiltroEstatusSuperSacosDTO filtro)
        {
            var hoy = DateTime.Today;

            var movimientosActual = await context.MovimientosSupersacosAlmacenamiento
                .Where(m => m.FechaHora >= filtro.FechaInicio && m.FechaHora <= filtro.FechaFinal && !m.Borrado)
                .OrderBy(m => m.FechaHora)
                .ToListAsync();

            int diasPeriodo = (filtro.FechaFinal.Date - filtro.FechaInicio.Date).Days + 1;

            var fechaInicioAnterior = filtro.FechaInicio.Date.AddDays(-diasPeriodo);
            var fechaFinalAnterior = filtro.FechaInicio.Date.AddDays(-1);

            var movimientosAnterior = await context.MovimientosSupersacosAlmacenamiento
                .Where(m => m.FechaHora >= fechaInicioAnterior && m.FechaHora <= fechaFinalAnterior && !m.Borrado)
                .OrderBy(m => m.FechaHora)
                .ToListAsync();

            var todosLosMovimientos = movimientosActual
                .Concat(movimientosAnterior)
                .ToList();

            var productosId = todosLosMovimientos
                .Select(m => m.IdProducto)
                .Distinct()
                .ToList();

            var productos = await contextGerencia.Productos
                .Where(p => productosId.Contains(p.IdUnico))
                .ToListAsync();

            var productosDTO = mapper.Map<List<ProductoDepositoAlmacenamientoDTO>>(productos);

            var productosPorId = productosDTO
                .ToDictionary(p => p.IdUnico);

            List<MovimientosSupersacosAlmacenamientoDTO>                 
                MapearMovimientos(
                List<MovimientosSupersacosAlmacenamiento> lista)
            {
                return [.. lista.Select(d =>
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
                })];
            }

            var modelosActualDTO = MapearMovimientos(movimientosActual);
            var modelosAnteriorDTO = MapearMovimientos(movimientosAnterior);

            if (modelosActualDTO.Count == 0)
            {
                var ultimo = await context.MovimientosSupersacosAlmacenamiento
                    .Where(m => m.FechaHora < filtro.FechaInicio && !m.Borrado)
                    .OrderByDescending(m => m.FechaHora)
                    .FirstOrDefaultAsync();

                if (ultimo != null)
                {
                    productosPorId.TryGetValue(ultimo.IdProducto, out var producto);

                    modelosActualDTO.Add(new MovimientosSupersacosAlmacenamientoDTO
                    {
                        CantidadInicial = ultimo.CantidadInicial,
                        CantidadFinal = ultimo.CantidadFinal,
                        CantidadMovimiento = 0,
                        FechaHora = filtro.FechaInicio.Date,
                        Producto = producto?.Nombre ?? string.Empty,
                        ColorProducto = producto?.CodigoColor,
                        ProductoDTO = producto
                    });
                }
            }

            if (modelosAnteriorDTO.Count == 0)
            {
                var ultimo = await context.MovimientosSupersacosAlmacenamiento
                    .Where(m => m.FechaHora < fechaInicioAnterior && !m.Borrado)
                    .OrderByDescending(m => m.FechaHora)
                    .FirstOrDefaultAsync();

                if (ultimo != null)
                {
                    productosPorId.TryGetValue(ultimo.IdProducto, out var producto);

                    modelosAnteriorDTO.Add(new MovimientosSupersacosAlmacenamientoDTO
                    {
                        CantidadInicial = ultimo.CantidadInicial,
                        CantidadFinal = ultimo.CantidadFinal,
                        CantidadMovimiento = 0,
                        FechaHora = fechaInicioAnterior,
                        Producto = producto?.Nombre ?? string.Empty,
                        ColorProducto = producto?.CodigoColor,
                        ProductoDTO = producto
                    });
                }
            }

            var graficoActual = await ConstruirGrafico(
                modelosActualDTO,
                filtro.FechaInicio,
                filtro.FechaFinal,
                hoy
            );

            var graficoAnterior = await ConstruirGrafico(
                modelosAnteriorDTO,
                fechaInicioAnterior,
                fechaFinalAnterior,
                hoy
            );

            var estatusDTO = new SupersacosEstatusAlmacenamientoDTO
            {
                Entradas = [.. modelosActualDTO
                    .Where(x => x.CantidadMovimiento > 0)
                    .OrderByDescending(x => x.FechaHora)
                    .ThenByDescending(x => x.FechaRegistro)
                    .ThenByDescending(x => x.Id)],

                Salidas = [.. modelosActualDTO.Where(x => x.CantidadMovimiento < 0)
                    .OrderByDescending(x => x.FechaHora)
                    .ThenByDescending(x => x.FechaRegistro)
                    .ThenByDescending(x => x.Id)],

                PeriodoActual = graficoActual,
                PeriodoAnterior = graficoAnterior
            };

            return Ok(estatusDTO);
        }

        private async Task<SupersacosGraficoDTO> ConstruirGrafico(
            List<MovimientosSupersacosAlmacenamientoDTO> movimientos,
            DateTime fechaInicio,
            DateTime fechaFinal,
            DateTime hoy)
        {
            int diferenciaDias = (fechaFinal.Date - fechaInicio.Date).Days;

            var movimientosOrdenados = movimientos
                .OrderBy(m => m.FechaHora)
                .ThenBy(m => m.FechaRegistro)
                .ThenBy(m => m.Id)
                .ToList();

            decimal ultimoValor = await context.MovimientosSupersacosAlmacenamiento
                .Where(m => m.FechaHora < fechaInicio)
                .OrderByDescending(m => m.FechaHora)
                .ThenByDescending(m => m.FechaRegistro)
                .ThenByDescending(m => m.Id)
                .Select(m => m.CantidadFinal)
                .FirstOrDefaultAsync();

            var lista = new List<SupersacosMovimientosGraficaDTO>();
            int indiceMovimiento = 0;

            for (int i = 0; i <= diferenciaDias; i++)
            {
                var fechaActual = fechaInicio.Date.AddDays(i);
                var finDia = fechaActual.AddDays(1);

                string nombreTiempo;
                if (diferenciaDias <= 7)
                {
                    var dia = fechaActual.ToString("dddd", new CultureInfo("es-MX"));
                    nombreTiempo = char.ToUpper(dia[0]) + dia.Substring(1);
                }
                else
                {
                    nombreTiempo = fechaActual.ToString("dd/MM/yy");
                }

                if (fechaActual > hoy.Date)
                {
                    lista.Add(new SupersacosMovimientosGraficaDTO
                    {
                        NombreTiempo = nombreTiempo,
                        FechaHora = fechaActual,
                        ValorMovimiento = 0
                    });
                    continue;
                }

                while (
                    indiceMovimiento < movimientosOrdenados.Count &&
                    movimientosOrdenados[indiceMovimiento].FechaHora < finDia
                )
                {
                    ultimoValor = movimientosOrdenados[indiceMovimiento].CantidadFinal;
                    indiceMovimiento++;
                }

                lista.Add(new SupersacosMovimientosGraficaDTO
                {
                    NombreTiempo = nombreTiempo,
                    FechaHora = fechaActual,
                    ValorMovimiento = ultimoValor
                });
            }

            return new SupersacosGraficoDTO
            {
                NombrePeriodo = $"{fechaInicio:dd/MM/yyyy} - {fechaFinal:dd/MM/yyyy}",
                FechaInicial = fechaInicio,
                FechaFinal = fechaFinal,
                MovimientosPeriodo = lista,
                ColorProducto = movimientosOrdenados.FirstOrDefault()?.ColorProducto ?? string.Empty,
            };
        }
    }
}
