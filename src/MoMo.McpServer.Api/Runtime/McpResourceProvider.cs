using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using MoMo.McpServer.Application.Interfaces;

namespace MoMo.McpServer.Api.Runtime;

/// <summary>
/// MCP Runtime Resource provider.
/// Exposes a meta-tool to list available MoMo resources to MCP clients.
/// </summary>
[McpServerToolType]
public sealed class McpResourceProvider(IResourceRepository resourceRepository)
{
    [McpServerTool(Name = "momo_list_resources")]
    [Description("List all available MoMo MCP resources (for discovery and introspection)")]
    public async Task<string> ListAvailableResourcesAsync(CancellationToken cancellationToken = default)
    {
        var resources = await resourceRepository.GetAllAsync(cancellationToken);
        var enabled = resources.Where(r => r.Enabled).Select(r => new
        {
            r.Code,
            r.Name,
            r.Description,
            r.Uri,
            r.MimeType,
            r.Category,
            r.Tags,
        });
        return JsonSerializer.Serialize(enabled, new JsonSerializerOptions { WriteIndented = true });
    }
}
