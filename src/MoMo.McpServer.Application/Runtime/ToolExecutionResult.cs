namespace MoMo.McpServer.Application.Runtime;

/// <summary>
/// The result produced by a tool executor after handling an MCP tool call.
/// </summary>
/// <param name="Content">The response content to return to the MCP client.</param>
/// <param name="IsError">True if the execution encountered an error condition.</param>
public sealed record ToolExecutionResult(
    string Content,
    bool IsError = false
)
{
    /// <summary>Creates a successful result with the given content.</summary>
    public static ToolExecutionResult Success(string content) => new(content);

    /// <summary>Creates an error result with the given message.</summary>
    public static ToolExecutionResult Error(string message) => new(message, IsError: true);
}
