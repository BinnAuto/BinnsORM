using System.Data;

namespace BinnsORM.Console.SQL
{
    public class StoredProcedureClassGenerator : ModelGeneratorBase
    {
        public string GenerateClass(DataRowCollection parameters)
        {
            SetSharedParameters(parameters[0]);

            string result =
                GetUsingStatements() +
                "\r\n" +
                GetStoredProcedureNamespaceAndClassDeclaration() +
                "\r\n" +
                GetDatabaseNameProperty(false) +
                "\r\n";

            int rowIndex = 0;
            List<DataRow> procedureParameters = new();
            DataRow currentRow;
            string currentProcedure;
            while (rowIndex < parameters.Count)
            {
                currentRow = parameters[rowIndex];
                currentProcedure = currentRow["ObjectName"].ToString();
                procedureParameters.Add(currentRow);
                rowIndex++;
                if (rowIndex < parameters.Count)
                {
                    currentRow = parameters[rowIndex];
                    while (rowIndex < parameters.Count
                        && string.Equals(currentProcedure, currentRow["ObjectName"].ToString()))
                    {
                        procedureParameters.Add(currentRow);
                        rowIndex++;
                        if (rowIndex < parameters.Count)
                        {
                            currentRow = parameters[rowIndex];
                        }
                    }
                }
                result += GetStoreProcedureMethodDefinition(procedureParameters);
                procedureParameters.Clear();
            }
            result +=
                "\t}\r\n" +
                "}\r\n";
            return result;
        }


        private string GetStoreProcedureMethodDefinition(List<DataRow> parameters)
        {
            string procedureName = parameters[0]["ObjectName"].ToString();
            string cSharpParameterList = string.Empty;
            string sqlParameterList = string.Empty;
            string addParametersToCall = string.Empty;
            foreach (var param in parameters)
            {
                string parameterName = param["ParamName"].ToString();
                if (parameterName.Length == 0)
                {
                    continue;
                }
                parameterName = parameterName[1..];
                string dataType = GetCSharpDataType((int)param["DataType"], (bool)param["Nullable"]);
                cSharpParameterList += $" {dataType} {parameterName},";
                addParametersToCall += $"\t\t\tcall[\"{parameterName}\"] = {parameterName};\r\n";
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
                $"\t\tpublic static DataSet {procedureName}({cSharpParameterList})\r\n" +
                "\t\t{\r\n" +
                "\t\t\tDataSet result;\r\n" +
                $"\t\t\tstring sqlText = $\"EXEC {procedureName}{sqlParameterList}\";\r\n" +
                "\t\t\tresult = SqlQueryInterface.ExecuteQueryTextIntoDataSet(sqlText);\r\n" +
                "\t\t\treturn result;\r\n" +
                "\t\t}\r\n" +
                "\r\n" +
                $"\t\tpublic static SqlStoredProcedureCall Create{procedureName}Call({cSharpParameterList})\r\n" +
                "\t\t{\r\n" +
                $"\t\t\tSqlStoredProcedureCall call = new(\"{procedureName}\");\r\n" +
                addParametersToCall +
                "\t\t\treturn call;\r\n" +
                "\t\t}\r\n" +
                "\r\n";
            return result;
        }
    }
}
