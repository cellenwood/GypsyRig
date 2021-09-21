using GypsyRig.SqlToSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GypsyRig.Examples
{
    public static class ExampleConfigurator
    {
        public static IServiceCollection ConfigExample(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddGypsyRig();

            services
                .AddSqlProcessor(name: "Test", processor => processor
                .Configure(config, "dbo.Destination")
                .Maps(maps => {
                    maps
                    .Map("Id")
                    .Map("InstitutionType")
                    .Map("CorpName")
                    .Map("Crd")
                    .Map("Npn")
                    .Map("Tin")
                    .Map("Ein")));

            services
                .AddSqlProcessor(name: "Test2", processor => processor
                .Configure(config, "dbo.Destination"));

            return services;
        }

        public static SqlToSqlProcessorBuilder Configure(this SqlToSqlProcessorBuilder builder, IConfiguration config, string destTable)
        {
            Action<Source> GetSource() => async source =>
            {
                source.Connection = config.GetConnectionString("Source");
                source.Script = await ScriptHelper.GetScriptContents("Scripts/MyScript.sql");
            };

            Action<Destination> GetDestination(string table) => dest =>
            {
                dest.Connection = config.GetConnectionString("Destination");
                dest.Table = table;
            };

            builder
                .Source(GetSource())
                .Destination(GetDestination(destTable)));

            return builder;
        }
    }
}
