namespace API.Utilidades.Constantes
{
    public class ConstsGenerales
    {
        public const string LLAVE_CLIENTE_ACTUAL = "Actual";
        /// <summary>
        /// Static cambiable al momento de comenzar la API
        /// </summary>
        public static int HoraCorte { private set; get; }

        //Método para cambiar el valor de HoraCorte
        //De este modo, ya no puede ser modificada accidentalmente
        //ej. ConstGenerales.HoraCorte = ....
        //Ahora se tiene que deliberadamente usar este método. Solo
        //debería tener una referencia en todo el proyecto.
        public static void CambiarHoraCorte(int value)
        {
            HoraCorte = value;
        }
    }
}
