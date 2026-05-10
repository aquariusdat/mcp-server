namespace MoMo.McpServer.Contract.Prompts;

public sealed class CreatePromptRequest
{
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public List<string> Tags { get; init; } = [];
    public bool Enabled { get; init; } = true;
    public string Template { get; init; } = string.Empty;
    public List<PromptArgumentRequest> Arguments { get; init; } = [];
}

public sealed class PromptArgumentRequest
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public bool Required { get; init; } = false;
}
