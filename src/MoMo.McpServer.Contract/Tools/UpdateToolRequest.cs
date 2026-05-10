namespace MoMo.McpServer.Contract.Tools;

public sealed class UpdateToolRequest
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public List<string> Tags { get; init; } = [];
    public string InputSchema { get; init; } = "{}";
    public string OutputSchema { get; init; } = "{}";
    public string HandlerRoute { get; init; } = string.Empty;
}
