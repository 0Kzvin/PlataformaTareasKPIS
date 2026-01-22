using API.Database.Core;
using API.Database.Core.DTOs.Dashboard;
using API.Database.Core.Entidades;
using API.Database.Core.Enums;
using API.Servicios.Reportes;
using API.Utilidades.Constantes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controladores.Core
{
    [ApiExplorerSettings(GroupName = ConstantesModulos.REPORTES)]
    [Route("reportes")]
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

        [HttpPost("ExportarPDF")]
        public async Task<IActionResult> ExportarPdf([FromQuery] int departamentoId)
        {
            var depto = await _context.Set<Departamentos>().FindAsync(departamentoId);
            if (depto == null) return NotFound("Departamento no encontrado");

            // TODO: Validate user is Leader/Admin

            var tareasQuery = _context.Set<Tareas>().Where(t => t.DepartamentoId == departamentoId);
            
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

        [HttpPost("ExportarExcel")]
        public IActionResult ExportarExcel()
        {
            var contenido = "Departamento,TotalTareas\nDemo,0\n";
            var bytes = System.Text.Encoding.UTF8.GetBytes(contenido);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Reporte_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        [HttpPost("ExportarCSV")]
        public IActionResult ExportarCsv()
        {
            var contenido = "Departamento,TotalTareas\nDemo,0\n";
            var bytes = System.Text.Encoding.UTF8.GetBytes(contenido);
            return File(bytes, "text/csv", $"Reporte_{DateTime.Now:yyyyMMdd}.csv");
        }
    }
}
