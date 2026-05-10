using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using MoMo.McpServer.Application.Runtime;

namespace MoMo.McpServer.Runtime.Stdio.Providers;

/// <summary>
/// MCP Runtime Resource provider.
/// Exposes enabled resource definitions to MCP clients.
/// Uses IResourceCapabilityProvider — no direct repository access.
/// </summary>
[McpServerToolType]
public sealed class McpResourceProvider(IResourceCapabilityProvider capabilityProvider)
{
    private static readonly JsonSerializerOptions JsonOpts = new() { WriteIndented = true };

    /// <summary>
    /// Lists all enabled MoMo MCP resource definitions with their metadata.
    /// </summary>
    [McpServerTool(Name = "momo_list_resources")]
    [Description("List all enabled MoMo MCP resources with their metadata (for discovery and introspection)")]
    public async Task<string> ListAvailableResourcesAsync(CancellationToken cancellationToken = default)
    {
        var resources = await capabilityProvider.GetEnabledResourcesAsync(cancellationToken);
        var summary = resources.Select(r => new
        {
            r.Code,
            r.Name,
            r.Description,
            r.Uri,
            r.MimeType,
            r.Category,
            r.Tags,
        });
        return JsonSerializer.Serialize(summary, JsonOpts);
    }
}
