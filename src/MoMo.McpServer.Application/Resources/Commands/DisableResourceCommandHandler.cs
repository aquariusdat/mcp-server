using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Interfaces;

namespace MoMo.McpServer.Application.Resources.Commands;

public sealed class DisableResourceCommandHandler(IResourceRepository repository) : ICommandHandler<DisableResourceCommand, bool>
{
    public async Task<bool> HandleAsync(DisableResourceCommand command, CancellationToken cancellationToken = default)
    {
        var resource = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (resource is null) return false;
        resource.Disable();
        await repository.SaveAsync(resource, cancellationToken);
        return true;
    }
}
