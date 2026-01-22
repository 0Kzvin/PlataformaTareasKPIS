using System;

namespace API.Database.Administracion.DTOs.DLogs
{
    public class SincronizarLogDTO
    {
        /// <summary>
        /// Identificador único del registro de log.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador del usuario que generó el log.
        /// </summary>
        public string IdUnico { get; set; }

        /// <summary>
        /// Fecha y hora en que se generó el log.
        /// </summary>
        public DateTime FechaHora { get; set; }

        /// <summary>
        /// Mensaje del log.
        /// </summary>
        public string Mensaje { get; set; }

        /// <summary>
        /// Nivel de severidad del log (por ejemplo, "Error", "Warning", "Info").
        /// </summary>
        public string NivelLog { get; set; }

        /// <summary>
        /// Metodo componente del sistema que generó el log.
        /// </summary>
        public string Metodo { get; set; }

        /// <summary>
        /// DescripcionCliente del log
        /// </summary>
        public string DescripcionCliente { get; set; }

        /// <summary>
        /// Equipo que genero el log
        /// </summary>
        public string Origen { get; set; }

        /// <summary>
        /// Nombre del cliente al que pertenece el log.
        /// </summary>
        public string Cliente { get; set; }
    }
}
