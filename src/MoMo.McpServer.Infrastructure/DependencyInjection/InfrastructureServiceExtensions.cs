using Microsoft.Extensions.DependencyInjection;
using MoMo.McpServer.Application.Interfaces;
using MoMo.McpServer.Infrastructure.Storage;

namespace MoMo.McpServer.Infrastructure.DependencyInjection;

/// <summary>
/// Registers all Infrastructure layer services: repositories and storage options.
/// Call this from the Api project startup.
/// </summary>
public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IToolRepository, JsonFileToolRepository>();
        services.AddScoped<IResourceRepository, JsonFileResourceRepository>();
        services.AddScoped<IPromptRepository, JsonFilePromptRepository>();

        return services;
    }
}
