using BinnsORM.Objects;

namespace BinnsORM.SQL.Querying
{
    public static class ObjectExtensions
    {
        #region SqlColumn | Predicate methods

        public static SqlPredicate IsEqualTo(this SqlColumn sqlColumn, object value)
        {
            return new(sqlColumn, SqlComparison.IsEqualTo, value);
        }


        public static SqlPredicate NotEqualTo(this SqlColumn sqlColumn, object value)
        {
            return new(sqlColumn, SqlComparison.NotEqualTo, value);
        }


        public static SqlPredicate LessThan(this SqlColumn sqlColumn, object value)
        {
            return new(sqlColumn, SqlComparison.LessThan, value);
        }


        public static SqlPredicate LessThanOrEqualTo(this SqlColumn sqlColumn, object value)
        {
            return new(sqlColumn, SqlComparison.LessThanOrEqualTo, value);
        }


        public static SqlPredicate GreaterThan(this SqlColumn sqlColumn, object value)
        {
            return new(sqlColumn, SqlComparison.GreaterThan, value);
        }


        public static SqlPredicate GreaterThanOrEqualTo(this SqlColumn sqlColumn, object value)
        {
            return new(sqlColumn, SqlComparison.GreaterThanOrEqualTo, value);
        }


        public static SqlPredicate Like(this SqlColumn sqlColumn, object value)
        {
            return new(sqlColumn, SqlComparison.Like, value);
        }


        public static SqlPredicate NotLike(this SqlColumn sqlColumn, object value)
        {
            return new(sqlColumn, SqlComparison.NotLike, value);
        }


        public static SqlPredicate In(this SqlColumn sqlColumn, params object[] values)
        {
            return new(sqlColumn, SqlComparison.In, values);
        }


        public static SqlPredicate NotIn(this SqlColumn sqlColumn, params object[] values)
        {
            return new(sqlColumn, SqlComparison.NotIn, values);
        }


        public static SqlPredicate IsNull(this SqlColumn sqlColumn)
        {
            return new(sqlColumn, SqlComparison.IsNull);
        }


        public static SqlPredicate IsNotNull(this SqlColumn sqlColumn)
        {
            return new(sqlColumn, SqlComparison.IsNotNull);
        }


        public static SqlPredicate Between(this SqlColumn sqlColumn, object value1, object value2)
        {
            return new(sqlColumn, SqlComparison.Between, value1, value2);
        }


        public static SqlPredicate NotBetween(this SqlColumn sqlColumn, object value1, object value2)
        {
            return new(sqlColumn, SqlComparison.NotBetween, value1, value2);
        }

        #endregion


        #region SqlRawText | Predicate methods

        public static SqlPredicate IsEqualTo(this SqlRawText SqlRawText, object value)
        {
            return new(SqlRawText, SqlComparison.IsEqualTo, value);
        }


        public static SqlPredicate NotEqualTo(this SqlRawText SqlRawText, object value)
        {
            return new(SqlRawText, SqlComparison.NotEqualTo, value);
        }


        public static SqlPredicate LessThan(this SqlRawText SqlRawText, object value)
        {
            return new(SqlRawText, SqlComparison.LessThan, value);
        }


        public static SqlPredicate LessThanOrEqualTo(this SqlRawText SqlRawText, object value)
        {
            return new(SqlRawText, SqlComparison.LessThanOrEqualTo, value);
        }


        public static SqlPredicate GreaterThan(this SqlRawText SqlRawText, object value)
        {
            return new(SqlRawText, SqlComparison.GreaterThan, value);
        }


        public static SqlPredicate GreaterThanOrEqualTo(this SqlRawText SqlRawText, object value)
        {
            return new(SqlRawText, SqlComparison.GreaterThanOrEqualTo, value);
        }


        public static SqlPredicate Like(this SqlRawText SqlRawText, object value)
        {
            return new(SqlRawText, SqlComparison.Like, value);
        }


        public static SqlPredicate NotLike(this SqlRawText SqlRawText, object value)
        {
            return new(SqlRawText, SqlComparison.NotLike, value);
        }


        public static SqlPredicate In(this SqlRawText SqlRawText, params object[] values)
        {
            return new(SqlRawText, SqlComparison.In, values);
        }


        public static SqlPredicate NotIn(this SqlRawText SqlRawText, params object[] values)
        {
            return new(SqlRawText, SqlComparison.NotIn, values);
        }


        public static SqlPredicate IsNull(this SqlRawText SqlRawText)
        {
            return new(SqlRawText, SqlComparison.IsNull);
        }


        public static SqlPredicate IsNotNull(this SqlRawText SqlRawText)
        {
            return new(SqlRawText, SqlComparison.IsNotNull);
        }


        public static SqlPredicate Between(this SqlRawText SqlRawText, object value1, object value2)
        {
            return new(SqlRawText, SqlComparison.Between, value1, value2);
        }


        public static SqlPredicate NotBetween(this SqlRawText SqlRawText, object value1, object value2)
        {
            return new(SqlRawText, SqlComparison.NotBetween, value1, value2);
        }

        #endregion
    }
}
