using iTextSharp.text;
using iTextSharp.text.pdf;

namespace API.Servicios.Reportes.PDF
{
    public static class PDFUtils
    {
        public static PdfPTable FilasVacias(int numeroFilas)
        {
            PdfPTable tablaVacia = new PdfPTable(1);
            tablaVacia.WidthPercentage = 100;
            tablaVacia.HorizontalAlignment = Element.ALIGN_LEFT;

            for (int i = 0; i < numeroFilas; i++)
            {
                PdfPCell celda = new PdfPCell(new Phrase("\n"));
                celda.HorizontalAlignment = Element.ALIGN_LEFT;
                celda.Border = 0;

                tablaVacia.AddCell(celda);
                tablaVacia.CompleteRow();
            }

            return tablaVacia;
        }
    }
}
