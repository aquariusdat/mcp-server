using MoMo.McpServer.Domain.Entities;

namespace MoMo.McpServer.Application.Interfaces;

/// <summary>
/// Repository abstraction for MCP tool definitions.
/// Application layer depends only on this interface — infrastructure provides the implementation.
/// </summary>
public interface IToolRepository
{
    Task<IReadOnlyList<McpToolDefinition>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<McpToolDefinition?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<McpToolDefinition?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task SaveAsync(McpToolDefinition tool, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
