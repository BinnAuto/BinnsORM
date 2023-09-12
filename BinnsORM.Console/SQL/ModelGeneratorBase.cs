using System.Data;

namespace BinnsORM.Console.SQL
{
    public abstract class ModelGeneratorBase
    {
        public string? Schema { get; private set; }
        public string? EntityType { get; private set; }
        public string? ObjectName { get; private set; }
        public string? ObjectAlias { get; private set; }

        public void SetSharedParameters(DataRow schemaRow)
        {
            Schema = schemaRow["SchemaName"].ToString()!;
            EntityType = schemaRow["EntityType"].ToString()!;
            ObjectName = schemaRow["ObjectName"].ToString()!;
            ObjectAlias = schemaRow["ObjectAlias"].ToString()!;
        }

        public string BuildObjectModelClass()
        {
            return
                GetUsingStatements() +
                "\r\n" +
                GetObjectNamespaceAndClassDeclaration() +
                "\r\n" +
                GetDatabaseNameProperty() +
                "\r\n" +
                GetOverrideProperties() +
                "\r\n" +
                $"\t\tpublic {ObjectName}Data Data {{ get; protected set; }} = new();\r\n" +
                "\r\n" +
                "\t}\r\n";
        }


        protected string GetUsingStatements()
        {
            return
                "using BinnsORM.Objects;\r\n" +
                "using BinnsORM.Objects.TableData;\r\n" +
                "using BinnsORM.Objects.TableFields;\r\n" +
                "using BinnsORM.SQL.Querying;\r\n" +
                "using System.Data;\r\n";
        }


        private string GetObjectNamespaceAndClassDeclaration()
        {
            return
                $"namespace BinnsORM.Model.{SessionSettings.NamespaceName}.{Schema}.{EntityType}s\r\n" +
                "{\r\n" +
                $"\tpublic class {ObjectName} : BinnsORMTableBase\r\n" +
                "\t{\r\n";
        }


        protected string GetFunctionNamespaceAndClassDeclaration()
        {
            return
                $"namespace BinnsORM.Model.{SessionSettings.NamespaceName}.{Schema}.Functions\r\n" +
                "{\r\n" +
                $"\tpublic class Functions\r\n" +
                "\t{\r\n";
        }


        protected string GetStoredProcedureNamespaceAndClassDeclaration()
        {
            return
                $"namespace BinnsORM.Model.{SessionSettings.NamespaceName}.{Schema}.StoredProcedures\r\n" +
                "{\r\n" +
                $"\tpublic class StoredProcedures\r\n" +
                "\t{\r\n";
        }


        protected string GetDatabaseNameProperty(bool doOverride = true, bool isStatic = false)
        {
            return
                $"\t\tpublic {(doOverride ? "override" : string.Empty)}{(isStatic ? "static" : string.Empty)} string DatabaseName\r\n" +
                 "\t\t{\r\n" +
                 "\t\t\tget\r\n" +
                 "\t\t\t{\r\n" +
                 "\t\t\t\tif(!string.IsNullOrEmpty(databaseName)) { return databaseName; }\r\n" +
                $"\t\t\t\tdatabaseName = BinnsORMConfig.GetDatabaseNameOrOverride(\"{SessionSettings.NamespaceName}\");\r\n" +
                "\t\t\t\treturn databaseName;\r\n" +
                "\t\t\t}\r\n" +
                "\t\t}\r\n" +
                $"\t\tprivate {(isStatic ? "static" : string.Empty)} string databaseName;\r\n";
        }


        private string GetOverrideProperties()
        {
            return
                $"\t\tpublic override string SchemaName => \"{Schema}\";\r\n" +
                "\r\n" +
                $"\t\tpublic override string ObjectName => \"{ObjectName}\";\r\n" +
                "\r\n" +
                $"\t\tpublic override string ObjectAlias => \"{ObjectAlias}\";\r\n" +
                "\r\n" +
                $"\t\tpublic override {ObjectName}Fields Fields => new();\r\n" +
                "\r\n" +
                "\t\tpublic override void SetDataField(string field, object value)\r\n" +
                "\t\t{\r\n" +
                "\t\t\tData.SetProperty(field, value);\r\n" +
                "\t\t}\r\n" +
                "\r\n" +
                "\t\tpublic override T GetDataField<T>(string field)\r\n" +
                "\t\t{\r\n" +
                "\t\t\treturn Data.GetProperty<T>(field);\r\n" +
                "\t\t}\r\n";
        }


        public string GetCSharpDataType(int userTypeId, bool isNullable)
        {
            string result = string.Empty;
            switch (userTypeId)
            {
                case ConsoleConstants.SqlDataTypes.TEXT:
                case ConsoleConstants.SqlDataTypes.NTEXT:
                case ConsoleConstants.SqlDataTypes.VARCHAR:
                case ConsoleConstants.SqlDataTypes.NVARCHAR:
                    result = "string";
                    break;

                case ConsoleConstants.SqlDataTypes.UNIQUEIDENTIFIER:
                    result = "Guid";
                    break;

                case ConsoleConstants.SqlDataTypes.INT:
                    result = "int";
                    break;

                case ConsoleConstants.SqlDataTypes.DATETIME:
                    result = "DateTime";
                    break;

                case ConsoleConstants.SqlDataTypes.BIT:
                    result = "bool";
                    break;

                case ConsoleConstants.SqlDataTypes.NUMERIC:
                    result = "decimal";
                    break;
            }
            result += isNullable ? "?" : string.Empty;
            return result;
        }
    }
}
