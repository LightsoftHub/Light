using Light.Models;

namespace Sample.Models;

public class ApiResult : Result
{
    public string RequestId { get; set; } = Guid.NewGuid().ToString();
}
