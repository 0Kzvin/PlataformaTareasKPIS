using API.Database.Almacenamiento;
using API.Database.Almacenamiento.DTOs.Cargas;
using API.Database.Almacenamiento.DTOs.Cargas.Filtros;
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
    public class CargasDepositosAlmacenamientoController : ControllerBase
    {
        private readonly ModuloAlmacenamientoExplosivosContext context;
        private readonly ModuloGerenciaExplosivosContext contextGerencia;
        private readonly IMapper mapper;

        public CargasDepositosAlmacenamientoController(ModuloAlmacenamientoExplosivosContext context, ModuloGerenciaExplosivosContext contextGerencia,IMapper mapper)
        {
            this.context = context;
            this.contextGerencia = contextGerencia;
            this.mapper = mapper;
        }

        [AutorizarPermisos(PermisosAlmacenamiento.PERMISO_LISTAR_CARGAS)]
        [HttpGet("[action]")]
        public async Task<ActionResult> Listar([FromQuery] FiltroCargasAlmacenamientoDTO filtro)
        {
            var modelos = context.CargasDepositosAlmacenamientos
                .OrderByDescending(c => c.FechaHoraInicial)
                .AsQueryable();

            if (filtro.TraerDescargas != true)
            {
                modelos = modelos.Where(c => c.VolumenCargado > 0);
            }

            if (filtro.TraerCargas != true)
            {
                modelos = modelos.Where(c => c.VolumenCargado < 0);
            }

            if (!String.IsNullOrWhiteSpace(filtro.Producto))
            {
                modelos = modelos.Where(c => c.Producto == filtro.Producto);
            }

            if (!String.IsNullOrWhiteSpace(filtro.Deposito))
            {
                modelos = modelos.Where(c => c.Deposito == filtro.Deposito);
            }

            if (!String.IsNullOrWhiteSpace(filtro.NumeroEconomico))
            {
                modelos = modelos.Where(c => c.NumeroEconomico == filtro.NumeroEconomico);
            }

            if (filtro.FechaInicial.HasValue)
            {
                if(filtro.FechaFinal.HasValue)
                {
                    var fechaFinalMasUnDia = filtro.FechaFinal.Value.AddDays(1);
                    modelos = modelos.Where(c => c.FechaHoraInicial >= filtro.FechaInicial.Value && c.FechaHoraInicial < fechaFinalMasUnDia);
                }
                else
                {
                    modelos = modelos.Where(c => c.FechaHoraInicial.Date == filtro.FechaInicial.Value.Date && c.FechaHoraInicial.Year == filtro.FechaInicial.Value.Year);
                }
            }

            var listaModelos = await modelos.ToListAsync();

            var modelosDTO = mapper.Map<List<CargasDepositoAlmacenamientoDTO>>(listaModelos);

            var nombresProductos = modelosDTO
                .Select(m => m.Producto)
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Distinct()
                .ToList();

            var nombreDepositos = modelosDTO
                .Select(x => x.Deposito)
                .Where(d => !string.IsNullOrWhiteSpace(d))
                .Distinct()
                .ToList();

            var depositos = await context.DepositosAlmacenamiento
                .Where(x => nombreDepositos.Contains(x.Nombre))
                .ToListAsync();

            var coloresPorProducto = await contextGerencia.Productos
                .Where(p => nombresProductos.Contains(p.Nombre))
                .ToDictionaryAsync(p => p.Nombre, p => p.CodigoColor);

            foreach (var modelo in modelosDTO)
            {
                var depositoModelo = depositos.Find(x => x.Nombre == modelo.Deposito);

                if(!string.IsNullOrWhiteSpace(modelo.Deposito) && depositoModelo != null)
                {
                    modelo.CapacidadMaximaDeposito = depositoModelo.CapacidadMaxima;
                    modelo.PorcentajeInicial = (modelo.VolumenInicial * 100) / depositoModelo.CapacidadMaxima;
                    modelo.PorcentajeFinal = (modelo.VolumenFinal * 100) / depositoModelo.CapacidadMaxima;
                    modelo.LimiteMaximo = depositoModelo.LimiteMaximo;
                    modelo.LimiteAlto = depositoModelo.LimiteAlto;
                    modelo.LimiteBajo= depositoModelo.LimiteBajo;
                    modelo.LimiteMinimo= depositoModelo.LimiteMinimo;
                }

                if (!string.IsNullOrWhiteSpace(modelo.Producto) &&
                    coloresPorProducto.TryGetValue(modelo.Producto, out var color))
                {
                    modelo.ColorProducto = color;
                }
                else
                {
                    modelo.ColorProducto = string.Empty;
                }
            }

            var respuestaValidada = RespuestaGenericaDTO<List<CargasDepositoAlmacenamientoDTO>>.CrearRespuestaExitosa(modelosDTO);

            return Ok(respuestaValidada);
        }
    }
}
