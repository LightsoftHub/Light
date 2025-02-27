using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Light.AspNetCore.Modularity
{
    public abstract class Module : IModuleServiceCollection
    {
        public virtual void Configure(IServiceCollection services)
        { }

        public virtual void Configure(IServiceCollection services, IConfiguration configuration)
        { }
    }
}