using MoMo.McpServer.Domain.Entities;

namespace MoMo.McpServer.Application.Runtime;

/// <summary>
/// Provides access to the enabled prompt definitions for MCP runtime discovery.
/// Used by the MCP runtime layer to enumerate available prompts.
/// </summary>
public interface IPromptCapabilityProvider
{
    /// <summary>Returns all enabled prompt definitions for MCP client discovery.</summary>
    Task<IReadOnlyList<McpPromptDefinition>> GetEnabledPromptsAsync(CancellationToken cancellationToken = default);

    /// <summary>Returns a specific prompt definition by its code, or null if not found/disabled.</summary>
    Task<McpPromptDefinition?> FindEnabledPromptAsync(string code, CancellationToken cancellationToken = default);
}
