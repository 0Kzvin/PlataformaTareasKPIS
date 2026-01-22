using AutoMapper;
using API.Database.Core.Entidades;
using API.Database.Core.DTOs.Departamentos;
using API.Database.Core.DTOs.Tareas;
using API.Database.Core.DTOs.Dashboard;

namespace API.Modelos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Departamentos
            CreateMap<Departamentos, DepartamentoDTO>()
                .ForMember(d => d.LiderNombre, o => o.MapFrom(s => s.Lider != null ? s.Lider.Nombre : "Sin Asignar"))
                .ForMember(d => d.NumeroMiembros, o => o.MapFrom(s => s.Miembros != null ? s.Miembros.Count : 0));
                
            CreateMap<RegistrarDepartamentoDTO, Departamentos>()
                .ForMember(d => d.Lider, o => o.Ignore())
                .ForMember(d => d.Miembros, o => o.Ignore());

            // Tareas
            CreateMap<Tareas, TareaDTO>()
                .ForMember(d => d.AsignadoNombre, o => o.MapFrom(s => s.Asignado != null ? s.Asignado.Nombre : "Sin Asignar"));

            CreateMap<RegistrarTareaDTO, Tareas>()
                .ForMember(d => d.Departamento, o => o.Ignore())
                .ForMember(d => d.Asignado, o => o.Ignore())
                .ForMember(d => d.Creador, o => o.Ignore())
                .ForMember(d => d.Comentarios, o => o.Ignore());
        }
    }
}
