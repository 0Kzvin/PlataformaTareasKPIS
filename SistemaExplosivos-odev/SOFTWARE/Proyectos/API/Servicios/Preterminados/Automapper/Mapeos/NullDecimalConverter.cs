using AutoMapper;

namespace API.Servicios.Preterminados.Automapper.Mapeos
{
    public class NullDecimalConverter : ITypeConverter<decimal?, decimal>
    {
        public decimal Convert(decimal? source, decimal destination, ResolutionContext context)
        {
            return source ?? 0;
        }
    }

    public class NullFirstDecimalConverter : ITypeConverter<decimal?, decimal?>
    {
        public decimal? Convert(decimal? source, decimal? destination, ResolutionContext context)
        {
            return source ?? 0;
        }
    }
}
