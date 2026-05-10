using Microsoft.Extensions.Logging;
using MoMo.McpServer.Application.Runtime;

namespace MoMo.McpServer.Infrastructure.Runtime;

/// <summary>
/// MVP stub implementation of IToolRegistry.
/// Returns null for all tool codes — no real executors are registered yet.
///
/// This is intentional for MVP: the registry seam exists so future implementations can register
/// concrete IToolExecutor classes without changing Application or Api layer code.
///
/// To add a real executor:
///   1. Create a class implementing IToolExecutor (e.g. CrmGetCustomerExecutor)
///   2. Replace or extend this registry to map HandlerRoute → IToolExecutor
///   3. Register the new registry in InfrastructureServiceExtensions
/// </summary>
public sealed class NoOpToolRegistry(ILogger<NoOpToolRegistry> logger) : IToolRegistry
{
    public IToolExecutor? Resolve(string toolCode)
    {
        logger.LogDebug("IToolRegistry.Resolve({ToolCode}): no executor registered (NoOpToolRegistry MVP stub)", toolCode);
        return null;
    }
}
