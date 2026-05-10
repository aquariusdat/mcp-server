namespace MoMo.McpServer.Application.Abstractions;

/// <summary>
/// Marker interface for all commands (write operations).
/// TResult is the return type (use Unit for void commands).
/// </summary>
public interface ICommand<TResult> { }
