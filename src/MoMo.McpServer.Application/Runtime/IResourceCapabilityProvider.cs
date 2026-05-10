using MoMo.McpServer.Domain.Entities;

namespace MoMo.McpServer.Application.Runtime;

/// <summary>
/// Provides access to the enabled resource definitions for MCP runtime discovery.
/// Used by the MCP runtime layer to enumerate available resources.
/// </summary>
public interface IResourceCapabilityProvider
{
    /// <summary>Returns all enabled resource definitions for MCP client discovery.</summary>
    Task<IReadOnlyList<McpResourceDefinition>> GetEnabledResourcesAsync(CancellationToken cancellationToken = default);

    /// <summary>Returns a specific resource definition by its URI, or null if not found/disabled.</summary>
    Task<McpResourceDefinition?> FindEnabledResourceAsync(string uri, CancellationToken cancellationToken = default);
}
