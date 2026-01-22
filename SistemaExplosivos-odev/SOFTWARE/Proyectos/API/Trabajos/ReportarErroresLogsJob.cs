using Microsoft.AspNetCore.Hosting;
using Quartz;
using System.Threading.Tasks;
using System;
using ServiciosAPIG3.ServiciosAPI.Quartz.Base;
using ServiciosAPIG3.ServiciosAPI.Logger.Context;

namespace API.Trabajos
{
    [DisallowConcurrentExecution]
    public class ReportarErroresLogsJob : QuartzBaseJob
    {
        private readonly SerilogDbContext serilogDbContext;

        public ReportarErroresLogsJob(SerilogDbContext serilogDbContext)
        {
            this.serilogDbContext = serilogDbContext;
        }

        protected override async Task TrabajoEspecificoAsync(IJobExecutionContext context)
        {
            try
            {
                //// TODO: Enviar reporte de logs

                //using (var scope = services.CreateScope())
                //{
                //    var adminContext = scope.ServiceProvider.GetRequiredService<ModuloAdministracionCalidadAceitesContext>();
                //    var datosCliente = scope.ServiceProvider.GetRequiredService<DatosClienteDTO>();
                //    var _correo = scope.ServiceProvider.GetRequiredService<IServicioCorreo>();
                //    var mailkitDto = scope.ServiceProvider.GetRequiredService<IOptions<MailkitDTO>>();

                //    TimeSpan HoraCorte = new TimeSpan(6, 45, 0);
                //    var ahora = DateTime.Now.Date + HoraCorte;
                //    var fechaReporteString = ahora.ToString("dd-MM-yy");
                //    var ayer = ahora.AddDays(-1);
                //    string mensajeHtml = string.Empty;

                //    var logs = await adminContext.Logs.Where(x => x.FechaHora >= ayer && x.Usuario == "OptixPanel").Select(x => new ReporteLogsDTO
                //    {
                //        Accion = x.Accion,
                //        Cliente = x.DatosPeticion,
                //        Direccion = x.Direccion,
                //        FechaHora = x.FechaHora,
                //        Mensaje = x.Mensaje
                //    }).ToListAsync();


                //    // Crear lista anónima agrupando por Direccion y contando registros
                //    var resumenDirecciones = logs
                //        .GroupBy(x => x.Direccion)
                //        .Select(g => new
                //        {
                //            Direccion = g.Key,
                //            CantidadRegistros = g.Count()
                //        })
                //        .OrderByDescending(x => x.CantidadRegistros)
                //        .ToList();


                //    var reporte = new ReporteEXCELGenerico<ReporteLogsDTO>();
                //    var bytesReporteExcel = reporte.Generar(logs, datosCliente.RazonSocial, "Reporte de Logs", "Logs de OptixPanels");

                //    var archivosAdjuntos = new List<ArchivoAdjuntoCorreoDTO>
                //    {
                //        new ArchivoAdjuntoCorreoDTO()
                //        {
                //            Contenido = bytesReporteExcel,
                //            TipoContenido = new ContentType(
                //                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                //                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                //            ),
                //            NombreArchivo = $"Reporte de Recepciones del dia {fechaReporteString}.xlsx"
                //        }
                //    };

                //    mensajeHtml += UtilsGenerales.ObtenerTablaHTML(resumenDirecciones);


                //    var destinatarios = new List<DestinatarioCorreoDTO>
                //    {
                //        new DestinatarioCorreoDTO()
                //        {
                //            Correo = mailkitDto.Value.CorreoLog,
                //            Nombre = "G3 Ingenieria"
                //        }
                //    };

                //    await _correo.EnviarCorreoAsincronoConArchivoAdjunto(mailkitDto.Value, destinatarios, $"{datosCliente.ClienteActual} reporte de logs {fechaReporteString}", "", cuerpoMensajeHTML: mensajeHtml, archivosAdjuntos, reenviarTambienAG3: false);
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
