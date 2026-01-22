using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace API.Servicios.Preterminados.Autorizacion.PermisosAutorizacion
{
    /// <summary>
    /// Filtro de autorización personalizado que verifica si el usuario tiene los permisos requeridos.
    /// Este filtro se utiliza en conjunto con el atributo <see cref="PermisoFiltroAttribute"/>.
    /// </summary>
    public class PermisoFiltroAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly IAuthorizationService _servicioAutorizacion;
        private readonly PermisoRequerido _permisoRequerido;

        /// <summary>
        /// Constructor que recibe las dependencias necesarias.
        /// </summary>
        /// <param name="servicioAutorizacion">Servicio de autorización para verificar los permisos.</param>
        /// <param name="permisoRequerido">Requisito de permiso que debe ser satisfecho.</param>
        public PermisoFiltroAttribute(
            IAuthorizationService servicioAutorizacion,
            PermisoRequerido permisoRequerido)
        {
            _servicioAutorizacion = servicioAutorizacion;
            _permisoRequerido = permisoRequerido;
        }

        /// <summary>
        /// Método que se ejecuta durante la autorización para verificar si el usuario tiene los permisos requeridos.
        /// </summary>
        /// <param name="context">Contexto de autorización que contiene información sobre la solicitud HTTP.</param>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // Verificar si el usuario tiene los permisos requeridos
            var resultado = await _servicioAutorizacion.AuthorizeAsync(context.HttpContext.User, null, _permisoRequerido);

            // Si el usuario no tiene los permisos, devolver un resultado de desafío (Challenge)
            if (!resultado.Succeeded)
            {
                context.Result = new ChallengeResult();
            }
        }
    }
}