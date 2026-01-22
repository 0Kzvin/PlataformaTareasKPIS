using API.Database.Core;
using API.Database.Core.DTOs.Tareas;
using API.Database.Core.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controladores.Core
{
    [Route("api/core/Tareas")]
    [ApiController]
    // [Authorize]
    public class TareasControlador : Controller
    {
        private readonly SistemaProductividadContext _context;
        private readonly IMapper _mapper;
        // private readonly IAuthorizationService _authService; // To implement later

        public TareasControlador(SistemaProductividadContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("ListarPorDepartamento/{dptoId}")]
        public async Task<ActionResult<IEnumerable<TareaDTO>>> ListarPorDepartamento(int dptoId)
        {
            // TODO: Validate user access to department
            var tareas = await _context.Set<Tareas>()
                .Include(t => t.Asignado)
                .Include(t => t.Creador)
                .Where(t => t.DepartamentoId == dptoId)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<TareaDTO>>(tareas);
            
            // Filter Private Fields if not Leader (Mock logic for now, assumes 'Collaborator' role by default logic)
            // In real impl, check User Roles/Claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Check if user is Leader of dptoId or Admin
            // For now, return all (MVP)
            
            return Ok(dtos);
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult> Registrar([FromBody] RegistrarTareaDTO modelo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tarea = _mapper.Map<Tareas>(modelo);
            tarea.CreadorId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Current User
            
            _context.Add(tarea);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
