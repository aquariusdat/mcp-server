using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Interfaces;
using MoMo.McpServer.Application.Mapping;
using MoMo.McpServer.Contract.Prompts;

namespace MoMo.McpServer.Application.Prompts.Queries;

public sealed class ListPromptsQueryHandler(IPromptRepository repository) : IQueryHandler<ListPromptsQuery, IReadOnlyList<PromptResponse>>
{
    public async Task<IReadOnlyList<PromptResponse>> HandleAsync(ListPromptsQuery query, CancellationToken cancellationToken = default)
    {
        var all = await repository.GetAllAsync(cancellationToken);
        var filtered = query.EnabledOnly.HasValue
            ? all.Where(p => p.Enabled == query.EnabledOnly.Value)
            : all;
        return filtered.Select(p => p.ToResponse()).ToList();
    }
}
