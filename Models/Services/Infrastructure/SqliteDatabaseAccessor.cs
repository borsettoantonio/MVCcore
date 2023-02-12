using System.Data;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Configuration;

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

        public async Task<DataSet> QueryAsync(FormattableString formattableQuery)
        {
            logger.LogInformation(formattableQuery.Format,formattableQuery.GetArguments());
            //logger.LogInformation(formattableQuery.Format);

            // sanificazione contro la SQL Injection
            var queryArguments = formattableQuery.GetArguments();
            var sqliteParameters = new List<SqliteParameter>();
            for (int i = 0; i < queryArguments.Length; i++)
            {
                var parameter = new SqliteParameter(i.ToString(), queryArguments[i]);
                sqliteParameters.Add(parameter);
                queryArguments[i] = "@" + i;
            }
            string query = formattableQuery.ToString();

            using (var conn = new SqliteConnection(config.GetSection("ConnectionStrings").GetValue<string>("Default")))
            {
                await conn.OpenAsync();
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddRange(sqliteParameters);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
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
                }
            }

        }
    }
}