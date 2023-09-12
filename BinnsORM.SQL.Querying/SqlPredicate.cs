using BinnsORM.Objects;

namespace BinnsORM.SQL.Querying
{
    public enum SqlAnyAll
    {
        ANY,
        ALL
    }

    public struct SqlPredicate
    {
        private object? Value1 { get; set; }
        private object? Value2 { get; set; } = null;
        private SqlComparison Comparison { get; set; }


        #region Constructors and related methods

        /// <summary>
        /// Create predicates using IS NULL or IS NOT NULL directives
        /// </summary>
        public SqlPredicate(object value1, SqlComparison comparison)
        {
            Value1 = value1;
            Comparison = comparison;
            if (!Comparison.Equals(SqlComparison.IsNull)
                && !Comparison.Equals(SqlComparison.IsNotNull))
            {
                throw new InvalidClauseException(this);
            }
        }


        /// <summary>
        /// Create predicates comparing two values
        /// </summary>
        public SqlPredicate(object value1, SqlComparison comparison, object value2)
        {
            Value1 = value1;
            Comparison = comparison;
            Value2 = value2.ToSqlString();
            if (Comparison.Equals(SqlComparison.IsNull)
                || Comparison.Equals(SqlComparison.IsNotNull)
                || Comparison.Equals(SqlComparison.Between)
                || Comparison.Equals(SqlComparison.NotBetween)
                || ComparisonIsExists(comparison))
            {
                throw new InvalidClauseException(this);
            }
            if (Comparison.Equals(SqlComparison.In) || Comparison.Equals(SqlComparison.NotIn))
            {
                BuildInList(value2);
            }
        }


        /// <summary>
        /// Create predicates using ANY or ALL
        /// </summary>
        public SqlPredicate(object value1, SqlComparison comparison, SqlAnyAll anyAll, SqlSelect query)
        {
            Value1 = value1;
            Comparison = comparison;
            Value2 = $"{anyAll} ({query})";
            if (Comparison.Equals(SqlComparison.IsNull)
                || Comparison.Equals(SqlComparison.IsNotNull)
                || Comparison.Equals(SqlComparison.Between)
                || Comparison.Equals(SqlComparison.NotBetween)
                || ComparisonIsExists(comparison))
            {
                throw new InvalidClauseException(this);
            }
        }


        /// <summary>
        /// Create predicates using BETWEEN or NOT BETWEEN directives
        /// </summary>
        public SqlPredicate(object value1, SqlComparison comparison, object value2, object value3)
        {
            Value1 = value1;
            Comparison = comparison;
            if (ComparisonIsIn(Comparison))
            {
                BuildInList(value2, value3);
            }
            else if (Comparison.Equals(SqlComparison.Between) || Comparison.Equals(SqlComparison.NotBetween))
            {
                Value2 = $"{value2.ToSqlString()} AND {value3.ToSqlString()}";
            }
            else
            {
                throw new InvalidClauseException(this);
            }
        }


        /// <summary>
        /// Create predicates using IN or NOT IN directives
        /// </summary>
        public SqlPredicate(object value1, SqlComparison comparison, params object[] values)
        {
            Value1 = value1;
            Comparison = comparison;
            if (!ComparisonIsIn(Comparison))
            {
                throw new InvalidClauseException(this);
            }
            BuildInList(values);
        }


        /// <summary>
        /// Create predicates using EXISTS or NOT EXISTS directives
        /// </summary>
        public SqlPredicate(SqlComparison comparison, SqlSelect select)
        {
            Value1 = null;
            Comparison = comparison;
            Value2 = $"({select})";
            if (!ComparisonIsExists(Comparison))
            {
                throw new InvalidClauseException(this);
            }
        }

        public override string ToString()
        {
            string result = string.Empty;
            if (Value1 != null)
            {
                result = $"{Value1.ToSqlString()} ";
            }
            result += $"{Comparison}";
            if (Value2 != null)
            {
                result += $" {Value2}";
            }
            return result;
        }


        private void BuildInList(params object[] values)
        {
            string temp = "(";
            foreach (object value in values)
            {
                temp += $"{value.ToSqlString()}, ";
            }
            Value2 = temp[..^2] + ")";
        }


        private bool ComparisonIsIn(SqlComparison comparison)
        {
            return comparison.Equals(SqlComparison.In) || comparison.Equals(SqlComparison.NotIn);
        }


        private bool ComparisonIsExists(SqlComparison comparison)
        {
            return comparison.Equals(SqlComparison.Exists) || comparison.Equals(SqlComparison.NotExists);
        }

        #endregion
    }
}
