using API.Database.Core;
using API.Database.Core.DTOs.Auditoria;
using API.Utilidades.Constantes;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controladores.Core
{
    [ApiExplorerSettings(GroupName = ConstantesModulos.AUDITORIA)]
    [Route("auditoria")]
    [ApiController]
    public class AuditoriaControlador : Controller
    {
        private readonly SistemaProductividadContext _context;
        private readonly IMapper _mapper;

        public AuditoriaControlador(SistemaProductividadContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("Registros/Listar")]
        public async Task<ActionResult<IEnumerable<RegistroAuditoriaDTO>>> Listar()
        {
            var registros = await _context.RegistrosAuditoria
                .Include(r => r.Usuario)
                .OrderByDescending(r => r.Fecha)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<RegistroAuditoriaDTO>>(registros));
        }
    }
}
