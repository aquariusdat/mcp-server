using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Interfaces;

namespace MoMo.McpServer.Application.Prompts.Commands;

public sealed class DisablePromptCommandHandler(IPromptRepository repository) : ICommandHandler<DisablePromptCommand, bool>
{
    public async Task<bool> HandleAsync(DisablePromptCommand command, CancellationToken cancellationToken = default)
    {
        var prompt = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (prompt is null) return false;
        prompt.Disable();
        await repository.SaveAsync(prompt, cancellationToken);
        return true;
    }
}
