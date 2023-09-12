using BinnsORM.Objects;
using System.Data.Common;

namespace BinnsORM.SQL.Querying
{
    internal enum TopType
    {
        Percent,
        Quantity
    }

    public enum SortOrder
    {
        Ascending,
        Descending
    }

    public partial class SqlSelect
    {
        private bool SelectTopPercentage { get; set; } = false;
        private bool SelectDistinct { get; set; } = false;
        private int? SelectTop { get; set; } = null;
        private bool SelectTopWithTies { get; set; } = false;
        private int? OffsetRows { get; set; } = null;
        private int? FetchNextRowsOnly { get; set; } = null;
        private bool SelectAll { get; set; } = false;
        private string? FromTable { get; set; } = null;
        private BuildOnClause BuildOnClause { get; set; } = BuildOnClause.None;
        private List<string> SelectList { get; set; } = new() { "*" };
        private List<string> GroupByList { get; set; } = new();
        private List<string> OrderByList { get; set; } = new();
        private SqlClause? WhereClause { get; set; }
        private SqlClause? HavingClause { get; set; }
        private List<SqlHint> Hints { get; set; } = new();
        private List<SqlJoin> TableJoins { get; set; } = new();
        private List<SqlQueryChain> ChainedQueries { get; set; } = new();
        private SqlPivot? pivot { get; set; } = null;
        private SqlUnpivot? unPivot { get; set; } = null;

        private bool SelectIsStar
        {
            get
            {
                return SelectList.Count == 1 && SelectList[0].EndsWith("*");
            }
        }

        private bool HasNoLock
        {
            get
            {
                return Hints.Any(e => e.HintType == SqlHintType.NOLOCK);
            }
        }

        public static SqlSelect From<T>() where T : BinnsORMTableBase
        {
            return From<T>(null);
        }


        public static SqlSelect From<T>(string? tableAlias) where T : BinnsORMTableBase
        {
            T tableInstance = Activator.CreateInstance<T>();
            return new SqlSelect(tableInstance, tableAlias);
        }


        private SqlSelect(BinnsORMTableBase tableInstance, string? tableAlias)
        {
            if (string.IsNullOrEmpty(tableAlias))
            {
                tableAlias = tableInstance.ObjectAlias;
            }
            FromTable = $"{tableInstance.DatabaseName}.[{tableInstance.SchemaName}].[{tableInstance.ObjectName}] AS [{tableAlias}]";
        }

        #region SELECT

        public SqlSelect Select(params string[] columns)
        {
            if (SelectIsStar)
            {
                SelectList.Clear();
            }
            SelectList.AddRange(columns);
            return this;
        }


        public SqlSelect Select(SqlCase @case)
        {
            if (SelectIsStar)
            {
                SelectList.Clear();
            }
            SelectList.Add(@case.ToString());
            return this;
        }


        public SqlSelect Select<T>() where T : BinnsORMTableBase
        {
            return Select<T>(null);
        }


        public SqlSelect Select<T>(string? alias) where T : BinnsORMTableBase
        {
            T tableInstance = Activator.CreateInstance<T>();
            SelectList.Clear();
            if (string.IsNullOrEmpty(alias))
            {
                alias = tableInstance.ObjectAlias;
            }
            SelectList.Add($"{alias}.*");
            return this;
        }

        #endregion


        #region SELECT Helper Methods

        public SqlSelect ResetSelect()
        {
            SelectList = new() { "*" };
            return this;
        }


        public SqlSelect Top(int amount)
        {
            return Top(amount, TopType.Quantity, false);
        }


        public SqlSelect TopPercent(int amount)
        {
            return Top(amount, TopType.Percent, false);
        }


        public SqlSelect WithTies()
        {
            return WithTies(true);
        }


        public SqlSelect WithTies(bool withTies)
        {
            SelectTopWithTies = withTies;
            return this;
        }


