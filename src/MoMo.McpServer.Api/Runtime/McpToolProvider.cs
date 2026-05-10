using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using MoMo.McpServer.Application.Runtime;

namespace MoMo.McpServer.Api.Runtime;

/// <summary>
/// MCP Runtime Tool provider.
/// Exposes enabled tool definitions to MCP clients via the MCP protocol.
///
/// Responsibilities:
/// - Enumerate enabled tools for tools/list (momo_list_tools meta-tool)
/// - Delegate actual tool execution to IToolRegistry → IToolExecutor
///
/// This class is intentionally thin. All capability data comes from IToolCapabilityProvider.
/// All execution routing goes through IToolRegistry. No direct repository access.
/// </summary>
[McpServerToolType]
public sealed class McpToolProvider(
    IToolCapabilityProvider capabilityProvider,
    IToolRegistry toolRegistry,
    ILogger<McpToolProvider> logger)
{
    private static readonly JsonSerializerOptions JsonOpts = new() { WriteIndented = true };

    /// <summary>
    /// Lists all enabled MoMo MCP tool definitions with their metadata.
    /// Use this for discovery and introspection before calling a specific tool.
    /// </summary>
    [McpServerTool(Name = "momo_list_tools")]
    [Description("List all enabled MoMo MCP tools with their metadata (for discovery and introspection)")]
    public async Task<string> ListAvailableToolsAsync(CancellationToken cancellationToken = default)
    {
        var tools = await capabilityProvider.GetEnabledToolsAsync(cancellationToken);
        var summary = tools.Select(t => new
        {
            t.Code,
            t.Name,
            t.Description,
            t.Category,
            t.Tags,
            t.HandlerRoute,
        });
        return JsonSerializer.Serialize(summary, JsonOpts);
    }

    /// <summary>
    /// Invokes a registered MoMo tool by its code.
    /// Returns an error message if the tool is not found or no executor is registered.
    /// </summary>
    [McpServerTool("momo_invoke_tool", Description = "Invokes a specific CRM tool by its route code (e.g. momo.crm.get_team_members).")]
    public async Task<string> InvokeToolAsync(
        [Description("The HandlerRoute of the tool to invoke")] string toolRouteCode,
        [Description("The JSON arguments required by the tool's input schema")] JsonNode arguments,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("==================================================");
        logger.LogInformation("🤖 CLAUDE INVOKED TOOL: {ToolRoute}", toolRouteCode);
        logger.LogInformation("📦 ARGUMENTS: {Args}", arguments.ToJsonString());
        
        var definitions = await capabilityProvider.GetEnabledCapabilitiesAsync(cancellationToken);
        var targetTool = definitions.FirstOrDefault(t => 
            string.Equals(t.HandlerRoute, toolRouteCode, StringComparison.OrdinalIgnoreCase));

        if (targetTool is null)
        {
            logger.LogWarning("❌ Tool invocation failed: Route '{ToolRoute}' not found or disabled.", toolRouteCode);
            return $"Error: Tool with route '{toolRouteCode}' not found or not enabled.";
        }

        var executor = toolRegistry.Resolve(targetTool.HandlerRoute);
        if (executor is null)
        {
            logger.LogWarning("❌ Tool invocation failed: No executor mapped for route '{ToolRoute}'.", targetTool.HandlerRoute);
            return $"Error: Tool '{toolRouteCode}' is enabled but has no concrete executor registered.";
        }

        // Convert JsonNode back to a Dictionary for the executor context
        var dictArgs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if (arguments is JsonObject jsonObj)
        {
            foreach (var kvp in jsonObj)
            {
                dictArgs[kvp.Key] = kvp.Value?.ToString() ?? string.Empty;
            }
        }

        var context = new ToolExecutionContext(targetTool.HandlerRoute, dictArgs);
        
        try
        {
            var result = await executor.ExecuteAsync(context, cancellationToken);
            logger.LogInformation("✅ TOOL EXECUTION SUCCESS. Length: {Length} chars.", result.Data?.Length ?? 0);
            logger.LogInformation("==================================================");
            return result.IsSuccess 
                ? result.Data ?? "Success" 
                : $"Execution Failed: {result.Error}";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ TOOL EXECUTION CRASHED: {ToolRoute}", toolRouteCode);
            logger.LogInformation("==================================================");
            return $"Execution Failed with Exception: {ex.Message}";
        }
    }
}
