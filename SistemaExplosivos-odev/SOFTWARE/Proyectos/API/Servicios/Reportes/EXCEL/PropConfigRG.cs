using API.Utilidades.Constantes;

namespace API.Servicios.Reportes.EXCEL
{
    public class PropConfigRG
    {
        /// <summary>
        /// Es la propiedad a la cual se le aplicaran las configuraciones.
        /// <br /><br />
        /// Debe de ser el nombre tal cual de la propiedad en string
        /// </summary>
        public string LlavePropiedad { get; set; }

        /// <summary>
        /// Si se encuentra diferente a nulo, hace override al titulo interpuesto en el reporte y lo reemplaza
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Cualquier string despues del titulo de la columna
        /// <br /><br />
        /// Por defecto inyecta un espacio entre el titulo y el suffix.
        /// </summary>
        public string TituloSuffix { get; set; }

        /// <summary>
        /// Cualquier string antes del titulo de la columna.
        /// <br /><br />
        /// Por defecto inyecta un espacio entre el titulo y el prefix.
        /// </summary>
        public string TituloPrefix { get; set; }

        /// <summary>
        /// El formato el cual se intentara ejecutar al momento de generar el reporte.
        /// <br /><br />
        /// Si es una string vacia, se ignora como si estuviera nulo.
        /// </summary>
        public string FormateoCelda { get; set; }

        /// <summary>
        /// Si se encuentra diferente a 0, se realiza un override de la posicion de atributos
        /// </summary>
        public int Posicion { get; set; } = 0;

        /// <summary>
        /// Por defecto se encuentra en [false]
        /// </summary>
        public bool Ocultar { get; set; } = false;

        /// <summary>
        /// Si se encuentra diferente a nulo, hace override al valor del reporte y lo reemplaza
        /// </summary>
        public string Valor { get; set; }
        /// <summary>
        /// Define el tipo de propiedad para intentar forzar un formateo o tratamiento especifico
        /// </summary>
        public TipoPropiedadRGEnum TipoPropiedad { get; set; }
        public bool RemoverEspacioTituloSuffix { get; set; }
        public bool RemoverEspacioTituloPrefix { get; set; }
        /// <summary>
        /// Automaticamente se formatea DateTime con [dd-MM-yy HH:mm:ss AM/PM] y numeros (decimal, double, float) con [###,###,##0.00########] .
        /// <br /><br />
        /// En True desactiva completamente el formateo.
        /// <br /><br />
        /// Si tiene formateo de celda, esta variable es ignorada.
        /// </summary>
        public bool DesactivarFormateoAutomatico { get; set; }
        /// <summary>
        /// En variables [decimal, double, float] automaticamente se hace una suma de la columna en la fila de totales.
        /// <br /><br />
        /// En True desactiva que salga esa suma automaticamente con el reporte
        /// </summary>
        public bool DesactivarSumaTotal { get; set; }

        /// <summary>
        /// Busca el tipo de calculo para el reporte en sus totales
        /// </summary>
        public TipoTotalizacion TipoTotalizacion { get; set; }
    }
}
