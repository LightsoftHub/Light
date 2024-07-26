namespace Light.Identity;

public interface ISearchUserRequest : IPage
{
    string? Value { get; }
}
