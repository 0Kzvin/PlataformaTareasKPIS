using API.Modelos.Configuraciones;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace API.Servicios.Archivos
{
    public class AlmacenadorArchivosLocal : IAlmacenadorArchivos
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly OpcionesAlmacenadorDTO opcionesAlmacenador;
        public AlmacenadorArchivosLocal(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor, OpcionesAlmacenadorDTO opcionesAlmacenador)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
            this.opcionesAlmacenador = opcionesAlmacenador;
        }

        public Task BorrarArchivo(string ruta, string contenedor)
        {
            if (ruta != null)
            {
                var nombreArchivo = Path.GetFileName(ruta);
                string directorioArchivo = Path.Combine(opcionesAlmacenador.RutaBase, contenedor, nombreArchivo);

                if (File.Exists(directorioArchivo))
                {
                    File.Delete(directorioArchivo);
                }
            }

            return Task.FromResult(0);
        }

        public Task<string> ObtenerRutaInternaArchivo(string rutaContenedor, string filename)
        {
            return Task.FromResult(opcionesAlmacenador.RutaBase + rutaContenedor + filename);
        }

        public async Task<string> EditarArchivo(byte[] contenido, string extension, string contenedor, string ruta, string contentType)
        {
            await BorrarArchivo(ruta, contenedor);
            return await GuardarArchivo(contenido, extension, contenedor, contentType);
        }

        public async Task<string> GuardarArchivo(IFormFile contenidoFile, string contenedor)
        {
            using (var memoryStream = new MemoryStream())
            {
                await contenidoFile.CopyToAsync(memoryStream);
                var contenidoArr = memoryStream.ToArray();
                var extension = Path.GetExtension(contenidoFile.FileName);
                return await GuardarArchivo(contenidoArr, extension, contenedor, contenidoFile.ContentType);
            }
        }

        public async Task<string> GuardarArchivo(byte[] contenido, string extension, string contenedor, string contentType, string guid = "")
        {
            var nombreArchivo = "";

            if (String.IsNullOrWhiteSpace(guid))
            {
                nombreArchivo = $"{Guid.CreateVersion7()}{extension}";
            }
            else
            {
                nombreArchivo = $"{guid}{extension}";
            }

            string folder = Path.Combine(opcionesAlmacenador.RutaBase, contenedor);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string ruta = Path.Combine(folder, nombreArchivo);
            await File.WriteAllBytesAsync(ruta, contenido);

            var urlActual = String.Empty;

            urlActual = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";

            // ! Solo es necesario cuando se programa en flutter en modo desarrollador
            //if (env.IsDevelopment())
            //{
            //    urlActual = $"http://10.0.2.2:5000";
            //}

            var urlParaDB = Path.Combine(urlActual, contenedor, nombreArchivo).Replace("\\", "/");

            return urlParaDB;
        }

    }
}
