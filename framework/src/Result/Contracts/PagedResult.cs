using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Contracts
{
    public class PagedResult<T> : IResult<IEnumerable<T>>
    {
        public PagedResult() { }

        public PagedResult(IEnumerable<T> data, PagedInfo pagedInfo)
        {
            Code = ResultCode.success.ToString();
            Succeeded = true;
            PagedInfo = pagedInfo;
            Data = data;
        }

        public PagedResult(IEnumerable<T> data, int page, int pageSize, int count)
        {
            Code = ResultCode.success.ToString();
            Succeeded = true;
            PagedInfo = new PagedInfo(page, pageSize, count);
            Data = data;
        }

        public PagedResult(IEnumerable<T> data)
        {
            var page = 1;
            var count = data.Count();

            Code = ResultCode.success.ToString();
            Succeeded = true;
            PagedInfo = new PagedInfo(page, count, count);
            Data = data;
        }

        public string Code { get; set; }

        public bool Succeeded { get; set; }

        public string Message { get; set; } = "";

        public string RequestId { get; set; } = Guid.NewGuid().ToString();

        public PagedInfo PagedInfo { get; set; }

        public IEnumerable<T> Data { get; set; } = new List<T>();
    }
}
