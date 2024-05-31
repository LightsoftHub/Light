using Light.Contracts;
using System;

namespace Light.Models
{
    public class PagedInfo : IPagedInfo
    {
        public PagedInfo() { }

        protected internal PagedInfo(int page, int pageSize, int count)
        {
            Page = page;
            PageSize = pageSize;
            TotalRecords = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviousPage => Page > 1;

        public bool HasNextPage => Page < TotalPages;
    }
}
