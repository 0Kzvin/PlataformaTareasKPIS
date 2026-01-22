using API.Database.Core;
using API.Database.Core.DTOs.Departamentos;
using API.Database.Core.Entidades;
using API.Database.Core.Enums;
using API.Utilidades.Constantes;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controladores.Administracion
{
    [ApiExplorerSettings(GroupName = ConstantesModulos.ADMINISTRACION)]
    [Route("administracion/Departamentos")]
    [ApiController]
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
            var departamentos = await _context.Departamentos
                .Include(d => d.Lider)
                .Include(d => d.Miembros)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<DepartamentoDTO>>(departamentos));
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] RegistrarDepartamentoDTO modelo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var departamento = _mapper.Map<Departamentos>(modelo);
            departamento.Configuracion = new ConfiguracionDepartamento
            {
                ModoAsignacion = ModoAsignacionEnum.A,
                PermiteAsignarOtros = true,
                PermiteCamposPrivados = true
            };

            _context.Departamentos.Add(departamento);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Editar")]
        public async Task<IActionResult> Editar([FromBody] EditarDepartamentoDTO modelo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var departamento = await _context.Departamentos
                .Include(d => d.Configuracion)
                .FirstOrDefaultAsync(d => d.Id == modelo.Id);

            if (departamento == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(modelo.Nombre))
            {
                departamento.Nombre = modelo.Nombre;
            }

            if (!string.IsNullOrWhiteSpace(modelo.Descripcion))
            {
                departamento.Descripcion = modelo.Descripcion;
            }

            if (!string.IsNullOrWhiteSpace(modelo.LiderId))
            {
                departamento.LiderId = modelo.LiderId;
            }

            if (modelo.Activo.HasValue)
            {
                departamento.Activo = modelo.Activo.Value;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