        private SqlSelect Top(int amount, TopType topType, bool withTies)
        {
            if (amount < 0)
            {
                throw new InvalidTopAmountException(amount);
            }
            if (topType == TopType.Percent && amount > 100)
            {
                throw new InvalidTopPercentAmountException(amount);
            }
            SelectTop = amount;
            SelectTopPercentage = (topType == TopType.Percent);
            SelectTopWithTies = withTies;
            return this;
        }


        public SqlSelect Distinct()
        {
            return Distinct(true);
        }


        public SqlSelect Distinct(bool useDistinct)
        {
            SelectDistinct = useDistinct;
            return this;
        }


        public SqlSelect All()
        {
            return All(true);
        }


        public SqlSelect All(bool selectAll)
        {
            SelectAll = selectAll;
            return this;
        }

        #endregion


        #region WHERE methods

        public SqlSelect Where(object value1, SqlComparison comparison)
        {
            SqlPredicate predicate = new(value1, comparison);
            return Where(predicate);
        }


        public SqlSelect Where(object value1, SqlComparison comparison, object value2)
        {
            SqlPredicate predicate = new(value1, comparison, value2);
            return Where(predicate);
        }


        public SqlSelect Where(object value1, SqlComparison comparison, object value2, object value3)
        {
            SqlPredicate predicate = new(value1, comparison, value2, value3);
            return Where(predicate);
        }


        public SqlSelect Where(object value1, SqlComparison comparison, params object[] values)
        {
            SqlPredicate predicate = new(value1, comparison, values);
            return Where(predicate);
        }


        public SqlSelect Where(SqlComparison comparison, SqlSelect query)
        {
            SqlPredicate predicate = new(comparison, query);
            return Where(predicate);
        }


        public SqlSelect Where(SqlPredicate predicate)
        {
            SqlClause clause = new(predicate);
            return Where(clause);
        }


        public SqlSelect Where(SqlClause clause)
        {
            BuildOnClause = BuildOnClause.Where;
            WhereClause ??= new();
            WhereClause.And(clause);
            return this;
        }

        #endregion


        #region JOIN methods

        public SqlSelect InnerJoin<T>() where T : BinnsORMTableBase
        {
            return AddJoin<T>(JoinType.INNER, null);
        }


        public SqlSelect InnerJoin<T>(string alias) where T : BinnsORMTableBase
        {
            return AddJoin<T>(JoinType.INNER, alias);
        }


        public SqlSelect InnerJoin<T>(SqlPredicate on) where T : BinnsORMTableBase
        {
            return AddJoin<T>(JoinType.INNER, null, on);
        }


        public SqlSelect InnerJoin(SqlSelect query, string alias)
        {
            return AddJoin(JoinType.INNER, query, alias, null);
        }


        public SqlSelect InnerJoin(SqlSelect query, string alias, SqlPredicate on)
        {
            return AddJoin(JoinType.INNER, query, alias, on);
        }


        public SqlSelect LeftJoin<T>() where T : BinnsORMTableBase
        {
            return AddJoin<T>(JoinType.LEFT, null);
        }


        public SqlSelect LeftJoin<T>(string alias) where T : BinnsORMTableBase
        {
            return AddJoin<T>(JoinType.LEFT, alias);
        }


        public SqlSelect LeftJoin<T>(SqlPredicate on) where T : BinnsORMTableBase
        {
            return AddJoin<T>(JoinType.LEFT, null, on);
        }


        public SqlSelect LeftJoin(SqlSelect query, string alias)
        {
            return AddJoin(JoinType.LEFT, query, alias, null);
        }


        public SqlSelect LeftJoin(SqlSelect query, string alias, SqlPredicate on)
        {
            return AddJoin(JoinType.LEFT, query, alias, on);
        }


        public SqlSelect RightJoin<T>() where T : BinnsORMTableBase
        {
            return AddJoin<T>(JoinType.RIGHT, null);
        }


        public SqlSelect RightJoin<T>(string alias) where T : BinnsORMTableBase
        {
            return AddJoin<T>(JoinType.RIGHT, alias);
        }


