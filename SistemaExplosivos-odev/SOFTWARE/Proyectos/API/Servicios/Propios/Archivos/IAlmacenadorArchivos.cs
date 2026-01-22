using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace API.Servicios.Archivos
{
    public interface IAlmacenadorArchivos
    {
        Task<string> EditarArchivo(byte[] contenido, string extension, string contenedor, string ruta, string contentType);

        Task BorrarArchivo(string ruta, string contenedor);

        Task<string> GuardarArchivo(byte[] contenido, string extension, string contenedor, string contentType, string guid = "");

        Task<string> GuardarArchivo(IFormFile contenido, string contenedor);

        Task<string> ObtenerRutaInternaArchivo(string rutaContenedor, string filename);
    }
}
