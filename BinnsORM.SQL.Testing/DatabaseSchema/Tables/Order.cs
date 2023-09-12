namespace BinnsORM.SQL.Testing.DatabaseSchema.Tables
{
    public class Order : BinnsORMTableBase
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

        public override string ObjectName => "Order";

        public override string ObjectAlias => "Order";

        public override OrderFields Fields => new();

        public override void SetDataField(string field, object value)
        {
            Data.SetProperty(field, value);
        }

        public override T GetDataField<T>(string field)
        {
            return Data.GetProperty<T>(field);
        }

        public OrderData Data { get; protected set; } = new();
    }
}
namespace BinnsORM.Objects.TableFields
{
    public class OrderFields : BinnsORMFieldCollection
    {
        [Identity]
        [PrimaryKey]
        [DatabaseColumn]
        public static SqlColumn OrderId { get; } = new("Order", "OrderId", "OrderId");

        [DatabaseColumn]
        public static SqlColumn OrderNumber { get; } = new("Order", "OrderNumber", "OrderNumber");

        [DatabaseColumn]
        public static SqlColumn CustomerId { get; } = new("Order", "CustomerId", "CustomerId");

        [DatabaseColumn]
        public static SqlColumn CreatedDate { get; } = new("Order", "CreatedDate", "CreatedDate");
    }
}

namespace BinnsORM.Objects.TableData
{
    public class OrderData : BinnsORMDataCollection
    {
        public int OrderId { get; set; }

        public string OrderName { get; set; }

        public int CustomerId { get; set; }

        public DateTime CreatedDate { get; set; }

        public OrderData()
        {
            CreatedDate = DateTime.UtcNow;
        }
    }
}
