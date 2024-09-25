using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Light.AspNetCore.Modularity
{
    internal interface IModule
    {
        /// <summary>
        /// Module Service Collection
        /// </summary>
        void Add(IServiceCollection services);

        /// <summary>
        /// Module Service Collection with IConfiguration
        /// </summary>
        void Add(IServiceCollection services, IConfiguration configuration);

        /// <summary>
        /// For separate job services
        /// </summary>
        void AddJob(IServiceCollection services);

        /// <summary>
        /// For separate job services with IConfiguration
        /// </summary>
        void AddJob(IServiceCollection services, IConfiguration configuration);
    }

    public abstract class Module : IModule
    {
        public virtual void Add(IServiceCollection services)
        { }

        public virtual void Add(IServiceCollection services, IConfiguration configuration)
        { }

        public virtual void AddJob(IServiceCollection services)
        { }

        public virtual void AddJob(IServiceCollection services, IConfiguration configuration)
        { }
    }
}