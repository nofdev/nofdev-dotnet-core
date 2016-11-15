namespace Nofdev.Core
{
    /// <summary>
    ///     该类是当查询时用于分页展示的页面导航器
    /// </summary>
    public class Paginator
    {
        private const int DefaultPageSize = 10;
        private const int DefaultFirstPage = 1;
        private int _pageSize = DefaultPageSize;


        /// <summary>
        ///     当前页码
        /// </summary>
        public long Page { get; set; } = DefaultFirstPage;

        /// <summary>
        ///     每页显示多少条记录
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value < 1 ? 1 : _pageSize; }
        }
    }
}