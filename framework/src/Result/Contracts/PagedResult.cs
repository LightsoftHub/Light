using System.Collections.Generic;

namespace Light.Contracts
{
    public class PagedResult<T> : ResultBase, IResult<Paged<T>>
    {
        public PagedResult() { }

        public PagedResult(IEnumerable<T> data, int page, int pageSize, int count)
        {
            Code = ResultCode.success.ToString();
            Succeeded = true;
            Data = new Paged<T>(data, page, pageSize, count);
        }

        public Paged<T> Data { get; set; }
    }
}
