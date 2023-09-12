using System.Data;
using System.Reflection;

namespace BinnsORM.Objects
{
    public static class ObjectExtensions
    {
        public static string ToSqlString(this object input)
        {
            if (input == null)
            {
                return "NULL";
            }
            else if (input is string inputString)
            {
                inputString = inputString.Replace("'", "''");
                return $"N'{inputString}'";
            }
            else if (input is bool)
            {
                bool inputBool = (bool)input;
                return (inputBool ? "1" : "0");
            }
            else if (input is bool?)
            {
                bool? inputBool = (bool?)input;
                return (inputBool.Value ? "1" : "0");
            }
            else if (input is DateTime)
            {
                DateTime inputDate = (DateTime)input;
                string result = inputDate.ToString("MM/dd/yyyy HH:mm:ss.fff");
                return $"'{result}'";
            }
            else if (input is DateTime?)
            {
                DateTime? inputDate = (DateTime?)input;
                string result = inputDate.Value.ToString("MM/dd/yyyy HH:mm:ss.fff");
                return $"'{result}'";
            }
            else if (input is Guid)
            {
                return $"'{input}'";
            }
            else
            {
                return input.ToString()!;
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


        public static T GetProperty<T>(this object input, string propertyName)
        {
            object result = input;
            string[] properties = propertyName.Split('.');
            var type = result.GetType();
            foreach (string prop in properties)
            {
                var propData = GetPropertyInfo(type, prop);
                result = propData.GetValue(result, null);
            }
            return (T)result;
        }


        public static T SetProperty<T>(this T input, string propertyName, object value)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            var property = GetPropertyInfo(input.GetType(), propertyName);
            if (property.PropertyType == typeof(bool) && value is int valueInt)
            {
                value = (valueInt == 1);
            }
            property.SetValue(input, value);
            return input;
        }


        private static PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName);
            if (property == null)
            {
                throw new NonexistentPropertyException(type.Name, propertyName);
            }
            return property;
        }
    }
}
