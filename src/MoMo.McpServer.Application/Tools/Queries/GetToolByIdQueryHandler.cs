using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Interfaces;
using MoMo.McpServer.Application.Mapping;
using MoMo.McpServer.Contract.Tools;

namespace MoMo.McpServer.Application.Tools.Queries;

public sealed class GetToolByIdQueryHandler(IToolRepository repository) : IQueryHandler<GetToolByIdQuery, ToolResponse?>
{
    public async Task<ToolResponse?> HandleAsync(GetToolByIdQuery query, CancellationToken cancellationToken = default)
    {
        var tool = await repository.GetByIdAsync(query.Id, cancellationToken);
        return tool?.ToResponse();
    }
}
