using API.Database.Accesorio.DTOs.Categorias;
using API.Database.Accesorio.DTOs.Consumos;
using API.Database.Accesorio.DTOs.Consumos.Salidas;
using API.Database.Accesorio.DTOs.Destinos;
using API.Database.Accesorio.DTOs.Proveedores;
using API.Database.Accesorio.Entidades;
using AutoMapper;
using System;

namespace API.Servicios.Preterminados.Automapper.Mapeos
{
    public class AutoMapperAccesorios : Profile
    {
        public AutoMapperAccesorios()
        {
            //CONSUMOS
            CreateMap<RegistrarConsumosAccesorios, ConsumosAccesorios>()
                .ForMember(x => x.FechaRegistro,
                    opt => opt.MapFrom(y => DateTime.Now))
                .ForMember(x => x.FechaModificacion,
                    opt => opt.MapFrom(y => DateTime.Now))
                .ForMember(x => x.Borrado,
                    opt => opt.MapFrom(y => false))
                .ForMember(x => x.Salidas, 
                    opt => opt.MapFrom(src => src.Salidas));

            CreateMap<RegistrarSalidaAccesorios, SalidasAccesorios>()
                .ForMember(x => x.IdUnico,
                    opt => opt.MapFrom(y => Guid.NewGuid().ToString()));

            CreateMap<ModificarConsumosAccesorios, ConsumosAccesorios>()
                .ForMember(x => x.FechaModificacion,
                    opt => opt.MapFrom(y => DateTime.Now))
                .ForMember(x => x.Salidas,
                    opt => opt.MapFrom(src => src.Salidas));

            CreateMap<ModificarConsumosAccesorios, RegistrarConsumosAccesorios>();

            CreateMap<ModificarSalidaAccesorios, RegistrarSalidaAccesorios>();

            CreateMap<ModificarSalidaAccesorios, SalidasAccesorios>();

            //CATEGORIAS ACCESORIOS
            CreateMap<RegistrarCategoriasAccesorios, CategoriasAccesorios>()
                .ForMember(x => x.Estado,
                    opt => opt.MapFrom(y => true))
                .ForMember(x => x.Borrado,
                    opt => opt.MapFrom(y => false));

            CreateMap<ModificarCategoriasAccesorios, CategoriasAccesorios>();

            CreateMap<ModificarCategoriasAccesorios, RegistrarCategoriasAccesorios>();

            //DESTINOS ACCESORIOS
            CreateMap<RegistrarDestinosAccesorios, DestinosAccesorios>()
                .ForMember(x => x.Estado,
                    opt => opt.MapFrom(y => true))
                .ForMember(x => x.Borrado,
                    opt => opt.MapFrom(y => false));

            CreateMap<ModificarDestinosAccesorios, DestinosAccesorios>();

            CreateMap<ModificarDestinosAccesorios, RegistrarDestinosAccesorios>();

            //PROVEEDORES ACCESORIOS
            CreateMap<RegistrarProveedoresAccesorios, ProveedoresAccesorios>()
                .ForMember(x => x.Estado,
                    opt => opt.MapFrom(y => true))
                .ForMember(x => x.Borrado,
                    opt => opt.MapFrom(y => false));

            CreateMap<ModificarProveedoresAccesorios, ProveedoresAccesorios>();

            CreateMap<ModificarProveedoresAccesorios, RegistrarProveedoresAccesorios>();
        }
    }
}
