namespace BinnsORM.SQL.Querying
{
    public partial class SqlSelect
    {
        protected SqlSelect SetHints(List<SqlHint> hints)
        {
            Hints = hints;
            return this;
        }


        public SqlSelect NoExpand()
        {
            return NoExpand(true);
        }


        public SqlSelect NoExpand(bool useHint)
        {
            int index = Hints.FindIndex(e => e.HintType == SqlHintType.NOEXPAND);
            if (useHint && index == -1)
            {
                Hints.Add(new(SqlHintType.NOEXPAND));
            }
            if (!useHint && index > -1)
            {
                Hints.RemoveAt(index);
            }
            foreach (var query in ChainedQueries)
            {
                query.NoExpand(useHint);
            }
            return this;
        }


        public SqlSelect ForceScan()
        {
            return ForceScan(true);
        }


        public SqlSelect ForceScan(bool useHint)
        {
            int index = Hints.FindIndex(e => e.HintType == SqlHintType.FORCESCAN);
            if (useHint && index == -1)
            {
                Hints.Add(new(SqlHintType.FORCESCAN));
            }
            if (!useHint && index > -1)
            {
                Hints.RemoveAt(index);
            }
            foreach (var query in ChainedQueries)
            {
                query.ForceScan(useHint);
            }
            return this;
        }


        public SqlSelect HoldLock()
        {
            return HoldLock(true);
        }


        public SqlSelect HoldLock(bool useHint)
        {
            int index = Hints.FindIndex(e => e.HintType == SqlHintType.HOLDLOCK);
            if (useHint && index == -1)
            {
                Hints.Add(new(SqlHintType.HOLDLOCK));
            }
            if (!useHint && index > -1)
            {
                Hints.RemoveAt(index);
            }
            foreach (var query in ChainedQueries)
            {
                query.HoldLock(useHint);
            }
            return this;
        }


        public SqlSelect NoLock()
        {
            return NoLock(true);
        }


        public SqlSelect NoLock(bool useHint)
        {
            int index = Hints.FindIndex(e => e.HintType == SqlHintType.NOLOCK);
            if (useHint && index == -1)
            {
                Hints.Add(new(SqlHintType.NOLOCK));
            }
            if (!useHint && index > -1)
            {
                Hints.RemoveAt(index);
            }
            foreach (var join in TableJoins)
            {
                join.NoLock(useHint);
            }
            foreach (var query in ChainedQueries)
            {
                query.NoLock(useHint);
            }
            return this;
        }


        public SqlSelect NoWait()
        {
            return NoWait(true);
        }


        public SqlSelect NoWait(bool useHint)
        {
            int index = Hints.FindIndex(e => e.HintType == SqlHintType.NOWAIT);
            if (useHint && index == -1)
            {
                Hints.Add(new(SqlHintType.NOWAIT));
            }
            if (!useHint && index > -1)
            {
                Hints.RemoveAt(index);
            }
            foreach (var query in ChainedQueries)
            {
                query.NoWait(useHint);
            }
            return this;
        }


        public SqlSelect PagLock()
        {
            return PagLock(true);
        }


        public SqlSelect PagLock(bool useHint)
        {
            int index = Hints.FindIndex(e => e.HintType == SqlHintType.PAGLOCK);
            if (useHint && index == -1)
            {
                Hints.Add(new(SqlHintType.PAGLOCK));
            }
            if (!useHint && index > -1)
            {
                Hints.RemoveAt(index);
            }
            foreach (var query in ChainedQueries)
            {
                query.PagLock(useHint);
            }
            return this;
        }


        public SqlSelect ReadCommitted()
        {
            return ReadCommitted(true);
        }


        public SqlSelect ReadCommitted(bool useHint)
        {
            int index = Hints.FindIndex(e => e.HintType == SqlHintType.READCOMMITTED);
            if (useHint && index == -1)
            {
                Hints.Add(new(SqlHintType.READCOMMITTED));
            }
            if (!useHint && index > -1)
            {
                Hints.RemoveAt(index);
            }
            foreach (var query in ChainedQueries)
            {
                query.ReadCommitted(useHint);
            }
            return this;
        }


        public SqlSelect ReadCommittedLock()
        {
            return ReadCommittedLock(true);
        }


        public SqlSelect ReadCommittedLock(bool useHint)
        {
            int index = Hints.FindIndex(e => e.HintType == SqlHintType.READCOMMITTEDLOCK);
            if (useHint && index == -1)
            {
                Hints.Add(new(SqlHintType.READCOMMITTEDLOCK));
            }
            if (!useHint && index > -1)
            {
                Hints.RemoveAt(index);
            }
            foreach (var query in ChainedQueries)
            {
                query.ReadCommittedLock(useHint);
            }
            return this;
        }


        public SqlSelect ReadPast()
        {
            return ReadPast(true);
        }


        public SqlSelect ReadPast(bool useHint)
        {
            int index = Hints.FindIndex(e => e.HintType == SqlHintType.READPAST);
            if (useHint && index == -1)
            {
                Hints.Add(new(SqlHintType.READPAST));
            }
            if (!useHint && index > -1)
            {
                Hints.RemoveAt(index);
            }
            foreach (var query in ChainedQueries)
            {
                query.ReadPast(useHint);
            }
            return this;
        }


        public SqlSelect ReadUncommitted()
        {
            return ReadUncommitted(true);
        }


