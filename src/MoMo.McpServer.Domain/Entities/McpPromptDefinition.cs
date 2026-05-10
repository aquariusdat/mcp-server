namespace MoMo.McpServer.Domain.Entities;

/// <summary>
/// Represents a stored MCP prompt definition.
/// Prompts are reusable message templates that MCP clients can retrieve and render.
/// </summary>
public sealed class McpPromptDefinition : BaseDefinition
{
    /// <summary>
    /// The prompt template string. May contain named argument placeholders.
    /// Example: "Summarize the customer {{customerId}} account status."
    /// </summary>
    public string Template { get; set; } = string.Empty;

    /// <summary>
    /// Declared arguments for this prompt.
    /// </summary>
    public List<PromptArgumentDefinition> Arguments { get; set; } = [];
}

/// <summary>
/// Describes a single argument accepted by a prompt template.
/// </summary>
public sealed class PromptArgumentDefinition
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool Required { get; set; } = false;
}
