using AutoMapper;
using API.Database.Core.Entidades;
using API.Database.Core.DTOs.Departamentos;
using API.Database.Core.DTOs.Tareas;
using API.Database.Core.DTOs.Dashboard;
using API.Database.Core.DTOs.Kpis;
using API.Database.Core.DTOs.Notificaciones;
using API.Database.Core.DTOs.Auditoria;

namespace API.Modelos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Departamentos
            CreateMap<Departamentos, DepartamentoDTO>()
                .ForMember(d => d.LiderNombre, o => o.MapFrom(s => s.Lider != null ? s.Lider.Nombre : "Sin Asignar"))
                .ForMember(d => d.NumeroMiembros, o => o.MapFrom(s => s.Miembros != null ? s.Miembros.Count : 0))
                .ForMember(d => d.Activo, o => o.MapFrom(s => s.Activo));

            CreateMap<Departamentos, DepartamentoDetalleDTO>()
                .ForMember(d => d.LiderNombre, o => o.MapFrom(s => s.Lider != null ? s.Lider.NombreCompleto : "Sin Asignar"));

            CreateMap<ConfiguracionDepartamento, ConfiguracionDepartamentoDTO>();
                
            CreateMap<RegistrarDepartamentoDTO, Departamentos>()
                .ForMember(d => d.Lider, o => o.Ignore())
                .ForMember(d => d.Miembros, o => o.Ignore())
                .ForMember(d => d.Usuarios, o => o.Ignore())
                .ForMember(d => d.Tareas, o => o.Ignore())
                .ForMember(d => d.Kpis, o => o.Ignore());

            // Tareas
            CreateMap<Tareas, TareaDTO>()
                .ForMember(d => d.ResponsablePrincipalNombre, o => o.MapFrom(s => s.ResponsablePrincipal != null ? s.ResponsablePrincipal.Nombre : "Sin Asignar"))
                .ForMember(d => d.CreadorNombre, o => o.MapFrom(s => s.Creador != null ? s.Creador.NombreCompleto : string.Empty))
                .ForMember(d => d.DificultadEstimada, o => o.MapFrom(s => s.CamposPrivados != null ? s.CamposPrivados.DificultadEstimada : null))
                .ForMember(d => d.TiempoEstimado, o => o.MapFrom(s => s.CamposPrivados != null ? s.CamposPrivados.TiempoEstimado : null))
                .ForMember(d => d.TiempoReal, o => o.MapFrom(s => s.CamposPrivados != null ? s.CamposPrivados.TiempoReal : null))
                .ForMember(d => d.EvaluacionDesempeno, o => o.MapFrom(s => s.CamposPrivados != null ? s.CamposPrivados.EvaluacionDesempeno : null))
                .ForMember(d => d.NotasPrivadas, o => o.MapFrom(s => s.CamposPrivados != null ? s.CamposPrivados.NotasPrivadas : null))
                .ForMember(d => d.ImpactoInterno, o => o.MapFrom(s => s.CamposPrivados != null ? s.CamposPrivados.ImpactoInterno : null))
                .ForMember(d => d.ClasificacionInterna, o => o.MapFrom(s => s.CamposPrivados != null ? s.CamposPrivados.ClasificacionInterna : null));

            CreateMap<RegistrarTareaDTO, Tareas>()
                .ForMember(d => d.Departamento, o => o.Ignore())
                .ForMember(d => d.ResponsablePrincipal, o => o.Ignore())
                .ForMember(d => d.Creador, o => o.Ignore())
                .ForMember(d => d.Comentarios, o => o.Ignore())
                .ForMember(d => d.Asignados, o => o.Ignore())
                .ForMember(d => d.Historial, o => o.Ignore())
                .ForMember(d => d.Evidencias, o => o.Ignore())
                .ForMember(d => d.CamposPrivados, o => o.Ignore());

            // KPIs
            CreateMap<KpiDepartamento, KpiDepartamentoDTO>();

            // Notificaciones
            CreateMap<Notificacion, NotificacionDTO>();

            // Auditoria
            CreateMap<RegistroAuditoria, RegistroAuditoriaDTO>()
                .ForMember(d => d.UsuarioNombre, o => o.MapFrom(s => s.Usuario != null ? s.Usuario.NombreCompleto : string.Empty));
        }
    }
}
