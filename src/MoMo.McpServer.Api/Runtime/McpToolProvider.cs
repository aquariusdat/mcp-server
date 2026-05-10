using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using MoMo.McpServer.Application.Interfaces;

namespace MoMo.McpServer.Api.Runtime;

/// <summary>
/// MCP Runtime Tool provider.
/// Reads enabled tool definitions from storage and exposes them to MCP clients.
/// This is the bridge between the Management Layer storage and the MCP protocol runtime.
///
/// How it works:
/// - At startup, the MCP SDK discovers this type via WithToolsFromAssembly
/// - When a client calls tools/list, all [McpServerTool] methods are enumerated
/// - When a client calls a tool by name, the matching method is invoked
///
/// To extend: add new tool definitions via the /admin/tools API.
/// No code changes needed to expose new metadata to MCP clients.
/// </summary>
[McpServerToolType]
public sealed class McpToolProvider(IToolRepository toolRepository)
{
    /// <summary>
    /// Lists all enabled MoMo MCP tools with their metadata for introspection.
    /// </summary>
    [McpServerTool(Name = "momo_list_tools")]
    [Description("List all available MoMo MCP tools with metadata (for discovery and introspection)")]
    public async Task<string> ListAvailableToolsAsync(CancellationToken cancellationToken = default)
    {
        var tools = await toolRepository.GetAllAsync(cancellationToken);
        var enabled = tools.Where(t => t.Enabled).Select(t => new
        {
            t.Code,
            t.Name,
            t.Description,
            t.Category,
            t.Tags,
        });
        return JsonSerializer.Serialize(enabled, new JsonSerializerOptions { WriteIndented = true });
    }
}
