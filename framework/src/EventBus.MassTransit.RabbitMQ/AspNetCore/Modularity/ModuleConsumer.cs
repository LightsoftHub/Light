using MassTransit;

namespace Light.AspNetCore.Modularity
{
    public interface IModuleConsumer
    {
        /// <summary>
        /// Add Module Masstransit Comsumers
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        IBusRegistrationConfigurator AddComsumers(
            IBusRegistrationConfigurator configurator) => configurator;
    }

    public abstract class ModuleConsumer : IModuleConsumer
    {
        public virtual IBusRegistrationConfigurator AddComsumers(
            IBusRegistrationConfigurator configurator) => configurator;
    }
}