        public SqlSelect RightJoin<T>(SqlPredicate on) where T : BinnsORMTableBase
        {
            return AddJoin<T>(JoinType.RIGHT, null, on);
        }


        public SqlSelect RightJoin(SqlSelect query, string alias)
        {
            return AddJoin(JoinType.RIGHT, query, alias, null);
        }


        public SqlSelect RightJoin(SqlSelect query, string alias, SqlPredicate on)
        {
            return AddJoin(JoinType.RIGHT, query, alias, on);
        }


        public SqlSelect FullJoin<T>() where T : BinnsORMTableBase
        {
            return AddJoin<T>(JoinType.FULL, null);
        }


        public SqlSelect FullJoin<T>(string alias) where T : BinnsORMTableBase
        {
            return AddJoin<T>(JoinType.FULL, alias);
        }


        public SqlSelect FullJoin<T>(SqlPredicate on) where T : BinnsORMTableBase
        {
            return AddJoin<T>(JoinType.FULL, null, on);
        }


        public SqlSelect FullJoin(SqlSelect query, string alias)
        {
            return AddJoin(JoinType.FULL, query, alias, null);
        }


        public SqlSelect FullJoin(SqlSelect query, string alias, SqlPredicate on)
        {
            return AddJoin(JoinType.FULL, query, alias, on);
        }


        public SqlSelect CrossJoin<T>() where T : BinnsORMTableBase
        {
            return AddJoin<T>(JoinType.CROSS, null);
        }


        public SqlSelect CrossJoin<T>(string alias) where T : BinnsORMTableBase
        {
            return AddJoin<T>(JoinType.CROSS, alias);
        }


        public SqlSelect CrossJoin(SqlSelect query, string alias)
        {
            return AddJoin(JoinType.CROSS, query, alias, null);
        }


        private SqlSelect AddJoin<T>(JoinType joinType, string? alias) where T : BinnsORMTableBase
        {
            return AddJoin<T>(joinType, alias, null);
        }


        private SqlSelect AddJoin<T>(JoinType joinType, string? alias, SqlPredicate? on) where T : BinnsORMTableBase
        {
            T tableInstance = Activator.CreateInstance<T>();
            string table = $"{tableInstance.DatabaseName}.[{tableInstance.SchemaName}].[{tableInstance.ObjectName}]";
            if (string.IsNullOrEmpty(alias))
            {
                alias = tableInstance.ObjectAlias;
            }
            return AddJoin(joinType, table, alias, on);
        }


        private SqlSelect AddJoin(JoinType joinType, SqlSelect query, string alias, SqlPredicate? on)
        {
            query.SetHints(Hints);
            SqlJoin join = new(joinType, query, alias);
            join.NoLock(HasNoLock);
            TableJoins.Add(join);
            if(on != null)
            {
                On(on.Value);
            }
            return this;
        }


        private SqlSelect AddJoin(JoinType joinType, string table, string alias, SqlPredicate? on)
        {
            SqlJoin join = new(joinType, table, alias);
            join.NoLock(HasNoLock);
            TableJoins.Add(join);
            if (on != null)
            {
                On(on.Value);
            }
            return this;
        }

        #endregion


        #region ON methods

        public SqlSelect On(object value1, SqlComparison comparison)
        {
            SqlPredicate predicate = new(value1, comparison);
            return On(predicate);
        }


        public SqlSelect On(object value1, SqlComparison comparison, object value2)
        {
            SqlPredicate predicate = new(value1, comparison, value2);
            return On(predicate);
        }


        public SqlSelect On(object value1, SqlComparison comparison, object value2, object value3)
        {
            SqlPredicate predicate = new(value1, comparison, value2, value3);
            return On(predicate);
        }


        public SqlSelect On(object value1, SqlComparison comparison, params object[] values)
        {
            SqlPredicate predicate = new(value1, comparison, values);
            return On(predicate);
        }


        public SqlSelect On(SqlComparison comparison, SqlSelect query)
        {
            SqlPredicate predicate = new(comparison, query);
            return On(predicate);
        }


