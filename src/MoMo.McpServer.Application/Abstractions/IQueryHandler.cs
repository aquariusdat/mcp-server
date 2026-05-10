namespace MoMo.McpServer.Application.Abstractions;

/// <summary>
/// Contract for query handlers. Implement this for every query.
/// </summary>
public interface IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
