using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace GypsyRig.Examples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .Build();

            var services = new ServiceCollection()
                .ConfigExample(config);

            var provider = services.BuildServiceProvider();
            var bus = provider.GetService<IProcessorBus>();

            Console.WriteLine("Migrating...");
            await bus.Migrate();
            Console.WriteLine("Done migrating!");

            Console.WriteLine("Migrating specific...");
            await bus.Migrate("Test");
            Console.WriteLine("Done migrating!");
        }
    }
}
