using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using MoMo.McpServer.Application.Runtime;

namespace MoMo.McpServer.Runtime.Stdio.Providers;

/// <summary>
/// MCP Runtime Prompt provider.
/// Exposes enabled prompt definitions to MCP clients.
/// Uses IPromptCapabilityProvider — no direct repository access.
/// </summary>
[McpServerToolType]
public sealed class McpPromptProvider(IPromptCapabilityProvider capabilityProvider)
{
    private static readonly JsonSerializerOptions JsonOpts = new() { WriteIndented = true };

    /// <summary>
    /// Lists all enabled MoMo MCP prompt definitions with their metadata and arguments.
    /// </summary>
    [McpServerTool(Name = "momo_list_prompts")]
    [Description("List all enabled MoMo MCP prompts with their metadata and argument definitions (for discovery and introspection)")]
    public async Task<string> ListAvailablePromptsAsync(CancellationToken cancellationToken = default)
    {
        var prompts = await capabilityProvider.GetEnabledPromptsAsync(cancellationToken);
        var summary = prompts.Select(p => new
        {
            p.Code,
            p.Name,
            p.Description,
            p.Template,
            p.Category,
            p.Tags,
            Arguments = p.Arguments.Select(a => new { a.Name, a.Description, a.Required }),
        });
        return JsonSerializer.Serialize(summary, JsonOpts);
    }
}
