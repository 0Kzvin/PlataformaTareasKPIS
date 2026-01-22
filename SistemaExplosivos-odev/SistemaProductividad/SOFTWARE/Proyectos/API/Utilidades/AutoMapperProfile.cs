using AutoMapper;
using API.Database.Core.Entidades;
using API.Database.Core.DTOs.Departamentos;
using API.Database.Core.DTOs.Tareas;
using API.Database.Administracion.Entidades.Identidad;

namespace API.Utilidades
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Departamentos, DepartamentoDTO>()
                .ForMember(d => d.LiderNombre, o => o.MapFrom(s => s.Lider != null ? s.Lider.NombreCompleto : "Sin Líder"))
                .ForMember(d => d.NumeroMiembros, o => o.MapFrom(s => s.Miembros != null ? s.Miembros.Count : 0));

            CreateMap<RegistrarDepartamentoDTO, Departamentos>();

            CreateMap<Tareas, TareaDTO>()
                .ForMember(d => d.AsignadoNombre, o => o.MapFrom(s => s.Asignado != null ? s.Asignado.NombreCompleto : "Sin Asignar"))
                .ForMember(d => d.CreadorNombre, o => o.MapFrom(s => s.Creador != null ? s.Creador.NombreCompleto : "Desconocido"));

            CreateMap<RegistrarTareaDTO, Tareas>();
        }
    }
}
