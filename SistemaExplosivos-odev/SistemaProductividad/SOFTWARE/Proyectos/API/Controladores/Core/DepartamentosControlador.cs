using API.Database.Core;
using API.Database.Core.DTOs.Departamentos;
using API.Database.Core.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controladores.Core
{
    [Route("api/core/Departamentos")]
    [ApiController]
    // [Authorize] // Temporarily disabled for testing
    public class DepartamentosControlador : Controller
    {
        private readonly SistemaProductividadContext _context;
        private readonly IMapper _mapper;

        public DepartamentosControlador(SistemaProductividadContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("Listar")]
        public async Task<ActionResult<IEnumerable<DepartamentoDTO>>> Listar()
        {
            var deptos = await _context.Set<Departamentos>()
                .Include(d => d.Lider)
                .Include(d => d.Miembros)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<DepartamentoDTO>>(deptos));
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult> Registrar([FromBody] RegistrarDepartamentoDTO modelo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var depto = _mapper.Map<Departamentos>(modelo);
            _context.Add(depto);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
