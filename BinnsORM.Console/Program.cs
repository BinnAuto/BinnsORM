using BinnsORM.Console;
using BinnsORM.Console.SQL;

bool pauseOnEnd = true;
bool skipBuild = false;
try
{
    BinnsORMConfiguration.LoadFromFile("BinnsORMConsoleConfig.json");
    int parseResult = ParseArguments();
    if(parseResult == 0)
    {
        ProcessArguments();
        switch(SessionSettings.DatabaseType)
        {
            case ConsoleConstants.DatabaseTypes.SQL:
                SQLCodeGenerator.Run();
                if(!skipBuild)
                {
                    SQLCodeBuilder.Run();
                }
                break;

            default:
                throw new($"Database type {SessionSettings.DatabaseType} not recognized");
        }
    }
    ConsoleLogger.LogLine("Program complete.");
    OnProgramEnd();
}
catch(UnknownArgumentException e)
{
    LogError(e);
    PrintHelp();
    OnProgramEnd();
}
catch (Exception e)
{
    LogError(e);
    OnProgramEnd();
}


int ParseArguments()
{
    if(args == null 
        || args.Length == 0
        || args[0].Equals(ConsoleConstants.ArgumentConstants.HelpSwitch, StringComparison.InvariantCultureIgnoreCase))
    {
        PrintHelp();
        return 1;
    }

    for(int i = 0; i < args.Length; i++)
    {
        string arg = args[i];
        switch (arg)
        {
            case ConsoleConstants.ArgumentConstants.SourceDatabaseSwitch:
                i++;
                SessionSettings.SourceDatabase = args[i];
                continue;

            case ConsoleConstants.ArgumentConstants.SchemaSwitch:
                i++;
                SessionSettings.Schemas = args[i];
                continue;

            case ConsoleConstants.ArgumentConstants.NamespaceSwitch:
                i++;
                SessionSettings.NamespaceName = args[i];
                continue;

            case ConsoleConstants.ArgumentConstants.ConnectionStringSwitch:
                i++;
                SessionSettings.ConnectionString = args[i];
                continue;

            case ConsoleConstants.ArgumentConstants.ConfigurationFileSwitch:
                i++;
                BinnsORMConfiguration.LoadFromFile(args[i]);
                SessionSettings.ConnectionString = BinnsORMConfiguration.ConnectionString;
                continue;

            case ConsoleConstants.ArgumentConstants.NoPauseSwitch:
                pauseOnEnd = false;
                continue;

            case ConsoleConstants.ArgumentConstants.NoBuildSwitch:
                skipBuild = true;
                continue;

            case ConsoleConstants.ArgumentConstants.DatabaseTypeSwitch:
                i++;
                SessionSettings.DatabaseType = args[i];
                continue;

            default:
                throw new UnknownArgumentException(arg);
        }
    }
    return 0;
}


void ProcessArguments()
{
    if(string.IsNullOrEmpty(SessionSettings.DatabaseType))
    {
        ConsoleLogger.LogLine($"Database type not specified. Defaulting to '{ConsoleConstants.DatabaseTypes.SQL}'");
        SessionSettings.DatabaseType = ConsoleConstants.DatabaseTypes.SQL;
    }
    if(string.IsNullOrEmpty(SessionSettings.Schemas))
    {
        ConsoleLogger.LogLine("No schema(s) specified. Defaulting to schema 'dbo'");
        SessionSettings.Schemas = "dbo";
    }
    if(string.IsNullOrEmpty(SessionSettings.NamespaceName))
    {
        ConsoleLogger.LogLine($"Namespace name not specified. Defaulting to '{SessionSettings.SourceDatabase}'");
        SessionSettings.NamespaceName = SessionSettings.SourceDatabase;
    }
    if(string.IsNullOrEmpty(SessionSettings.NamespaceName))
    {
        throw new UnspecifiedNamespaceException();
    }
}


void PrintHelp()
{
    ConsoleLogger.LogLine($"  {ConsoleConstants.ArgumentConstants.SourceDatabaseSwitch} <database> - Specify the source database to reference when generating the ORM model.");
    ConsoleLogger.LogLine($"  {ConsoleConstants.ArgumentConstants.SchemaSwitch} <schemas> - Specify the schema(s) to include when generating the ORM model, separated by commas.");
    ConsoleLogger.LogLine($"  {ConsoleConstants.ArgumentConstants.NamespaceSwitch} <namespace> - Specify the namespace to use when generating the ORM model.");
    ConsoleLogger.LogLine($"  {ConsoleConstants.ArgumentConstants.ConnectionStringSwitch} <connectionstring> - Specify the connection string used to connect to the source database.");
    ConsoleLogger.LogLine($"  {ConsoleConstants.ArgumentConstants.HelpSwitch} - Print this Help section");
    ConsoleLogger.LogLine($"  {ConsoleConstants.ArgumentConstants.NoPauseSwitch} - The program does not stop upon completion.");
    ConsoleLogger.LogLine($"  {ConsoleConstants.ArgumentConstants.NoBuildSwitch} - Skip code build step");
    ConsoleLogger.LogLine(string.Empty);
}


void LogError(Exception e)
{
    ConsoleLogger.LogLine(e.Message);
#if DEBUG
    ConsoleLogger.LogLine(e.StackTrace);
#endif
}


void OnProgramEnd()
{
    if(pauseOnEnd)
    {
        ConsoleLogger.LogLine("Press Enter to exit");
        Console.ReadLine();
    }
}