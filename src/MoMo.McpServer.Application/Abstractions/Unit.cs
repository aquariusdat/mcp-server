namespace MoMo.McpServer.Application.Abstractions;

/// <summary>
/// Represents a void result for commands that do not return a value.
/// Avoids using void in generic constraints.
/// </summary>
public readonly struct Unit
{
    public static readonly Unit Value = new();
}
