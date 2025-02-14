using Serilog;

namespace Light.AspNetCore.Hosting.Middlewares;

internal static class MiddlewareLogger
{
    // https://github.com/serilog/serilog-sinks-file#rolling-policies

    private static Serilog.Core.Logger? requestLogger;

    internal static void Write(string fromIp, string resource, string log)
    {
        requestLogger ??= new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File(@"logs\request-logs.txt",
                shared: true,
                //rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                fileSizeLimitBytes: 104857600)
            .CreateLogger();

        var content = $"\t{fromIp}\t{resource}\r\n{log}\r\n";

        requestLogger.Information(content);
    }
}