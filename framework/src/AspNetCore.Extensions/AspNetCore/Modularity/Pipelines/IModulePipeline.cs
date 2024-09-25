using Microsoft.AspNetCore.Builder;

namespace Light.AspNetCore.Modularity.Pipelines;

internal interface IModulePipeline
{
    /// <summary>
    /// Configure Module Application Builder
    /// </summary>
    void ConfigurePipelines(IApplicationBuilder builder);

    /// <summary>
    /// For separate job pipelines
    /// </summary>
    void InitializeJobs(IApplicationBuilder builder);
}