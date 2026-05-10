using MoMo.McpServer.Application.Interfaces;
using MoMo.McpServer.Domain.Entities;

namespace MoMo.McpServer.Application.Runtime;

/// <summary>
/// Reads enabled tool definitions from the repository and exposes them for MCP runtime discovery.
/// This is the Application layer's bridge between storage and the MCP runtime layer.
/// The runtime layer (Api/Runtime/) depends on this interface, not the raw repository.
/// </summary>
public sealed class ToolCapabilityProvider(IToolRepository repository) : IToolCapabilityProvider
{
    public async Task<IReadOnlyList<McpToolDefinition>> GetEnabledToolsAsync(CancellationToken cancellationToken = default)
    {
        var all = await repository.GetAllAsync(cancellationToken);
        return all.Where(t => t.Enabled).ToList();
    }

    public async Task<McpToolDefinition?> FindEnabledToolAsync(string code, CancellationToken cancellationToken = default)
    {
        var tool = await repository.GetByCodeAsync(code, cancellationToken);
        return tool is { Enabled: true } ? tool : null;
    }
}
