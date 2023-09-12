using System.Data;
using BinnsORM.Objects;

namespace BinnsORM.SQL.Querying
{
    public class SqlStoredProcedureCall
    {
        private readonly Dictionary<string, object> Parameters = new();

        public string StoredProcedureName { get; private set; }

        public object this[string paramName]
        {
            get => Parameters[paramName];
            set => AddParameter(paramName, value);
        }

        public SqlStoredProcedureCall(string storedProcedureName)
        {
            StoredProcedureName = storedProcedureName;
        }


        public void AddParameter(string parameterName, object value)
        {
            if (parameterName.StartsWith("@"))
            {
                parameterName = parameterName[1..];
            }
            Parameters[parameterName] = value;
        }


        public DataSet ExecuteIntoDataSet()
        {
            return SqlQueryInterface.ExecuteQueryTextIntoDataSet(ToString());
        }


        public void ExecuteNonQuery()
        {
            SqlQueryInterface.ExecuteNonQueryText(ToString());
        }


        public override string ToString()
        {
            string result = $"EXEC {StoredProcedureName} ";
            foreach (var kvp in Parameters)
            {
                result += $"@{kvp.Key}={kvp.Value.ToSqlString()},";
            }
            result = result[0..^1];
            return result;
        }


        public static implicit operator string(SqlStoredProcedureCall s)
        {
            return s.ToString();
        }
    }
}
