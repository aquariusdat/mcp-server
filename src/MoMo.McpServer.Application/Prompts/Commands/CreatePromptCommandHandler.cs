using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Interfaces;
using MoMo.McpServer.Application.Mapping;
using MoMo.McpServer.Contract.Prompts;
using MoMo.McpServer.Domain.Entities;

namespace MoMo.McpServer.Application.Prompts.Commands;

public sealed class CreatePromptCommandHandler(IPromptRepository repository) : ICommandHandler<CreatePromptCommand, PromptResponse>
{
    public async Task<PromptResponse> HandleAsync(CreatePromptCommand command, CancellationToken cancellationToken = default)
    {
        var req = command.Request;
        var prompt = new McpPromptDefinition
        {
            Code = req.Code,
            Name = req.Name,
            Description = req.Description,
            Type = req.Type,
            Category = req.Category,
            Tags = req.Tags,
            Enabled = req.Enabled,
            Template = req.Template,
            Arguments = req.Arguments.Select(a => new PromptArgumentDefinition
            {
                Name = a.Name,
                Description = a.Description,
                Required = a.Required,
            }).ToList(),
        };

        await repository.SaveAsync(prompt, cancellationToken);
        return prompt.ToResponse();
    }
}
