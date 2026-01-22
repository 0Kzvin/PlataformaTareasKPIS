using API.Database.Almacenamiento.DTOs.Cargas;
using API.Database.Almacenamiento.DTOs.Depositos;
using API.Database.Almacenamiento.DTOs.Equipos;
using API.Database.Almacenamiento.DTOs.Sincronizacion;
using API.Database.Almacenamiento.DTOs.Supersacos;
using API.Database.Almacenamiento.Entidades;
using AutoMapper;
using System;

namespace API.Servicios.Preterminados.Automapper.Mapeos
{
    public class AutoMapperAlmacenamiento : Profile
    {
        public AutoMapperAlmacenamiento()
        {
            //DEPÓSITOS
            CreateMap<RegistrarDepositoAlmacenamiento, DepositosAlmacenamiento>()
                .ForMember(x => x.FechaRegistro,
                    opt => opt.MapFrom(y => DateTime.Now))
                .ForMember(x => x.FechaModificacion,
                    opt => opt.MapFrom(y => DateTime.Now))
                .ForMember(x => x.Estado,
                    opt => opt.MapFrom(y => true))
                .ForMember(x => x.Borrado,
                    opt => opt.MapFrom(y => false));

            CreateMap<ModificarDepositoAlmacenamiento, DepositosAlmacenamiento>()
                .ForMember(x => x.FechaModificacion,
                    opt => opt.MapFrom(y => DateTime.Now));

            CreateMap<ModificarDepositoAlmacenamiento, RegistrarDepositoAlmacenamiento>();

            //EQUIPOS
            CreateMap<RegistrarEquiposAlmacenamiento, EquiposAlmacenamiento>()
                .ForMember(x => x.FechaRegistro,
                    opt => opt.MapFrom(y => DateTime.Now))
                .ForMember(x => x.FechaModificacion,
                    opt => opt.MapFrom(y => DateTime.Now))
                .ForMember(x => x.Estado,
                    opt => opt.MapFrom(y => true))
                .ForMember(x => x.Borrado,
                    opt => opt.MapFrom(y => false));

            CreateMap<ModificarEquiposAlmacenamiento, EquiposAlmacenamiento>()
                .ForMember(x => x.FechaModificacion,
                    opt => opt.MapFrom(y => DateTime.Now));

            CreateMap<ModificarEquiposAlmacenamiento, RegistrarEquiposAlmacenamiento>();

            //CARGAS DEPOSITOS
            CreateMap<CargasDepositosAlmacenamiento, CargasDepositoAlmacenamientoDTO>();

            //SUPERSACOS
            CreateMap<MovimientosSupersacosAlmacenamiento, MovimientosSupersacosAlmacenamientoDTO>();
            CreateMap<RegistrarMovimientoSupersacoAlmacenamientoDTO, MovimientosSupersacosAlmacenamiento>()
                .ForMember(x => x.FechaHora,
                    opt => opt.Ignore())
                .ForMember(x => x.FechaRegistro,
                    opt => opt.MapFrom(y => DateTime.Now))
                .ForMember(x => x.FechaModificacion,
                    opt => opt.MapFrom(y => DateTime.Now))
                .ForMember(x => x.Borrado,
                    opt => opt.MapFrom(y => false));

            CreateMap<ModificarMovimientoSupersacosAlmacenamientoDTO, RegistrarMovimientoSupersacoAlmacenamientoDTO>();
            CreateMap<ModificarMovimientoSupersacosAlmacenamientoDTO, MovimientosSupersacosAlmacenamiento>()
                .ForMember(x => x.FechaModificacion,
                    opt => opt.MapFrom(y => DateTime.Now));

            //SINCRONIZACION
            CreateMap<CargasDepositosAlmacenamiento, CargasDepositosAlmacenamiento>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<SincronizacionEstatusAlmacenamientoDTO, EstatusDepositosAlmacenamiento>();
            CreateMap<SincronizacionCargasDepositosAlmacenamientoDTO, CargasDepositosAlmacenamiento>();
        }
    }
}
