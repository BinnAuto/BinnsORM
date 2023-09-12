namespace BinnsORM.SQL.Querying
{
    public class SqlRawText
    {
        private readonly string Text;

        public SqlRawText(string sqlText)
        {
            Text = sqlText;
        }


        public override string ToString()
        {
            return Text;
        }


        public static implicit operator string?(SqlRawText s)
        {
            return s.Text;
        }
    }
}
