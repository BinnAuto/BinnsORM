using System.Data;

namespace BinnsORM.Console.SQL
{
    public class DatabaseTableModelGenerator : ModelGeneratorBase
    {
        public string GenerateTableClasses(List<DataRow> objectSchema)
        {
            SetSharedParameters(objectSchema[0]);

            string modelClass = BuildObjectModelClass();

            string tableFieldClass =
                $"namespace BinnsORM.Objects.TableFields\r\n" +
                "{\r\n" +
                $"\tpublic class {ObjectName}Fields : BinnsORMFieldCollection\r\n" +
                "\t{\r\n";

            string tableDataClass =
                $"namespace BinnsORM.Objects.TableData\r\n" +
                "{\r\n" +
                $"\tpublic class {ObjectName}Data : BinnsORMDataCollection\r\n" +
                "\t{\r\n";

            string dataConstructor =
                $"\t\tpublic {ObjectName}Data()\r\n" +
                "\t\t{\r\n";

            string defaultValue, currentColumnName;
            int? dataType, precision;
            short? maxLength;
            byte scale;
            bool isNullable, isPrimaryKey, isIdentity;
            foreach (var column in objectSchema)
            {
                currentColumnName = (string)column["ColumnName"];
                maxLength = (short?)column["MaxLength"];
                precision = (int)column["Precision"];
                scale = (byte)column["Scale"];
                isNullable = (bool)column["Nullable"];
                isPrimaryKey = (int)column["IsPrimaryKey"] == 1;
                isIdentity = (int)column["IsIdentity"] == 1;
                defaultValue = column["Default"] == DBNull.Value
                    ? "" : column["Default"].ToString();
                dataType = (int?)column["DataType"];

                if (isPrimaryKey)
                {
                    tableFieldClass += "\t\t[PrimaryKey]\r\n";
                }
                if (isIdentity)
                {
                    tableFieldClass += "\t\t[Identity]\r\n";
                }
                tableFieldClass +=
                    "\t\t[DatabaseColumn]\r\n" +
                    $"\t\tpublic static SqlColumn {currentColumnName} {{ get; }} = new(\"{ObjectAlias}\", \"{currentColumnName}\", \"{currentColumnName}\");\r\n" +
                    "\r\n";

                if (!string.IsNullOrEmpty(defaultValue))
                {
                    dataConstructor += GetDefaultStatement(currentColumnName, dataType.Value, defaultValue);
                }
                string cSharpDataType = GetCSharpDataType(dataType.Value, isNullable);
                tableDataClass += $"\t\tpublic {cSharpDataType} {currentColumnName} {{ get; set; }}\r\n\r\n";
            }
            modelClass += "}\r\n";
            tableFieldClass += "\t}\r\n}\r\n";
            dataConstructor += "\t\t}\r\n\t}\r\n}";
            string result = $"{modelClass}\r\n{tableFieldClass}\r\n{tableDataClass}\r\n{dataConstructor}";
            return result;
        }


        private string GetDefaultStatement(string columnName, int userTypeId, string defaultValue)
        {
            switch (userTypeId)
            {
                case ConsoleConstants.SqlDataTypes.INT:
                    if (int.TryParse(defaultValue[2..^2], out int intValue))
                    {
                        return $"\t\t\tthis.SetProperty(\"{columnName}\", {intValue});\r\n";
                    }
                    break;

                case ConsoleConstants.SqlDataTypes.BIT:
                    char defaultBit = defaultValue[2];
                    if ("01".Contains(defaultBit))
                    {
                        string boolValue = (defaultBit == '1').ToString().ToLower();
                        return $"\t\t\tthis.SetProperty(\"{columnName}\", {boolValue});\r\n";
                    }
                    break;

                case ConsoleConstants.SqlDataTypes.NUMERIC:
                    if (defaultValue.IndexOf('.') == -1)
                    {
                        defaultValue = defaultValue[..^2] + ".0))";
                    }
                    if (decimal.TryParse(defaultValue[2..^2], out decimal decimalValue))
                    {
                        return $"\t\t\tthis.SetProperty(\"{columnName}\", {decimalValue}m);\r\n";
                    }
                    break;

                case ConsoleConstants.SqlDataTypes.TEXT:
                case ConsoleConstants.SqlDataTypes.NTEXT:
                case ConsoleConstants.SqlDataTypes.VARCHAR:
                case ConsoleConstants.SqlDataTypes.NVARCHAR:
                    if (defaultValue.StartsWith("('") && defaultValue.EndsWith("')"))
                    {
                        defaultValue = defaultValue[2..^2]
                            .Replace("''", "'");
                        return $"\t\t\tthis.SetProperty(\"{columnName}\", \"{defaultValue}\");\r\n";
                    }
                    break;
            }

            string? defaultStatementOverride = BinnsORMConfiguration.GetDefaultValueMapping(defaultValue);
            if (string.IsNullOrEmpty(defaultStatementOverride))
            {
                return $"\t\t\tthis.SetProperty(\"{columnName}\", SqlQueryInterface.ExecuteQueryText(\"SELECT {defaultValue}\").Get<object>(0));\r\n";
            }
            return $"\t\t\tthis.SetProperty(\"{columnName}\", {defaultStatementOverride});\r\n";
        }
    }
}
