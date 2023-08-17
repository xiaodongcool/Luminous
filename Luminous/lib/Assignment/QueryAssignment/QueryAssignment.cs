using Luminous.HttpContext;
using Microsoft.Extensions.Primitives;

namespace Luminous
{
    public class QueryAssignment : Assignment, IQueryAssignment
    {
        private readonly QueryAssignmentOptions _options;
        private string _queryString;
        private bool _isInit;
        private IQueryCollection _condition;

        public QueryAssignment(QueryAssignmentOptions options, IHttpContexter contextAccessor) : base(contextAccessor)
        {
            _options = options;
        }

        public IQueryCollection Condition
        {
            get
            {
                if (!_isInit || _condition == null)
                {
                    //  暂时为了 unit test
                    var query = NotEmpty(_queryString) ? _queryString : GetValue(_options.Condition);

                    if (NotEmpty(query))
                    {
                        if (query.StartsWith('?'))
                        {
                            query = query.Substring(1);
                        }

                        if (query.Length > 1)
                        {
                            try
                            {
                                var paramters = query.Split('&').Select(_ => _.Split('=')).ToDictionary(_ => _[0], _ => new StringValues(_[1]));
                                _condition = new QueryCollection(paramters);
                            }
                            catch { }
                        }
                    }

                    _isInit = true;
                }

                if (_condition == null)
                {
                    _condition = new QueryCollection();
                }

                return _condition;
            }
        }

        internal void Set(string queryString)
        {
            _isInit = false;
            _condition = null;
            _queryString = queryString;
        }
    }
}
