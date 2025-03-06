using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Light.AspNetCore.Swagger;

public class CustomSwaggerUIOptions(IOptions<SwaggerSettings> options) : IConfigureOptions<SwaggerUIOptions>
{
    private readonly SwaggerSettings _settings = options.Value;

    public void Configure(SwaggerUIOptions options)
    {
        var title = _settings.Title;

        if (!string.IsNullOrEmpty(title))
        {
            // change Tab name in browser
            options.DocumentTitle = title;
        }
    }
}

