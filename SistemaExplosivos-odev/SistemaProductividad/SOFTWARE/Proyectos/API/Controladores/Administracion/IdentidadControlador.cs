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
        public IActionResult MantenerSesion([FromBody] MantenerSesionDTO data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.Token))
            {
                return Ok(new
                {
                    exito = false,
                    payload = new
                    {
                        operacionExitosa = false,
                        tokenExpirado = true,
                        errores = new[] { "Token inválido" }
                    }
                });
            }

            return Ok(new
            {
                exito = true,
                payload = new
                {
                    operacionExitosa = true,
                    token = data.Token,
                    actualizarToken = string.IsNullOrWhiteSpace(data.ActualizarToken) ? data.Token : data.ActualizarToken,
                    expiracion = data.Expiracion ?? DateTime.Now.AddDays(1),
                    datosUsuario = data.DatosUsuario,
                    tokenExpirado = false
                }
            });
        }
    }
    
    public class LoginDTO {
        public string UsuarioOCorreo { get; set; }
        public string Password { get; set; }
    }

    public class MantenerSesionDTO
    {
        public string Token { get; set; }
        public string ActualizarToken { get; set; }
        public DateTime? Expiracion { get; set; }
        public DatosUsuarioDTO DatosUsuario { get; set; }
    }

    public class DatosUsuarioDTO
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string NombreCompleto { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string[] Roles { get; set; }
        public bool? Estado { get; set; }
        public string AreaSeleccionada { get; set; }
        public string NumeroTelefonico { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string Foto { get; set; }
        public object ConfiguracionesModulos { get; set; }
    }
}
