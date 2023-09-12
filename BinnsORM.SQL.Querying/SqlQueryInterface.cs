using BinnsORM.Objects;
using System.Data;
using System.Data.SqlClient;

namespace BinnsORM.SQL.Querying
{
    public static class SqlQueryInterface
    {
        private static readonly string ConnectionString = BinnsORMConfig.ConnectionString;

        public static DataTable ExecuteQueryText(SqlSelect select)
        {
            return ExecuteQueryText(select.ToString());
        }


        public static DataTable ExecuteQueryText(string query)
        {
            SqlConnection sqlConnection = new(ConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = new(query, sqlConnection);
            DataTable result = new();
            using (SqlDataAdapter adapter = new(sqlCommand))
            {
                adapter.Fill(result);
            }
            sqlConnection.Close();
            return result;
        }


        public static DataSet ExecuteQueryTextIntoDataSet(string query)
        {
            SqlConnection sqlConnection = new(ConnectionString);
            sqlConnection.Open();
            var sqlCommand = new SqlCommand(query, sqlConnection);
            var result = new DataSet();
            using (var sqlDataAdapter = new SqlDataAdapter(sqlCommand))
            {
                sqlDataAdapter.Fill(result);
            }
            sqlConnection.Close();
            return result;
        }


        public static int ExecuteNonQueryText(string sqlText)
        {
            SqlConnection sqlConnection = new(ConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = new(sqlText, sqlConnection);
            int result = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return result;
        }


        public static int Count(this SqlSelect select)
        {
            select.ResetSelect();
            select.Select(SqlAggregates.Count()!);
            select.ResetOrderBy();
            return select.ToScalarValueOrDefault<int>();
        }


        public static T FirstOrDefault<T>(this SqlSelect select) where T : BinnsORMTableBase
        {
            var dataTable = ExecuteQueryText(select);
            if (dataTable.Rows.Count == 0)
            {
                return default;
            }
            return ConvertToModel<T>(dataTable.Rows[0]);
        }


        public static T SingleOrDefault<T>(this SqlSelect select) where T : BinnsORMTableBase
        {
            var dataTable = ExecuteQueryText(select);
            if (dataTable.Rows.Count == 0)
            {
                return default;
            }
            if (dataTable.Rows.Count > 1)
            {
                throw new MoreThanOneResultException(dataTable.Rows.Count);
            }
            return ConvertToModel<T>(dataTable.Rows[0]);
        }


        public static T[] ToModelArray<T>(this SqlSelect select) where T : BinnsORMTableBase
        {
            var dataTable = ExecuteQueryText(select);
            return ConvertToModelArray<T>(dataTable);
        }


        public static T ToScalarValueOrDefault<T>(this SqlSelect select)
        {
            var dataTable = ExecuteQueryText(select);
            if (dataTable.Rows.Count == 0)
            {
                return default;
            }
            return (T)dataTable.Rows[0].ItemArray[0];
        }


        public static T[] ToPrimitiveArray<T>(this SqlSelect select)
        {
            var dataTable = ExecuteQueryText(select);
            T[] result = new T[dataTable.Rows.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (T)(dataTable.Rows[i].ItemArray[0]);
            }
            return result;
        }


        public static T ConvertToModel<T>(this DataRow row) where T : BinnsORMTableBase
        {
            T entity = Activator.CreateInstance<T>();
            string[] fields = entity.Fields.GetFieldNames(true, true);
            foreach (string field in fields)
            {
                object cellValue = row.TryGetColumnValue<object>(field);
                entity.SetDataField(field, cellValue);
            }
            return entity;
        }


        public static T[] ConvertToModelArray<T>(this DataTable table) where T : BinnsORMTableBase
        {
            T[] result = new T[table.Rows.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = ConvertToModel<T>(table.Rows[i]);
            }
            return result;
        }


        public static string InsertString<T>(this T entity) where T : BinnsORMTableBase
        {
            return InsertString(entity, false);
        }


        public static string InsertString<T>(this T entity, bool identityInsert) where T : BinnsORMTableBase
        {
            string result = string.Empty;
            string table = $"{entity.DatabaseName}.[{entity.SchemaName}].[{entity.ObjectName}]";
            bool hasIdentityColumn = entity.Fields.HasIdentityColumn();
            if (hasIdentityColumn && identityInsert)
            {
                result += $"SET IDENTITY_INSERT {table} ON ";
            }
            string[] fields = entity.Fields.GetFieldNames(true, identityInsert);
            result += $"INSERT INTO {table} (";
            foreach (string field in fields)
            {
                result += $"{field}, ";
            }
            result = result[..^2] + ") VALUES (";
            object property;
            foreach (string field in fields)
            {
                property = entity.GetDataField<object>(field);
                result += property.ToSqlString() + ", ";
            }
            result = result[..^2] + ")";
            if (hasIdentityColumn && identityInsert)
            {
                result += $" SET IDENTITY_INSERT {table} OFF ";
            }
            return result;
        }


        public static void Insert<T>(this T entity) where T : BinnsORMTableBase
        {
            string insertSql = entity.InsertString();
            ExecuteNonQueryText(insertSql);
        }


        public static string UpdateString<T>(this T entity) where T : BinnsORMTableBase
        {
            T tableInstance = Activator.CreateInstance<T>();
            string[] fields = entity.Fields.GetFieldNames(false, false);
            string updateString =
                $"UPDATE {tableInstance.FullyQualifiedName} SET ";
            foreach (string field in fields)
            {
                object value = entity.GetDataField<object>(field);
                updateString += $"{field} = {value.ToSqlString()},";
            }
            updateString = updateString[..^1];

            var primaryKeys = entity.Fields.GetPrimaryKeyFields();
            object primaryKeyValue = entity.GetDataField<object>(primaryKeys[0].Name);
            updateString += $" WHERE {primaryKeys[0].Name} = {primaryKeyValue.ToSqlString()} ";
            for (int i = 1; i < primaryKeys.Length; i++)
            {
                primaryKeyValue = entity.GetDataField<object>(primaryKeys[i].Name);
                updateString += $"AND {primaryKeys[i].Name} = {primaryKeyValue.ToSqlString()} ";
            }
            return updateString;
        }


        public static void Update<T>(this T entity) where T : BinnsORMTableBase
        {
            string updateSql = entity.UpdateString();
            ExecuteNonQueryText(updateSql);
        }


        public static string DeleteString<T>(this T entity) where T : BinnsORMTableBase
        {
            T tableInstance = Activator.CreateInstance<T>();
            string deleteString = $"DELETE FROM {tableInstance.FullyQualifiedName} ";
            var primaryKeys = entity.Fields.GetPrimaryKeyFields();
            object primaryKeyValue = entity.GetDataField<object>(primaryKeys[0].Name);
            deleteString += $"WHERE {primaryKeys[0].Name} = {primaryKeyValue.ToSqlString()} ";
            for (int i = 1; i < primaryKeys.Length; i++)
            {
                primaryKeyValue = entity.GetDataField<object>(primaryKeys[i].Name);
                deleteString += $"AND {primaryKeys[i].Name} = {primaryKeyValue.ToSqlString()} ";
            }
            return deleteString;
        }


        public static void Delete<T>(this T entity) where T : BinnsORMTableBase
        {
            string deleteSql = entity.DeleteString();
            ExecuteNonQueryText(deleteSql);
        }


        public static T? TryGetColumnValue<T>(this DataTable table, string columnName)
        {
            return table.TryGetColumnValue<T>(columnName, 0);
        }


        public static T? TryGetColumnValue<T>(this DataTable table, string columnName, int rowIndex = 0)
        {
            if (rowIndex < table.Rows.Count)
            {
                DataRow row = table.Rows[rowIndex];
                return row.TryGetColumnValue<T>(columnName);
            }
            // TODO: Log index out of range
            return default;
        }


        public static T? TryGetColumnValue<T>(this DataRow row, string columnName)
        {
            if (!row.Table.Columns.Contains(columnName))
            {
                return default;
            }
            object value = row[columnName];
            if (value == DBNull.Value)
            {
                return default;
            }
            else
            {
                return (T)value;
            }
        }


        public static T Get<T>(this DataRow row, string columnName)
        {
            return (T)row[columnName];
        }


        public static T Get<T>(this DataRow row, int index)
        {
            return (T)row.ItemArray[index];
        }


        public static T Get<T>(this DataTable table, string columnName, int rowIndex = 0)
        {
            return (T)table.Rows[rowIndex][columnName];
        }


        public static T Get<T>(this DataTable table, int columnIndex, int rowIndex = 0)
        {
            return (T)table.Rows[rowIndex].ItemArray[columnIndex];
        }
    }
}
