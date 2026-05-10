namespace MoMo.McpServer.Contract.Prompts;

public sealed class PromptResponse
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
    public string Template { get; init; } = string.Empty;
    public List<PromptArgumentResponse> Arguments { get; init; } = [];
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}

public sealed class PromptArgumentResponse
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public bool Required { get; init; }
}
