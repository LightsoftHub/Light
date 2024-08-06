using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Light.AspNetCore.Builder;

public static class JsonConfigurationLocation
{
    /// <summary>
    /// Add configuration files *.json from folder
    /// </summary>
    public static WebApplicationBuilder LoadJsonConfigurations(this WebApplicationBuilder builder, string[]? paths = null)
    {
        var configuration = builder.Configuration;

        // If not configure paths
        //      get default configuration paths in "JsonConfigurationPaths" section
        paths ??= builder.Configuration.GetSection("JsonConfigurationPaths").Get<string[]>();

        if (paths is null)
        {
            return builder; // use default config
        }

        var env = builder.Environment.EnvironmentName;

        // combine paths to string
        var path = Path.Combine(paths);

        // get directory info
        var dInfo = new DirectoryInfo(path);
        if (dInfo.Exists)
        {
            var files = dInfo.GetFiles("*.json").Where(x => !x.Name.Contains("appsettings"));

            foreach (var file in files)
            {
                configuration
                    .AddJsonFile(Path.Combine(path, file.Name), optional: false, reloadOnChange: true);
            }
        }

        // load after add json files for can override values at root configurations
        configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);

        configuration.AddEnvironmentVariables();

        return builder;
    }
}
