using OfficeOpenXml.Drawing.Chart;
using System;
using System.Drawing;
using System.Globalization;
using System.Xml;

namespace API.Servicios.Reportes.EXCEL
{
    public static class AyudaExcel
    {

        public static void SetLineChartColor(this ExcelChart chart, int serieIdx, int chartSeriesIndex, Color color)
        {
            var chartXml = chart.ChartXml;

            var nsa = chart.WorkSheet.Drawings.NameSpaceManager.LookupNamespace("a");
            var nsuri = chartXml.DocumentElement.NamespaceURI;

            var nsm = new XmlNamespaceManager(chartXml.NameTable);
            nsm.AddNamespace("a", nsa);
            nsm.AddNamespace("c", nsuri);

            var serieNode = chart.ChartXml.SelectSingleNode($@"c:chartSpace/c:chart/c:plotArea/c:barChart/c:ser[c:idx[@val='{serieIdx}']]", nsm);
            var serie = chart.Series[chartSeriesIndex];
            var points = serie.Series.Length;

            //Add reference to the color for the legend
            var srgbClr = chartXml.CreateNode(XmlNodeType.Element, "srgbClr", nsa);
            var att = chartXml.CreateAttribute("val");
            att.Value = $"{color.R:X2}{color.G:X2}{color.B:X2}";
            srgbClr.Attributes.Append(att);

            var solidFill = chartXml.CreateNode(XmlNodeType.Element, "solidFill", nsa);
            solidFill.AppendChild(srgbClr);

            var ln = chartXml.CreateNode(XmlNodeType.Element, "ln", nsa);
            ln.AppendChild(solidFill);

            var spPr = chartXml.CreateNode(XmlNodeType.Element, "spPr", nsuri);
            spPr.AppendChild(ln);

            serieNode.AppendChild(spPr);
        }

        public static void SetDataPointStyle(this ExcelBarChart chart, ExcelChartSerie series, Color color)
        {
            var i = 0;
            var found = false;
            foreach (var s in chart.Series)
            {
                if (s == series)
                {
                    found = true;
                    break;
                }
                ++i;
            }
            if (!found) throw new InvalidOperationException("series not found.");

            var nsm = chart.WorkSheet.Drawings.NameSpaceManager;
            var nschart = nsm.LookupNamespace("c");
            var nsa = nsm.LookupNamespace("a");
            var node = chart.ChartXml.SelectSingleNode(@"c:chartSpace/c:chart/c:plotArea/c:barChart/c:ser[c:idx[@val='" + i.ToString(CultureInfo.InvariantCulture) + "']]", nsm);
            var doc = chart.ChartXml;

            var spPr = doc.CreateElement("c:spPr", nschart);
            var solidFill = spPr.AppendChild(doc.CreateElement("a:solidFill", nsa));
            var srgbClr = solidFill.AppendChild(doc.CreateElement("a:srgbClr", nsa));
            var valattrib = srgbClr.Attributes.Append(doc.CreateAttribute("val"));
            valattrib.Value = color.ToHex().Substring(1);

            var ln = spPr.AppendChild(doc.CreateElement("a:ln", nsa));
            var lnSolidFill = ln.AppendChild(doc.CreateElement("a:solidFill", nsa));
            var lnSrgbClr = lnSolidFill.AppendChild(doc.CreateElement("a:srgbClr", nsa));
            var lnValattrib = lnSrgbClr.Attributes.Append(doc.CreateAttribute("val"));
            lnValattrib.Value = Color.Black.ToHex().Substring(1);

            node.AppendChild(spPr);
        }

        public static String ToHex(this Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }
    }
}
