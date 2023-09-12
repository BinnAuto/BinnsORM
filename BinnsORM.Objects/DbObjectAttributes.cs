namespace BinnsORM
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKey : Attribute { }


    [AttributeUsage(AttributeTargets.Property)]
    public class DatabaseColumn : Attribute { }


    [AttributeUsage(AttributeTargets.Property)]
    public class Identity : Attribute { }
}