        public SqlSelect ReadUncommitted(bool useHint)
        {
            int index = Hints.FindIndex(e => e.HintType == SqlHintType.READUNCOMMITTED);
            if (useHint && index == -1)
            {
                Hints.Add(new(SqlHintType.READUNCOMMITTED));
            }
            if (!useHint && index > -1)
            {
                Hints.RemoveAt(index);
            }
            foreach (var query in ChainedQueries)
            {
                query.ReadUncommitted(useHint);
            }
            return this;
        }


        public SqlSelect RepeatableRead()
        {
            return RepeatableRead(true);
        }


        public SqlSelect RepeatableRead(bool useHint)
        {
            int index = Hints.FindIndex(e => e.HintType == SqlHintType.REPEATABLEREAD);
            if (useHint && index == -1)
            {
                Hints.Add(new(SqlHintType.REPEATABLEREAD));
            }
            if (!useHint && index > -1)
            {
                Hints.RemoveAt(index);
            }
            foreach (var query in ChainedQueries)
            {
                query.RepeatableRead(useHint);
            }
            return this;
        }


        public SqlSelect RowLock()
        {
            return RowLock(true);
        }


        public SqlSelect RowLock(bool useHint)
        {
            int index = Hints.FindIndex(e => e.HintType == SqlHintType.ROWLOCK);
            if (useHint && index == -1)
            {
                Hints.Add(new(SqlHintType.ROWLOCK));
            }
            if (!useHint && index > -1)
            {
                Hints.RemoveAt(index);
            }
            foreach (var query in ChainedQueries)
            {
                query.RowLock(useHint);
            }
            return this;
        }


        public SqlSelect Serializable()
        {
            return Serializable(true);
        }


        public SqlSelect Serializable(bool useHint)
        {
            int index = Hints.FindIndex(e => e.HintType == SqlHintType.SERIALIZABLE);
            if (useHint && index == -1)
            {
                Hints.Add(new(SqlHintType.SERIALIZABLE));
            }
            if (!useHint && index > -1)
            {
                Hints.RemoveAt(index);
            }
            foreach (var query in ChainedQueries)
            {
                query.Serializable(useHint);
            }
            return this;
        }


        public SqlSelect Snapshot()
        {
            return Snapshot(true);
        }


        public SqlSelect Snapshot(bool useHint)
        {
            int index = Hints.FindIndex(e => e.HintType == SqlHintType.SNAPSHOT);
            if (useHint && index == -1)
            {
                Hints.Add(new(SqlHintType.SNAPSHOT));
            }
            if (!useHint && index > -1)
            {
                Hints.RemoveAt(index);
            }
            foreach (var query in ChainedQueries)
            {
                query.Snapshot(useHint);
            }
            return this;
        }


        public SqlSelect SpatialWindowMaxCells(int maxCells)
        {
            return SpatialWindowMaxCells(maxCells, true);
        }


        public SqlSelect SpatialWindowMaxCells(int maxCells, bool useHint)
        {
            int index = Hints.FindIndex(e => e.HintType == SqlHintType.SPATIAL_WINDOW_MAX_CELLS);
            if (useHint && index == -1)
            {
                Hints.Add(new(SqlHintType.SPATIAL_WINDOW_MAX_CELLS, maxCells));
            }
            if (!useHint && index > -1)
            {
                Hints.RemoveAt(index);
            }
            foreach (var query in ChainedQueries)
            {
                query.SpatialWindowMaxCells(maxCells, useHint);
            }
            return this;
        }


        public SqlSelect TabLock()
        {
            return TabLock(true);
        }


        public SqlSelect TabLock(bool useHint)
        {
            int index = Hints.FindIndex(e => e.HintType == SqlHintType.TABLOCK);
            if (useHint && index == -1)
            {
                Hints.Add(new(SqlHintType.TABLOCK));
            }
            if (!useHint && index > -1)
            {
                Hints.RemoveAt(index);
            }
            foreach (var query in ChainedQueries)
            {
                query.TabLock(useHint);
            }
            return this;
        }


        public SqlSelect TabLockX()
        {
            return TabLockX(true);
        }


        public SqlSelect TabLockX(bool useHint)
        {
            int index = Hints.FindIndex(e => e.HintType == SqlHintType.TABLOCKX);
            if (useHint && index == -1)
            {
                Hints.Add(new(SqlHintType.TABLOCKX));
            }
            if (!useHint && index > -1)
            {
                Hints.RemoveAt(index);
            }
            foreach (var query in ChainedQueries)
            {
                query.TabLockX(useHint);
            }
            return this;
        }


        public SqlSelect UpdLock()
        {
            return UpdLock(true);
        }


        public SqlSelect UpdLock(bool useHint)
        {
            int index = Hints.FindIndex(e => e.HintType == SqlHintType.UPDLOCK);
            if (useHint && index == -1)
            {
                Hints.Add(new(SqlHintType.UPDLOCK));
            }
            if (!useHint && index > -1)
            {
                Hints.RemoveAt(index);
            }
            foreach (var query in ChainedQueries)
            {
                query.UpdLock(useHint);
            }
            return this;
        }


        public SqlSelect XLock()
        {
            return XLock(true);
        }


        public SqlSelect XLock(bool useHint)
        {
            int index = Hints.FindIndex(e => e.HintType == SqlHintType.XLOCK);
            if (useHint && index == -1)
            {
                Hints.Add(new(SqlHintType.XLOCK));
            }
            if (!useHint && index > -1)
            {
                Hints.RemoveAt(index);
            }
            foreach (var query in ChainedQueries)
            {
                query.XLock(useHint);
            }
            return this;
        }
    }
}
