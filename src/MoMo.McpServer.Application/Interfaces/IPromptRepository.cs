using MoMo.McpServer.Domain.Entities;

namespace MoMo.McpServer.Application.Interfaces;

/// <summary>
/// Repository abstraction for MCP prompt definitions.
/// </summary>
public interface IPromptRepository
{
    Task<IReadOnlyList<McpPromptDefinition>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<McpPromptDefinition?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<McpPromptDefinition?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task SaveAsync(McpPromptDefinition prompt, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
