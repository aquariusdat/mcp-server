using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Interfaces;

namespace MoMo.McpServer.Application.Tools.Commands;

public sealed class DisableToolCommandHandler(IToolRepository repository) : ICommandHandler<DisableToolCommand, bool>
{
    public async Task<bool> HandleAsync(DisableToolCommand command, CancellationToken cancellationToken = default)
    {
        var tool = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (tool is null) return false;

        tool.Disable();
        await repository.SaveAsync(tool, cancellationToken);
        return true;
    }
}
