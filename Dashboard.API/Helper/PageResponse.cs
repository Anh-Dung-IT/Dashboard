using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Helper
{
    public class PageResponse
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public string PagePrevious { get; set; }
        public string PageNext { get; set; }

        public PageResponse(int totalPage, int currentPage, int pageSize, int totalCount, string pagePrevious, string pageNext)
        {
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
            this.TotalPage = totalPage;                 // (int)Math.Ceiling(1.0 * totalCount / pageSize);
            this.PagePrevious = pagePrevious;
            this.PageNext = pageNext;
        }
    }
}
