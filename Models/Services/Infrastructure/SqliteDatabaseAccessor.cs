using System.Data;
using Microsoft.Data.Sqlite;

namespace MyCourse.Models.Services.Infrastructure
{
    public class SqliteDatabaseAccessor : IDatabaseAccessor
    {
        public async Task<DataSet> QueryAsync(FormattableString formattableQuery)
        {
            // sanificazione contro la SQL Injection
            var queryArguments=formattableQuery.GetArguments();
            var sqliteParameters = new List<SqliteParameter>();
            for(int i=0;i< queryArguments.Length; i++)
            {
                var parameter = new SqliteParameter(i.ToString() ,queryArguments[i]);
                sqliteParameters.Add(parameter);
                queryArguments[i]="@" + i;
            }
            string query= formattableQuery.ToString();

            using (var conn = new SqliteConnection("Data Source=Data/MyCourse.db"))
            {
                await conn.OpenAsync();
                using (var cmd = new SqliteCommand(query, conn))
                {cmd.Parameters.AddRange(sqliteParameters);

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