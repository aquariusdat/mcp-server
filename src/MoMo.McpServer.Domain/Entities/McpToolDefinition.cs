using System.Text.Json.Nodes;

namespace MoMo.McpServer.Domain.Entities;

/// <summary>
/// Represents a stored MCP tool definition.
/// Tools are callable functions exposed to MCP clients.
/// </summary>
public sealed class McpToolDefinition : BaseDefinition
{
    /// <summary>
    /// JSON Schema object describing the tool's input parameters.
    /// Must conform to JSON Schema draft-07 as required by the MCP spec.
    /// Stored as a JsonNode to ensure structural validity and clean serialization.
    /// Example: {"type":"object","properties":{"query":{"type":"string"}},"required":["query"]}
    /// </summary>
    public JsonNode? InputSchema { get; set; } = JsonNode.Parse("{}");

    /// <summary>
    /// Optional JSON Schema object describing the tool's output structure.
    /// May be null or an empty object if the output schema is not defined.
    /// </summary>
    public JsonNode? OutputSchema { get; set; } = JsonNode.Parse("{}");

    /// <summary>
    /// Internal handler route key used to dispatch tool invocations.
    /// Maps to a concrete IToolExecutor registered in the runtime layer.
    /// Convention: "{domain}.{action}" e.g. "crm.get_customer"
    /// </summary>
    public string HandlerRoute { get; set; } = string.Empty;
}
