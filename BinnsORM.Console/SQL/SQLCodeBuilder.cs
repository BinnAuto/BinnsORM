using System.Diagnostics;

namespace BinnsORM.Console.SQL
{
    public static class SQLCodeBuilder
    {
        public static string Namespace { get; set; }

        public static void Run()
        {
            Namespace = SessionSettings.NamespaceName;
            string[] schemas = SessionSettings.Schemas.Split(',');
            foreach (string s in schemas)
            {
                string outputDirectory = BinnsORMConfiguration.CodeOutputDirectory;
                outputDirectory += $"/{SessionSettings.SourceDatabase}.{s}";
                if (Directory.GetFiles(outputDirectory, "*.csproj").Length == 0)
                {
                    WriteProjectFile(outputDirectory, s);
                }
                string projectFilePath = Directory.GetFiles(outputDirectory, "*.csproj")[0];
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo("cmd", $"/c dotnet build {projectFilePath}")
                    {
                        UseShellExecute = false,
                        RedirectStandardOutput = true
                    }
                };
                process.Start();
                string processResult = process.StandardOutput.ReadToEnd();
                if (process.ExitCode == 0)
                {
                    string[] copyDirectories = BinnsORMConfiguration.DllCopyDirectories;
                    if (copyDirectories != null)
                    {
                        string fileName = $"BinnsORM.Model.{Namespace}.{s}.dll";
                        string outputDll = outputDirectory + $"/bin/Debug/net6.0/{fileName}";
                        foreach (string directory in copyDirectories)
                        {
                            Directory.CreateDirectory(directory);
                            File.Copy(outputDll, Path.Combine(directory, fileName), true);
                        }
                    }
                }
                else
                {
                    ConsoleLogger.LogLine("ERROR(S) IN BUILD");
                    ConsoleLogger.LogLine(processResult);
                    ConsoleLogger.LogLine(string.Empty);
                }
            }
        }


        private static void WriteProjectFile(string outputDirectory, string schema)
        {
            string fileContent =
                "<Project Sdk=\"Microsoft.NET.Sdk\">\r\n" +
                "  <PropertyGroup>\r\n" +
                "    <TargetFramework>net6.0</TargetFramework>\r\n" +
                "    <ImplicitUsings>enable</ImplicitUsings>\r\n" +
                "    <Nullable>enable</Nullable>\r\n" +
                "  </PropertyGroup>\r\n" +
                "  <ItemGroup>\r\n" +
                "    <Reference Include=\"BinnsORM.Objects\">\r\n" +
                "        <HintPath>..\\..\\BinnsORM.Objects.dll</HintPath>\r\n" +
                "    </Reference>\r\n" +
                "    <Reference Include=\"BinnsORM.SQL.Querying\">\r\n" +
                "        <HintPath>..\\..\\BinnsORM.SQL.Querying.dll</HintPath>\r\n" +
                "    </Reference>\r\n" +
                "    <Reference Include=\"System.Data.SqlClient\">\r\n" +
                "        <HintPath>..\\..\\System.Data.SqlClient.dll</HintPath>\r\n" +
                "    </Reference>\r\n" +
                "  </ItemGroup>\r\n" +
                "</Project>";
            string fileName = $"BinnsORM.Model.{Namespace}.{schema}.csproj";
            outputDirectory += $"/{fileName}";
            File.WriteAllText(outputDirectory, fileContent);
        }
    }
}
