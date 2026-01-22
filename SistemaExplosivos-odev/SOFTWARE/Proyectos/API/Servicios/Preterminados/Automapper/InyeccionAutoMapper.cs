using API.Servicios.Preterminados.Automapper.Mapeos;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Servicios.Preterminados.Automapper
{
    public class InyeccionAutoMapper : IInyeccionServicios
    {
        public void InstalarServicios(IServiceCollection servicios, IConfiguration configuracion)
        {
            // AUTOMAPPER NUEVO AUTOMAPPER V16 - Forma nueva
            servicios.AddAutoMapper(cfg =>
            {
                // Convertidores
                cfg.CreateMap<bool?, bool>().ConvertUsing<NullBooleanConverter>();
                cfg.CreateMap<bool?, bool?>().ConvertUsing<NullFirstBooleanConverter>();
                cfg.CreateMap<string, string>().ConvertUsing<NullStringConverter>();
                cfg.CreateMap<decimal?, decimal>().ConvertUsing<NullDecimalConverter>();
                cfg.CreateMap<decimal?, decimal?>().ConvertUsing<NullFirstDecimalConverter>();

                // Perfiles
                cfg.AddProfile<AutoMapperAdministracion>();
                cfg.AddProfile<AutoMapperAlmacenamiento>();
                cfg.AddProfile<AutoMapperRecepcion>();
                cfg.AddProfile<AutoMapperAccesorios>();
                cfg.AddProfile<AutoMapperGerencia>();

            }, typeof(InyeccionAutoMapper).Assembly);

            // AUTOMAPPER ANTIGUO V14 - Forma antigua
            //servicios.AddSingleton(provider =>
            //    new MapperConfiguration(config =>
            //    {
            //        // Configuración de AutoMapper
            //        config.CreateMap<bool?, bool>().ConvertUsing<NullBooleanConverter>();
            //        config.CreateMap<bool?, bool?>().ConvertUsing<NullFirstBooleanConverter>();
            //        config.CreateMap<string, string>().ConvertUsing<NullStringConverter>();
            //        config.CreateMap<decimal?, decimal>().ConvertUsing<NullDecimalConverter>();
            //        config.CreateMap<decimal?, decimal?>().ConvertUsing<NullFirstDecimalConverter>();

            //        config.AddProfile(new AutoMapperAdministracion());

            //        config.AddProfile(new AutoMapperAlmacenamiento());

            //        config.AddProfile(new AutoMapperRecepcion());

            //        config.AddProfile(new AutoMapperAccesorios());

            //        config.AddProfile(new AutoMapperGerencia());
            //    }).CreateMapper()
            //);
        }
    }
}