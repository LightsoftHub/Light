using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Light.AspNetCore.Swagger
{
    public class TitleFilter(IOptions<SwaggerSettings> options) : IDocumentFilter
    {
        private readonly SwaggerSettings _settings = options.Value;

        public void Apply(OpenApiDocument doc, DocumentFilterContext context)
        {
            doc.Info.Title = _settings.Title;
        }
    }
}
