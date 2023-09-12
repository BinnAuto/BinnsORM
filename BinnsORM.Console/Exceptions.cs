namespace BinnsORM.Console
{
    public class UnknownArgumentException : Exception
    {
        public UnknownArgumentException(string arg)
            : base($"The argument \"{arg}\" is not recognized") { }
    }


    public class UnspecifiedNamespaceException : Exception
    {
        public UnspecifiedNamespaceException()
            : base($"Namespace not specified. Specify a namespace using the {ConsoleConstants.ArgumentConstants.NamespaceSwitch} command.") { }
    }
}
