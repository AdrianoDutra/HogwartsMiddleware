using Hogwarts.Middleware.Interfaces.Service;
using Hogwarts.Middleware.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Hogwarts.Middleware.DependencyInjection
{
    public class ConfigurationDependency
    {
        public static void ConfigureDependencies(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ICharacterService, CharacterService>();
            
            
        }
    }
}
