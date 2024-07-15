using Light.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Models
{
    public class PagedResult<T> : IResult<IEnumerable<T>>
    {
        public PagedResult() { }

        public PagedResult(IEnumerable<T> data, PagedInfo pagedInfo)
        {
            if (data == null)
            {
                Code = ResultCode.NotFound;
            }
            else
            {
                Code = ResultCode.Ok;

                PagedInfo = pagedInfo;

                Data = data;
            }
        }

        public PagedResult(IEnumerable<T> data, int page, int pageSize, int count)
        {
            if (data == null)
            {
                Code = ResultCode.NotFound;
            }
            else
            {
                Code = ResultCode.Ok;

                PagedInfo = new PagedInfo(page, pageSize, count);

                Data = data;
            }
        }

        public PagedResult(IEnumerable<T> data)
        {
            if (data == null)
            {
                Code = ResultCode.NotFound;
            }
            else
            {
                Code = ResultCode.Ok;

                var page = 1;
                var count = data.Count();

                PagedInfo = new PagedInfo(page, count, count);

                Data = data;
            }
        }

        public ResultCode Code { get; set; }

        public bool Succeeded => Code == ResultCode.Ok && Data != null;

        public bool Failed => !Succeeded;

        public string Message { get; set; } = "";

        public IEnumerable<string> Errors { get; set; } = Array.Empty<string>();

        public PagedInfo PagedInfo { get; set; }

        public IEnumerable<T> Data { get; set; } = new List<T>();
    }
}
