using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MoMo.McpServer.Application.Runtime;

namespace MoMo.McpServer.Infrastructure.Runtime;

/// <summary>
/// A concrete implementation of IToolRegistry that resolves IToolExecutor instances
/// based on a predefined routing dictionary.
///
/// This serves as the Execution Seam dispatch table.
/// </summary>
public sealed class DictionaryToolRegistry(
    IServiceProvider serviceProvider,
    IReadOnlyDictionary<string, Type> routeMap,
    ILogger<DictionaryToolRegistry> logger) : IToolRegistry
{
    public IToolExecutor? Resolve(string toolCode)
    {
        if (routeMap.TryGetValue(toolCode, out var executorType))
        {
            logger.LogDebug("Resolved executor {ExecutorType} for tool code '{ToolCode}'", executorType.Name, toolCode);
            // Must be registered in DI
            return serviceProvider.GetRequiredService(executorType) as IToolExecutor;
        }

        logger.LogWarning("No executor registered for tool code '{ToolCode}'", toolCode);
        return null;
    }
}
