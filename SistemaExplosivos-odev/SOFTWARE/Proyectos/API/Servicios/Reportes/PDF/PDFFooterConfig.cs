using iTextSharp.text;
using iTextSharp.text.pdf;

namespace API.Servicios.Reportes.PDF
{
    //CLASE DE AYUDA PARA GENERAR LOS NUMEROS DE PAGINAS AUTOMATICOS EN EL FOOTER DE LOS PDF
    class PDFFooterConfig : PdfPageEventHelper
    {
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            PdfPTable tbFooter = new PdfPTable(3);
            tbFooter.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            tbFooter.DefaultCell.Border = 0;

            tbFooter.AddCell(new Paragraph());
            tbFooter.AddCell(new Paragraph());

            var estilo = FontFactory.GetFont("Calibri", 10f, 0);

            PdfPCell celdaFooter = new PdfPCell(new Paragraph(writer.PageNumber.ToString(), estilo));
            celdaFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
            celdaFooter.Border = 0;

            tbFooter.AddCell(celdaFooter);
            tbFooter.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetBottom(document.BottomMargin) + 8, writer.DirectContent);
        }
    }
}
