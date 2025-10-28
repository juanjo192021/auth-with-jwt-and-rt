using Mapster;
using System.Reflection;


namespace Auth.Application.Mappings
{
    public static class MapsterConfiguration
    {
        public static void RegisterMappings()
        {
            var config = TypeAdapterConfig.GlobalSettings;
            // 📦 Escanea automáticamente todas las clases que implementan IRegister
            config.Scan(Assembly.GetExecutingAssembly());
        }
    }
}
