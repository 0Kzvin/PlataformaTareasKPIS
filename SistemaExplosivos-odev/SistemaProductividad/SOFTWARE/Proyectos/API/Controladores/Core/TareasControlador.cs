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
    [Route("api/tareas")]
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

        [HttpGet("Listar")]
        public async Task<ActionResult<IEnumerable<TareaDTO>>> Listar([FromQuery] int? departamentoId)
        {
            // TODO: Validate user access to department
            var query = _context.Tareas
                .Include(t => t.ResponsablePrincipal)
                .Include(t => t.Creador)
                .Include(t => t.CamposPrivados)
                .AsQueryable();

            if (departamentoId.HasValue)
            {
                query = query.Where(t => t.DepartamentoId == departamentoId.Value);
            }

            var tareas = await query.ToListAsync();

            var dtos = _mapper.Map<IEnumerable<TareaDTO>>(tareas);
            
            // Filter Private Fields if not Leader (Mock logic for now, assumes 'Collaborator' role by default logic)
            // In real impl, check User Roles/Claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Check if user is Leader of dptoId or Admin
            // For now, return all (MVP)
            
            return Ok(dtos);
        }

        [HttpPost("Crear")]
        public async Task<ActionResult> Crear([FromBody] RegistrarTareaDTO modelo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tarea = _mapper.Map<Tareas>(modelo);
            tarea.CreadorId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Current User
            
            _context.Tareas.Add(tarea);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Editar")]
        public async Task<ActionResult> Editar([FromBody] EditarTareaDTO modelo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tarea = await _context.Tareas.FindAsync(modelo.Id);
            if (tarea == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(modelo.Titulo))
            {
                tarea.Titulo = modelo.Titulo;
            }

            if (!string.IsNullOrWhiteSpace(modelo.Descripcion))
            {
                tarea.Descripcion = modelo.Descripcion;
            }

            if (modelo.Deadline.HasValue)
            {
                tarea.Deadline = modelo.Deadline;
            }

            if (modelo.Prioridad.HasValue)
            {
                tarea.Prioridad = modelo.Prioridad.Value;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("CambiarEstado")]
        public async Task<ActionResult> CambiarEstado([FromBody] CambiarEstadoTareaDTO modelo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tarea = await _context.Tareas.FindAsync(modelo.Id);
            if (tarea == null) return NotFound();

            tarea.Estado = modelo.Estado;
            _context.TareasHistorial.Add(new TareaHistorial
            {
                TareaId = tarea.Id,
                Cambio = $"Estado cambiado a {modelo.Estado}",
                UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            });

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Asignar")]
        public async Task<ActionResult> Asignar([FromBody] AsignarTareaDTO modelo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tarea = await _context.Tareas.FindAsync(modelo.TareaId);
            if (tarea == null) return NotFound();

            var asignacion = new TareaAsignado
            {
                TareaId = modelo.TareaId,
                UsuarioId = modelo.UsuarioId,
                RolAsignacion = modelo.RolAsignacion
            };

            _context.TareasAsignados.Add(asignacion);

            if (modelo.EsResponsablePrincipal)
            {
                tarea.ResponsablePrincipalId = modelo.UsuarioId;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("AgregarComentario")]
        public async Task<ActionResult> AgregarComentario([FromBody] AgregarComentarioDTO modelo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tarea = await _context.Tareas.FindAsync(modelo.TareaId);
            if (tarea == null) return NotFound();

            var comentario = new TareaComentario
            {
                TareaId = modelo.TareaId,
                UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Comentario = modelo.Comentario
            };

            _context.TareasComentarios.Add(comentario);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("SubirEvidencia")]
        public async Task<ActionResult> SubirEvidencia([FromBody] SubirEvidenciaDTO modelo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tarea = await _context.Tareas.FindAsync(modelo.TareaId);
            if (tarea == null) return NotFound();

            var evidencia = new TareaEvidencia
            {
                TareaId = modelo.TareaId,
                RutaArchivo = modelo.RutaArchivo,
                Tipo = modelo.Tipo
            };

            _context.TareasEvidencias.Add(evidencia);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("CompletarCamposPrivados")]
        public async Task<ActionResult> CompletarCamposPrivados([FromBody] CompletarCamposPrivadosDTO modelo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tarea = await _context.Tareas
                .Include(t => t.CamposPrivados)
                .FirstOrDefaultAsync(t => t.Id == modelo.TareaId);

            if (tarea == null) return NotFound();

            if (tarea.CamposPrivados == null)
            {
                tarea.CamposPrivados = new CamposPrivadosTarea { TareaId = tarea.Id };
            }

            tarea.CamposPrivados.DificultadEstimada = modelo.DificultadEstimada;
            tarea.CamposPrivados.TiempoEstimado = modelo.TiempoEstimado;
            tarea.CamposPrivados.TiempoReal = modelo.TiempoReal;
            tarea.CamposPrivados.EvaluacionDesempeno = modelo.EvaluacionDesempeno;
            tarea.CamposPrivados.NotasPrivadas = modelo.NotasPrivadas;
            tarea.CamposPrivados.ImpactoInterno = modelo.ImpactoInterno;
            tarea.CamposPrivados.ClasificacionInterna = modelo.ClasificacionInterna;

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
