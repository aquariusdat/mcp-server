using MoMo.McpServer.Application.Interfaces;
using MoMo.McpServer.Domain.Entities;

namespace MoMo.McpServer.Application.Runtime;

/// <summary>
/// Reads enabled resource definitions from the repository for MCP runtime discovery.
/// </summary>
public sealed class ResourceCapabilityProvider(IResourceRepository repository) : IResourceCapabilityProvider
{
    public async Task<IReadOnlyList<McpResourceDefinition>> GetEnabledResourcesAsync(CancellationToken cancellationToken = default)
    {
        var all = await repository.GetAllAsync(cancellationToken);
        return all.Where(r => r.Enabled).ToList();
    }

    public async Task<McpResourceDefinition?> FindEnabledResourceAsync(string uri, CancellationToken cancellationToken = default)
    {
        var all = await repository.GetAllAsync(cancellationToken);
        return all.FirstOrDefault(r => r.Enabled && string.Equals(r.Uri, uri, StringComparison.OrdinalIgnoreCase));
    }
}
