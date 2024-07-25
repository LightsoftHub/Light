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
            if (data == null)
            {
                Code = ResultCode.Error.ToString();
            }
            else
            {
                Code = ResultCode.Ok.ToString();

                PagedInfo = pagedInfo;

                Data = data;
            }
        }

        public PagedResult(IEnumerable<T> data, int page, int pageSize, int count)
        {
            if (data == null)
            {
                Code = ResultCode.Error.ToString();
            }
            else
            {
                Code = ResultCode.Ok.ToString();

                PagedInfo = new PagedInfo(page, pageSize, count);

                Data = data;
            }
        }

        public PagedResult(IEnumerable<T> data)
        {
            if (data == null)
            {
                Code = ResultCode.Error.ToString();
            }
            else
            {
                Code = ResultCode.Ok.ToString();

                var page = 1;
                var count = data.Count();

                PagedInfo = new PagedInfo(page, count, count);

                Data = data;
            }
        }

        public string Code { get; set; }

        public bool Succeeded => Code == ResultCode.Ok.ToString() && Data != null;

        public string Message { get; set; } = "";

        public IEnumerable<string> Errors { get; set; } = Array.Empty<string>();

        public PagedInfo PagedInfo { get; set; }

        public IEnumerable<T> Data { get; set; } = new List<T>();
    }
}
