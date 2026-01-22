using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Controladores.Administracion
{
    [Route("api/administracion/Identidad")]
    [ApiController]
    public class IdentidadControlador : Controller
    {
        private readonly IConfiguration _config;
        
        public IdentidadControlador(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("IniciarSesion")]
        [AllowAnonymous]
        public IActionResult IniciarSesion([FromBody] LoginDTO login)
        {
            // Mock Login Logic
            // In real app, check DB via UserManager<Usuario>
            
            if (login.UsuarioOCorreo == "admin" && login.Password == "admin123")
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, "admin"),
                    new Claim(JwtRegisteredClaimNames.Email, "admin@test.com"),
                    new Claim(ClaimTypes.Role, "SuperAdmin"),
                    new Claim("NombreCompleto", "Administrador Principal"),
                    new Claim("Id", "1")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "SecretKeyVeryLongAndSecure123456"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                var expiracion = DateTime.Now.AddDays(1);
                
                return Ok(new
                {
                    exito = true,
                    payload = new {
                        token = tokenString,
                        actualizarToken = tokenString, // In real app, this would be a separate refresh token
                        expiracion = expiracion,
                        datosUsuario = new {
                           nombre = "Admin",
                           apellidos = "Principal",
                           nombreCompleto = "Administrador Principal",
                           username = "admin",
                           email = "admin@test.com",
                           phoneNumber = "",
                           roles = new[] { "SuperAdmin" },
                           estado = true,
                           areaSeleccionada = "Gerencia",
                           numeroTelefonico = "",
                           fechaRegistro = DateTime.Now,
                           fechaModificacion = DateTime.Now,
                           foto = (string)null,
                           configuracionesModulos = (object)null
                        },
                        esMantenimiento = false
                    }
                });
            }
            
             // Mock Leader
            if (login.UsuarioOCorreo == "lider" && login.Password == "lider123")
            {
                 var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, "lider"),
                    new Claim(ClaimTypes.Role, "Lider"),
                    new Claim("NombreCompleto", "Lider Departamento"),
                    new Claim("Id", "2")
                };
                 var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "SecretKeyVeryLongAndSecure123456"));
                 var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                 var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                     signingCredentials: creds
                );

                 return Ok(new {
                    exito = true,
                    payload = new {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        datosUsuario = new { roles = new[] { "Lider" } }
                    }
                });
            }

            return Ok(new { exito = false, payload = new { errores = new[] { "Credenciales inválidas" } } });
        }
        
        [HttpGet("ObtenerModulosOtorgados/{usuario}")]
        public IActionResult ObtenerModulos(string usuario)
        {
             return Ok(new { exito = true, payload = new object[] {} });
        }

        [HttpGet("ObtenerPermisosOtorgados/{usuario}")]
        public IActionResult ObtenerPermisos(string usuario)
        {
             return Ok(new { exito = true, payload = new object[] {} });
        }
        
        [HttpPost("MantenerSesion")]
        public IActionResult MantenerSesion([FromBody] object data)
        {
             // Simply echo back success/new token if needed
             // For now just return true
             return Ok(new { exito = true, payload = new { operacionExitosa = true, token = "refreshed_token", tokenExpirado = false } });
        }
    }
    
    public class LoginDTO {
        public string UsuarioOCorreo { get; set; }
        public string Password { get; set; }
    }
}
