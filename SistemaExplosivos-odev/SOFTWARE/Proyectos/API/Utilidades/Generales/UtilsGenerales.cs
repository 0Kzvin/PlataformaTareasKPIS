using API.Atributos;
using API.Utilidades.Constantes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace API.Utilidades.Generales
{
    public static class UtilsGenerales
    {
        // Obtiene el nombre de una columna de excel tomando en cuenta un entero, es decir, lo convierte
        public static string ObtenerNombreColumnaExcel(int numeroColumna)
        {
            string nombreColumna = "";

            while (numeroColumna > 0)
            {
                int modulo = (numeroColumna - 1) % 26;
                nombreColumna = $"{Convert.ToChar('A' + modulo)}{nombreColumna}";
                numeroColumna = (numeroColumna - modulo) / 26;
            }

            return nombreColumna;
        }

        /// <summary>
        /// Toma un array de objetos y regresa un string de HTML formateando una tabla especial para correos
        /// Solo debe de recibir las propiedades a mostrar en la tabla
        /// Se debe de utilizar la etiqueta [DisplayName] para ponerle nombre a las columnas
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="imprimirTotales"></param>
        /// <returns></returns>
        public static string ObtenerTablaHTML<T>(List<T> array, bool imprimirTotales = false)
        {
            var mensajeHTML = string.Empty;
            var propiedadesInfo = typeof(T).GetProperties();
            var nombresPropiedades = new List<string>();
            var totales = new Dictionary<string, decimal>();
            var conteoElementos = new Dictionary<string, int>();

            // Obtener los nombres de las propiedades y los tipos de totalización
            var totalizacionPropiedades = new Dictionary<string, TipoTotalizacion>();

            foreach (var propiedad in propiedadesInfo)
            {
                var atributoDisplayName = propiedad.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                    .Cast<DisplayNameAttribute>().SingleOrDefault();
                string displayName = atributoDisplayName?.DisplayName ?? propiedad.Name;
                nombresPropiedades.Add(displayName);

                // Verificar si la propiedad tiene el atributo OperacionTotalizacion
                var atributoTotalizacion = propiedad.GetCustomAttributes(typeof(OperacionTotalizacionAttribute), true)
                    .Cast<OperacionTotalizacionAttribute>().SingleOrDefault();

                if (atributoTotalizacion != null)
                {
                    totalizacionPropiedades[propiedad.Name] = atributoTotalizacion.Tipo;
                }

                // Inicializar totales y conteo solo para propiedades numéricas
                if (propiedad.PropertyType == typeof(decimal) || propiedad.PropertyType == typeof(double) || propiedad.PropertyType == typeof(float))
                {
                    totales[propiedad.Name] = 0;
                    conteoElementos[propiedad.Name] = 0;
                }
            }

            if (array.Count > 0)
            {
                mensajeHTML += @"
                        <table 
                          style=""border-collapse: collapse;
                                margin: 25px 0;
                                font-size: 0.9em;
                                font-family: sans-serif;
                                min-width: 400px;
                                width: 100%;
                              border-collapse: collapse;
                              border-radius: 10px 10px 0 0;
                              overflow: hidden;""
                        >
                        <tbody>
                        <tr
                            style=""background-color: #009879;
                                color: #ffffff;
                                text-align: left;
                         text-align: center;"">";

                // Generar los headers
                foreach (var nombre in nombresPropiedades)
                {
                    mensajeHTML += $@"<th
                            style=""padding: 4px 10px;  text-align: center;""
                            >{nombre}</th>";
                }

                mensajeHTML += @"</tr>";

                // Generar las filas de la tabla
                foreach (var itemArray in array)
                {
                    mensajeHTML += @"<tr>";
                    foreach (var prop in propiedadesInfo)
                    {
                        var valorProp = prop.GetValue(itemArray);
                        if (prop.PropertyType == typeof(DateTime))
                        {
                            mensajeHTML += $@"
                                     <td
                    style=""padding: 12px 15px;  text-align: center;""
                    ><span>{((DateTime)valorProp).ToString("dd/MM/yy hh:mm tt")}</span></td>";
                        }
                        else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(float) || prop.PropertyType == typeof(decimal))
                        {
                            var formattedNumber = Convert.ToDecimal(valorProp).ToString("N5").TrimEnd('0').TrimEnd('.');
                            mensajeHTML += $@"
                                 <td
            style=""padding: 12px 15px;  text-align: center;""
            ><span>{formattedNumber}</span></td>";

                            // Acumular el valor para el total o promedio de la columna
                            totales[prop.Name] += Convert.ToDecimal(valorProp);
                            conteoElementos[prop.Name]++;
                        }
                        else
                        {
                            mensajeHTML += $@"
                                     <td
                style=""padding: 12px 15px;  text-align: center;""
                ><span>{valorProp}</span></td>";
                        }
                    }
                    mensajeHTML += @"</tr>";
                }

                // Si imprimirTotales es verdadero, agregar la fila de totales/promedios
                if (imprimirTotales)
                {
                    mensajeHTML += @"<tr style=""background-color: #009879; color: #ffffff; text-align: center;"">";
                    foreach (var prop in propiedadesInfo)
                    {
                        if (totales.ContainsKey(prop.Name))
                        {
                            // Aplicar la lógica de totalización basada en el atributo
                            if (totalizacionPropiedades.ContainsKey(prop.Name))
                            {
                                if (totalizacionPropiedades[prop.Name] == TipoTotalizacion.Promedio)
                                {
                                    // Calcular el promedio
                                    var promedio = (totales[prop.Name] / conteoElementos[prop.Name]).ToString("N2");
                                    mensajeHTML += $@"<td style=""padding: 12px 15px; text-align: center;""><strong>{promedio}</strong></td>";
                                }
                                else if (totalizacionPropiedades[prop.Name] == TipoTotalizacion.Suma)
                                {
                                    // Mostrar la suma
                                    var suma = totales[prop.Name].ToString("N2");
                                    mensajeHTML += $@"<td style=""padding: 12px 15px; text-align: center;""><strong>{suma}</strong></td>";
                                }
                                else
                                {
                                    // Mostrar la suma
                                    var suma = totales[prop.Name].ToString("N2");
                                    mensajeHTML += $@"<td style=""padding: 12px 15px; text-align: center;""><strong>{suma}</strong></td>";
                                }
                            }
                            else
                            {
                                // Dejar la celda vacía para columnas no relevantes
                                mensajeHTML += @"<td style=""padding: 12px 15px; text-align: center;""><strong>-</strong></td>";
                            }
                        }
                        else
                        {
                            // Dejar la celda vacía para columnas no numéricas
                            mensajeHTML += @"<td style=""padding: 12px 15px; text-align: center;""><strong>-</strong></td>";
                        }
                    }
                    mensajeHTML += @"</tr>";
                }
            }

            mensajeHTML += @"
                    </tbody>
                    </table>
                    <br/>
                    <br/>
                    <br/>
                    <footer style=""text-align: justify; color: black; font-size: 12px"">Este mensaje es automático y no debe ser respondido bajo ninguna circunstancia. El contenido es confidencial y para uso exclusivo del destinatario. Si usted no es el destinatario, se le pide guardar discreción y se prohíbe difundir el contenido públicamente.</footer>";

            return mensajeHTML;
        }

    }
}
