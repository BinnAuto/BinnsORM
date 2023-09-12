namespace BinnsORM.Objects
{
    public readonly struct SqlColumn
    {
        public readonly string ColumnName { get; }
        public readonly string ParentAlias { get; }
        public readonly string? ColumnAlias { get; }


        #region Constructors

        /// <summary>
        /// Use of this constructor is not allowed
        /// </summary>
        /// <exception cref="EmptyFieldException">This exception will occur</exception>
        public SqlColumn()
        {
            throw new EmptyFieldException();
        }


        public SqlColumn(string tableAlias, string columnName)
        {
            ParentAlias = tableAlias;
            ColumnName = columnName;
            ColumnAlias = null;
        }


        public SqlColumn(string tableAlias, string columnName, string columnAlias)
        {
            ParentAlias = tableAlias;
            ColumnName = columnName;
            ColumnAlias = columnAlias;
        }


        public SqlColumn(SqlColumn baseField, string columnAlias)
        {
            ParentAlias = baseField.ParentAlias;
            ColumnName = baseField.ColumnName;
            ColumnAlias = columnAlias;
        }


        public SqlColumn(string tableAlias, SqlColumn baseField)
        {
            ParentAlias = tableAlias;
            ColumnName = baseField.ColumnName;
            ColumnAlias = baseField.ColumnAlias;
        }


        public SqlColumn(string tableAlias, SqlColumn baseField, string columnAlias)
        {
            ParentAlias = tableAlias;
            ColumnName = baseField.ColumnName;
            ColumnAlias = columnAlias;
        }

        #endregion


        public string GetReadableFieldName()
        {
            string uppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string fieldName = ColumnName;
            bool previousCharIsLowercase = false;
            string result = string.Empty;
            for (int i = 0; i < fieldName.Length; i++)
            {
                if (uppercaseLetters.Contains(fieldName[i]))
                {
                    if (previousCharIsLowercase)
                    {
                        result += " ";
                    }
                    previousCharIsLowercase = false;
                }
                else
                {
                    previousCharIsLowercase = true;
                }
                result += fieldName[i];
            }
            if (result.StartsWith("Is "))
            {
                result += "?";
            }
            return result;
        }


        public override string ToString()
        {
            string result = $"[{ParentAlias}].[{ColumnName}]";
            if (!string.IsNullOrEmpty(ColumnAlias) 
                && ColumnAlias != ColumnName)
            {
                result = $"[{ColumnAlias}] = " + result;
            }
            return result;
        }


        public string ToStringWithoutAlias()
        {
            return $"[{ParentAlias}].[{ColumnName}]";
        }


        public static implicit operator string(SqlColumn s)
        {
            return s.ToString();
        }
    }
}
