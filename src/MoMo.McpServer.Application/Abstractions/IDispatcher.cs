namespace MoMo.McpServer.Application.Abstractions;

/// <summary>
/// Central dispatcher that resolves and invokes the correct command/query handler via DI.
/// This is the lightweight in-house mediator pattern — no external libraries required.
/// </summary>
public interface IDispatcher
{
    Task<TResult> SendCommandAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
    Task<TResult> SendQueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
}
