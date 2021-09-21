using System;
using System.Data.SqlClient;

namespace GypsyRig.SqlToSql
{
    public class SqlToSqlProcessorBuilder
    {
        private Source _source;
        private Destination _destination;
        private Action<SqlBulkCopyColumnMappingCollection> _maps;

        public SqlToSqlProcessorBuilder Source(Action<Source> config)
        {
            var source = new Source();
            config.Invoke(source);
            _source = source;
            return this;
        }

        public SqlToSqlProcessorBuilder Destination(Action<Destination> config)
        {
            var destination = new Destination();
            config.Invoke(destination);
            _destination = destination;
            return this;
        }

        public SqlToSqlProcessorBuilder Maps(Action<SqlBulkCopyColumnMappingCollection> maps)
        {
            _maps = maps;
            return this;
        }

        public SqlToSqlProcessor ToProcessor()
        {
            var processor = new SqlToSqlProcessor(
                sourceConnection: _source.Connection,
                sourceQuery: _source.Script,
                destinationConnection: _destination.Connection,
                destinationTable: _destination.Table,
                mappingConfig: _maps);

            return processor;
        }
    }

    public static class SqlBulkCopyColumnMappingCollectionExtensions
    {
        public static SqlBulkCopyColumnMappingCollection Map(this SqlBulkCopyColumnMappingCollection maps, string name)
            => maps.Map(name, name);

        public static SqlBulkCopyColumnMappingCollection Map(this SqlBulkCopyColumnMappingCollection maps, string input, string output)
        {
            maps.Add(input, output);
            return maps;
        }
    }
}
