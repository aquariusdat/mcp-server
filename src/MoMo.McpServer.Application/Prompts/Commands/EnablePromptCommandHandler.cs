using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Interfaces;

namespace MoMo.McpServer.Application.Prompts.Commands;

public sealed class EnablePromptCommandHandler(IPromptRepository repository) : ICommandHandler<EnablePromptCommand, bool>
{
    public async Task<bool> HandleAsync(EnablePromptCommand command, CancellationToken cancellationToken = default)
    {
        var prompt = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (prompt is null) return false;
        prompt.Enable();
        await repository.SaveAsync(prompt, cancellationToken);
        return true;
    }
}
