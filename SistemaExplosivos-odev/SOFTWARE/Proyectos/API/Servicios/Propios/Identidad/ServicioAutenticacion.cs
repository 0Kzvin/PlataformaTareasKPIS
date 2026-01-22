using API.Database.Administracion;
using API.Database.Administracion.DTOs.Autentificacion;
using API.Database.Administracion.DTOs.Identidad;
using API.Database.Administracion.DTOs.Respuestas;
using API.Database.Administracion.Entidades.Identidad;
using API.Properties;
using API.Servicios.Archivos;
using API.Servicios.Preterminados.Autenticacion;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServiciosAPIG3.ServiciosAPI.Correo;
using ServiciosAPIG3.ServiciosAPI.Correo.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToolkitG3Core.Utilidades;

namespace API.Servicios.Propios.Identidad
{
    public class ServicioAutenticacion : IServicioAutenticacion
    {
        private readonly UserManager<Usuarios> _userManager;
        private readonly IMapper _mapper;
        private readonly JwtConfiguraciones _jwtConfiguraciones;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly ModuloAdministracionExplosivosContext _context;
        private readonly IAlmacenadorArchivos _almacenador;
        private readonly IServicioCorreo _servicioCorreo;
        private const string CONTENEDOR_CARPETA = "Usuarios";
        private readonly MailkitDTO mailkitDTO;

        public ServicioAutenticacion(
            UserManager<Usuarios> userManager,
            IMapper mapper,
            JwtConfiguraciones jwtConfiguraciones,
            TokenValidationParameters tokenValidationParameters,
            ModuloAdministracionExplosivosContext context,
            IAlmacenadorArchivos almacenador,
            IOptions<MailkitDTO> mailkitDTO,
            IServicioCorreo servicioCorreo)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtConfiguraciones = jwtConfiguraciones;
            _tokenValidationParameters = tokenValidationParameters;
            _context = context;
            _almacenador = almacenador;
            _servicioCorreo = servicioCorreo;
            this.mailkitDTO = mailkitDTO.Value;
        }

        public async Task<AutentificacionRespuesta> IniciarSesionAsync(string usuarioOCorreo, string password)
        {
            var usuario = await _userManager.FindByNameAsync(usuarioOCorreo) ??
                         await _userManager.FindByEmailAsync(usuarioOCorreo);

            if (usuario == null)
            {
                return new AutentificacionRespuesta { Errores = new[] { "Usuario o Contraseña Incorrecta" } };
            }

            if (!await _userManager.CheckPasswordAsync(usuario, password))
            {
                return new AutentificacionRespuesta { Errores = new[] { "Usuario o Contraseña Incorrecta" } };
            }

            if (!usuario.Estado)
            {
                return new AutentificacionRespuesta { Errores = new[] { "Usuario desactivado" } };
            }

            return await GenerarTokenAsync(usuario);
        }

        public async Task<AutentificacionInformacion> ObtenerInformacionUsuarioAsync(string usuarioOCorreo)
        {
            var usuario = await _userManager.FindByNameAsync(usuarioOCorreo) ??
                         await _userManager.FindByEmailAsync(usuarioOCorreo);

            if (usuario == null)
            {
                return new AutentificacionInformacion { Errores = new[] { "Usuario no encontrado" } };
            }

            return new AutentificacionInformacion
            {
                Estado = usuario.Estado,
                NumeroTelefonico = usuario.PhoneNumber,
                FechaRegistro = usuario.FechaRegistro,
                FechaModificacion = usuario.FechaModificacion,
                Nombre = usuario.Nombre,
                Apellidos = usuario.Apellidos,
                OperacionExitosa = true,
                Foto = usuario.Foto,
                ConfiguracionesModulos = new ConfiguracionesModulos
                {
                    ConfiguracionesVL = new ConfiguracionesVL { ProductoEncargado = "TEST" }
                }
            };
        }

