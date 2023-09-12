namespace BinnsORM.SQL.Querying
{
    public enum SqlHintType
    {
        NOEXPAND,
        FORCESCAN,
        HOLDLOCK,
        NOLOCK,
        NOWAIT,
        PAGLOCK,
        READCOMMITTED,
        READCOMMITTEDLOCK,
        READPAST,
        READUNCOMMITTED,
        REPEATABLEREAD,
        ROWLOCK,
        SERIALIZABLE,
        SNAPSHOT,
        SPATIAL_WINDOW_MAX_CELLS,
        TABLOCK,
        TABLOCKX,
        UPDLOCK,
        XLOCK
    }

    public class SqlHint
    {
        public readonly SqlHintType HintType;

        public readonly int? Amount = null;

        public SqlHint(SqlHintType hintType)
        {
            HintType = hintType;
        }


        public SqlHint(SqlHintType hintType, int amount)
        {
            HintType = hintType;
            Amount = amount;
        }


        public string ToString()
        {
            string result = HintType.ToString();
            if(Amount.HasValue)
            {
                result += $"={Amount}";
            }
            return result;
        }
    }
}
