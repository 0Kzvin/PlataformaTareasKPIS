using API.Database.Core;
using API.Database.Core.DTOs.Dashboard;
using API.Database.Core.Entidades;
using API.Database.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controladores.Core
{
    [Route("api/core/Dashboards")]
    [ApiController]
    // [Authorize]
    public class DashboardsControlador : Controller
    {
        private readonly SistemaProductividadContext _context;

        public DashboardsControlador(SistemaProductividadContext context)
        {
            _context = context;
        }

        [HttpGet("Global")]
        public async Task<ActionResult<DashboardGlobalDTO>> GetGlobal()
        {
            // Verify SuperAdmin access here
            
            var stats = new DashboardGlobalDTO
            {
                TotalDepartamentos = await _context.Set<Departamentos>().CountAsync(),
                TotalUsuarios = await _context.Users.CountAsync(),
                TotalTareas = await _context.Set<Tareas>().CountAsync(),
                TareasVencidas = await _context.Set<Tareas>().CountAsync(t => t.Estado == EstadoEnum.Vencida),
                TareasCompletadas = await _context.Set<Tareas>().CountAsync(t => t.Estado == EstadoEnum.Terminada)
            };

            return Ok(stats);
        }

        [HttpGet("Departamento/{id}")]
        public async Task<ActionResult<DashboardDepartamentoDTO>> GetDepartamento(int id)
        {
            var depto = await _context.Set<Departamentos>().FindAsync(id);
            if (depto == null) return NotFound();

            var tareasQuery = _context.Set<Tareas>().Where(t => t.DepartamentoId == id);
            
            var stats = new DashboardDepartamentoDTO
            {
                NombreDepartamento = depto.Nombre,
                TotalTareas = await tareasQuery.CountAsync(),
                Pendientes = await tareasQuery.CountAsync(t => t.Estado == EstadoEnum.Pendiente),
                EnProceso = await tareasQuery.CountAsync(t => t.Estado == EstadoEnum.EnProceso),
                Terminadas = await tareasQuery.CountAsync(t => t.Estado == EstadoEnum.Terminada),
                Vencidas = await tareasQuery.CountAsync(t => t.Estado == EstadoEnum.Vencida),
                Eficiencia = 0 // Calculate complex logic
            };
            
            if (stats.TotalTareas > 0)
            {
                stats.Eficiencia = (double)stats.Terminadas / stats.TotalTareas * 100;
            }

            return Ok(stats);
        }
    }
}
