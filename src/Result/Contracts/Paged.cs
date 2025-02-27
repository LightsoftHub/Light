using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Light.Contracts
{
    public class Paged : IPaged
    {
        [JsonPropertyOrder(-1)]
        public int Page { get; set; }

        [JsonPropertyOrder(-1)]
        public int PageSize { get; set; }

        [JsonPropertyOrder(-1)]
        public int TotalRecords { get; set; }

        [JsonPropertyOrder(-1)]
        public int TotalPages { get; set; }

        [JsonPropertyOrder(-1)]
        public bool HasPreviousPage => Page > 1;

        [JsonPropertyOrder(-1)]
        public bool HasNextPage => Page < TotalPages;
    }

    public class Paged<T> : Paged, IPaged<T>
    {
        public Paged() { }

        public Paged(IEnumerable<T> data, int page, int pageSize, int count)
        {
            Page = page;
            PageSize = pageSize;
            TotalRecords = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Records = data;
        }

        public IEnumerable<T> Records { get; set; }
    }
}