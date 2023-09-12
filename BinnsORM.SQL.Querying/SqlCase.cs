using BinnsORM.Objects;

namespace BinnsORM.SQL.Querying
{
    public class SqlCase
    {
        private string? SwitchValue { get; set; } = null;
        private string? ElseValue { get; set; }

        private string? Alias;

        private readonly Dictionary<string, string> Cases = new();


        #region Constructors

        public SqlCase() { }

        public SqlCase(string switchValue)
        {
            SwitchValue = switchValue;
        } 

        #endregion

        public SqlCase When(object value, object outValue)
        {
            if(SwitchValue == null)
            {
                throw new InvalidSqlCaseException(value);
            }
            Cases[value.ToSqlString()] = outValue.ToSqlString();
            return this;
        }


        public SqlCase When(SqlPredicate predicate, object value)
        {
            var clause = new SqlClause(predicate);
            return When(clause, value);
        }


        public SqlCase When(SqlClause clause, object value)
        {
            if (SwitchValue != null)
            {
                throw new InvalidSwitchCaseException(clause);
            }
            Cases[clause.ToString()] = value.ToSqlString();
            return this;
        }


        public SqlCase Else(object value)
        {
            ElseValue = value.ToSqlString();
            return this;
        }


        public SqlCase As(string alias)
        {
            Alias = alias;
            return this;
        }


        public override string ToString()
        {
            string result = "";
            if(!string.IsNullOrEmpty(Alias))
            {
                result = $"[{Alias}] = ";
            }
            result += "CASE ";
            if(SwitchValue != null)
            {
                result += $"{SwitchValue} ";
            }
            foreach (var keyValuePair in Cases)
            {
                result += $"WHEN {keyValuePair.Key} THEN {keyValuePair.Value} ";
            }
            if (ElseValue != null)
            {
                result += $"ELSE {ElseValue} ";
            }
            result += "END";
            return result;
        }
    }
}
