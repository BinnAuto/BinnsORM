using System.Data;

namespace BinnsORM.Console.SQL
{
    public static class SQLCodeGenerator
    {
        private static string CurrentDatabase { get; set; }
        private static string CurrentSchema { get; set; }
        private static string OutputDirectory { get; set; }
        private static DataTable DatabaseSchema { get; set; }
        private static List<DataRow> CurrentObjectSchema { get; set; } = new();

        public static void Run()
        {
            CurrentDatabase = SessionSettings.SourceDatabase;
            string[] schemas = SessionSettings.Schemas.Split(",");
            foreach (string s in schemas)
            {
                CurrentSchema = s;
                PrepareOutputDirectory();
                GetDatabaseSchema();
                BuildDatabaseObjects();
                BuildFunctionClass();
                BuildStoredProcedureClass();
            }
        }


        private static void PrepareOutputDirectory()
        {
            OutputDirectory = BinnsORMConfiguration.CodeOutputDirectory
                + $"/{CurrentDatabase}.{CurrentSchema}";
            Directory.CreateDirectory(OutputDirectory);
            Directory.CreateDirectory($"{OutputDirectory}/Functions");
            Directory.CreateDirectory($"{OutputDirectory}/StoredProcedures");
            foreach (var file in Directory.GetFiles(OutputDirectory, "*.cs"))
            {
                File.Delete(file);
            }
        }


        private static void GetDatabaseSchema()
        {
            string schemaQueryFilePath = BinnsORMConfiguration.SchemaQueryFilePath;
            if (!File.Exists(schemaQueryFilePath))
            {
                throw new FileNotFoundException("Schema query file not found", schemaQueryFilePath);
            }

            string schemaQuery = File.ReadAllText(schemaQueryFilePath);
            schemaQuery = schemaQuery.Replace("%_DATABASE_%", CurrentDatabase)
                .Replace("%_SCHEMA_%", CurrentSchema);
            DatabaseSchema = SQLQueryHandler.GetQueryResults(schemaQuery);
        }


        private static void BuildDatabaseObjects()
        {
            DatabaseTableModelGenerator tableModelGenerator = new();
            DatabaseViewModelGenerator viewModelGenerator = new(); int rowIndex = 0;
            DataRow currentRow;
            string objectType, currentObjectName, currentObjectAlias;

            while (rowIndex < DatabaseSchema.Rows.Count)
            {
                currentRow = DatabaseSchema.Rows[rowIndex];
                objectType = currentRow["EntityType"].ToString();
                currentObjectName = currentRow["ObjectName"].ToString();
                CurrentObjectSchema.Add(currentRow);
                rowIndex++;
                currentRow = DatabaseSchema.Rows[rowIndex];
                while (rowIndex < DatabaseSchema.Rows.Count
                    && currentObjectName.Equals(currentRow["ObjectName"].ToString()))
                {
                    CurrentObjectSchema.Add(currentRow);
                    rowIndex++;
                    if (rowIndex < DatabaseSchema.Rows.Count)
                    {
                        currentRow = DatabaseSchema.Rows[rowIndex];
                    }
                }
                if (string.Equals(objectType, "Table", StringComparison.InvariantCultureIgnoreCase))
                {
                    string tableClassFile = tableModelGenerator.GenerateTableClasses(CurrentObjectSchema);
                    string outputPath = $"{OutputDirectory}/{currentObjectName}.cs";
                    File.WriteAllText(outputPath, tableClassFile);
                }
                if (string.Equals(objectType, "View", StringComparison.InvariantCultureIgnoreCase))
                {
                    string viewClassFile = viewModelGenerator.GenerateViewClasses(CurrentObjectSchema);
                    string outputPath = $"{OutputDirectory}/{currentObjectName}.cs";
                    File.WriteAllText(outputPath, viewClassFile);
                }
                CurrentObjectSchema.Clear();
            }
        }


        private static void BuildFunctionClass()
        {
            string functionQuery =
                $"USE {CurrentDatabase}" +
                " SELECT " +
                " [SchemaName] = s.name " +
                ", [EntityType] = 'Function' " +
                ", [ObjectName] = o.name " +
                ", [ObjectAlias] = o.name " +
                ", [ParameterName] = p.name " +
                ", [DataType] = p.user_type_id " +
                ", [Nullable] = p.is_nullable " +
                " FROM sys.parameters p " +
                " INNER JOIN sys.objects o ON o.object_id = p.object_id " +
                " INNER JOIN sys.schemas s ON s.schema_id = o.schema_id " +
                " WHERE [type] IN ('FN', 'IF', 'AF', 'FS', 'FT') " +
                $" AND s.name = '{CurrentSchema}'";
            var functions = SQLQueryHandler.GetQueryResults(functionQuery);
            if (functions.Rows.Count == 0)
            {
                return;
            }

            FunctionClassGenerator functionClassGenerator = new();
            string functionClass = functionClassGenerator.GenerateClass(functions.Rows);
            string outputPath = $"{OutputDirectory}/Functions/Functions.cs";
            File.WriteAllText(outputPath, functionClass);
        }


        private static void BuildStoredProcedureClass()
        {
            string storedProcedureQuery =
                $"USE {CurrentDatabase}" +
                " SELECT " +
                " [SchemaName] = s.name " +
                ", [EntityType] = 'Procedure' " +
                ", [ObjectName] = o.name " +
                ", [ObjectAlias] = o.name " +
                ", [ParamName] = ISNULL(p.name, '') " +
                ", [DataType] = p.user_type_id" +
                ", [Nullable] = p.is_nullable " +
                " FROM sys.procedures c" +
                " LEFT JOIN sys.parameters p ON c.object_id = p.object_id" +
                " INNER JOIN sys.objects o ON o.object_id = c.object_id" +
                " INNER JOIN sys.schemas s ON s.schema_id = o.schema_id" +
                $" WHERE s.name = '{CurrentSchema}'" +
                " ORDER BY c.name, ISNULL(p.parameter_id, 1)";
            var procedures = SQLQueryHandler.GetQueryResults(storedProcedureQuery);
            if (procedures.Rows.Count == 0)
            {
                return;
            }

            StoredProcedureClassGenerator storedProcedureClassGenerator = new();
            string storedProcedureClass = storedProcedureClassGenerator.GenerateClass(procedures.Rows);
            string outputPath = $"{OutputDirectory}/StoredProcedures/StoredProcedures.cs";
            File.WriteAllText(outputPath, storedProcedureClass);
        }
    }
}
