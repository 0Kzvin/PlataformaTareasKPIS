using API.Atributos;
using API.Utilidades.Constantes;
using API.Utilidades.Generales;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;

namespace API.Servicios.Reportes.EXCEL
{
    public class ReporteEXCELGenerico<T>
    {
        string titulo = "Reporte Sistema G+MMS";
        public byte[] Generar(List<T> datos, string cliente, string titulo = "", string subtitulo = "", List<PropConfigRG> configArray = null)
        {
            if (configArray == null) configArray = new List<PropConfigRG>();
            using (var libro = new ExcelPackage())
            {
                if (!String.IsNullOrWhiteSpace(titulo)) this.titulo = titulo;
                libro.Workbook.Properties.Title = this.titulo;
                libro.Workbook.Properties.Author = "G+MMS";
                libro.Workbook.Properties.Created = DateTime.Now;
                libro.Workbook.Properties.Subject = "Reporte Generado a traves del Sistema G+MMS";

                var worksheet = libro.Workbook.Worksheets.Add(this.titulo);

                worksheet.Cells.Style.Font.Name = "Calibri";
                worksheet.Cells.Style.Border.Top.Style = ExcelBorderStyle.None;
                worksheet.Cells.Style.Border.Bottom.Style = ExcelBorderStyle.None;
                worksheet.Cells.Style.Border.Left.Style = ExcelBorderStyle.None;
                worksheet.Cells.Style.Border.Right.Style = ExcelBorderStyle.None;
                worksheet.Cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells.Style.Fill.BackgroundColor.SetColor(Color.White);

                #region Encabezado Reporte

                worksheet.Cells["D1:H1"].Merge = true;
                worksheet.Cells["D1"].Value = cliente;
                worksheet.Cells["D1"].Style.Font.Size = 16;
                worksheet.Cells["D1"].Style.Font.Bold = true;
                worksheet.Cells["D2:H2"].Merge = true;
                worksheet.Cells["D2"].Value = this.titulo;
                worksheet.Cells["D2"].Style.Font.Size = 14;
                worksheet.Cells["D2"].Style.Font.Bold = true;
                worksheet.Cells["D3:H3"].Merge = true;
                // MensajeOperacion justo debajo del titulo
                worksheet.Cells["D3"].Value = subtitulo;
                worksheet.Cells["D3"].Style.Font.Size = 12;
                worksheet.Cells["D3"].Style.Font.Bold = true;

                worksheet.Cells["D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["D1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells["D2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["D2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells["D3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["D3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                #endregion

                #region Tabla

                int filaEncabezadosTabla = 5;
                int filaDatosTabla = 6;
                int totalFilas = datos.Count;
                int filaDatosTotales = filaEncabezadosTabla + 1;

                var propiedadesInfo = typeof(T).GetProperties();
                var propiedadesRG = new List<TituloRG>();
                string letraFinal = UtilsGenerales.ObtenerNombreColumnaExcel(propiedadesInfo.Length);

                foreach (var propiedad in propiedadesInfo)
                {
                    var getAtributos = propiedad.GetCustomAttributes(true);
                    var atributo = getAtributos.FirstOrDefault();

                    string displayName = propiedad.Name;
                    TituloRG tituloRG = new TituloRG(displayName)
                    {
                        NombrePropiedad = propiedad.Name,
                        TipoVariable = propiedad.PropertyType,
                    };

                    if (atributo != null)
                    {

                        if (atributo is DisplayNameAttribute displayNameAttribute)
                        {
                            tituloRG = new TituloRG(displayNameAttribute.DisplayName)
                            {
                                NombrePropiedad = propiedad.Name,
                                TipoVariable = propiedad.PropertyType,
                            };

                        }
                        else if (atributo is TituloRG tituloRGAttribute)
                        {
                            tituloRG = tituloRGAttribute;
                            tituloRG.NombrePropiedad = propiedad.Name;
                            tituloRG.TipoVariable = propiedad.PropertyType;
                        }
                    }

                    propiedadesRG.Add(tituloRG);
                }

                if (configArray.Any())
                {
                    foreach (var config in configArray)
                    {
                        int indexPropiedadRG = propiedadesRG
                            .FindIndex((x) => x.NombrePropiedad == config.LlavePropiedad);

                        if (indexPropiedadRG == -1) continue;

                        if (config.Posicion != 0)
                        {
                            propiedadesRG[indexPropiedadRG].Posicion = config.Posicion;
                        }

                        propiedadesRG[indexPropiedadRG].Ocultar = config.Ocultar;
                    }
                }

                if (propiedadesRG.Exists(x => x.Posicion != 0))
                {
                    propiedadesRG = propiedadesRG
                        .OrderByDescending(x => x.Posicion > 0)
                        .ThenBy(x => x.Posicion)
                        .ToList();
                }

                propiedadesRG
                    .RemoveAll(x => x.Ocultar);

                using (ExcelRange rangeCells = worksheet.Cells[$"A{filaEncabezadosTabla}:{letraFinal}{totalFilas + filaEncabezadosTabla}"])
                {
                    ExcelTable tabla = worksheet.Tables.Add(rangeCells, "GENERICTABLE");

                    for (int i = 0; i < propiedadesRG.Count; i++)
                    {
                        tabla.Columns[i].Name = propiedadesRG[i].Nombre;

                        var customConfig = configArray
                            .Find(x => x.LlavePropiedad == propiedadesRG[i].NombrePropiedad);

                        if (customConfig != null)
                        {
                            var tituloColumna = String.IsNullOrEmpty(customConfig.Titulo) ? propiedadesRG[i].Nombre : customConfig.Titulo;

                            tabla.Columns[i].Name = $"{customConfig.TituloPrefix ?? ""}{(customConfig.RemoverEspacioTituloPrefix ? "" : " ")}{tituloColumna}{(customConfig.RemoverEspacioTituloSuffix ? "" : " ")}{customConfig.TituloSuffix ?? ""}";

                            if (!String.IsNullOrWhiteSpace(customConfig.FormateoCelda))
                            {
                                string letraColumna = UtilsGenerales.ObtenerNombreColumnaExcel(i + 1);
                                string dateCellFormat = customConfig.FormateoCelda;
                                using (ExcelRange rangoSeleccionado = worksheet.Cells[$"{letraColumna}{filaEncabezadosTabla}:{letraColumna}{(totalFilas + filaDatosTotales)}"])
                                {
                                    rangoSeleccionado.Style.Numberformat.Format = dateCellFormat;
                                }
                                continue;
                            }


                            if (!customConfig.DesactivarFormateoAutomatico)
                            {
                                if (propiedadesRG[i].TipoVariable == typeof(DateTime) || propiedadesRG[i].TipoVariable == typeof(DateTime?))
                                {
                                    string letraColumna = UtilsGenerales.ObtenerNombreColumnaExcel(i + 1);
                                    string dateCellFormat = "dd-MM-yy HH:mm:ss AM/PM";
                                    using (ExcelRange rangoFecha = worksheet.Cells[$"{letraColumna}{filaEncabezadosTabla}:{letraColumna}{(totalFilas + filaDatosTotales)}"])
                                    {
                                        rangoFecha.Style.Numberformat.Format = dateCellFormat;
                                    }
                                }

                                if (propiedadesRG[i].TipoVariable == typeof(decimal) || propiedadesRG[i].TipoVariable == typeof(double) || propiedadesRG[i].TipoVariable == typeof(float))
                                {
                                    // Verificar si el atributo personalizado especifica sumar o promediar
                                    if (customConfig.TipoTotalizacion == TipoTotalizacion.Suma)
                                    {
                                        tabla.Columns[i].TotalsRowFunction = RowFunctions.Sum;
                                    }
                                    else if (customConfig.TipoTotalizacion == TipoTotalizacion.Promedio)
                                    {
                                        tabla.Columns[i].TotalsRowFunction = RowFunctions.Average;
                                    }
                                    else if (customConfig.TipoTotalizacion == TipoTotalizacion.Minimo)
                                    {
                                        tabla.Columns[i].TotalsRowFunction = RowFunctions.Min;
                                    }
                                    else if (customConfig.TipoTotalizacion == TipoTotalizacion.Maximo)
                                    {
                                        tabla.Columns[i].TotalsRowFunction = RowFunctions.Max;
                                    }
                                    else
                                    {
                                        tabla.Columns[i].TotalsRowFunction = RowFunctions.Sum;
                                    }

                                    string letraColumna = UtilsGenerales.ObtenerNombreColumnaExcel(i + 1);
                                    string numberFormat = "###,###,##0.00########";
                                    using (ExcelRange rangoCantidadFacturada = worksheet.Cells[$"{letraColumna}{filaEncabezadosTabla}:{letraColumna}{totalFilas + filaDatosTotales}"])
                                    {
                                        rangoCantidadFacturada.Style.Numberformat.Format = numberFormat;
                                    }
                                }
                            }

                            continue;
                        }


                        if (propiedadesRG[i].TipoVariable == typeof(DateTime) || propiedadesRG[i].TipoVariable == typeof(DateTime?))
                        {
                            string letraColumna = UtilsGenerales.ObtenerNombreColumnaExcel(i + 1);
                            string dateCellFormat = "dd-MM-yy HH:mm:ss AM/PM";
                            using (ExcelRange rangoFecha = worksheet.Cells[$"{letraColumna}{filaEncabezadosTabla}:{letraColumna}{(totalFilas + filaDatosTotales)}"])
                            {
                                rangoFecha.Style.Numberformat.Format = dateCellFormat;
                            }
                        }
                        if (propiedadesRG[i].TipoVariable == typeof(decimal) || propiedadesRG[i].TipoVariable == typeof(double) || propiedadesInfo[i].PropertyType == typeof(float))
                        {
                            tabla.Columns[i].TotalsRowFunction = RowFunctions.Sum;
                            string letraColumna = UtilsGenerales.ObtenerNombreColumnaExcel(i + 1);
                            string dateCellFormat = "###,###,##0.00########";
                            using (ExcelRange rangoCantidadFacturada = worksheet.Cells[$"{letraColumna}{filaEncabezadosTabla}:{letraColumna}{totalFilas + filaDatosTotales}"])
                            {
                                rangoCantidadFacturada.Style.Numberformat.Format = dateCellFormat;
                            }
                        }
                    }

                    tabla.ShowTotal = true;

                    if (totalFilas > 0)
                    {
                        tabla.ShowHeader = true;
                        tabla.ShowFilter = true;
                    }

                    tabla.TableStyle = TableStyles.Medium25;
                }

                //Filas
                for (int i = 0; i < datos.Count; i++)
                {
                    int indexFila = i + 1;
                    int numeroFilaTablaExcel = filaEncabezadosTabla+indexFila;
                    for (int c = 0; c < propiedadesRG.Count; c++)
                    {
                        int indexColumna = c+1;
                        var propiedad = typeof(T).GetProperty(propiedadesRG[c].NombrePropiedad);
                        var valor = propiedad.GetValue(datos[i]);
                        worksheet.Cells[$"{UtilsGenerales.ObtenerNombreColumnaExcel(indexColumna)}{numeroFilaTablaExcel}"].Value = valor;
                    }
                }

                //worksheet.Cells[$"A{filaDatosTotales}"].LoadFromCollection(datos, false);

                worksheet.Cells[$"A{filaDatosTabla}:{letraFinal}{totalFilas + filaDatosTotales}"].Style.Font.Size = 10;

                if (totalFilas > 0)
                {
                    worksheet.Cells[$"A{filaDatosTabla}:{letraFinal}{totalFilas + filaEncabezadosTabla}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));
                }

                #endregion

                #region Estilos Tabla

                //COLOR A TODOS LAS FILAS Y ALTO FILAS
                int altoFila = 15;

                for (int i = 1; i <= totalFilas; i++)
                {
                    if (i % 2 != 0)
                    {
                        worksheet.Cells[$"A{i + filaEncabezadosTabla}:{letraFinal}{i + filaEncabezadosTabla}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
                    }

                    worksheet.Row(i + filaEncabezadosTabla).Height = altoFila;
                }

                //COLOR HEADERS TABLA
                worksheet.Cells[$"A{filaEncabezadosTabla}:{letraFinal}{filaEncabezadosTabla}"].Style.Font.Size = 14;
                worksheet.Cells[$"A{filaEncabezadosTabla}:{letraFinal}{filaEncabezadosTabla}"].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[$"A{filaEncabezadosTabla}:{letraFinal}{filaEncabezadosTabla}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(215, 40, 47));
                worksheet.Cells[$"A{filaEncabezadosTabla}:{letraFinal}{filaEncabezadosTabla}"].Style.WrapText = true;
                worksheet.Row(filaEncabezadosTabla).CustomHeight = false;
                worksheet.Cells[$"A{filaEncabezadosTabla}:{letraFinal}{filaEncabezadosTabla}"].AutoFitColumns();


                //FILAS TODAS
                worksheet.Cells[$"A{filaDatosTabla}:{letraFinal}{(totalFilas + filaDatosTabla)}"].AutoFitColumns(20);
                worksheet.Cells[$"A{filaEncabezadosTabla}:{letraFinal}{(totalFilas + filaDatosTotales)}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{filaEncabezadosTabla}:{letraFinal}{(totalFilas + filaDatosTotales)}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                #endregion

                #region Imagenes Encabezado

                Image imagen1 = Properties.Resources.grupoMexicoNuevo;
                Image imagen2 = Properties.Resources.gfuelLogoFull;

                var imagenExcel1 = worksheet.Drawings.AddPicture("Logo", imagen1);

                imagenExcel1.SetPosition(0, 0, 0, 0);
                imagenExcel1.SetSize(15);
                #endregion

                #region ConfigurarImpresion

                worksheet.Column(8).PageBreak = true;
                worksheet.PrinterSettings.Scale = 75;
                worksheet.PrinterSettings.Orientation = eOrientation.Landscape;


                #endregion

                return libro.GetAsByteArray();
            }
        }
    }
}
