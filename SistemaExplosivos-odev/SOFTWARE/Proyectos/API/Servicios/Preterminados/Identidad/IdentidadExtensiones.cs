using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace API.Servicios.Preterminados.Identidad
{
    public static class IdentidadExtensiones
    {
        public static string ObtenerIDUsuario(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }

            return httpContext.User.Claims.Single(x => x.Type == "Id").Value;
        }

        public static string ObtenerNombreCompletoUsuario(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }

            return httpContext.User.Claims.Single(x => x.Type == "NombreCompleto").Value;
        }

        public static string ObtenerUsernameUsuario(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }

            if (!httpContext.User.Claims.Any())
            {
                return string.Empty;
            }

            return httpContext.User.Claims.Single(x => x.Type == ClaimTypes.Name).Value ?? string.Empty;
        }

        public static string ObtenerCorreoUsuario(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }

            return httpContext.User.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Email).Value;
        }

        public static string ObtenerRolUsuario(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }

            if (!httpContext.User.Claims.Any())
            {
                return string.Empty;
            }

            return httpContext.User.Claims.Single(x => x.Type == ClaimTypes.Role)?.Value ?? string.Empty;
        }
    }
}
