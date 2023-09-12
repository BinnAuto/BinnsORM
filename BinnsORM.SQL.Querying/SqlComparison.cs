namespace BinnsORM.SQL.Querying
{
    public struct SqlComparison
    {
        public static SqlComparison IsEqualTo = new("=");
        public static SqlComparison NotEqualTo = new("!=");
        public static SqlComparison LessThan = new("<");
        public static SqlComparison NotLessThan = new("!<");
        public static SqlComparison LessThanOrEqualTo = new("<=");
        public static SqlComparison GreaterThan = new(">");
        public static SqlComparison NotGreaterThan = new("!>");
        public static SqlComparison GreaterThanOrEqualTo = new(">=");
        public static SqlComparison Like = new("LIKE");
        public static SqlComparison NotLike = new("NOT LIKE");
        public static SqlComparison In = new("IN");
        public static SqlComparison NotIn = new("NOT IN");
        public static SqlComparison IsNull = new("IS NULL");
        public static SqlComparison IsNotNull = new("IS NOT NULL");
        public static SqlComparison Between = new("BETWEEN");
        public static SqlComparison NotBetween = new("NOT BETWEEN");
        public static SqlComparison Exists = new("EXISTS");
        public static SqlComparison NotExists = new("NOT EXISTS");

        private readonly string SqlString { get; }

        private SqlComparison(string sqlString)
        {
            SqlString = sqlString;
        }


        public override string ToString()
        {
            return SqlString;
        }


        public bool Equals(SqlComparison other)
        {
            return string.Equals(SqlString, other.SqlString);
        }
    }
}
