using BinnsORM.Objects.TableFields;

namespace BinnsORM.Objects
{
    public abstract class BinnsORMTableBase
    {
        public abstract string DatabaseName { get; }
        public abstract string SchemaName { get; }
        public abstract string ObjectName { get; }
        public abstract string ObjectAlias { get; }
        public abstract BinnsORMFieldCollection Fields { get; }

        public string FullyQualifiedName
        {
            get
            {
                return $"[{DatabaseName}].[{SchemaName}].[{ObjectName}]";
            }
        }

        public abstract void SetDataField(string field, object value);


        public abstract T GetDataField<T>(string field);


        public string ToJson()
        {
            string result = "{";
            string[] tableFields = Fields.GetFieldNames(true, true);
            foreach (string field in tableFields)
            {
                result += $"\"{field.Replace("\"", "\\\"")}\": ";
                var value = GetDataField<object>(field);
                if (value == null)
                {
                    result += "null";
                }
                else if (value is string)
                {
                    string valueString = (string)value;
                    result += $"\"{valueString.Replace("\"", "\\\"")}\"";
                }
                else if (value is bool)
                {
                    result += (bool)value ? "true" : "false";
                }
                else if (value is bool?)
                {
                    bool? valueBool = (bool?)value;
                    result += valueBool.Value ? "true" : "false";
                }
                else if (value is decimal)
                {
                    decimal valueDecimal = (decimal)value;
                    result += $"\"{valueDecimal}\"";
                }
                else if (value is decimal?)
                {
                    decimal? valueDecimal = (decimal?)value;
                    result += $"\"{valueDecimal}\"";
                }
                else if (value is DateTime)
                {
                    DateTime valueDate = (DateTime)value;
                    result += $"\"{valueDate:yyyy-MM-ddTHH:mm:ss.fff}\"";
                }
                else if (value is DateTime?)
                {
                    DateTime? valueDate = (DateTime?)value;
                    result += $"\"{valueDate:yyyy-MM-ddTHH:mm:ss.fff}\"";
                }
                else
                {
                    result += $"\"{value}\"";
                }
                result += ",";
            }
            result = result[..^1];
            result += "}";
            return result;
        }
    }
}