        public async Task<AutentificacionRespuesta> RegistrarAsync(RegistroUsuarioDTO registroUsuarioDTO)
        {
            var existeUsuario = await _userManager.FindByNameAsync(registroUsuarioDTO.Username);
            if (existeUsuario != null)
            {
                return new AutentificacionRespuesta { Errores = new[] { "El Usuario Ya Existe" } };
            }

            if (!string.IsNullOrWhiteSpace(registroUsuarioDTO.Email))
            {
                var existeCorreo = await _userManager.FindByEmailAsync(registroUsuarioDTO.Email);
                if (existeCorreo != null)
                {
                    return new AutentificacionRespuesta { Errores = new[] { "El Correo Ya Existe" } };
                }
            }

            var nuevoUsuario = _mapper.Map<Usuarios>(registroUsuarioDTO);

            if (registroUsuarioDTO.Foto != null)
            {
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await registroUsuarioDTO.Foto.CopyToAsync(memoryStream);
                        var contenido = memoryStream.ToArray();
                        var extension = Path.GetExtension(registroUsuarioDTO.Foto.FileName);
                        nuevoUsuario.Foto = await _almacenador.GuardarArchivo(contenido, extension, CONTENEDOR_CARPETA, registroUsuarioDTO.Foto.ContentType);
                    }
                }
                catch { /* Manejar error si es necesario */ }
            }

            var crearUsuario = await _userManager.CreateAsync(nuevoUsuario, registroUsuarioDTO.Password);
            if (!crearUsuario.Succeeded)
            {
                await _almacenador.BorrarArchivo(nuevoUsuario.Foto, CONTENEDOR_CARPETA);
                return new AutentificacionRespuesta { Errores = crearUsuario.Errors.Select(x => x.Description) };
            }

            if (!String.IsNullOrWhiteSpace(registroUsuarioDTO.RolId))
            {
                var usuariosRoles = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == nuevoUsuario.Id);
                if (usuariosRoles != null)
                {
                    _context.UserRoles.Remove(usuariosRoles);
                }
                _context.UserRoles.Add(new UsuariosRoles()
                {
                    UserId = nuevoUsuario.Id,
                    RoleId = registroUsuarioDTO.RolId,
                });
            }

            return await GenerarTokenAsync(nuevoUsuario);
        }

        public async Task<AutentificacionRespuesta> ActualizarToken(string token, string actualizarToken, DateTime expiracion)
        {
            var tokenValidado = ObtenerClaimsPrincipalesDeToken(token);

            if (tokenValidado == null)
            {
                return new AutentificacionRespuesta { Errores = new[] { "Token Invalido" } };
            }

            var fechaExpiracionUnix = long.Parse(tokenValidado.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var fechaExpiracionDateTimeUTC = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(fechaExpiracionUnix);

            if (fechaExpiracionDateTimeUTC > DateTime.UtcNow)
            {
                return new AutentificacionRespuesta
                {
                    TokenExpirado = false,
                    Errores = new[] { "Este Token aun no ha expirado" }
                };
            }

            var jti = tokenValidado.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            var almacenActualizarToken = await _context.ActualizarTokens.SingleOrDefaultAsync(x => x.Token == actualizarToken);

            if (almacenActualizarToken == null)
            {
                return new AutentificacionRespuesta
                {
                    Errores = new[] { "Este Actualizar Token no existe" },
                    TokenExpirado = true,
                };
            }

            if (DateTime.UtcNow > almacenActualizarToken.FechaExpiracion)
            {
                return new AutentificacionRespuesta
                {
                    Errores = new[] { "Este Actualizar Token expiro" },
                    TokenExpirado = true,
                };
            }

            if (almacenActualizarToken.Invalidado)
            {
                return new AutentificacionRespuesta
                {
                    Errores = new[] { "Este Actualizar Token fue Invalidado" },
                    TokenExpirado = true,
                };
            }

            if (almacenActualizarToken.Usado)
            {
                return new AutentificacionRespuesta
                {
                    Errores = new[] { "Este Actualizar Token fue Usado" },
                    TokenExpirado = true,
                };
            }

            if (almacenActualizarToken.JwtId != jti)
            {
                return new AutentificacionRespuesta
                {
                    Errores = new[] { "Este Actualizar Token no coincide" },
                    TokenExpirado = true,
                };
            }

            almacenActualizarToken.Usado = true;
            _context.ActualizarTokens.Update(almacenActualizarToken);
            await _context.SaveChangesAsync();

            var usuario = await _userManager.FindByIdAsync(tokenValidado.Claims.Single(x => x.Type == "Id").Value);
            if (usuario == null)
            {
                return new AutentificacionRespuesta
                {
                    Errores = new[] { "Acceso Denegado por el sistema" },
                    TokenExpirado = true,
                };
            }

            return await GenerarTokenAsync(usuario);
        }

        public async Task<RespuestaGenerica> CambiarPassword(CambiarPasswordDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return new RespuestaGenerica
                {
                    Mensaje = "Ocurrió un error",
                    Errores = new List<string> { "El Usuario no existe" },
                    OperacionExitosa = false,
                };
            }

            var passwordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, passwordToken, model.PasswordNuevo);

            if (!result.Succeeded)
            {
                return new RespuestaGenerica
                {
                    Mensaje = "Ocurrió un error al momento de cambiar la contraseña",
                    Errores = result.Errors.Select(x => x.Description).ToList(),
                    OperacionExitosa = false,
                };
            }

            return new RespuestaGenerica
            {
                Mensaje = "Contraseña cambiada con éxito",
                OperacionExitosa = true,
            };
        }

        public async Task<RespuestaGenerica> SolicitarRecuperacionDeCuenta(string usuarioOCorreo)
        {
            var user = await _userManager.FindByEmailAsync(usuarioOCorreo) ??
                       await _userManager.FindByNameAsync(usuarioOCorreo);

            if (user == null)
            {
                return new RespuestaGenerica
                {
                    Mensaje = "Ocurrió un error",
                    Errores = new List<string> { "El Usuario o Correo no existe" },
                    OperacionExitosa = false,
                };
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                return new RespuestaGenerica
                {
                    Mensaje = "Ocurrió un error",
                    Errores = new List<string> { "El Usuario no tiene correo electronico para recuperar su cuenta" },
                    OperacionExitosa = false,
                };
            }

            string codigoDeRecuperacion = GenerarCodigoRecuperacion();
            string logoStringBase64 = string.Empty;

            try
            {
                var imagenMMS = ImageUtils.RedimensionarBitmap(Resources.gfuelLogoFull, 350, 150);
                logoStringBase64 = ImageUtils.BitmapToStringBase64(imagenMMS);
            }
            catch { }

            string mensajeHTML = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            text-align: center;
            background-color: #f2f2f2;
            margin: 0;
            padding: 0;
        }}

        #container {{
            max-width: 500px;
            margin: 0 auto;
            padding: 150px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }}

        #container header {{
            text-align: center;
        }}

        #container header img {{ 
            max-width: 100px !important;
            min-width: 50px !important;
            width: 100px !important;
            height: auto !important;
        }}

        #message {{
            margin-top: 20px;
            text-align: left;
            padding: 0 20px;
        }}

        #message p {{
            font-size: 16px;
            text-align: center;
        }}

        #code {{
            margin-top: 20px;
            background-color: #3498db;
            color: white;
            font-size: 24px;
            padding: 10px;
            text-align: center;
            border-radius: 5px;
        }}

        #divider {{
            border-top: 1px solid #ccc;
            margin: 20px 0;
        }}

        #footer {{
            text-align: justify;
            color: black;
            font-size: 12px;
            padding: 0 20px;
        }}
    </style>
