namespace MoMo.McpServer.Contract.Prompts;

public sealed class UpdatePromptRequest
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public List<string> Tags { get; init; } = [];
    public string Template { get; init; } = string.Empty;
    public List<PromptArgumentRequest> Arguments { get; init; } = [];
}
