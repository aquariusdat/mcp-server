namespace MoMo.McpServer.Contract.Resources;

public sealed class UpdateResourceRequest
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public List<string> Tags { get; init; } = [];
    public string Uri { get; init; } = string.Empty;
    public string MimeType { get; init; } = "application/json";
    public string HandlerRoute { get; init; } = string.Empty;
}
