using MoMo.McpServer.Domain.Entities;

namespace MoMo.McpServer.Application.Interfaces;

/// <summary>
/// Repository abstraction for MCP resource definitions.
/// </summary>
public interface IResourceRepository
{
    Task<IReadOnlyList<McpResourceDefinition>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<McpResourceDefinition?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<McpResourceDefinition?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task SaveAsync(McpResourceDefinition resource, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
