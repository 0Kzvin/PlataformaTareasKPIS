using AutoMapper;

namespace API.Servicios.Preterminados.Automapper.Mapeos
{
    public class NullStringConverter : ITypeConverter<string, string>
    {
        public string Convert(string source, string destination, ResolutionContext context)
        {
            return source ?? string.Empty;
        }
    }
}
