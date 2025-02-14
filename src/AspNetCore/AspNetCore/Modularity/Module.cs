using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Light.AspNetCore.Modularity
{
    public abstract class Module : IModuleServiceCollection
    {
        public virtual void ConfigureServices(IServiceCollection services)
        { }

        public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        { }
    }
}