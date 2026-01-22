using AutoMapper;

namespace API.Servicios.Preterminados.Automapper.Mapeos
{
    public class NullBooleanConverter : ITypeConverter<bool?, bool>
    {
        public bool Convert(bool? source, bool destination, ResolutionContext context)
        {
            return source ?? false;
        }
    }

    public class NullFirstBooleanConverter : ITypeConverter<bool?, bool?>
    {
        public bool? Convert(bool? source, bool? destination, ResolutionContext context)
        {
            return source ?? false;
        }
    }
}
