using System.Diagnostics;
using System.Text;

namespace LangM.AspNetCore.DbInterface
{
    public class SqlContext
    {
        private readonly StringBuilder _sql;

        private string _sharedDb;
        private string _sharedTb;

        public SqlContext(QueryType queryType)
        {
            QueryType = queryType;
            _sql = new StringBuilder();
            SharedDb = "";
            SharedTb = "";
        }

        public void Append(string sql)
        {
            _sql.Append(sql);
        }

        public void InsertAtStart(string sql)
        {
            _sql.Insert(0, sql);
        }

        public string SharedDb
        {
            get => _sharedDb;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    EnsureStatus = SharedEnsureStatus.Sharding;
                }

                _sharedDb = value;
            }
        }

        public string SharedTb
        {
            get => _sharedTb;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    EnsureStatus = SharedEnsureStatus.Sharding;
                }

                _sharedTb = value;
            }
        }

        public QueryType QueryType { get; }

        public SharedEnsureStatus EnsureStatus { get; set; }

        public string Sql => _sql.ToString();

        public object Parameters { get; set; }

        public override string ToString()
        {
            var db = string.IsNullOrEmpty(SharedDb) ? string.Empty : $"{Environment.NewLine}sharedDb={SharedDb}";
            var tb = string.IsNullOrEmpty(SharedTb) ? string.Empty : $"{Environment.NewLine}sharedTb={SharedTb}";
            var parameters = Parameters == null ? string.Empty : $"{Environment.NewLine}parameters=" + JsonConvert.SerializeObject(Parameters);
            return $"{Sql}{db}{tb}{parameters}";
        }

        public void Logger()
        {
#if DEBUG
            Console.WriteLine(ToString());
            Debug.WriteLine(ToString());
#endif
        }
    }
}
