using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Interfaces;
using MoMo.McpServer.Application.Mapping;
using MoMo.McpServer.Contract.Prompts;
using MoMo.McpServer.Domain.Entities;

namespace MoMo.McpServer.Application.Prompts.Commands;

public sealed class UpdatePromptCommandHandler(IPromptRepository repository) : ICommandHandler<UpdatePromptCommand, PromptResponse?>
{
    public async Task<PromptResponse?> HandleAsync(UpdatePromptCommand command, CancellationToken cancellationToken = default)
    {
        var prompt = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (prompt is null) return null;

        var req = command.Request;
        prompt.Name = req.Name;
        prompt.Description = req.Description;
        prompt.Type = req.Type;
        prompt.Category = req.Category;
        prompt.Tags = req.Tags;
        prompt.Template = req.Template;
        prompt.Arguments = req.Arguments.Select(a => new PromptArgumentDefinition
        {
            Name = a.Name,
            Description = a.Description,
            Required = a.Required,
        }).ToList();
        prompt.BumpVersion();

        await repository.SaveAsync(prompt, cancellationToken);
        return prompt.ToResponse();
    }
}
