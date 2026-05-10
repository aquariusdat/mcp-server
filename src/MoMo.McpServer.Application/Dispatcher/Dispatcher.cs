using Microsoft.Extensions.DependencyInjection;
using MoMo.McpServer.Application.Abstractions;

namespace MoMo.McpServer.Application.Dispatcher;

/// <summary>
/// DI-based dispatcher that resolves handlers from the service container at runtime.
/// This eliminates the need for any external mediator library (no MediatR).
/// Commands and queries are resolved by their exact handler type.
/// </summary>
public sealed class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
{
    public Task<TResult> SendCommandAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
        var handler = serviceProvider.GetRequiredService(handlerType);

        // Invoke HandleAsync via reflection — minimal overhead, only happens once per request
        var method = handlerType.GetMethod(nameof(ICommandHandler<ICommand<TResult>, TResult>.HandleAsync))!;
        return (Task<TResult>)method.Invoke(handler, [command, cancellationToken])!;
    }

    public Task<TResult> SendQueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var handler = serviceProvider.GetRequiredService(handlerType);

        var method = handlerType.GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync))!;
        return (Task<TResult>)method.Invoke(handler, [query, cancellationToken])!;
    }
}
