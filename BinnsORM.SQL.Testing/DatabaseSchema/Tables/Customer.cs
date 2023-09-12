namespace BinnsORM.SQL.Testing.DatabaseSchema.Tables
{
    public class Customer : BinnsORMTableBase
    {
        public override string DatabaseName
        {
            get
            {
                if (!string.IsNullOrEmpty(databaseName)) { return databaseName; }
                databaseName = BinnsORMConfig.GetDatabaseNameOrOverride("BINN");
                return databaseName;
            }
        }
        private string databaseName;

        public override string SchemaName => "dbo";

        public override string ObjectName => "Customer";

        public override string ObjectAlias => "Customer";

        public override CustomerFields Fields => new();

        public override void SetDataField(string field, object value)
        {
            Data.SetProperty(field, value);
        }

        public override T GetDataField<T>(string field)
        {
            return Data.GetProperty<T>(field);
        }

        public CustomerData Data { get; protected set; } = new();
    }
}

namespace BinnsORM.Objects.TableFields
{
    public class CustomerFields : BinnsORMFieldCollection
    {
        [Identity]
        [PrimaryKey]
        [DatabaseColumn]
        public static SqlColumn CustomerId { get; } = new("Customer", "CustomerId", "CustomerId");

        [DatabaseColumn]
        public static SqlColumn CustomerName { get; } = new("Customer", "CustomerName", "CustomerName");

        [DatabaseColumn]
        public static SqlColumn CreatedDate { get; } = new("Customer", "CreatedDate", "CreatedDate");
    }
}

namespace BinnsORM.Objects.TableData
{
    public class CustomerData : BinnsORMDataCollection
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public DateTime CreatedDate { get; set; }

        public CustomerData()
        {
            CreatedDate = DateTime.UtcNow;
        }
    }
}