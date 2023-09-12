using System.Reflection;

namespace BinnsORM.Objects.TableFields
{
    public abstract class BinnsORMFieldCollection
    {
        public bool HasIdentityColumn()
        {
            if (!hasIdentityColumn.HasValue)
            {
                hasIdentityColumn = false;
                var properties = GetType().GetProperties();
                foreach (var property in properties)
                {
                    if (Attribute.IsDefined(property, typeof(Identity)))
                    {
                        hasIdentityColumn = true;
                        return true;
                    }
                }
            }
            return hasIdentityColumn.Value;
        }
        private bool? hasIdentityColumn;


        public string[] GetFieldNames()
        {
            return GetFieldNames(true, false);
        }


        public string[] GetFieldNames(bool includePrimaryKeyFields, bool includeIdentityFields)
        {
            List<string> result = new();
            var properties = GetType().GetProperties();
            foreach (var property in properties)
            {
                if (Attribute.IsDefined(property, typeof(DatabaseColumn))
                    && (includePrimaryKeyFields || !Attribute.IsDefined(property, typeof(PrimaryKey)))
                    && (includeIdentityFields || !Attribute.IsDefined(property, typeof(Identity))))
                {
                    result.Add(property.Name);
                }
            }
            return result.ToArray();
        }


        public PropertyInfo[] GetPrimaryKeyFields()
        {
            List<PropertyInfo> primaryKeysList = new();
            var properties = GetType().GetProperties();
            foreach (var property in properties)
            {
                if (Attribute.IsDefined(property, typeof(PrimaryKey)))
                {
                    primaryKeysList.Add(property);
                }
            }
            if (primaryKeysList.Count == 0)
            {
                string tableName = GetType().Name[..^6];
                throw new PrimaryKeyNotDefinedException(tableName);
            }
            return primaryKeysList.ToArray();
        }
    }
}
