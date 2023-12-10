namespace BinnsORM.SQL.Testing
{
    [TestClass]
    public class SqlSelectTests
    {
        private string Database
        {
            get
            {
                return BinnsORMConfig.GetDatabaseNameOrOverride("BINN");
            }
        }


        [TestMethod]
        public void SelectFromTable()
        {
            var select = SqlSelect.From<Customer>();
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer]";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectFromTableWithAlias()
        {
            var select = SqlSelect.From<Customer>("C");
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [C]";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void Select1FromTable()
        {
            var select = SqlSelect.From<Customer>()
                .Select(1.ToSqlString());
            string result = select.ToString();
            string expected = $"SELECT 1 FROM {Database}.[dbo].[Customer] AS [Customer]";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectInList()
        {
            var select = SqlSelect.From<Customer>()
                .Where(CustomerFields.CustomerId.In(3, 5, 6));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WHERE ([Customer].[CustomerId] IN (3, 5, 6))";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectInArray()
        {
            var select = SqlSelect.From<Customer>()
                .Where(CustomerFields.CustomerId.In(new int[] { 3, 5, 6 }));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WHERE ([Customer].[CustomerId] IN (3, 5, 6))";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectCase()
        {
            var select = SqlSelect.From<Customer>()
                .Select(new SqlCase()
                    .When(CustomerFields.CustomerId.IsEqualTo(10), "A")
                    .When(CustomerFields.CustomerId.Between(11, 100), "B")
                    .Else("X")
                    .As("Type")
                );
            string result = select.ToString();
            string expected = $"SELECT [Type] = CASE WHEN [Customer].[CustomerId] = 10 THEN N'A' WHEN [Customer].[CustomerId] BETWEEN 11 AND 100 THEN N'B' ELSE N'X' END FROM {Database}.[dbo].[Customer] AS [Customer]";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectCaseSwitch()
        {
            var select = SqlSelect.From<Customer>()
                .Select(new SqlCase(CustomerFields.CustomerId)
                    .When(15, "A")
                    .When(34, "B")
                    .Else("X")
                    .As("Type")
                );
            string result = select.ToString();
            string expected = $"SELECT [Type] = CASE [Customer].[CustomerId] WHEN 15 THEN N'A' WHEN 34 THEN N'B' ELSE N'X' END FROM {Database}.[dbo].[Customer] AS [Customer]";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectTopFromTable()
        {
            var select = SqlSelect.From<Customer>()
                .Top(10);
            string result = select.ToString();
            string expected = $"SELECT TOP 10 * FROM {Database}.[dbo].[Customer] AS [Customer]";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectTopPercentFromTable()
        {
            var select = SqlSelect.From<Customer>()
                .TopPercent(33);
            string result = select.ToString();
            string expected = $"SELECT TOP 33 PERCENT * FROM {Database}.[dbo].[Customer] AS [Customer]";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectInvalidTopAmount()
        {
            try
            {
                var select = SqlSelect.From<Customer>()
                    .Top(-1);
                throw new AssertFailedException();
            }
            catch (InvalidTopAmountException)
            {

            }
        }


        [TestMethod]
        public void SelectInvalidTopPercentage()
        {
            try
            {
                var select = SqlSelect.From<Customer>()
                    .TopPercent(133);
                throw new AssertFailedException();
            }
            catch(InvalidTopPercentAmountException)
            {

            }
        }


        [TestMethod]
        public void SelectTopWithTies()
        {
            var select = SqlSelect.From<Customer>()
                .Top(10).WithTies();
            string result = select.ToString();
            string expected = $"SELECT TOP 10 WITH TIES * FROM {Database}.[dbo].[Customer] AS [Customer]";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectTopPercentWithTies()
        {
            var select = SqlSelect.From<Customer>()
                .TopPercent(33).WithTies();
            string result = select.ToString();
            string expected = $"SELECT TOP 33 PERCENT WITH TIES * FROM {Database}.[dbo].[Customer] AS [Customer]";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void ResetSelect()
        {
            var select = SqlSelect.From<Customer>()
                .Select(1.ToSqlString())
                .ResetSelect();
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer]";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectDistinct()
        {
            var select = SqlSelect.From<Customer>().Distinct();
            string result = select.ToString();
            string expected = $"SELECT DISTINCT * FROM {Database}.[dbo].[Customer] AS [Customer]";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectDisableDistinct()
        {
            var select = SqlSelect.From<Customer>().Distinct().Distinct(false);
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer]";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectAll()
        {
            var select = SqlSelect.From<Customer>().All();
            string result = select.ToString();
            string expected = $"SELECT ALL * FROM {Database}.[dbo].[Customer] AS [Customer]";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectDisableAll()
        {
            var select = SqlSelect.From<Customer>().All().All(false);
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer]";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectDistinctAll()
        {
            var select = SqlSelect.From<Customer>().All().Distinct();
            string result = select.ToString();
            string expected = $"SELECT ALL DISTINCT * FROM {Database}.[dbo].[Customer] AS [Customer]";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectWithNoExpand()
        {
            var select = SqlSelect.From<Customer>().NoExpand();
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WITH (NOEXPAND)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectWithForceScan()
        {
            var select = SqlSelect.From<Customer>().ForceScan();
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WITH (FORCESCAN)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectWithHoldLock()
        {
            var select = SqlSelect.From<Customer>().HoldLock();
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WITH (HOLDLOCK)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectWithNolock()
        {
            var select = SqlSelect.From<Customer>().NoLock();
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WITH (NOLOCK)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectWithNoWait()
        {
            var select = SqlSelect.From<Customer>().NoWait();
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WITH (NOWAIT)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectWithPagLock()
        {
            var select = SqlSelect.From<Customer>().PagLock();
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WITH (PAGLOCK)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectWithReadCommitted()
        {
            var select = SqlSelect.From<Customer>().ReadCommitted();
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WITH (READCOMMITTED)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectWithReadCommittedLock()
        {
            var select = SqlSelect.From<Customer>().ReadCommittedLock();
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WITH (READCOMMITTEDLOCK)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectWithReadPast()
        {
            var select = SqlSelect.From<Customer>().ReadPast();
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WITH (READPAST)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectWithReadUncommitted()
        {
            var select = SqlSelect.From<Customer>().ReadUncommitted();
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WITH (READUNCOMMITTED)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectWithRepeatableRead()
        {
            var select = SqlSelect.From<Customer>().RepeatableRead();
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WITH (REPEATABLEREAD)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectWithRowLock()
        {
            var select = SqlSelect.From<Customer>().RowLock();
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WITH (ROWLOCK)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectWithSerializable()
        {
            var select = SqlSelect.From<Customer>().Serializable();
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WITH (SERIALIZABLE)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectWithSnapshot()
        {
            var select = SqlSelect.From<Customer>().Snapshot();
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WITH (SNAPSHOT)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectWithSpatialWindowMaxCells()
        {
            var select = SqlSelect.From<Customer>().SpatialWindowMaxCells(10);
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WITH (SPATIAL_WINDOW_MAX_CELLS=10)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectWithTabLock()
        {
            var select = SqlSelect.From<Customer>().TabLock();
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WITH (TABLOCK)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectWithTabLockX()
        {
            var select = SqlSelect.From<Customer>().TabLockX();
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WITH (TABLOCKX)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectWithUpdLock()
        {
            var select = SqlSelect.From<Customer>().UpdLock();
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WITH (UPDLOCK)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectWithXLock()
        {
            var select = SqlSelect.From<Customer>().XLock();
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WITH (XLOCK)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectWithMultipleHints()
        {
            var select = SqlSelect.From<Customer>().UpdLock().PagLock().SpatialWindowMaxCells(4);
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WITH (UPDLOCK, PAGLOCK, SPATIAL_WINDOW_MAX_CELLS=4)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectInnerJoin()
        {
            var select = SqlSelect.From<Order>()
                .InnerJoin<Customer>()
                    .On(OrderFields.CustomerId.IsEqualTo(CustomerFields.CustomerId));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Order] AS [Order] INNER JOIN {Database}.[dbo].[Customer] AS [Customer] ON ([Order].[CustomerId] = [Customer].[CustomerId])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectInnerJoinWithAlias()
        {
            var select = SqlSelect.From<Order>()
                .InnerJoin<Customer>("C")
                    .On(OrderFields.CustomerId.IsEqualTo(new SqlColumn("C", CustomerFields.CustomerId)));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Order] AS [Order] INNER JOIN {Database}.[dbo].[Customer] AS [C] ON ([Order].[CustomerId] = [C].[CustomerId])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectInnerJoin2()
        {
            var select = SqlSelect.From<Order>()
                .InnerJoin<Customer>(OrderFields.CustomerId.IsEqualTo(CustomerFields.CustomerId));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Order] AS [Order] INNER JOIN {Database}.[dbo].[Customer] AS [Customer] ON ([Order].[CustomerId] = [Customer].[CustomerId])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectInnerJoinWithAlias2()
        {
            var joinSelect = SqlSelect.From<Customer>();
            var select = SqlSelect.From<Customer>()
                .InnerJoin(joinSelect, "T")
                    .On(CustomerFields.CustomerId.NotEqualTo(new SqlColumn("T", CustomerFields.CustomerId)));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] INNER JOIN ({joinSelect}) AS [T] ON ([Customer].[CustomerId] != [T].[CustomerId])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectInnerJoinWithAlias3()
        {
            var joinSelect = SqlSelect.From<Customer>();
            var select = SqlSelect.From<Customer>()
                .InnerJoin(joinSelect, "T", CustomerFields.CustomerId.NotEqualTo(new SqlColumn("T", CustomerFields.CustomerId)));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] INNER JOIN ({joinSelect}) AS [T] ON ([Customer].[CustomerId] != [T].[CustomerId])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectInvalidInnerJoin()
        {
            var select = SqlSelect.From<Order>()
                .InnerJoin<Customer>();
            try
            {
                select.ToString();
                throw new AssertFailedException();
            }
            catch(InvalidJoinException) { }
        }


        [TestMethod]
        public void SelectLeftJoin()
        {
            var select = SqlSelect.From<Order>()
                .LeftJoin<Customer>()
                    .On(OrderFields.CustomerId.IsEqualTo(CustomerFields.CustomerId));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Order] AS [Order] LEFT JOIN {Database}.[dbo].[Customer] AS [Customer] ON ([Order].[CustomerId] = [Customer].[CustomerId])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectLeftJoinWithAlias()
        {
            var select = SqlSelect.From<Order>()
                .LeftJoin<Customer>("C")
                    .On(OrderFields.CustomerId.IsEqualTo(new SqlColumn("C", CustomerFields.CustomerId)));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Order] AS [Order] LEFT JOIN {Database}.[dbo].[Customer] AS [C] ON ([Order].[CustomerId] = [C].[CustomerId])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectLeftJoin2()
        {
            var select = SqlSelect.From<Order>()
                .LeftJoin<Customer>(OrderFields.CustomerId.IsEqualTo(CustomerFields.CustomerId));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Order] AS [Order] LEFT JOIN {Database}.[dbo].[Customer] AS [Customer] ON ([Order].[CustomerId] = [Customer].[CustomerId])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectLeftJoinWithAlias2()
        {
            var joinSelect = SqlSelect.From<Customer>();
            var select = SqlSelect.From<Customer>()
                .LeftJoin(joinSelect, "T")
                    .On(CustomerFields.CustomerId.NotEqualTo(new SqlColumn("T", CustomerFields.CustomerId)));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] LEFT JOIN ({joinSelect}) AS [T] ON ([Customer].[CustomerId] != [T].[CustomerId])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectLeftJoinWithAlias3()
        {
            var joinSelect = SqlSelect.From<Customer>();
            var select = SqlSelect.From<Customer>()
                .LeftJoin(joinSelect, "T", CustomerFields.CustomerId.NotEqualTo(new SqlColumn("T", CustomerFields.CustomerId)));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] LEFT JOIN ({joinSelect}) AS [T] ON ([Customer].[CustomerId] != [T].[CustomerId])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectInvalidLeftJoin()
        {
            var select = SqlSelect.From<Order>()
                .LeftJoin<Customer>();
            try
            {
                select.ToString();
                throw new AssertFailedException();
            }
            catch (InvalidJoinException) { }
        }


        [TestMethod]
        public void SelectRightJoin()
        {
            var select = SqlSelect.From<Order>()
                .RightJoin<Customer>()
                    .On(OrderFields.CustomerId.IsEqualTo(CustomerFields.CustomerId));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Order] AS [Order] RIGHT JOIN {Database}.[dbo].[Customer] AS [Customer] ON ([Order].[CustomerId] = [Customer].[CustomerId])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectRightJoinWithAlias()
        {
            var select = SqlSelect.From<Order>()
                .RightJoin<Customer>("C")
                    .On(OrderFields.CustomerId.IsEqualTo(new SqlColumn("C", CustomerFields.CustomerId)));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Order] AS [Order] RIGHT JOIN {Database}.[dbo].[Customer] AS [C] ON ([Order].[CustomerId] = [C].[CustomerId])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectRightJoin2()
        {
            var select = SqlSelect.From<Order>()
                .RightJoin<Customer>(OrderFields.CustomerId.IsEqualTo(CustomerFields.CustomerId));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Order] AS [Order] RIGHT JOIN {Database}.[dbo].[Customer] AS [Customer] ON ([Order].[CustomerId] = [Customer].[CustomerId])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectRightJoinWithAlias2()
        {
            var joinSelect = SqlSelect.From<Customer>();
            var select = SqlSelect.From<Customer>()
                .RightJoin(joinSelect, "T")
                    .On(CustomerFields.CustomerId.NotEqualTo(new SqlColumn("T", CustomerFields.CustomerId)));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] RIGHT JOIN ({joinSelect}) AS [T] ON ([Customer].[CustomerId] != [T].[CustomerId])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectRightJoinWithAlias3()
        {
            var joinSelect = SqlSelect.From<Customer>();
            var select = SqlSelect.From<Customer>()
                .RightJoin(joinSelect, "T", CustomerFields.CustomerId.NotEqualTo(new SqlColumn("T", CustomerFields.CustomerId)));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] RIGHT JOIN ({joinSelect}) AS [T] ON ([Customer].[CustomerId] != [T].[CustomerId])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectInvalidRightJoin()
        {
            var select = SqlSelect.From<Order>()
                .RightJoin<Customer>();
            try
            {
                select.ToString();
                throw new AssertFailedException();
            }
            catch (InvalidJoinException) { }
        }


        [TestMethod]
        public void SelectFullJoin()
        {
            var select = SqlSelect.From<Order>()
                .FullJoin<Customer>()
                    .On(OrderFields.CustomerId.IsEqualTo(CustomerFields.CustomerId));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Order] AS [Order] FULL JOIN {Database}.[dbo].[Customer] AS [Customer] ON ([Order].[CustomerId] = [Customer].[CustomerId])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectFullJoinWithAlias()
        {
            var select = SqlSelect.From<Order>()
                .FullJoin<Customer>("C")
                    .On(OrderFields.CustomerId.IsEqualTo(new SqlColumn("C", CustomerFields.CustomerId)));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Order] AS [Order] FULL JOIN {Database}.[dbo].[Customer] AS [C] ON ([Order].[CustomerId] = [C].[CustomerId])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectFullJoin2()
        {
            var select = SqlSelect.From<Order>()
                .FullJoin<Customer>(OrderFields.CustomerId.IsEqualTo(CustomerFields.CustomerId));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Order] AS [Order] FULL JOIN {Database}.[dbo].[Customer] AS [Customer] ON ([Order].[CustomerId] = [Customer].[CustomerId])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectFullJoinWithAlias2()
        {
            var joinSelect = SqlSelect.From<Customer>();
            var select = SqlSelect.From<Customer>()
                .FullJoin(joinSelect, "T")
                    .On(CustomerFields.CustomerId.NotEqualTo(new SqlColumn("T", CustomerFields.CustomerId)));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] FULL JOIN ({joinSelect}) AS [T] ON ([Customer].[CustomerId] != [T].[CustomerId])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectFullJoinWithAlias3()
        {
            var joinSelect = SqlSelect.From<Customer>();
            var select = SqlSelect.From<Customer>()
                .FullJoin(joinSelect, "T", CustomerFields.CustomerId.NotEqualTo(new SqlColumn("T", CustomerFields.CustomerId)));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] FULL JOIN ({joinSelect}) AS [T] ON ([Customer].[CustomerId] != [T].[CustomerId])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectInvalidFullJoin()
        {
            var select = SqlSelect.From<Order>()
                .FullJoin<Customer>();
            try
            {
                select.ToString();
                throw new AssertFailedException();
            }
            catch (InvalidJoinException) { }
        }


        [TestMethod]
        public void SelectCrossJoin()
        {
            var select = SqlSelect.From<Customer>()
                .CrossJoin<Order>();
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] CROSS JOIN {Database}.[dbo].[Order] AS [Order]";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectCrossJoinWithAlias()
        {
            var select = SqlSelect.From<Customer>()
                .CrossJoin<Order>("T");
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] CROSS JOIN {Database}.[dbo].[Order] AS [T]";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectCrossJoinWithAlias2()
        {
            var joinSelect = SqlSelect.From<Customer>();
            var select = SqlSelect.From<Customer>()
                .CrossJoin(joinSelect, "T");
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] CROSS JOIN ({joinSelect}) AS [T]";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectGroupBy()
        {
            var select = SqlSelect.From<Customer>()
                .InnerJoin<Order>(OrderFields.CustomerId.IsEqualTo(CustomerFields.CustomerId))
                .Select(CustomerFields.CustomerId,
                    SqlAggregates.Max(OrderFields.CreatedDate)!)
                .GroupBy(CustomerFields.CustomerId);
            string result = select.ToString();
            string expected = $"SELECT [Customer].[CustomerId], MAX([Order].[CreatedDate]) FROM {Database}.[dbo].[Customer] AS [Customer] INNER JOIN {Database}.[dbo].[Order] AS [Order] ON ([Order].[CustomerId] = [Customer].[CustomerId]) GROUP BY [Customer].[CustomerId]";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectGroupByHaving()
        {
            var select = SqlSelect.From<Customer>()
                .InnerJoin<Order>(OrderFields.CustomerId.IsEqualTo(CustomerFields.CustomerId))
                .Select(CustomerFields.CustomerId,
                    SqlAggregates.CountDistinct(OrderFields.OrderId)!)
                .GroupBy(CustomerFields.CustomerId)
                .Having(new SqlRawText($"{SqlAggregates.CountDistinct(OrderFields.OrderId)}").GreaterThan(5));
            string result = select.ToString();
            string expected = $"SELECT [Customer].[CustomerId], COUNT(DISTINCT [Order].[OrderId]) FROM {Database}.[dbo].[Customer] AS [Customer] INNER JOIN {Database}.[dbo].[Order] AS [Order] ON ([Order].[CustomerId] = [Customer].[CustomerId]) GROUP BY [Customer].[CustomerId] HAVING (COUNT(DISTINCT [Order].[OrderId]) > 5)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectWhere()
        {
            var select = SqlSelect.From<Customer>()
                .Where(CustomerFields.CustomerId.GreaterThan(100));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WHERE ([Customer].[CustomerId] > 100)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectOrderBy()
        {
            var select = SqlSelect.From<Customer>()
                .OrderBy(CustomerFields.CustomerName);
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] ORDER BY [Customer].[CustomerName]";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectOrderByDescending()
        {
            var select = SqlSelect.From<Customer>()
                .OrderByDescending(CustomerFields.CustomerName);
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] ORDER BY [Customer].[CustomerName] DESC";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectResetOrderBy()
        {
            var select = SqlSelect.From<Customer>()
                .OrderBy(CustomerFields.CustomerName)
                .ResetOrderBy();
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer]";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectWhereAnd()
        {
            var select = SqlSelect.From<Customer>()
                .Where(CustomerFields.CustomerId.GreaterThan(100))
                .And(CustomerFields.CustomerId.LessThan(1000));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WHERE ([Customer].[CustomerId] > 100) AND ([Customer].[CustomerId] < 1000)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectWhereOr()
        {
            var select = SqlSelect.From<Customer>()
                .Where(CustomerFields.CustomerId.GreaterThan(100))
                .Or(CustomerFields.CustomerId.IsEqualTo(1000));
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] WHERE ([Customer].[CustomerId] > 100) OR ([Customer].[CustomerId] = 1000)";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectUnion()
        {
            var select = SqlSelect.From<Customer>()
                .Union(
                    SqlSelect.From<Customer>()
                );
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] UNION (SELECT * FROM {Database}.[dbo].[Customer] AS [Customer])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectUnionAll()
        {
            var select = SqlSelect.From<Customer>()
                .UnionAll(
                    SqlSelect.From<Customer>()
                );
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] UNION ALL (SELECT * FROM {Database}.[dbo].[Customer] AS [Customer])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectExcept()
        {
            var select = SqlSelect.From<Customer>()
                .Except(
                    SqlSelect.From<Customer>()
                );
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] EXCEPT (SELECT * FROM {Database}.[dbo].[Customer] AS [Customer])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectIntersect()
        {
            var select = SqlSelect.From<Customer>()
                .Intersect(
                    SqlSelect.From<Customer>()
                );
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] INTERSECT (SELECT * FROM {Database}.[dbo].[Customer] AS [Customer])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectIntersectAll()
        {
            var select = SqlSelect.From<Customer>()
                .IntersectAll(
                    SqlSelect.From<Customer>()
                );
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] INTERSECT ALL (SELECT * FROM {Database}.[dbo].[Customer] AS [Customer])";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectOrderByOffset()
        {
            var select = SqlSelect.From<Customer>()
                .OrderBy(CustomerFields.CustomerName)
                .Offset(10);
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] ORDER BY [Customer].[CustomerName] OFFSET 10 ROWS";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectOrderByOffsetFetchNext()
        {
            var select = SqlSelect.From<Customer>()
                .OrderBy(CustomerFields.CustomerName)
                .Offset(10)
                .FetchNextOnly(5);
            string result = select.ToString();
            string expected = $"SELECT * FROM {Database}.[dbo].[Customer] AS [Customer] ORDER BY [Customer].[CustomerName] OFFSET 10 ROWS FETCH NEXT 5 ROWS ONLY";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectSuperCase()
        {
            var time = new DateTime(2000, 10, 4);
            var select = SqlSelect.From<Order>("O")
                .Select(
                    new SqlColumn("O", OrderFields.OrderNumber)
                    , new SqlColumn("O", OrderFields.CreatedDate, "Order Time")
                    , CustomerFields.CustomerName
                    , OrderItemFields.ItemId
                    , new SqlRawText($"[Price] = {OrderItemFields.Quantity} * {OrderItemFields.Price}")!
                )
                    .InnerJoin<Customer>(CustomerFields.CustomerId.IsEqualTo(new SqlColumn("O", OrderFields.CustomerId)))
                    .InnerJoin<OrderItem>(OrderItemFields.OrderId.IsEqualTo(new SqlColumn("O", OrderFields.OrderId)))
                .Where(new SqlColumn("O", OrderFields.CreatedDate).GreaterThan(time));
            string result = select.ToString();
            string expected = $"SELECT [O].[OrderNumber], [Order Time] = [O].[CreatedDate], [Customer].[CustomerName], [OrderItem].[ItemId], [Price] = [OrderItem].[Quantity] * [OrderItem].[Price] " +
                $"FROM {Database}.[dbo].[Order] AS [O] " +
                $"INNER JOIN {Database}.[dbo].[Customer] AS [Customer] ON ([Customer].[CustomerId] = [O].[CustomerId]) " +
                $"INNER JOIN {Database}.[dbo].[OrderItem] AS [OrderItem] ON ([OrderItem].[OrderId] = [O].[OrderId]) " +
                $"WHERE ([O].[CreatedDate] > {time.ToSqlString()})";
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SelectSuperCase2()
        {
            var time = new DateTime(2000, 10, 4);
            var select = SqlSelect.From<Order>("O")
                .Select(
                    new SqlColumn("O", OrderFields.OrderNumber)
                    , new SqlColumn("O", OrderFields.CreatedDate, "Order Time")
                    , CustomerFields.CustomerName
                    , new SqlRawText($"[Total Price] = {SqlAggregates.Sum(OrderItemFields.Quantity + " * " + OrderItemFields.Price)}")!
                )
                    .InnerJoin<Customer>(CustomerFields.CustomerId.IsEqualTo(new SqlColumn("O", OrderFields.CustomerId)))
                    .InnerJoin<OrderItem>(OrderItemFields.OrderId.IsEqualTo(new SqlColumn("O", OrderFields.OrderId)))
                .Where(new SqlColumn("O", OrderFields.CreatedDate).GreaterThan(time))
                .GroupBy(new SqlColumn("O", OrderFields.OrderNumber)
                    , new SqlColumn("O", OrderFields.CreatedDate, "Order Time")
                    , CustomerFields.CustomerName
                );
            string result = select.ToString();
            string expected = $"SELECT [O].[OrderNumber], [Order Time] = [O].[CreatedDate], [Customer].[CustomerName], [Total Price] = SUM([OrderItem].[Quantity] * [OrderItem].[Price]) " +
                $"FROM {Database}.[dbo].[Order] AS [O] " +
                $"INNER JOIN {Database}.[dbo].[Customer] AS [Customer] ON ([Customer].[CustomerId] = [O].[CustomerId]) " +
                $"INNER JOIN {Database}.[dbo].[OrderItem] AS [OrderItem] ON ([OrderItem].[OrderId] = [O].[OrderId]) " +
                $"WHERE ([O].[CreatedDate] > {time.ToSqlString()}) " +
                "GROUP BY [O].[OrderNumber], [Order Time] = [O].[CreatedDate], [Customer].[CustomerName]";
            Assert.AreEqual(expected, result);
        }
    }
}