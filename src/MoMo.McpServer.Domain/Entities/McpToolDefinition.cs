namespace MoMo.McpServer.Domain.Entities;

/// <summary>
/// Represents a stored MCP tool definition.
/// Tools are callable functions exposed to MCP clients.
/// </summary>
public sealed class McpToolDefinition : BaseDefinition
{
    /// <summary>
    /// JSON Schema string describing the tool's input parameters.
    /// Must conform to JSON Schema draft-07 as required by MCP spec.
    /// Example: {"type":"object","properties":{"query":{"type":"string"}},"required":["query"]}
    /// </summary>
    public string InputSchema { get; set; } = "{}";

    /// <summary>
    /// Optional JSON Schema describing the tool's output structure.
    /// </summary>
    public string OutputSchema { get; set; } = "{}";

    /// <summary>
    /// Internal handler route key used to dispatch tool invocations.
    /// Maps to a concrete handler registered in the application layer.
    /// Example: "crm.get_customer"
    /// </summary>
    public string HandlerRoute { get; set; } = string.Empty;
}
