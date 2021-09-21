using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GypsyRig.SqlToSql
{
    public class SqlToSqlProcessor : TableProcessorBase
    {
        private readonly string _sourceConnection;
        private readonly string _destinationConnection;
        private readonly string _sourceQuery;
        private readonly string _destinationTable;

        private readonly Action<SqlBulkCopyColumnMappingCollection> _mappingConfig;

        public int Timeout = 5; //in seconds
        public bool WipeBeforeImport;

        public SqlToSqlProcessor(string sourceConnection, string sourceQuery, string destinationConnection, string destinationTable)
        {
            _sourceConnection = sourceConnection;
            _sourceQuery = sourceQuery;
            _destinationConnection = destinationConnection;
            _destinationTable = destinationTable;
        }
        public SqlToSqlProcessor(string sourceConnection, string sourceQuery, string destinationConnection, string destinationTable, Action<SqlBulkCopyColumnMappingCollection> mappingConfig)
            : this(sourceConnection, sourceQuery, destinationConnection, destinationTable)
        {
            _mappingConfig = mappingConfig;
        }

        public async override Task Migrate()
        {
            if (WipeBeforeImport)
                await WipeData(_destinationTable, _destinationConnection);

            using var sourceConnection = new SqlConnection(_sourceConnection);

            sourceConnection.Open();

            using var command = PrepareCommand(sourceConnection, _sourceQuery, Timeout);
            using var reader = command.ExecuteReader();
            using var bulkCopy = new SqlBulkCopy(_destinationConnection, SqlBulkCopyOptions.KeepIdentity);

            bulkCopy.DestinationTableName = _destinationTable;
            MapDatabaseColumns(reader, bulkCopy);
            await bulkCopy.WriteToServerAsync(reader);
            bulkCopy.Close();
        }

        private static async Task WipeData(string destinationTable, string destinationConnection)
        {
            var script = $"delete from {destinationTable}";
            using var connection = new SqlConnection(destinationConnection);

            connection.Open();
            using var command = new SqlCommand(script, connection);
            await command.ExecuteNonQueryAsync();
        }

        private static SqlCommand PrepareCommand(SqlConnection connection, string script, int timeout)
        {
            var command = new SqlCommand(script, connection)
            {
                CommandTimeout = (int)TimeSpan.FromMinutes(timeout).TotalMilliseconds
            };
            return command;
        }

        private void MapDatabaseColumns(SqlDataReader reader, SqlBulkCopy bcp)
        {
            if (_mappingConfig == null)
                AutoMapDatabaseColumns(reader, bcp);

            _mappingConfig.Invoke(bcp.ColumnMappings);
        }

        private void AutoMapDatabaseColumns(SqlDataReader reader, SqlBulkCopy bcp)
        {
            var schema = reader.GetColumnSchema();
            foreach(var column in schema)
                bcp.ColumnMappings.Add(column.ColumnName, column.ColumnName);
        }
    }
}
