namespace BinnsORM.Objects
{
    public class EmptyFieldException : Exception
    {
        public EmptyFieldException()
            : base("An empty field cannot be created") { }
    }

    public class NonexistentPropertyException : Exception
    {
        public NonexistentPropertyException(string objectType, string propertyName)
            : base($"Object type {objectType} does not have a property named \"{propertyName}\"")
        { }
    }


    public class PrimaryKeyNotDefinedException : Exception
    {
        public PrimaryKeyNotDefinedException(string tableName)
            : base($"Table {tableName} does not have a primary key defined")
        { }
    }
}
