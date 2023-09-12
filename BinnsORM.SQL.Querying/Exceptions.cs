namespace BinnsORM.SQL.Querying
{
    public class EmptyClauseException : Exception
    {
        public EmptyClauseException()
            : base("An empty clause is not allowed") { }
    }


    public class InvalidClauseException : Exception
    {
        public InvalidClauseException(SqlPredicate clause)
            : base($"Clause {clause} is not valid")
        { }
    }


    public class InvalidSwitchCaseException : Exception
    {
        public InvalidSwitchCaseException(SqlClause clause)
            : base($"Clause '{clause}' cannot be used as a condition in a switch case")
        { }
    }


    public class InvalidSqlCaseException : Exception
    {
        public InvalidSqlCaseException(object value)
            : base($"Value '{value}' cannot be used as a condition in a standard case")
        { }
    }


    public class InvalidJoinException : Exception 
    {
        public InvalidJoinException(string join) 
            : base($"Join {join} is missing clause")
        { }
    }


    public class InvalidTopAmountException : Exception
    {
        public InvalidTopAmountException(int value)
            : base($"The value {value} is invalid for a TOP directive") { }
    }


    public class InvalidTopPercentAmountException : Exception
    {
        public InvalidTopPercentAmountException(int value)
            : base($"The value {value} is invalid for a TOP PERCENT directive") { }
    }


    public class EmptyUpdateStatementException : Exception
    {
        public EmptyUpdateStatementException()
            : base("The UPDATE statment does not set any values") { }
    }


    public class MoreThanOneResultException : Exception
    {
        public MoreThanOneResultException(int resultCount)
            : base($"{resultCount} results were found when at most one was expected")
        { }
    }


    public class InvalidPivotException : Exception
    {
        public InvalidPivotException(string message)
            : base(message) 
        { }
    }


    public class UninitializedPivotException : Exception
    {
        public UninitializedPivotException(string methodName)
            : base($"PIVOT must be initialized before calling the {methodName} method")
        { }
    }


    public class InvalidUnPivotException : Exception
    {
        public InvalidUnPivotException(string message)
            : base(message)
        { }
    }


    public class UninitializedUnPivotException : Exception
    {
        public UninitializedUnPivotException(string methodName)
            : base($"UNPIVOT must be initialized before calling the {methodName} method")
        { }
    }
}
