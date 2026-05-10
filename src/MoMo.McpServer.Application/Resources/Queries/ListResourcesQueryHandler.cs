using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Interfaces;
using MoMo.McpServer.Application.Mapping;
using MoMo.McpServer.Contract.Resources;

namespace MoMo.McpServer.Application.Resources.Queries;

public sealed class ListResourcesQueryHandler(IResourceRepository repository) : IQueryHandler<ListResourcesQuery, IReadOnlyList<ResourceResponse>>
{
    public async Task<IReadOnlyList<ResourceResponse>> HandleAsync(ListResourcesQuery query, CancellationToken cancellationToken = default)
    {
        var all = await repository.GetAllAsync(cancellationToken);
        var filtered = query.EnabledOnly.HasValue
            ? all.Where(r => r.Enabled == query.EnabledOnly.Value)
            : all;
        return filtered.Select(r => r.ToResponse()).ToList();
    }
}
