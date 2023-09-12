namespace BinnsORM.SQL.Querying
{
    internal class SqlPivot
    {
        public string Expression { get; set; }
        public string SwitchField { get; set; }
        public string[] InValues { get; set; }
        public string Alias { get; set; }

        public SqlPivot() { }

        public virtual void Validate()
        {
            if(string.IsNullOrEmpty(Expression))
            {
                throw new InvalidPivotException("Aggregate expression missing on PIVOT");
            }
            if (string.IsNullOrEmpty(SwitchField))
            {
                throw new InvalidPivotException("FOR not specified on PIVOT");
            }
            if(InValues == null || InValues.Length == 0)
            {
                throw new InvalidPivotException("IN not specified on PIVOT");
            }
            if (string.IsNullOrEmpty(Alias))
            {
                throw new InvalidPivotException("AS not specified on PIVOT");
            }
        }


        public override string ToString()
        {
            string result = $"PIVOT ({Expression} FOR {SwitchField} IN (";
            foreach(string value in InValues)
            {
                result += $"[{value}], ";
            }
            result += result[..^2] + $") ) AS {Alias}";
            return result;
        }
    }

    internal class SqlUnpivot : SqlPivot
    {
        public override void Validate()
        {
            if (string.IsNullOrEmpty(Expression))
            {
                throw new InvalidUnPivotException("Aggregate expression missing on UNPIVOT");
            }
            if (string.IsNullOrEmpty(SwitchField))
            {
                throw new InvalidUnPivotException("FOR not specified on UNPIVOT");
            }
            if (InValues == null || InValues.Length == 0)
            {
                throw new InvalidUnPivotException("IN not specified on UNPIVOT");
            }
            if (string.IsNullOrEmpty(Alias))
            {
                throw new InvalidUnPivotException("AS not specified on UNPIVOT");
            }
        }


        public override string ToString()
        {
            string result = $"UNPIVOT ([{Expression}] FOR {SwitchField} IN (";
            foreach (string value in InValues)
            {
                result += $"[{value}], ";
            }
            result += result[..^2] + $") ) AS {Alias}";
            return result;
        }
    }
}
