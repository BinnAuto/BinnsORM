using System.Data;
using System.Data.SqlClient;

namespace BinnsORM.Console.SQL
{
    public static class SQLQueryHandler
    {
        public static string? ConnectionString;
        private static SqlConnection SqlConnection
        {
            get
            {
                if (sqlConnection == null)
                {
                    if (string.IsNullOrEmpty(ConnectionString))
                    {
                        ConnectionString = BinnsORMConfiguration.ConnectionString;
                    }
                    sqlConnection = new(ConnectionString);
                    sqlConnection.Open();
                }
                return sqlConnection;
            }
        }
        private static SqlConnection sqlConnection;


        public static DataTable GetQueryResults(string query)
        {
            DataTable table = new();
            using (SqlCommand command = new(query, SqlConnection))
            {
                using SqlDataAdapter adapter = new(command);
                adapter.Fill(table);
            }
            return table;
        }


        public static void ExecuteNonQuery(string query)
        {
            using SqlCommand command = new(query, SqlConnection);
            command.ExecuteNonQuery();
        }
    }
}
