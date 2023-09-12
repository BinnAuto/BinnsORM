using System.Data;

namespace BinnsORM.Console.SQL
{
    public class FunctionClassGenerator : ModelGeneratorBase
    {
        public string GenerateClass(DataRowCollection parameters)
        {
            SetSharedParameters(parameters[0]);

            string result =
                GetUsingStatements() +
                "\r\n" +
                GetFunctionNamespaceAndClassDeclaration() +
                "\r\n" +
                GetDatabaseNameProperty(false, true) +
                "\r\n";

            int rowIndex = 0;
            List<DataRow> functionParameters = new();
            DataRow currentRow;
            string currentFunction;
            while (rowIndex < parameters.Count)
            {
                currentRow = parameters[rowIndex];
                currentFunction = currentRow["ObjectName"].ToString()!;
                functionParameters.Add(currentRow);
                rowIndex++;
                if (rowIndex < parameters.Count)
                {
                    currentRow = parameters[rowIndex];
                    while (rowIndex < parameters.Count
                        && string.Equals(currentFunction, currentRow["ObjectName"].ToString()))
                    {
                        functionParameters.Add(currentRow);
                        rowIndex++;
                        if (rowIndex < parameters.Count)
                        {
                            currentRow = parameters[rowIndex];
                        }
                    }
                }
                result += GetFunctionMethodDefinition(functionParameters);
                functionParameters.Clear();
            }
            result +=
                "\t}\r\n" +
                "}\r\n";
            return result;
        }


        private string GetFunctionMethodDefinition(List<DataRow> functionParameters)
        {
            string functionName = functionParameters[0]["ObjectName"].ToString()!;
            string cSharpParameterList = string.Empty;
            string sqlParameterList = string.Empty;
            foreach (var param in functionParameters)
            {
                string parameterName = param["ParameterName"].ToString()!;
                if (parameterName.Length == 0)
                {
                    continue;
                }
                parameterName = parameterName[1..];
                string dataType = GetCSharpDataType((int)param["DataType"], (bool)param["Nullable"]);
                cSharpParameterList += $" {dataType} {parameterName},";
                sqlParameterList += $" {{{parameterName}?.ToSqlString() ?? \"NULL\"}},";
            }
            if (cSharpParameterList.Length > 0)
            {
                cSharpParameterList = cSharpParameterList[1..^1];
            }
            if (sqlParameterList.Length > 0)
            {
                sqlParameterList = sqlParameterList[..^1];
            }
            string result =
                $"\t\tpublic static DataTable {functionName}({cSharpParameterList})\r\n" +
                "\t\t{\r\n" +
                "\t\t\tDataTable result;\r\n" +
                $"\t\t\tstring sqlText = $\"USE {{DatabaseName}} SELECT [{Schema}].[{functionName}]({sqlParameterList})\";\r\n" +
                "\t\t\tresult = SqlQueryInterface.ExecuteQueryText(sqlText);\r\n" +
                "\t\t\treturn result;\r\n" +
                "\t\t}\r\n" +
                "\r\n";
            return result;
        }
    }
}
