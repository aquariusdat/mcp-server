namespace MoMo.McpServer.Contract.Resources;

public sealed class ResourceResponse
{
    public Guid Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public List<string> Tags { get; init; } = [];
    public bool Enabled { get; init; }
    public int Version { get; init; }
    public string Uri { get; init; } = string.Empty;
    public string MimeType { get; init; } = "application/json";
    public string HandlerRoute { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}
