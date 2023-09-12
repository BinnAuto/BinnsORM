using System.Data;

namespace BinnsORM.Console.SQL
{
    public class DatabaseViewModelGenerator : ModelGeneratorBase
    {
        public string GenerateViewClasses(List<DataRow> objectSchema)
        {
            SetSharedParameters(objectSchema[0]);

            string modelClass = BuildObjectModelClass();

            string viewFieldClass =
                $"\tpublic class {ObjectName}Fields : BinnsORMFieldCollection\r\n" +
                "\t{\r\n";

            string viewDataClass =
                $"\tpublic class {ObjectName}Data : BinnsORMDataCollection\r\n" +
                "\t{\r\n";

            string dataConstructor =
                $"\t\tpublic {ObjectName}Data()\r\n" +
                "\t\t{\r\n";

            string currentColumnName;
            int? dataType, precision;
            short? maxLength;
            byte scale;
            bool isNullable;
            foreach (var column in objectSchema)
            {
                currentColumnName = (string)column["ColumnName"];
                maxLength = (short?)column["MaxLength"];
                precision = (int)column["Precision"];
                scale = (byte)column["Scale"];
                isNullable = (bool)column["Nullable"];
                dataType = (int?)column["DataType"];
                viewFieldClass +=
                    "\t\t[DatabaseColumn]\r\n" +
                    $"\t\tpublic static SqlColumn {currentColumnName} {{ get; }} = new(\"{ObjectAlias}\", \"{currentColumnName}\", \"{currentColumnName}\");\r\n" +
                    "\r\n";

                string cSharpDataType = GetCSharpDataType(dataType.Value, isNullable);
                viewDataClass += $"\t\tpublic {cSharpDataType} {currentColumnName} {{ get; set; }}\r\n\r\n";
            }
            viewFieldClass += "\t}\r\n";
            dataConstructor += "\t\t}\r\n\t}\r\n}";
            string result = $"{modelClass}\r\n{viewFieldClass}\r\n{viewDataClass}\r\n{dataConstructor}";
            return result;
        }
    }
}
