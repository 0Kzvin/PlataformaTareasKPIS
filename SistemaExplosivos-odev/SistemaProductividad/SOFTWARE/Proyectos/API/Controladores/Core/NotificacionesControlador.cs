using API.Database.Core;
using API.Database.Core.DTOs.Notificaciones;
using API.Utilidades.Constantes;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controladores.Core
{
    [ApiExplorerSettings(GroupName = ConstantesModulos.NOTIFICACIONES)]
    [Route("notificaciones")]
    [ApiController]
    public class NotificacionesControlador : Controller
    {
        private readonly SistemaProductividadContext _context;
        private readonly IMapper _mapper;

        public NotificacionesControlador(SistemaProductividadContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("Listar")]
        public async Task<ActionResult<IEnumerable<NotificacionDTO>>> Listar()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var notificaciones = await _context.Notificaciones
                .Where(n => n.UsuarioId == usuarioId)
                .OrderByDescending(n => n.Fecha)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<NotificacionDTO>>(notificaciones));
        }

        [HttpPost("MarcarLeida")]
        public async Task<IActionResult> MarcarLeida([FromBody] MarcarNotificacionDTO modelo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var notificacion = await _context.Notificaciones.FindAsync(modelo.Id);
            if (notificacion == null) return NotFound();

            notificacion.Leido = modelo.Leido;
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
