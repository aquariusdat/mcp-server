namespace MoMo.McpServer.Application.Runtime;

/// <summary>
/// The input context passed to a tool executor when an MCP client invokes a tool.
/// Contains the resolved tool code and the caller-supplied arguments.
/// </summary>
/// <param name="ToolCode">The tool's stable code identifier (e.g. "crm.get_customer").</param>
/// <param name="Arguments">Key-value arguments supplied by the MCP client.</param>
public sealed record ToolExecutionContext(
    string ToolCode,
    IReadOnlyDictionary<string, string?> Arguments
);
