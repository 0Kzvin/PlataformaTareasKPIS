using System;
using System.Collections.Generic;

namespace API.Modelos
{
    /// <summary>
    /// Clase genérica para representar una respuesta de operación en la aplicación.
    /// Esta clase se utiliza para encapsular el resultado de una operación, incluyendo el estado de éxito,
    /// mensajes de error, datos de respuesta, fecha y código de estado HTTP.
    /// </summary>
    /// <typeparam name="T">Tipo genérico que representa el tipo de datos de la respuesta.</typeparam>
    public class RespuestaGenericaDTO<T>
    {
        /// <summary>
        /// Indica si la operación fue exitosa.
        /// Por defecto, se establece en `true`.
        /// </summary>
        public bool OperacionExitosa { get; set; } = true;

        /// <summary>
        /// MensajeOperacion descriptivo sobre el resultado de la operación.
        /// Por defecto, es una cadena vacía.
        /// </summary>
        public string Mensaje { get; set; } = string.Empty;

        /// <summary>
        /// Lista de errores que ocurrieron durante la operación.
        /// Por defecto, se inicializa como una lista vacía.
        /// </summary>
        public List<string> Errores { get; set; } = new List<string>();

        /// <summary>
        /// Datos de respuesta generados por la operación.
        /// Puede ser `null` si no hay datos para devolver.
        /// Por defecto, se establece en el valor predeterminado del tipo `T`.
        /// </summary>
        public T? Respuesta { get; set; } = default;

        /// <summary>
        /// Fecha y hora en que se generó la respuesta.
        /// Por defecto, se establece en la fecha y hora actuales.
        /// </summary>
        public DateTime Fecha { get; set; } = DateTime.Now;

        /// <summary>
        /// Método estático para crear una respuesta exitosa.
        /// </summary>
        /// <param name="respuesta">Datos de la respuesta.</param>
        /// <param name="mensaje">MensajeOperacion descriptivo de la operación.</param>
        /// <returns>Una instancia de <see cref="RespuestaGenericaDTO{T}"/> con la operación marcada como exitosa.</returns>
        public static RespuestaGenericaDTO<T> CrearRespuestaExitosa(T respuesta, string mensaje = "Operación exitosa")
        {
            return new RespuestaGenericaDTO<T>
            {
                OperacionExitosa = true,
                Respuesta = respuesta,
                Mensaje = mensaje
            };
        }

        /// <summary>
        /// Método estático para crear una respuesta con errores.
        /// </summary>
        /// <param name="errores">Lista de errores que ocurrieron durante la operación.</param>
        /// <param name="mensaje">MensajeOperacion descriptivo del error.</param>
        /// <returns>Una instancia de <see cref="RespuestaGenericaDTO{T}"/> con la operación marcada como fallida.</returns>
        public static RespuestaGenericaDTO<T> CrearRespuestaConErrores(string mensaje = "Operación fallida", params string[] errores)
        {
            var listErrores = new List<string>();

            if (errores != null)
            {
                foreach (string error in errores)
                {
                    listErrores.Add(error);
                }
            }

            return new RespuestaGenericaDTO<T>
            {
                OperacionExitosa = false,
                Errores = listErrores,
                Mensaje = mensaje
            };
        }
    }
}
