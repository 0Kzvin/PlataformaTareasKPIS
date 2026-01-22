using API.Database.Administracion.DTOs.Identidad;
using API.Database.Administracion.DTOs.Modulos;
using API.Database.Administracion.DTOs.Respuestas;
using API.Database.Administracion.Entidades.Identidad;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Servicios.Propios.Identidad
{
    public interface IServicioAutorizacion
    {
        Task<RespuestaGenerica> AsignarRol(EditarRolUsuarioDTO model);
        Task<RespuestaGenerica> CrearRol(CrearRolDTO model);
        Task<RespuestaGenerica> EditarRol(EditarRol model, bool esSuperUsuario);
        Task<RespuestaGenerica> BorrarRol(string idRol);
        Task<List<PermisoOtorgadoDTO>> ObtenerPermisosOtorgados(string usuarioOCorreo);
        Task<List<ModuloDTO>> ObtenerModulosOtorgados(string usuarioOCorreo);
        Task<List<UsuariosRoles>> ObtenerRolesPorUsuario(Usuarios usuario);
    }
}