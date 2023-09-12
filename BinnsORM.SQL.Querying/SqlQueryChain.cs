namespace BinnsORM.SQL.Querying
{
    internal enum QueryChainType
    {
        UNION_ALL,
        UNION,
        EXCEPT,
        INTERSECT,
        INTERSECT_ALL
    }

    sealed class SqlQueryChain
    {
        public QueryChainType ChainType { get; private set; }

        public SqlSelect Query { get; private set; }


        public SqlQueryChain(SqlSelect query, QueryChainType chainType)
        {
            ChainType = chainType;
            Query = query;
        }


        public void NoExpand(bool useHint)
        {
            Query.NoExpand(useHint);
        }


        public void ForceScan(bool useHint)
        {
            Query.ForceScan(useHint);
        }


        public void HoldLock(bool useHint)
        {
            Query.HoldLock(useHint);
        }


        public void NoLock(bool useHint)
        {
            Query.NoLock(useHint);
        }


        public void NoWait(bool useHint)
        {
            Query.NoWait(useHint);
        }


        public void PagLock(bool useHint)
        {
            Query.PagLock(useHint);
        }


        public void ReadCommitted(bool useHint)
        {
            Query.ReadCommitted(useHint);
        }


        public void ReadCommittedLock(bool useHint)
        {
            Query.ReadCommittedLock(useHint);
        }


        public void ReadPast(bool useHint)
        {
            Query.ReadPast(useHint);
        }


        public void ReadUncommitted(bool useHint)
        {
            Query.ReadUncommitted(useHint);
        }


        public void RepeatableRead(bool useHint)
        {
            Query.RepeatableRead(useHint);
        }


        public void RowLock(bool useHint)
        {
            Query.RowLock(useHint);
        }


        public void Serializable(bool useHint)
        {
            Query.Serializable(useHint);
        }


        public void Snapshot(bool useHint)
        {
            Query.Snapshot(useHint);
        }


        public void SpatialWindowMaxCells(int maxCells, bool useHint)
        {
            Query.SpatialWindowMaxCells(maxCells, useHint);
        }


        public void TabLock(bool useHint)
        {
            Query.TabLock(useHint);
        }


        public void TabLockX(bool useHint)
        {
            Query.TabLockX(useHint);
        }


        public void UpdLock(bool useHint)
        {
            Query.UpdLock(useHint);
        }


        public void XLock(bool useHint)
        {
            Query.XLock(useHint);
        }
    }
}
