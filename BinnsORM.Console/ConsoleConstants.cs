using System.Reflection.Metadata;

namespace BinnsORM.Console
{
    public static class ConsoleConstants
    {
        public static class SqlDataTypes
        {
            public const int TEXT = 35;

            public const int UNIQUEIDENTIFIER = 36;

            public const int INT = 56;
            
            public const int DATETIME = 61;

            public const int NTEXT = 99;

            public const int BIT = 104;
            
            public const int NUMERIC = 108;

            public const int VARCHAR = 167;

            public const int NVARCHAR = 231;
        }

        public static class ArgumentConstants
        {
            public const string HelpSwitch = "-help";

            public const string SourceDatabaseSwitch = "-src";

            public const string SchemaSwitch = "-schema";

            public const string NamespaceSwitch = "-namespace";

            public const string ConnectionStringSwitch = "-connstr";

            public const string ConfigurationFileSwitch = "-config";

            public const string NoPauseSwitch = "-nopause";

            public const string NoBuildSwitch = "-nobuild";

            public const string DatabaseTypeSwitch = "-type";
        }

        public static class DatabaseTypes
        {
            public const string SQL = "SQL";
        }
    }
}
