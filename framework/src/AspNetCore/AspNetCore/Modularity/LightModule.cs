using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Light.AspNetCore.Modularity
{
    public abstract class LightModule : IModule
    {
        public virtual void ConfigureServices(IServiceCollection services)
        { }

        public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        { }

        public virtual void ConfigureJobs(IServiceCollection services)
        { }

        public virtual void ConfigureJobs(IServiceCollection services, IConfiguration configuration)
        { }
    }
}