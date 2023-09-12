namespace BinnsORM.Console
{
    public static class SessionSettings
    {
        /// <summary>
        /// The name of the source database to reference when generating the ORM model.
        /// </summary>
        public static string SourceDatabase { get; set; }

        /// <summary>
        /// The name of the source database to reference when generating the ORM model.
        /// </summary>
        public static string DatabaseType { get; set; }

        /// <summary>
        /// A comma-delimited list of database schemas to include when generating the ORM model.
        /// </summary>
        public static string Schemas { get; set; }

        /// <summary>
        /// The namespace to use when generating the ORM model.
        /// </summary>
        public static string NamespaceName { get; set; }

        /// <summary>
        /// The connection string used to connect to the source database.
        /// </summary>
        public static string ConnectionString { get; set; }
    }
}