        public SqlSelect On(SqlPredicate predicate)
        {
            SqlClause clause = new(predicate);
            return On(clause);
        }


        public SqlSelect On(SqlClause clause)
        {
            BuildOnClause = BuildOnClause.Join;
            if (TableJoins[^1].Clause == null)
            {
                TableJoins[^1].Clause = new();
            }
            TableJoins[^1].Clause.And(clause);
            return this;
        }

        #endregion


        #region GROUP BY and HAVING methods

        public SqlSelect GroupBy(params string[] columns)
        {
            GroupByList.AddRange(columns);
            return this;
        }


        public SqlSelect Having(object value1, SqlComparison comparison)
        {
            SqlPredicate predicate = new(value1, comparison);
            return Having(predicate);
        }


        public SqlSelect Having(object value1, SqlComparison comparison, object value2)
        {
            SqlPredicate predicate = new(value1, comparison, value2);
            return Having(predicate);
        }


        public SqlSelect Having(object value1, SqlComparison comparison, object value2, object value3)
        {
            SqlPredicate predicate = new(value1, comparison, value2, value3);
            return Having(predicate);
        }


        public SqlSelect Having(object value1, SqlComparison comparison, params object[] values)
        {
            SqlPredicate predicate = new(value1, comparison, values);
            return Having(predicate);
        }


        public SqlSelect Having(SqlComparison comparison, SqlSelect query)
        {
            SqlPredicate predicate = new(comparison, query);
            return Having(predicate);
        }


        public SqlSelect Having(SqlPredicate predicate)
        {
            SqlClause clause = new(predicate);
            return Having(clause);
        }


        public SqlSelect Having(SqlClause clause)
        {
            BuildOnClause = BuildOnClause.Having;
            HavingClause ??= new();
            HavingClause.And(clause);
            return this;
        }

        #endregion


        #region ORDER BY methods

        public SqlSelect OrderBy(string columnName)
        {
            return OrderBy(columnName, SortOrder.Ascending);
        }


        public SqlSelect OrderByDescending(string columnName)
        {
            return OrderBy(columnName, SortOrder.Descending);
        }


        public SqlSelect OrderBy(params string[] columnNames)
        {
            OrderByList.AddRange(columnNames);
            return this;
        }


        public SqlSelect OrderBy(string columnName, SortOrder sortOrder)
        {
            return AddOrderBy(columnName, sortOrder);
        }


        public SqlSelect OrderBy(int columnIndex)
        {
            return OrderBy(columnIndex, SortOrder.Ascending);
        }


        public SqlSelect OrderByDescending(int columnIndex)
        {
            return OrderBy(columnIndex, SortOrder.Descending);
        }


        public SqlSelect OrderBy(params int[] columnIndices)
        {
            foreach(int i in columnIndices)
            {
                AddOrderBy(i.ToString(), SortOrder.Ascending);
            }
            return this;
        }


        public SqlSelect OrderBy(int columnIndex, SortOrder sortOrder)
        {
            return AddOrderBy(columnIndex.ToSqlString(), sortOrder);
        }


        public SqlSelect ResetOrderBy()
        {
            OrderByList.Clear();
            return this;
        }


        private SqlSelect AddOrderBy(string name, SortOrder sortOrder)
        {
            if (sortOrder == SortOrder.Descending)
            {
                name += $" DESC";
            }
            OrderByList.Add(name);
            return this;
        }

        #endregion


        #region AND methods

        public SqlSelect And(object value1, SqlComparison comparison)
        {
            SqlPredicate predicate = new(value1, comparison);
            return And(predicate);
        }


        public SqlSelect And(object value1, SqlComparison comparison, object value2)
        {
            SqlPredicate predicate = new(value1, comparison, value2);
            return And(predicate);
        }


        public SqlSelect And(object value1, SqlComparison comparison, object value2, object value3)
        {
            SqlPredicate predicate = new(value1, comparison, value2, value3);
            return And(predicate);
        }


        public SqlSelect And(object value1, SqlComparison comparison, params object[] values)
        {
            SqlPredicate predicate = new(value1, comparison, values);
            return And(predicate);
        }


