using MoMo.McpServer.Domain.Entities;

namespace MoMo.McpServer.Application.Runtime;

/// <summary>
/// Provides access to the enabled tool definitions for MCP runtime discovery.
/// Used by the MCP runtime layer to enumerate what tools are available to clients.
/// Isolates the runtime from direct repository access.
/// </summary>
public interface IToolCapabilityProvider
{
    /// <summary>Returns all enabled tool definitions for MCP client discovery.</summary>
    Task<IReadOnlyList<McpToolDefinition>> GetEnabledToolsAsync(CancellationToken cancellationToken = default);

    /// <summary>Returns a specific tool definition by its code, or null if not found/disabled.</summary>
    Task<McpToolDefinition?> FindEnabledToolAsync(string code, CancellationToken cancellationToken = default);
}
