using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Light.AspNetCore.Modules
{
    public abstract class ModuleServiceCollection : IModuleServiceCollection
    {
        public virtual IServiceCollection ConfigureServices(
            IServiceCollection services) => services;

        public virtual IServiceCollection ConfigureServices(
            IServiceCollection services,
            IConfiguration configuration) => services;
    }
}