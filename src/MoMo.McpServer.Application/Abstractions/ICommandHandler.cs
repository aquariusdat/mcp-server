namespace MoMo.McpServer.Application.Abstractions;

/// <summary>
/// Contract for command handlers. Implement this for every command.
/// </summary>
public interface ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}
