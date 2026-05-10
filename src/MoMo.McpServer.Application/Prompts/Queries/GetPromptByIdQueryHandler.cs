using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Interfaces;
using MoMo.McpServer.Application.Mapping;
using MoMo.McpServer.Contract.Prompts;

namespace MoMo.McpServer.Application.Prompts.Queries;

public sealed class GetPromptByIdQueryHandler(IPromptRepository repository) : IQueryHandler<GetPromptByIdQuery, PromptResponse?>
{
    public async Task<PromptResponse?> HandleAsync(GetPromptByIdQuery query, CancellationToken cancellationToken = default)
    {
        var prompt = await repository.GetByIdAsync(query.Id, cancellationToken);
        return prompt?.ToResponse();
    }
}
