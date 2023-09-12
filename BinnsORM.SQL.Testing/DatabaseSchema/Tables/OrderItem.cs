namespace BinnsORM.SQL.Testing.DatabaseSchema.Tables
{
    public class OrderItem : BinnsORMTableBase
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

        public override string ObjectName => "OrderItem";

        public override string ObjectAlias => "OrderItem";

        public override OrderItemFields Fields => new();

        public override void SetDataField(string field, object value)
        {
            Data.SetProperty(field, value);
        }

        public override T GetDataField<T>(string field)
        {
            return Data.GetProperty<T>(field);
        }

        public OrderItemData Data { get; protected set; } = new();
    }
}
namespace BinnsORM.Objects.TableFields
{
    public class OrderItemFields : BinnsORMFieldCollection
    {
        [Identity]
        [PrimaryKey]
        [DatabaseColumn]
        public static SqlColumn OrderItemId { get; } = new("OrderItem", "OrderItemId", "OrderItemId");

        [DatabaseColumn]
        public static SqlColumn OrderId { get; } = new("OrderItem", "OrderId", "OrderId");

        [DatabaseColumn]
        public static SqlColumn CustomerId { get; } = new("OrderItem", "CustomerId", "CustomerId");

        [DatabaseColumn]
        public static SqlColumn ItemId { get; } = new("OrderItem", "ItemId", "ItemId");

        [DatabaseColumn]
        public static SqlColumn Quantity { get; } = new("OrderItem", "Quantity", "Quantity");

        [DatabaseColumn]
        public static SqlColumn Price { get; } = new("OrderItem", "Price", "Price");

        [DatabaseColumn]
        public static SqlColumn CreatedDate { get; } = new("OrderItem", "CreatedDate", "CreatedDate");
    }
}

namespace BinnsORM.Objects.TableData
{
    public class OrderItemData : BinnsORMDataCollection
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public int ItemId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedDate { get; set; }

        public OrderItemData()
        {
            CreatedDate = DateTime.UtcNow;
        }
    }
}
