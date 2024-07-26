using Light.Identity;

namespace WebApi.Models;

public class SearchUserRequest : ISearchUserRequest
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? Value { get; set; }
}
