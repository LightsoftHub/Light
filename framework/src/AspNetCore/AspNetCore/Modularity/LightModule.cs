using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Light.AspNetCore.Modularity
{
    public abstract class LightModule : IModule
    {
        public virtual IServiceCollection Add(IServiceCollection services) => services;

        public virtual IServiceCollection Add(IServiceCollection services, IConfiguration configuration) => services;

        public virtual IServiceCollection AddJob(IServiceCollection services) => services;

        public virtual IServiceCollection AddJob(IServiceCollection services, IConfiguration configuration) => services;
    }
}