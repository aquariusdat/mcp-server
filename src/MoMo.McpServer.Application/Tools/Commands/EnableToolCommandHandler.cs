using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Interfaces;

namespace MoMo.McpServer.Application.Tools.Commands;

public sealed class EnableToolCommandHandler(IToolRepository repository) : ICommandHandler<EnableToolCommand, bool>
{
    public async Task<bool> HandleAsync(EnableToolCommand command, CancellationToken cancellationToken = default)
    {
        var tool = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (tool is null) return false;

        tool.Enable();
        await repository.SaveAsync(tool, cancellationToken);
        return true;
    }
}
