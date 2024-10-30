using Microsoft.Extensions.Options;
using ExceptionHandlerOptions = Light.AspNetCore.ExceptionHandlers.ExceptionHandlerOptions;

namespace Sample.TestOption
{
    public class ErrorHandlerOptions : IConfigureOptions<ExceptionHandlerOptions>
    {
        public void Configure(ExceptionHandlerOptions options)
        {
            options.HideUnidentifiedException = false;
        }
    }
}
