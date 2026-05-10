using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Nodes;
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
    IToolRegistry toolRegistry)
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
    [McpServerTool(Name = "momo_invoke_tool")]
    [Description("Invoke a registered MoMo tool by its code with the provided arguments")]
    public async Task<string> InvokeToolAsync(
        [Description("The tool code to invoke (e.g. 'crm.get_customer')")] string toolCode,
        [Description("JSON string of key-value arguments for the tool")] string argumentsJson = "{}",
        CancellationToken cancellationToken = default)
    {
        // Resolve the tool definition
        var tool = await capabilityProvider.FindEnabledToolAsync(toolCode, cancellationToken);
        if (tool is null)
            return JsonSerializer.Serialize(ToolExecutionResult.Error($"Tool '{toolCode}' not found or is disabled."), JsonOpts);

        // Resolve the executor
        var executor = toolRegistry.Resolve(tool.HandlerRoute);
        if (executor is null)
            return JsonSerializer.Serialize(ToolExecutionResult.Error($"Tool '{toolCode}' has no registered executor (HandlerRoute: '{tool.HandlerRoute}'). Register an IToolExecutor implementation."), JsonOpts);

        // Parse arguments
        IReadOnlyDictionary<string, string?> args;
        try
        {
            args = JsonSerializer.Deserialize<Dictionary<string, string?>>(argumentsJson)
                   ?? new Dictionary<string, string?>();
        }
        catch
        {
            return JsonSerializer.Serialize(ToolExecutionResult.Error("argumentsJson is not valid JSON."), JsonOpts);
        }

        var context = new ToolExecutionContext(tool.HandlerRoute, args);
        var result = await executor.ExecuteAsync(context, cancellationToken);
        return JsonSerializer.Serialize(result, JsonOpts);
    }
}