        public SqlSelect And(SqlComparison comparison, SqlSelect query)
        {
            SqlPredicate predicate = new(comparison, query);
            return And(predicate);
        }


        public SqlSelect And(SqlPredicate predicate)
        {
            SqlClause condition = new(predicate);
            return And(condition);
        }


        public SqlSelect And(SqlClause clause)
        {
            switch (BuildOnClause)
            {
                case BuildOnClause.Where:
                    WhereClause.And(clause);
                    return this;

                case BuildOnClause.Join:
                    TableJoins[^1].Clause.And(clause);
                    return this;

                case BuildOnClause.Having:
                    HavingClause.And(clause);
                    return this;

                default:
                    throw new InvalidOperationException();
            }
        }

        #endregion


        #region OR methods

        public SqlSelect Or(object value1, SqlComparison comparison)
        {
            SqlPredicate predicate = new(value1, comparison);
            return Or(predicate);
        }


        public SqlSelect Or(object value1, SqlComparison comparison, object value2)
        {
            SqlPredicate predicate = new(value1, comparison, value2);
            return Or(predicate);
        }


        public SqlSelect Or(object value1, SqlComparison comparison, object value2, object value3)
        {
            SqlPredicate predicate = new(value1, comparison, value2, value3);
            return Or(predicate);
        }


        public SqlSelect Or(object value1, SqlComparison comparison, params object[] values)
        {
            SqlPredicate predicate = new(value1, comparison, values);
            return Or(predicate);
        }


        public SqlSelect Or(SqlComparison comparison, SqlSelect query)
        {
            SqlPredicate predicate = new(comparison, query);
            return Or(predicate);
        }


        public SqlSelect Or(SqlPredicate predicate)
        {
            SqlClause condition = new(predicate);
            return Or(condition);
        }


        public SqlSelect Or(SqlClause clause)
        {
            switch (BuildOnClause)
            {
                case BuildOnClause.Where:
                    WhereClause.Or(clause);
                    return this;

                case BuildOnClause.Join:
                    TableJoins[^1].Clause.Or(clause);
                    return this;

                case BuildOnClause.Having:
                    HavingClause.Or(clause);
                    return this;

                default:
                    throw new InvalidOperationException();
            }
        }

        #endregion


        #region Query Chaining methods

        public SqlSelect Union(SqlSelect query)
        {
            return AddQueryToChain(query, QueryChainType.UNION);
        }


        public SqlSelect UnionAll(SqlSelect query)
        {
            return AddQueryToChain(query, QueryChainType.UNION_ALL);
        }


        public SqlSelect Except(SqlSelect query)
        {
            return AddQueryToChain(query, QueryChainType.EXCEPT);
        }


        public SqlSelect Intersect(SqlSelect query)
        {
            return AddQueryToChain(query, QueryChainType.INTERSECT);
        }


        public SqlSelect IntersectAll(SqlSelect query)
        {
            return AddQueryToChain(query, QueryChainType.INTERSECT_ALL);
        }


        private SqlSelect AddQueryToChain(SqlSelect query, QueryChainType queryChainType)
        {
            query.SetHints(Hints);
            SqlQueryChain chainLink = new(query, queryChainType);
            ChainedQueries.Add(chainLink);
            return this;
        }

        #endregion


        #region OFFSET and FETCH methods

        public SqlSelect Offset(int? amount)
        {
            OffsetRows = amount;
            return this;
        }


        public SqlSelect FetchNextOnly(int? amount)
        {
            OffsetRows = OffsetRows.GetValueOrDefault(0);
            FetchNextRowsOnly = amount;
            return this;
        }

        #endregion


        #region PIVOT and UNPIVOT methods

        public SqlSelect Pivot(string expression, string alias)
        {
            if(unPivot != null)
            {
                throw new InvalidPivotException("PIVOT cannot be specified when UNPIVOT is specified");
            }
            pivot = new()
            {
                Expression = expression,
                Alias = alias
            };
            return this;
        }


