using Microsoft.Extensions.DependencyInjection;

namespace GypsyRig
{
    public static class BusConfigurator
    {
        public static IServiceCollection AddGypsyRig(this IServiceCollection services)
        {
            services.AddTransient<IProcessorBus, ProcessorBus>();
            return services;
        }
    }
}
