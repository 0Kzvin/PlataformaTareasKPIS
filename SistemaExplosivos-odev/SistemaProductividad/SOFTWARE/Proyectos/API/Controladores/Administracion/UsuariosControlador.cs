using API.Database.Administracion.Entidades.Identidad;
using API.Database.Core;
using API.Database.Core.DTOs.Administracion;
using API.Utilidades.Constantes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controladores.Administracion
{
    [ApiExplorerSettings(GroupName = ConstantesModulos.ADMINISTRACION)]
    [Route("administracion/Usuarios")]
    [ApiController]
    public class UsuariosControlador : Controller
    {
        private readonly SistemaProductividadContext _context;
        private readonly UserManager<Usuarios> _userManager;

        public UsuariosControlador(SistemaProductividadContext context, UserManager<Usuarios> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("Listar")]
        public async Task<ActionResult<IEnumerable<UsuarioResumenDTO>>> Listar()
        {
            var usuarios = await _context.Users
                .Include(u => u.Departamento)
                .ToListAsync();

            var resultado = usuarios.Select(u => new UsuarioResumenDTO
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Apellidos = u.Apellidos,
                Email = u.Email,
                Estado = u.Estado,
                DepartamentoPrincipal = u.Departamento != null ? u.Departamento.Nombre : string.Empty
            });

            return Ok(resultado);
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] CrearUsuarioDTO modelo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuario = new Usuarios
            {
                UserName = modelo.Email,
                Email = modelo.Email,
                Nombre = modelo.Nombre,
                Apellidos = modelo.Apellidos,
                DepartamentoId = modelo.DepartamentoId
            };

            var result = await _userManager.CreateAsync(usuario, modelo.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        [HttpPost("Editar")]
        public async Task<IActionResult> Editar([FromBody] EditarUsuarioDTO modelo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuario = await _userManager.FindByIdAsync(modelo.Id);
            if (usuario == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(modelo.Nombre))
            {
                usuario.Nombre = modelo.Nombre;
            }

            if (!string.IsNullOrWhiteSpace(modelo.Apellidos))
            {
                usuario.Apellidos = modelo.Apellidos;
            }

            if (!string.IsNullOrWhiteSpace(modelo.Email))
            {
                usuario.Email = modelo.Email;
                usuario.UserName = modelo.Email;
            }

            if (modelo.Estado.HasValue)
            {
                usuario.Estado = modelo.Estado.Value;
            }

            usuario.FechaModificacion = DateTime.UtcNow;

            await _userManager.UpdateAsync(usuario);

            return Ok();
        }

        [HttpPost("CambiarEstado")]
        public async Task<IActionResult> CambiarEstado([FromBody] CambiarEstadoUsuarioDTO modelo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuario = await _userManager.FindByIdAsync(modelo.Id);
            if (usuario == null) return NotFound();

            usuario.Estado = modelo.Estado;
            usuario.FechaModificacion = DateTime.UtcNow;

            await _userManager.UpdateAsync(usuario);

            return Ok();
        }
    }
}
