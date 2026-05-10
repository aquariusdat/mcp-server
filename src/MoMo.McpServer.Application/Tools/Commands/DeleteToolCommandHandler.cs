using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Interfaces;

namespace MoMo.McpServer.Application.Tools.Commands;

public sealed class DeleteToolCommandHandler(IToolRepository repository) : ICommandHandler<DeleteToolCommand, bool>
{
    public async Task<bool> HandleAsync(DeleteToolCommand command, CancellationToken cancellationToken = default)
    {
        var tool = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (tool is null) return false;

        await repository.DeleteAsync(command.Id, cancellationToken);
        return true;
    }
}
