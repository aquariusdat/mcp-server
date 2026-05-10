using MoMo.McpServer.Application.Interfaces;
using MoMo.McpServer.Domain.Entities;

namespace MoMo.McpServer.Application.Runtime;

/// <summary>
/// Reads enabled prompt definitions from the repository for MCP runtime discovery.
/// </summary>
public sealed class PromptCapabilityProvider(IPromptRepository repository) : IPromptCapabilityProvider
{
    public async Task<IReadOnlyList<McpPromptDefinition>> GetEnabledPromptsAsync(CancellationToken cancellationToken = default)
    {
        var all = await repository.GetAllAsync(cancellationToken);
        return all.Where(p => p.Enabled).ToList();
    }

    public async Task<McpPromptDefinition?> FindEnabledPromptAsync(string code, CancellationToken cancellationToken = default)
    {
        var prompt = await repository.GetByCodeAsync(code, cancellationToken);
        return prompt is { Enabled: true } ? prompt : null;
    }
}
