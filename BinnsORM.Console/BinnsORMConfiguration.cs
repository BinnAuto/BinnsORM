using System.Text.Json.Nodes;

namespace BinnsORM.Console
{
    public static class BinnsORMConfiguration
    {
        private static JsonNode ConfigJson;

        public static string CodeOutputDirectory
        {
            get
            {
                return ConfigJson["CodeOutputDirectory"].ToString();
            }
        }

        public static string SchemaQueryFilePath
        {
            get
            {
                return ConfigJson["SchemaQueryFilePath"].ToString();
            }
        }

        public static string ConnectionString
        {
            get
            {
                return ConfigJson["ConnectionString"].ToString();
            }
        }


        public static string[] DllCopyDirectories
        {
            get
            {
                var dirArray = ConfigJson["DllCopyDirectories"].AsArray();
                string[] result = new string[dirArray.Count];
                int index = 0;
                foreach(var dir in dirArray)
                {
                    result[index] = dir.ToString();
                    index++;
                }
                return result;
            }
        }


        public static string? GetDefaultValueMapping(string defaultValue)
        {
            return ConfigJson["DefaultValueMappings"]?[defaultValue]?.ToString();
        }


        public static void LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("BinnsORM Configuration file not found", filePath);
            }

            string jsonFile = File.ReadAllText(filePath);
            ConfigJson = JsonNode.Parse(jsonFile);
        }
    }
}
