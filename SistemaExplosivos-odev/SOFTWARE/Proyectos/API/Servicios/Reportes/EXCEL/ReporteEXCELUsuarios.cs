using API.Database.Administracion.DTOs.Identidad;
using API.Utilidades.Constantes;
using API.Utilidades.Generales;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace API.Servicios.Reportes.EXCEL
{
    public class ReporteEXCELUsuarios
    {
        string titulo = "Reporte de Usuarios y Roles G+Fuel";
        public byte[] Generar(ReporteUsuariosDTO model, string cliente, string titulo = "", string subtitulo = "")
        {
            using (var libro = new ExcelPackage())
            {
                if (!String.IsNullOrWhiteSpace(titulo)) this.titulo = titulo;
                libro.Workbook.Properties.Title = this.titulo;
                libro.Workbook.Properties.Author = "G+FUEL";
                libro.Workbook.Properties.Created = DateTime.Now;
                libro.Workbook.Properties.Subject = "Reporte Generado a traves del Sistema G+Fuel";

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
                int totalFilas = model.Usuarios.Count;
                int filaDatosTotales = filaEncabezadosTabla + 1;

                var propiedadesInfo = typeof(UsuariosReporteoDTO).GetProperties();
                var nombresPropiedades = new List<string>();
                string letraFinal = UtilsGenerales.ObtenerNombreColumnaExcel(propiedadesInfo.Length);

                foreach (var propiedad in propiedadesInfo)
                {
                    var atributo = propiedad.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                        .Cast<DisplayNameAttribute>().SingleOrDefault();
                    string displayName = atributo?.DisplayName ?? propiedad.Name;
                    nombresPropiedades.Add(displayName);
                }

                List<int> columnasList = new List<int>();

                using (ExcelRange rangeCells = worksheet.Cells[$"A{filaEncabezadosTabla}:{letraFinal}{totalFilas + filaEncabezadosTabla}"])
                {
                    ExcelTable tabla = worksheet.Tables.Add(rangeCells, "GENERICTABLE");

                    for (int i = 0; i < nombresPropiedades.Count; i++)
                    {
                        tabla.Columns[i].Name = nombresPropiedades[i];
                        if (propiedadesInfo[i].PropertyType == typeof(DateTime))
                        {
                            string letraColumna = UtilsGenerales.ObtenerNombreColumnaExcel(i + 1);
                            string dateCellFormat = "dd-MM-yy HH:mm:ss AM/PM";
                            using (ExcelRange rangoFecha = worksheet.Cells[$"{letraColumna}{filaEncabezadosTabla}:{letraColumna}{(totalFilas + filaDatosTotales)}"])
                            {
                                rangoFecha.Style.Numberformat.Format = dateCellFormat;
                            }
                        }
                        if (propiedadesInfo[i].PropertyType == typeof(decimal) || propiedadesInfo[i].PropertyType == typeof(double) || propiedadesInfo[i].PropertyType == typeof(float))
                        {
                            tabla.Columns[i].TotalsRowFunction = RowFunctions.Sum;
                            string letraColumna = UtilsGenerales.ObtenerNombreColumnaExcel(i + 1);
                            string dateCellFormat = "###,###,##0.00";
                            using (ExcelRange rangoCantidadFacturada = worksheet.Cells[$"{letraColumna}{filaEncabezadosTabla}:{letraColumna}{totalFilas + filaDatosTotales}"])
                            {
                                rangoCantidadFacturada.Style.Numberformat.Format = dateCellFormat;
                            }
                        }

                        if (propiedadesInfo[i].PropertyType == typeof(List<string>))
                        {
                            columnasList.Add(i);
                        }

                    }
                    tabla.ShowTotal = false;
                    tabla.ShowHeader = true;
                    tabla.ShowFilter = true;
                    tabla.TableStyle = TableStyles.Medium25;
                }

                worksheet.Cells[$"A{filaDatosTotales}"].LoadFromCollection(model.Usuarios, false);

                worksheet.Cells[$"A{filaDatosTabla}:{letraFinal}{totalFilas + filaDatosTotales}"].Style.Font.Size = 10;
                worksheet.Cells[$"A{filaDatosTabla}:{letraFinal}{totalFilas + filaEncabezadosTabla}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));

                //Construyendo modulos habilitados
                //foreach (var columnaList in columnasList)
                //{
                //    var numUsuarios = model.Usuarios.Count+6;

                //    for (int i = 6; i < numUsuarios; i++)
                //    {
                //        worksheet.Cells[$"D{i}"].Value = String.Join(", ", model.Usuarios[i].ModulosHabilitados);
                //    }
                //}

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

                //FILAS TODAS
                worksheet.Cells[$"A{filaDatosTabla}:{letraFinal}{(totalFilas + filaDatosTabla)}"].AutoFitColumns(20);
                worksheet.Cells[$"A{filaEncabezadosTabla}:{letraFinal}{(totalFilas + filaDatosTotales)}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{filaEncabezadosTabla}:{letraFinal}{(totalFilas + filaDatosTotales)}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                #endregion

                GenerarTablaRolesPermisos(worksheet, filaDatosTabla + totalFilas + 2, model);

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

        public void GenerarTablaRolesPermisos(ExcelWorksheet worksheet, int numFilaComenzar, ReporteUsuariosDTO model)
        {
            string letraFinal = UtilsGenerales.ObtenerNombreColumnaExcel(model.Roles.Count);

            worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Merge = true;
            worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Value = "Perfiles";
            worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Style.Font.Size = 14;
            //worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Style.WrapText = true;

            model.Roles = model.Roles
                .OrderBy(x => x.NombreRol)
                .ToList();

            foreach (var rol in model.Roles)
            {

                numFilaComenzar++;

                worksheet.Cells[$"A{numFilaComenzar}"].Value = "Nombre: ";
                worksheet.Cells[$"A{numFilaComenzar}"].Style.Font.Bold = true;
                worksheet.Cells[$"A{numFilaComenzar}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[$"B{numFilaComenzar}:D{numFilaComenzar}"].Merge = true;
                worksheet.Cells[$"B{numFilaComenzar}:D{numFilaComenzar}"].Value =
                    String.IsNullOrWhiteSpace(rol.NombreRol) ? "-" : rol.NombreRol;
                worksheet.Cells[$"B{numFilaComenzar}:D{numFilaComenzar}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Style.Font.Size = 14;
                worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(215, 40, 47));
                worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Style.WrapText = true;

                numFilaComenzar++;

                worksheet.Cells[$"A{numFilaComenzar}"].Value = "Descripción: ";
                worksheet.Cells[$"A{numFilaComenzar}"].Style.Font.Bold = true;
                worksheet.Cells[$"A{numFilaComenzar}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[$"B{numFilaComenzar}:D{numFilaComenzar}"].Merge = true;
                worksheet.Cells[$"B{numFilaComenzar}:D{numFilaComenzar}"].Style.WrapText = true;
                worksheet.Cells[$"B{numFilaComenzar}:D{numFilaComenzar}"].Value =
                    String.IsNullOrWhiteSpace(rol.Descripcion) ? "-" : rol.Descripcion;
                worksheet.Cells[$"B{numFilaComenzar}:D{numFilaComenzar}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Style.Font.Size = 14;
                worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(215, 40, 47));
                worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Style.WrapText = true;

                numFilaComenzar++;

                worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Merge = true;
                worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Value = "Permisos";
                worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Style.Font.Bold = true;
                worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Style.Font.Size = 14;
                worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(215, 40, 47));
                worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Style.WrapText = true;

                rol.Permisos = rol.Permisos
                    .OrderBy(x => x.NombreNormalizadoModulo)
                    .ToList();

                numFilaComenzar++;

                var permisosAgrupados = rol.Permisos
                    .GroupBy(x => x.NombreNormalizadoModulo)
                    .ToList();

                for (int i = 0; i < permisosAgrupados.Count; i++)
                {
                    var permisos = permisosAgrupados[i].ToList();
                    var primeraFila = numFilaComenzar;
                    var numTituloModulo = numFilaComenzar + 4;
                    worksheet.Cells[$"A{numFilaComenzar}:D{numTituloModulo}"].Merge = true;
                    worksheet.Cells[$"A{numFilaComenzar}"].Value = permisosAgrupados[i].Key;
                    worksheet.Cells[$"A{numFilaComenzar}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"A{numFilaComenzar}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[$"A{numFilaComenzar}"].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    worksheet.Cells[$"A{numFilaComenzar}"].Style.Font.Size = 14;
                    worksheet.Cells[$"A{numFilaComenzar}"].Style.Font.Bold = true;

                    numFilaComenzar +=4+1;

                    permisos = permisos
                        .OrderBy(x => x.GrupoNombreNormalizado)
                        .ToList();

                    //Reacomoda los roles y usuarios hasta el inicio por ser los mas importantes
                    if (rol.Permisos.Any(x => x.IdModulo == ConstantesModulos.ID_ADMINISTRACION))
                    {
                        var permisosRolesEncontrados = permisos
                        .Where(x => x.NombreNormalizado.Contains("Roles") || x.NombreNormalizado.Contains("Rol"))
                        .ToList();

                        permisos.RemoveAll(x => x.NombreNormalizado.Contains("Roles") || x.NombreNormalizado.Contains("Rol"));
                        permisos.InsertRange(0, permisosRolesEncontrados);

                        var permisosUsuariosEncontrados = permisos
                            .Where(x => x.NombreNormalizado.Contains("Usuarios") || x.NombreNormalizado.Contains("Usuario"))
                            .ToList();

                        permisos.RemoveAll(x => x.NombreNormalizado.Contains("Usuarios") || x.NombreNormalizado.Contains("Usuario"));
                        permisos.InsertRange(0, permisosUsuariosEncontrados);
                    }

                    for (int k = 0; k < permisos.Count; k++)
                    {
                        worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Merge = true;
                        worksheet.Cells[$"A{numFilaComenzar}"].Value = permisos[k].NombreNormalizado;
                        worksheet.Cells[$"A{numFilaComenzar}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[$"A{numFilaComenzar}"].Style.WrapText = true;
                        worksheet.Cells[$"A{numFilaComenzar}:D{numFilaComenzar}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        if (k != permisos.Count -1 || permisos.Count == 1)
                        {
                            numFilaComenzar++;
                        }
                    }

                    worksheet.Cells[$"A{primeraFila}:D{numFilaComenzar}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                }

                numFilaComenzar += 2;
            }
        }
    }
}
