using System.Text.Json.Nodes;

namespace MoMo.McpServer.Contract.Tools;

public sealed class UpdateToolRequest
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public List<string> Tags { get; init; } = [];

    /// <summary>JSON Schema object for tool input parameters (MCP draft-07).</summary>
    public JsonNode? InputSchema { get; init; }

    /// <summary>Optional JSON Schema object for tool output structure.</summary>
    public JsonNode? OutputSchema { get; init; }

    public string HandlerRoute { get; init; } = string.Empty;
}
