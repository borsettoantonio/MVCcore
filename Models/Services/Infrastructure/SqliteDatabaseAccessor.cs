using System.Data;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Configuration;
using MyCourse.Models.ValueObjects;
using MyCourse.Models.Exceptions;

namespace MyCourse.Models.Services.Infrastructure
{
    public class SqliteDatabaseAccessor : IDatabaseAccessor
    {
        private readonly IConfiguration config;
        private readonly ILogger<SqliteDatabaseAccessor> logger;

        public SqliteDatabaseAccessor(IConfiguration config, ILogger<SqliteDatabaseAccessor> logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public async Task<int> CommandAsync(FormattableString formattableCommand)
        {
            try
            {
                using SqliteConnection conn = await GetOpenedConnection();
                using SqliteCommand cmd = GetCommand(formattableCommand, conn);
                int affectedRows = await cmd.ExecuteNonQueryAsync();
                return affectedRows;
            }
            catch (SqliteException exc) when (exc.SqliteErrorCode == 19)
            {
                throw new ConstraintViolationException(exc);
            }
        }

        public async Task<T> QueryScalarAsync<T>(FormattableString formattableQuery)
        {
            using SqliteConnection conn = await GetOpenedConnection();
            using SqliteCommand cmd = GetCommand(formattableQuery, conn);
            object? result = await cmd.ExecuteScalarAsync();
            return (T)Convert.ChangeType(result!, typeof(T));
        }

        public async Task<DataSet> QueryAsync(FormattableString formattableQuery)
        {
            logger.LogInformation(formattableQuery.Format, formattableQuery.GetArguments());
            //logger.LogInformation(formattableQuery.Format);

            using SqliteConnection conn = await GetOpenedConnection();
            using SqliteCommand cmd = GetCommand(formattableQuery, conn);

            using var reader = await cmd.ExecuteReaderAsync();

            var dataSet = new DataSet();
            // ciclo per leggere più query
            do
            {
                var dataTable = new DataTable();
                dataSet.Tables.Add(dataTable);
                dataTable.Load(reader);
            } while (!reader.IsClosed);
            /*
            while (reader.Read())
            {
                string title = (string)reader["Title"];
                ....
            }
            */
            return dataSet;
        }

        private static SqliteCommand GetCommand(FormattableString formattableQuery, SqliteConnection conn)
        {
            // sanificazione contro la SQL Injection
            var queryArguments = formattableQuery.GetArguments();
            var sqliteParameters = new List<SqliteParameter>();
            for (int i = 0; i < queryArguments.Length; i++)
            {
                if (queryArguments[i] is Sql)
                {
                    continue;
                }
                var parameter = new SqliteParameter(i.ToString(), queryArguments[i]  ?? DBNull.Value);
                sqliteParameters.Add(parameter);
                queryArguments[i] = "@" + i;
            }
            string query = formattableQuery.ToString();
            var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddRange(sqliteParameters);
            return cmd;
        }

        private async Task<SqliteConnection> GetOpenedConnection()
        {
            var conn = new SqliteConnection(config.GetSection("ConnectionStrings").GetValue<string>("Default"));
            await conn.OpenAsync();
            return conn;
        }
    }
}