namespace BinnsORM.SQL.Querying
{
    internal enum JoinType
    {
        INNER,
        LEFT,
        RIGHT,
        CROSS,
        FULL
    }

    internal class SqlJoin
    {
        public SqlClause? Clause { get; set; } = null;
        private bool UseNolock { get; set; } = false;
        private string? Alias { get; set; } = null;
        private JoinType JoinType { get; set; }

        private readonly string JoinTable;
        private readonly SqlSelect JoinQuery;


        #region Constructors

        public SqlJoin(JoinType joinType, string table)
        {
            JoinTable = table;
            Initialize(joinType, null);
        }


        public SqlJoin(JoinType joinType, string table, string alias)
        {
            JoinTable = table;
            Initialize(joinType, alias);
        }


        public SqlJoin(JoinType joinType, SqlSelect query, string alias)
        {
            JoinQuery = query;
            Initialize(joinType, alias);
        }


        private void Initialize(JoinType joinType, string alias)
        {
            JoinType = joinType;
            Alias = alias;
        }

        #endregion


        public void NoLock(bool useHint)
        {
            UseNolock = useHint;
            JoinQuery?.NoLock(useHint);
        }


        public override string ToString()
        {
            string result = $"{JoinType} JOIN ";
            if (JoinQuery != null)
            {
                result += $"({JoinQuery}) AS [{Alias}]";
            }
            else
            {
                result += JoinTable;
                if (Alias != null)
                {
                    result += $" AS [{Alias}]";
                }
                if(UseNolock)
                {
                    result += " (NOLOCK)";
                }
            }
            if (JoinType != JoinType.CROSS)
            {
                if(Clause == null)
                {
                    throw new InvalidJoinException(result);
                }
                result += $" ON {Clause}";
            }
            return result;
        }
    }
}