        public SqlSelect Pivot(string expression, string column, string alias, params string[] values)
        {
            if (unPivot != null)
            {
                throw new InvalidPivotException("PIVOT cannot be specified when UNPIVOT is specified");
            }
            pivot = new()
            {
                Expression = expression,
                SwitchField = column,
                InValues = values,
                Alias = alias
            };
            return this;
        }


        public SqlSelect UnPivot(string expression, string alias)
        {
            if (pivot != null)
            {
                throw new InvalidUnPivotException("UNPIVOT cannot be specified when PIVOT is specified");
            }
            unPivot = new()
            {
                Expression = expression,
                Alias = alias
            };
            return this;
        }


        public SqlSelect UnPivot(string expression, string column, string alias, params string[] values)
        {
            if (pivot != null)
            {
                throw new InvalidUnPivotException("UNPIVOT cannot be specified when PIVOT is specified");
            }
            unPivot = new()
            {
                Expression = expression,
                SwitchField = column,
                InValues = values,
                Alias = alias
            };
            return this;
        }


        public SqlSelect For(string column)
        {
            if(pivot != null)
            {
                pivot.SwitchField = column;
            }
            if(unPivot != null)
            {
                unPivot.SwitchField = column;
            }
            return this;
        }


        public SqlSelect In(params string[] values)
        {
            if (pivot != null)
            {
                pivot.InValues = values;
            }
            if (unPivot != null)
            {
                unPivot.InValues = values;
            }
            return this;
        }

        #endregion


        public override string ToString()
        {
            string result = "SELECT ";
            if(SelectTop.HasValue)
            {
                result += $"TOP {SelectTop} ";
                if (SelectTopPercentage)
                {
                    result += "PERCENT ";
                }
                if (SelectTopWithTies)
                {
                    result += "WITH TIES ";
                }
            }
            if (SelectAll)
            {
                result += "ALL ";
            }
            if (SelectDistinct)
            {
                result += "DISTINCT ";
            }
            result += SelectList[0];
            for (int i = 1; i < SelectList.Count; i++)
            {
                result += $", {SelectList[i]}";
            }

            result += $" FROM {FromTable} ";

            if(Hints.Count > 0)
            {
                result += "WITH (";
                foreach(var h in Hints)
                {
                    result += $"{h.ToString()}, ";
                }
                result = result[..^2] + ") ";
            }

            if (TableJoins.Count > 0)
            {
                foreach (var tableJoin in TableJoins)
                {
                    result += $"{tableJoin} ";
                }
            }

            if(pivot != null)
            {
                pivot.Validate();
                result += $"{pivot} ";
            }

            if (unPivot != null)
            {
                unPivot.Validate();
                result += $"{unPivot} ";
            }

            if (WhereClause != null)
            {
                result += $"WHERE {WhereClause} ";
            }

            if (GroupByList.Count > 0)
            {
                result += $"GROUP BY {GroupByList[0]}";
                for (int i = 1; i < GroupByList.Count; i++)
                {
                    result += $", {GroupByList[i]}";
                }

                if (HavingClause != null)
                {
                    result += $" HAVING {HavingClause} ";
                }
            }

            if (OrderByList.Count > 0)
            {
                result += $"ORDER BY {OrderByList[0]}";
                for (int i = 1; i < OrderByList.Count; i++)
                {
                    result += $", {OrderByList[i]}";
                }
                result += " ";

                if (OffsetRows.HasValue)
                {
                    result += $"OFFSET {OffsetRows} ROWS ";
                    if (FetchNextRowsOnly.HasValue)
                    {
                        result += $"FETCH NEXT {FetchNextRowsOnly} ROWS ONLY ";
                    }
                }
            }

            if (ChainedQueries.Count > 0)
            {
                string chainString;
                foreach (var link in ChainedQueries)
                {
                    chainString = link.ChainType.ToString().Replace("_", " ");
                    result += $"{chainString} ({link.Query})";
                }
            }

            result = result.Trim();
            return result;
        }
    }
}
