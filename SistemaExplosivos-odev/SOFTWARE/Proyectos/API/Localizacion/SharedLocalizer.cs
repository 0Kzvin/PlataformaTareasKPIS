using Microsoft.Extensions.Localization;
using System.Reflection;

namespace API.Localizacion
{
    public interface ISharedLocalizer
    {
        public LocalizedString this[string key]
        {
            get;
        }
    }

    public class SharedResources
    {

    }

    public class SharedLocalizer : ISharedLocalizer
    {
        private readonly IStringLocalizer _localizer;

        public SharedLocalizer(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResources);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create($"Compartido.SharedResources", assemblyName.Name);
        }

        public LocalizedString this[string key] => _localizer[key];

        public LocalizedString GetLocalizedString(string key)
        {
            return _localizer[key];
        }
    }
}
