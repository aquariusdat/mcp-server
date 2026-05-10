using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoMo.McpServer.Application.Interfaces;
using MoMo.McpServer.Application.Runtime;
using MoMo.McpServer.Infrastructure.Runtime;
using MoMo.McpServer.Infrastructure.Storage;

namespace MoMo.McpServer.Infrastructure.DependencyInjection;

/// <summary>
/// Registers all Infrastructure layer services: repositories, storage configuration, and runtime stubs.
/// Call this from the Api project startup.
/// Accepts IConfiguration so the storage path resolution stays inside Infrastructure, not in Program.cs.
/// </summary>
public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration,
        string contentRootPath)
    {
        // ── Storage path resolution ────────────────────────────────────────────
        // Empty/missing BasePath defaults to {ContentRootPath}/data
        var configuredPath = configuration["Storage:BasePath"];
        var basePath = string.IsNullOrWhiteSpace(configuredPath)
            ? Path.Combine(contentRootPath, "data")
            : configuredPath;

        services.Configure<StorageOptions>(opts => opts.BasePath = basePath);

        // ── Repositories ────────────────────────────────────────────────────────
        services.AddScoped<IToolRepository, JsonFileToolRepository>();
        services.AddScoped<IResourceRepository, JsonFileResourceRepository>();
        services.AddScoped<IPromptRepository, JsonFilePromptRepository>();

        // ── Runtime stubs (MVP) ─────────────────────────────────────────────────
        // NoOpToolRegistry returns null for all tool codes until real executors are registered.
        // Replace with a concrete registry implementation when adding real tool execution.
        services.AddSingleton<IToolRegistry, NoOpToolRegistry>();

        return services;
    }
}
