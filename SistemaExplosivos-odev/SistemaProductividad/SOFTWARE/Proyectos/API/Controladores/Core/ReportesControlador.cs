using API.Database.Core;
using API.Database.Core.DTOs.Dashboard;
using API.Database.Core.Entidades;
using API.Database.Core.Enums;
using API.Servicios.Reportes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controladores.Core
{
    [Route("api/core/Reportes")]
    [ApiController]
    // [Authorize]
    public class ReportesControlador : Controller
    {
        private readonly SistemaProductividadContext _context;
        private readonly ServicioReportes _servicioReportes;

        public ReportesControlador(SistemaProductividadContext context)
        {
            _context = context;
            _servicioReportes = new ServicioReportes(); // Should inject via DI in real app
        }

        [HttpGet("Departamento/{id}")]
        public async Task<IActionResult> DescargarReporte(int id)
        {
            var depto = await _context.Set<Departamentos>().FindAsync(id);
            if (depto == null) return NotFound("Departamento no encontrado");

            // TODO: Validate user is Leader/Admin

            var tareasQuery = _context.Set<Tareas>().Where(t => t.DepartamentoId == id);
            
            var stats = new DashboardDepartamentoDTO
            {
                NombreDepartamento = depto.Nombre,
                TotalTareas = await tareasQuery.CountAsync(),
                Pendientes = await tareasQuery.CountAsync(t => t.Estado == EstadoEnum.Pendiente),
                EnProceso = await tareasQuery.CountAsync(t => t.Estado == EstadoEnum.EnProceso),
                Terminadas = await tareasQuery.CountAsync(t => t.Estado == EstadoEnum.Terminada),
                Vencidas = await tareasQuery.CountAsync(t => t.Estado == EstadoEnum.Vencida),
                Eficiencia = 0
            };
            
            if (stats.TotalTareas > 0)
            {
                stats.Eficiencia = (double)stats.Terminadas / stats.TotalTareas * 100;
            }

            var pdfBytes = _servicioReportes.GenerarReporteDepartamento(stats);

            return File(pdfBytes, "application/pdf", $"Reporte_{depto.Nombre}_{DateTime.Now:yyyyMMdd}.pdf");
        }
    }
}
