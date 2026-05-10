using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Interfaces;
using MoMo.McpServer.Application.Mapping;
using MoMo.McpServer.Contract.Tools;

namespace MoMo.McpServer.Application.Tools.Queries;

public sealed class ListToolsQueryHandler(IToolRepository repository) : IQueryHandler<ListToolsQuery, IReadOnlyList<ToolResponse>>
{
    public async Task<IReadOnlyList<ToolResponse>> HandleAsync(ListToolsQuery query, CancellationToken cancellationToken = default)
    {
        var all = await repository.GetAllAsync(cancellationToken);

        var filtered = query.EnabledOnly.HasValue
            ? all.Where(t => t.Enabled == query.EnabledOnly.Value)
            : all;

        return filtered.Select(t => t.ToResponse()).ToList();
    }
}
