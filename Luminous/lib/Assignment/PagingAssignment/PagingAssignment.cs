using Luminous.HttpContext;

namespace Luminous
{
    /// <summary>
    ///     分页赋值器
    /// </summary>
    public class PagingAssignment : Assignment, IPagingAssignment
    {
        private readonly PagingAssignmentOptions _options;
        private int _pageIndex;
        private int _pageSize;

        public int PageIndex
        {
            get
            {
                if (_pageIndex <= 0)
                {
                    _pageIndex = GetInt(GetValue(_options.PageIndexName), 1);
                }

                return _pageIndex;
            }
        }

        public int PageSize
        {
            get
            {
                if (_pageSize <= 0)
                {
                    _pageSize = GetInt(GetValue(_options.PageSizeName), _options.DefaultPageSize);
                }

                return _pageSize;
            }
        }

        public PagingAssignment(PagingAssignmentOptions options, IHttpContexter contextAccessor) : base(contextAccessor)
        {
            _options = options;
        }

        private int GetInt(string value, int defaults)
        {
            if (int.TryParse(value, out var intValue))
            {
                return intValue;
            }
            else
            {
                return defaults;
            }
        }

        public void Set(int pageSize, int pageIndex)
        {
            _pageSize = pageSize;
            _pageIndex = pageIndex;
        }
    }
}
