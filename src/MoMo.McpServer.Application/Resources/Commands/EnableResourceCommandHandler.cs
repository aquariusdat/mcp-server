using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Interfaces;

namespace MoMo.McpServer.Application.Resources.Commands;

public sealed class EnableResourceCommandHandler(IResourceRepository repository) : ICommandHandler<EnableResourceCommand, bool>
{
    public async Task<bool> HandleAsync(EnableResourceCommand command, CancellationToken cancellationToken = default)
    {
        var resource = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (resource is null) return false;
        resource.Enable();
        await repository.SaveAsync(resource, cancellationToken);
        return true;
    }
}
