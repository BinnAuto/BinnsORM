using System.Text.Json.Nodes;

namespace BinnsORM.Objects
{
    public static class BinnsORMConfig
    {
        private static JsonNode Configuration
        {
            get
            {
                if(configuration == null)
                {
                    LoadFromFile("BinnsORMConfig.json");
                }
                return configuration;
            }
        }
        private static JsonNode configuration;


        public static string ConnectionString
        {
            get
            {
                return Configuration["ConnectionString"].ToString();
            }
        }



        public static void LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("BinnsORM Configuration file not found", filePath);
            }

            string jsonFile = File.ReadAllText(filePath);
            configuration = JsonNode.Parse(jsonFile);
        }


        public static string? GetDatabaseOverride(string databaseName)
        {
            return Configuration["DatabaseOverrides"]?[databaseName]?.ToString();
        }


        public static string GetDatabaseNameOrOverride(string databaseName)
        {
            return Configuration["DatabaseOverrides"]?[databaseName]?.ToString()
                ?? databaseName;
        }
    }
}