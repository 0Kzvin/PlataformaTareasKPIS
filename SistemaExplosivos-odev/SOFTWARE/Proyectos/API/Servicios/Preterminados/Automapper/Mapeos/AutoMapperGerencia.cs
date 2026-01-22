using API.Database.Almacenamiento.DTOs.Depositos;
using API.Database.Gerencia.DTOs.Estaciones;
using API.Database.Gerencia.DTOs.Operadores;
using API.Database.Gerencia.DTOs.Productos;
using API.Database.Gerencia.Entidades;
using AutoMapper;
using System;

namespace API.Servicios.Preterminados.Automapper.Mapeos
{
    public class AutoMapperGerencia : Profile
    {
        public AutoMapperGerencia()
        {
            //OPERADORES
            CreateMap<RegistrarOperadoresGerencia, OperadoresGerencia>()
                .ForMember(x => x.Estado,
                    opt => opt.MapFrom(y => true))
                .ForMember(x => x.Borrado,
                    opt => opt.MapFrom(y => false));

            CreateMap<ModificarOperadoresGerencia, OperadoresGerencia>()
                .ForMember(x => x.FechaModificacion,
                    opt => opt.MapFrom(y => DateTime.Now));

            CreateMap<ModificarOperadoresGerencia, RegistrarOperadoresGerencia>();

            //ESTACIONES 
            CreateMap<RegistrarEstacionesGerencia, EstacionesGerencia>()
                .ForMember(x => x.FechaRegistro,
                    opt => opt.MapFrom(y => DateTime.Now))
                .ForMember(x => x.FechaModificacion,
                    opt => opt.MapFrom(y => DateTime.Now))
                .ForMember(x => x.Estado,
                    opt => opt.MapFrom(y => true))
                .ForMember(x => x.Borrado,
                    opt => opt.MapFrom(y => false));

            CreateMap<ModificarEstacionesGerencia, EstacionesGerencia>()
                .ForMember(x => x.FechaModificacion,
                    opt => opt.MapFrom(y => DateTime.Now));

            CreateMap<ModificarEstacionesGerencia, RegistrarEstacionesGerencia>();

            //PRODUCTOS
            CreateMap<ProductosGerencia, ProductoDepositoAlmacenamientoDTO>();

            CreateMap<RegistrarProductosGerencia, ProductosGerencia>()
                .ForMember(x => x.FechaRegistro,
                    opt => opt.MapFrom(y => DateTime.Now))
                .ForMember(x => x.FechaModificacion,
                    opt => opt.MapFrom(y => DateTime.Now))
                .ForMember(x => x.Estado,
                    opt => opt.MapFrom(y => true))
                .ForMember(x => x.Borrado,
                    opt => opt.MapFrom(y => false));

            CreateMap<ModificarProductosGerencia, ProductosGerencia>()
                .ForMember(x => x.FechaModificacion,
                    opt => opt.MapFrom(y => DateTime.Now));

            CreateMap<ModificarProductosGerencia, RegistrarProductosGerencia>();
        }
    }
}
