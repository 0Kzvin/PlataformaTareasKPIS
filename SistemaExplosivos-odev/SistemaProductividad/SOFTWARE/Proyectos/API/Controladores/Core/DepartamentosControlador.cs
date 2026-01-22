using API.Database.Core;
using API.Database.Core.DTOs.Departamentos;
using API.Database.Core.Entidades;
using API.Utilidades.Constantes;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controladores.Core
{
    [ApiExplorerSettings(GroupName = ConstantesModulos.DEPARTAMENTOS)]
    [Route("departamentos")]
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

        [HttpGet("Detalle/Ver")]
        public async Task<ActionResult<DepartamentoDetalleDTO>> VerDetalle([FromQuery] int id)
        {
            var depto = await _context.Departamentos
                .Include(d => d.Lider)
                .Include(d => d.Configuracion)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (depto == null) return NotFound();

            return Ok(_mapper.Map<DepartamentoDetalleDTO>(depto));
        }

        [HttpPost("Configuracion/Actualizar")]
        public async Task<ActionResult> ActualizarConfiguracion([FromBody] ActualizarConfiguracionDepartamentoDTO modelo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var departamento = await _context.Departamentos
                .Include(d => d.Configuracion)
                .FirstOrDefaultAsync(d => d.Id == modelo.DepartamentoId);

            if (departamento == null) return NotFound();

            if (departamento.Configuracion == null)
            {
                departamento.Configuracion = new ConfiguracionDepartamento();
            }

            departamento.Configuracion.ModoAsignacion = modelo.ModoAsignacion;
            departamento.Configuracion.PermiteAsignarOtros = modelo.PermiteAsignarOtros;
            departamento.Configuracion.PermiteCamposPrivados = modelo.PermiteCamposPrivados;
            departamento.Configuracion.KpisActivos = modelo.KpisActivos;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Usuarios/Invitar")]
        public async Task<ActionResult> InvitarUsuario([FromBody] InvitarUsuarioDepartamentoDTO modelo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var departamento = await _context.Departamentos.FindAsync(modelo.DepartamentoId);
            if (departamento == null) return NotFound();

            var relacion = new DepartamentoUsuario
            {
                DepartamentoId = modelo.DepartamentoId,
                UsuarioId = modelo.UsuarioId,
                RolDepartamento = modelo.RolDepartamento
            };

            _context.DepartamentosUsuarios.Add(relacion);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
