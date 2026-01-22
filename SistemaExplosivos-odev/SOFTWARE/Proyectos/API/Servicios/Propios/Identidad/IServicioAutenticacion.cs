using API.Database.Administracion.DTOs.Autentificacion;
using API.Database.Administracion.DTOs.Identidad;
using API.Database.Administracion.DTOs.Respuestas;
using System;
using System.Threading.Tasks;

namespace API.Servicios.Propios.Identidad
{
    public interface IServicioAutenticacion
    {
        Task<AutentificacionRespuesta> RegistrarAsync(RegistroUsuarioDTO registroUsuarioDTO);
        Task<AutentificacionRespuesta> IniciarSesionAsync(string usuarioOCorreo, string password);
        Task<AutentificacionRespuesta> ActualizarToken(string token, string actualizarToken, DateTime expiracion);
        Task<RespuestaGenerica> CambiarPassword(CambiarPasswordDTO model);
        Task<RespuestaGenerica> SolicitarRecuperacionDeCuenta(string usuarioOcorreo);
        Task<RespuestaGenerica> VerificarCodigoRecuperacion(string codigo);
        Task<RespuestaGenerica> RecuperarCuenta(CambiarPasswordRecuperadoDTO cambiarPasswordRecuperadoDTO);
        Task<RespuestaGenerica> CambiarNombre(CambiarNombreDTO model);
        Task<RespuestaGenerica> CambiarCorreo(CambiarCorreoDTO model);
        Task<AutentificacionInformacion> ObtenerInformacionUsuarioAsync(string usuarioOCorreo);
    }
}