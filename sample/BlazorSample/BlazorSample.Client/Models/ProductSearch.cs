using Light.Contracts;

namespace BlazorSample.Client.Models;

public class ProductSearch : IPage
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}
