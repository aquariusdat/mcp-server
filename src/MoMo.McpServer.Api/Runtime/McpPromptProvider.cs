using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using MoMo.McpServer.Application.Interfaces;

namespace MoMo.McpServer.Api.Runtime;

/// <summary>
/// MCP Runtime Prompt provider.
/// Exposes a meta-tool to list available MoMo prompt templates to MCP clients.
/// </summary>
[McpServerToolType]
public sealed class McpPromptProvider(IPromptRepository promptRepository)
{
    [McpServerTool(Name = "momo_list_prompts")]
    [Description("List all available MoMo MCP prompts (for discovery and introspection)")]
    public async Task<string> ListAvailablePromptsAsync(CancellationToken cancellationToken = default)
    {
        var prompts = await promptRepository.GetAllAsync(cancellationToken);
        var enabled = prompts.Where(p => p.Enabled).Select(p => new
        {
            p.Code,
            p.Name,
            p.Description,
            p.Template,
            p.Category,
            p.Tags,
            Arguments = p.Arguments.Select(a => new { a.Name, a.Description, a.Required }),
        });
        return JsonSerializer.Serialize(enabled, new JsonSerializerOptions { WriteIndented = true });
    }
}
