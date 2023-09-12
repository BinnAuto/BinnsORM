namespace BinnsORM.SQL.Querying
{
    public static class SqlAggregates
    {
        public static SqlRawText Count()
        {
            return Count("*");
        }


        public static SqlRawText CountDistinct(string fieldName)
        {
            return Count($"DISTINCT {fieldName}");
        }


        public static SqlRawText Count(string fieldName)
        {
            return new($"COUNT({fieldName})");
        }


        public static SqlRawText Sum()
        {
            return Sum("*");
        }


        public static SqlRawText SumDistinct(string fieldName)
        {
            return Sum($"DISTINCT {fieldName}");
        }


        public static SqlRawText Sum(string fieldName)
        {
            return new($"SUM({fieldName})");
        }


        public static SqlRawText Min()
        {
            return Min("*");
        }


        public static SqlRawText MinDistinct(string fieldName)
        {
            return Min($"DISTINCT {fieldName}");
        }


        public static SqlRawText Min(string fieldName)
        {
            return new($"MIN({fieldName})");
        }


        public static SqlRawText Max()
        {
            return Max("*");
        }


        public static SqlRawText MaxDistinct(string fieldName)
        {
            return Max($"DISTINCT {fieldName}");
        }


        public static SqlRawText Max(string fieldName)
        {
            return new($"MAX({fieldName})");
        }


        public static SqlRawText Avg()
        {
            return Avg("*");
        }


        public static SqlRawText AvgDistinct(string fieldName)
        {
            return Avg($"DISTINCT {fieldName}");
        }


        public static SqlRawText Avg(string fieldName)
        {
            return new($"AVG({fieldName})");
        }


        public static SqlRawText Average()
        {
            return Average("*");
        }


        public static SqlRawText AverageDistinct(string fieldName)
        {
            return Average($"DISTINCT {fieldName}");
        }


        public static SqlRawText Average(string fieldName)
        {
            return new ($"AVG({fieldName})");
        }
    }
}
