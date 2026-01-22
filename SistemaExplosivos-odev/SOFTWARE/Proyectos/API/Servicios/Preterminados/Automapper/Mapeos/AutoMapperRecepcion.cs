using API.Database.Recepcion.DTOs.Conductores;
using API.Database.Recepcion.DTOs.Equipos;
using API.Database.Recepcion.DTOs.Origenes;
using API.Database.Recepcion.DTOs.Proveedores;
using API.Database.Recepcion.DTOs.Recepciones;
using API.Database.Recepcion.DTOs.Transportistas;
using API.Database.Recepcion.Entidades;
using AutoMapper;
using System;

namespace API.Servicios.Preterminados.Automapper.Mapeos
{
    public class AutoMapperRecepcion : Profile
    {
        public AutoMapperRecepcion()
        {

            //PROVEEDORES
            CreateMap<RegistrarProveedorRecepcion, ProveedoresRecepcion>()
                .ForMember(x => x.FechaRegistro,
                    opt => opt.MapFrom(y => DateTime.Now))
                .ForMember(x => x.FechaModificacion,
                    opt => opt.MapFrom(y => DateTime.Now))
                .ForMember(x => x.Estado,
                    opt => opt.MapFrom(y => true))
                .ForMember(x => x.Borrado,
                    opt => opt.MapFrom(y => false));

            CreateMap<ModificarProveedorRecepcion, ProveedoresRecepcion>()
                .ForMember(x => x.FechaModificacion,
                    opt => opt.MapFrom(y => DateTime.Now));

            CreateMap<ModificarProveedorRecepcion, RegistrarProveedorRecepcion>();

            //TRANSPORTISTAS
            CreateMap<RegistrarTransportistaRecepcion, TransportistasRecepcion>()
                .ForMember(x => x.FechaRegistro,
                    opt => opt.MapFrom(y => DateTime.Now))
                .ForMember(x => x.FechaModificacion,
                    opt => opt.MapFrom(y => DateTime.Now))
                .ForMember(x => x.Estado,
                    opt => opt.MapFrom(y => true))
                .ForMember(x => x.Borrado,
                    opt => opt.MapFrom(y => false));

            CreateMap<ModificarTransportistaRecepcion, TransportistasRecepcion>()
                .ForMember(x => x.FechaModificacion,
                    opt => opt.MapFrom(y => DateTime.Now));

            CreateMap<ModificarTransportistaRecepcion, RegistrarTransportistaRecepcion>();

            //EQUIPOS
            CreateMap<RegistrarEquiposRecepcion, EquiposRecepcion>()
                .ForMember(x => x.FechaRegistro,
                    opt => opt.MapFrom(y => DateTime.Now))
                .ForMember(x => x.FechaModificacion,
                    opt => opt.MapFrom(y => DateTime.Now))
                .ForMember(x => x.Estado,
                    opt => opt.MapFrom(y => true))
                .ForMember(x => x.Borrado,
                    opt => opt.MapFrom(y => false));

            CreateMap<ModificarEquiposRecepcion, EquiposRecepcion>()
                .ForMember(x => x.FechaModificacion,
                    opt => opt.MapFrom(y => DateTime.Now));

            CreateMap<ModificarEquiposRecepcion, RegistrarEquiposRecepcion>();

            //ORIGENES
            CreateMap<RegistrarOrigenesRecepcion, OrigenesRecepcion>()
                .ForMember(x => x.Estado,
                    opt => opt.MapFrom(y => true))
                .ForMember(x => x.Borrado,
                    opt => opt.MapFrom(y => false));

            CreateMap<ModificarOrigenesRecepcion, OrigenesRecepcion>();

            CreateMap<ModificarOrigenesRecepcion, RegistrarOrigenesRecepcion>();

            //CONDUCTORES
            CreateMap<RegistrarConductoresRecepcion, ConductoresRecepcion>()
                .ForMember(x => x.Estado,
                    opt => opt.MapFrom(y => true))
                .ForMember(x => x.Borrado,
                    opt => opt.MapFrom(y => false));

            CreateMap<ModificarConductoresRecepcion, ConductoresRecepcion>();

            CreateMap<ModificarConductoresRecepcion, RegistrarConductoresRecepcion>();

            //RECEPCIONES
            CreateMap<RegistrarRecepciones, Recepciones>()
                .ForMember(x => x.Borrado,
                    opt => opt.MapFrom(y => false));

            CreateMap<ModificarRecepciones, Recepciones>();

            CreateMap<ModificarRecepciones, RegistrarRecepciones>();
        }
    }
}
