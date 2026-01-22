using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using API.Database.Core.DTOs.Dashboard;

namespace API.Servicios.Reportes
{
    public class ServicioReportes
    {
        public byte[] GenerarReporteDepartamento(DashboardDepartamentoDTO data)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text($"Reporte de Productividad: {data.NombreDepartamento}")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(20);

                            x.Item().Text($"Fecha de Solicitud: {DateTime.Now:g}");

                            x.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Métrica");
                                    header.Cell().Element(CellStyle).Text("Valor");

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                                    }
                                });

                                table.Cell().Element(CellStyle).Text("Total Tareas");
                                table.Cell().Element(CellStyle).Text(data.TotalTareas.ToString());

                                table.Cell().Element(CellStyle).Text("Completadas");
                                table.Cell().Element(CellStyle).Text(data.Terminadas.ToString());

                                table.Cell().Element(CellStyle).Text("Pendientes");
                                table.Cell().Element(CellStyle).Text(data.Pendientes.ToString());
                                
                                table.Cell().Element(CellStyle).Text("Vencidas");
                                table.Cell().Element(CellStyle).Text(data.Vencidas.ToString());

                                table.Cell().Element(CellStyle).Text("Eficiencia");
                                table.Cell().Element(CellStyle).Text($"{data.Eficiencia:F2}%");

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                                }
                            });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Generado por SistemaProductividad - ");
                            x.CurrentPageNumber();
                        });
                });
            });

            return document.GeneratePdf();
        }
    }
}