</head>
<body>
    <div id='container'>
        <div id='header'>
            <img src='data:image/png;base64,{logoStringBase64}' alt='Logo' />
        </div>
        <div id='message'>
            <p>Estimado usuario,</p>
            <p>Ha solicitado un correo de recuperaci&oacute;n. Por favor, mantenga esta informaci&oacute;n confidencial y no la comparta con nadie.</p>
            <p>Si usted no solicitó este código simplemente ignore el mensaje.</p>
        </div>
        <div id='code'>
            <strong>Código de recuperación:</strong>
            <br>
            <span style='font-size: 32px; font-weight: bold;'>{codigoDeRecuperacion.Trim()}</span>
        </div>
        <div id='divider'></div>
        <div id='footer'>
            <p>Este mensaje es autom&aacute;tico y no debe ser respondido bajo ninguna circunstancia. El contenido es confidencial y para uso exclusivo del destinatario. Si usted no es el destinatario, se le pide guardar discreción y se prohíbe difundir el contenido públicamente.</p>
        </div>
    </div>
</body>
</html>";
            bool seEnvio = await _servicioCorreo.EnviarCorreoAsincronoConArchivoAdjunto(
                    mailkitDTO,
                    new List<DestinatarioCorreoDTO>()
                    {
                        new DestinatarioCorreoDTO()
                        {
                            Correo = user.Email,
                            Nombre = user.NombreCompleto
                        }
                    },
 "Correo de Recuperación de Contraseña",
                    "",
                    mensajeHTML,
                    reenviarTambienAG3: false
                );

            if (!seEnvio)
            {
                return new RespuestaGenerica
                {
                    Mensaje = "Ocurrió un error",
                    Errores = new List<string> { "Hubo un error al enviar el correo para reestablecer la contraseña" },
                    OperacionExitosa = false,
                };
            }

            // Cancelar todos los codigos anteriores
            var codigosAnteriores = await _context.UsuariosRecuperacion
                .Where(x => !x.CodigoUsado && x.IdUsuario == user.Id)
                .ToListAsync();

            if (codigosAnteriores.Count > 0)
            {
                codigosAnteriores.ForEach(x => x.CodigoUsado = true);
                _context.UpdateRange(codigosAnteriores);
            }

            var registroRecuperacion = new UsuariosRecuperacion()
            {
                CodigoRecuperacion = codigoDeRecuperacion,
                CodigoUsado = false,
                FechaDeCreacion = DateTime.Now,
                FechaDeExpiracion = DateTime.Now.AddHours(2),
                IdUsuario = user.Id,
                MetodoRecuperacion = "CorreoElectronico"
            };

            await _context.UsuariosRecuperacion.AddAsync(registroRecuperacion);
            await _context.SaveChangesAsync();

            return new RespuestaGenerica
            {
                Mensaje = "Se ha enviado un correo para la recuperacion",
                OperacionExitosa = true,
            };
        }

        public async Task<RespuestaGenerica> VerificarCodigoRecuperacion(string codigo)
        {
            var verificarCodigo = await _context.UsuariosRecuperacion.FirstOrDefaultAsync(x => x.CodigoRecuperacion == codigo);

            if (verificarCodigo == null)
            {
                return new RespuestaGenerica
                {
                    Mensaje = "Codigo Inexistente",
                    OperacionExitosa = false,
                };
            }

            if (verificarCodigo.CodigoUsado)
            {
                return new RespuestaGenerica
                {
                    Mensaje = "Error",
                    Errores = new List<string> { "Codigo Usado" },
                    OperacionExitosa = false,
                };
            }

            if (DateTime.Now >= verificarCodigo.FechaDeExpiracion)
            {
                return new RespuestaGenerica
                {
                    Mensaje = "Error",
                    Errores = new List<string> { "Codigo Expirado" },
                    OperacionExitosa = false,
                };
            }

            verificarCodigo.CodigoUsado = true;
            verificarCodigo.FechaDeUtilizacion = DateTime.Now;

            _context.Entry(verificarCodigo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return new RespuestaGenerica
            {
                Mensaje = "Codigo Verificado",
                OperacionExitosa = true,
            };
        }

        public async Task<RespuestaGenerica> RecuperarCuenta(CambiarPasswordRecuperadoDTO cambiarPasswordRecuperadoDTO)
        {
            var codigoUsado = await _context.UsuariosRecuperacion.FirstOrDefaultAsync(x =>
                x.CodigoRecuperacion == cambiarPasswordRecuperadoDTO.Codigo && x.CodigoUsado);

            if (codigoUsado == null)
            {
                return new RespuestaGenerica
                {
                    Mensaje = "Ocurrió un error",
                    Errores = new List<string> { "No se encontró el codigo, no se puede proceder" },
                    OperacionExitosa = false,
                };
            }

            if (codigoUsado.FechaDeExpiracion <= DateTime.Now ||
                codigoUsado.FechaDeUtilizacion.Value.AddHours(1) <= DateTime.Now)
            {
                return new RespuestaGenerica
                {
                    Mensaje = "Ocurrió un error",
                    Errores = new List<string> { "Este código ha expirado, favor de solicitar otro" },
                    OperacionExitosa = false,
                };
            }

            if (cambiarPasswordRecuperadoDTO.Password != cambiarPasswordRecuperadoDTO.ConfirmarPassword)
            {
                return new RespuestaGenerica
                {
                    Mensaje = "Ocurrió un error",
                    Errores = new List<string> { "Contraseñas diferentes" },
                    OperacionExitosa = false,
                };
            }

            var usuario = await _userManager.FindByIdAsync(codigoUsado.IdUsuario);
            var codigo = await _userManager.GeneratePasswordResetTokenAsync(usuario);
            await _userManager.ResetPasswordAsync(usuario, codigo, cambiarPasswordRecuperadoDTO.Password);

            return new RespuestaGenerica
            {
                Mensaje = "Se ha reestablecido la contraseña correctamente",
                OperacionExitosa = true,
            };
        }

        public async Task<RespuestaGenerica> CambiarNombre(CambiarNombreDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return new RespuestaGenerica
                {
                    Mensaje = "Ocurrió un error",
                    Errores = new List<string> { "El Usuario no existe" },
                    OperacionExitosa = false,
                };
            }

            var userRecord = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.Id);
            userRecord.Nombre = model.Nombre;
            userRecord.Apellidos = model.Apellidos;
            userRecord.NombreCompleto = $"{model.Nombre} {model.Apellidos}";

            _context.Entry(userRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return new RespuestaGenerica
                {
                    Mensaje = "Nombre Actualizado con éxito",
                    OperacionExitosa = true,
                };
            }
            catch
            {
                return new RespuestaGenerica
                {
                    Mensaje = "Ocurrió un error",
                    Errores = new List<string> { "Error al modificar el nombre del usuario" },
                    OperacionExitosa = false,
                };
            }
        }

        public async Task<RespuestaGenerica> CambiarCorreo(CambiarCorreoDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return new RespuestaGenerica
                {
                    Mensaje = "Ocurrió un error",
                    Errores = new List<string> { "El Usuario no existe" },
                    OperacionExitosa = false,
                };
            }

            string changeToken = string.Empty;
            IdentityResult result;

            try
            {
                changeToken = await _userManager.GenerateChangeEmailTokenAsync(user, model.NuevoCorreo);
                result = await _userManager.ChangeEmailAsync(user, model.NuevoCorreo, changeToken);
            }
            catch
            {
                return new RespuestaGenerica
                {
                    Mensaje = "Ocurrió un problema inesperado al momento de cambiar el correo",
                    Errores = new List<string> { "Excepción encontrada al cambiar correo" },
                    OperacionExitosa = false,
                };
            }

            if (!result.Succeeded)
            {
                return new RespuestaGenerica
                {
                    Mensaje = "Ocurrió un error al momento de cambiar el correo",
                    Errores = result.Errors.Select(x => x.Description).ToList(),
                    OperacionExitosa = false,
                };
            }

            return new RespuestaGenerica
            {
                Mensaje = "Correo cambiado con éxito",
                OperacionExitosa = true,
            };
        }

        #region Métodos Privados

        private ClaimsPrincipal ObtenerClaimsPrincipalesDeToken(string token)
        {
            var jwtManejador = new JwtSecurityTokenHandler();

            try
            {
                var principal = jwtManejador.ValidateToken(token, _tokenValidationParameters, out var tokenValidado);
                if (!TuJwtCuentaConUnValidoAlgoritmoDeSeguridad(tokenValidado))
                {
                    return null;
                }
                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool TuJwtCuentaConUnValidoAlgoritmoDeSeguridad(SecurityToken tokenValidado)
        {
            return (tokenValidado is JwtSecurityToken jwtSecurityToken)
                && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
        }

        private async Task<AutentificacionRespuesta> GenerarTokenAsync(Usuarios usuario, bool customOperacionExtiosa = true)
        {
            if (!usuario.Estado)
            {
                return new AutentificacionRespuesta { Errores = new[] { "Usuario Desactivado" } };
            }

            var jwtManejador = new JwtSecurityTokenHandler();
            var llave = Encoding.ASCII.GetBytes(_jwtConfiguraciones.Key);
            var expiracion = DateTime.UtcNow.Add(_jwtConfiguraciones.VidaToken);
            var credenciales = new SigningCredentials(new SymmetricSecurityKey(llave), SecurityAlgorithms.HmacSha512Signature);

            var roles = await _userManager.GetRolesAsync(usuario);
            var esMantenimiento = roles.Contains("MantenimientoG3");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, usuario.UserName),
                    new Claim(JwtRegisteredClaimNames.Sub, usuario.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.CreateVersion7().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                    new Claim("Id", usuario.Id),
                    new Claim("NombreCompleto", usuario.NombreCompleto),
                    new Claim(ClaimTypes.Role, roles.Count != 0 ? roles[0] : "Desactivado"),
                }),
                Expires = expiracion,
                SigningCredentials = credenciales,
                Audience = _jwtConfiguraciones.Audience,
                Issuer = _jwtConfiguraciones.Issuer,
            };

            var token = jwtManejador.CreateToken(tokenDescriptor);

            var actualizarToken = new ActualizarToken
            {
                UsuarioId = usuario.Id,
                FechaCreacion = DateTime.UtcNow,
                JwtId = token.Id,
                FechaExpiracion = DateTime.UtcNow.AddMonths(6)
            };

            await _context.ActualizarTokens.AddAsync(actualizarToken);
            await _context.SaveChangesAsync();

            return new AutentificacionRespuesta
            {
                OperacionExitosa = customOperacionExtiosa,
                TokenExpirado = false,
                Token = jwtManejador.WriteToken(token),
                ActualizarToken = actualizarToken.Token,
                Expiracion = expiracion,
                EsMantenimiento = esMantenimiento,
            };
        }

        private string GenerarCodigoRecuperacion()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // Genera un número aleatorio de 6 dígitos
        }

        #endregion
    }
}