using Microsoft.Extensions.DependencyInjection;
using System;

namespace GypsyRig.SqlToSql
{
    public static class SqlToSqlConfigurator
    {
        public static IServiceCollection AddSqlProcessor(this IServiceCollection services, string name, Action<SqlToSqlProcessorBuilder> config)
        {
            services.AddTransient<ITableProcessor, SqlToSqlProcessor>(services =>
            {
                var builder = new SqlToSqlProcessorBuilder();
                config.Invoke(builder);

                var processor = builder.ToProcessor();
                processor.Name = name;
                return processor;
            });

            return services;
        }
    }
}
