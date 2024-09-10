using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Light.AspNetCore.Modularity
{
    internal interface IModule
    {
        /// <summary>
        /// Add Module Service Collection
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        IServiceCollection ConfigureServices(IServiceCollection services);

        /// <summary>
        /// Add Module Service Collection with IConfiguration
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration);
    }

    public abstract class Module : IModule
    {
        public virtual IServiceCollection ConfigureServices(IServiceCollection services) => services;

        public virtual IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration) => services;
    }
}