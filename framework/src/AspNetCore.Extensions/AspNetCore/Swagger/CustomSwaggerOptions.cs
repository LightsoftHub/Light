using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Light.AspNetCore.Swagger
{
    public class CustomSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly SwaggerSettings _settings;

        public CustomSwaggerOptions(IApiVersionDescriptionProvider provider,
            IOptions<SwaggerSettings> options)
            => (_provider, _settings) = (provider, options.Value);

        public void Configure(SwaggerGenOptions options)
        {
            var title = _settings.Title;

            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                var info = new OpenApiInfo()
                {
                    Title = title ?? "API",
                    Version = description.ApiVersion.ToString(),
                };

                if (description.IsDeprecated)
                {
                    info.Description += " This API version has been deprecated.";
                }

                options.SwaggerDoc(description.GroupName, info);
            }

            options.OperationFilter<SwaggerDefaultValues>();
        }
    }
}

