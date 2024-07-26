using Light.Contracts;

namespace WebApi.Models;

public class SearchUserRequest : IPage
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? Value { get; set; }
}
