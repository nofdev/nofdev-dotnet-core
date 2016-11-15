using System;
using System.Collections.Generic;

namespace Nofdev.Core
{

/// <summary>
/// 分页器
/// </summary>
/// <typeparam name="T"></typeparam>
   public class PagedList<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public PagedList()
        {

        }

        /// <summary>
        /// 使用自助填写参数
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="list"></param>
        public PagedList(long totalCount, long currentPage, int pageSize, List<T> list)
        {
            this.TotalCount = totalCount;
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
            this.List = list;
        }

        /// <summary>
        /// 使用分页标记参数
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="paginator"></param>
        /// <param name="list"></param>
        public PagedList(long totalCount, Paginator paginator, List<T> list)
        {
            this.TotalCount = totalCount;
            this.CurrentPage = paginator.Page;
            this.PageSize = paginator.PageSize;
            this.List = list;
        }


        /// <summary>
        /// 信息总数
        /// </summary>
        public long TotalCount { get; set; }
        /// <summary>
        ///  当前页数
        /// </summary>
        public long CurrentPage { get; set; }
        /// <summary>
        /// 每页的信息条数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 信息列表
        /// </summary>
        public List<T> List { get; set; }

      

        /// <summary>
        ///  总页数
        /// </summary>
        private long totalPage { get; set; }


        /// <summary>
        /// 获取总页数
        /// </summary>
        /// <returns></returns>
        public long TotalPage()
        {
            if (totalPage == 0)
            {
                if (this.TotalCount <= 0)
                {
                    totalPage = 1;
                }
                else {
                    totalPage = this.TotalCount % this.PageSize > 0 ? (long)Math.Floor(this.TotalCount / (double)this.PageSize) + 1L : (long)Math.Floor(this.TotalCount / (double)this.PageSize);
              }
            }
            return totalPage;
        }

       
    }
}
