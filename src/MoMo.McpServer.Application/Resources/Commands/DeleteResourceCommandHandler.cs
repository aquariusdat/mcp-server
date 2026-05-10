using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Interfaces;

namespace MoMo.McpServer.Application.Resources.Commands;

public sealed class DeleteResourceCommandHandler(IResourceRepository repository) : ICommandHandler<DeleteResourceCommand, bool>
{
    public async Task<bool> HandleAsync(DeleteResourceCommand command, CancellationToken cancellationToken = default)
    {
        var resource = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (resource is null) return false;

        await repository.DeleteAsync(command.Id, cancellationToken);
        return true;
    }
}
