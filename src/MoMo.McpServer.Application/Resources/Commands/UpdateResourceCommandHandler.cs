using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Interfaces;
using MoMo.McpServer.Application.Mapping;
using MoMo.McpServer.Contract.Resources;

namespace MoMo.McpServer.Application.Resources.Commands;

public sealed class UpdateResourceCommandHandler(IResourceRepository repository) : ICommandHandler<UpdateResourceCommand, ResourceResponse?>
{
    public async Task<ResourceResponse?> HandleAsync(UpdateResourceCommand command, CancellationToken cancellationToken = default)
    {
        var resource = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (resource is null) return null;

        var req = command.Request;
        resource.Name = req.Name;
        resource.Description = req.Description;
        resource.Type = req.Type;
        resource.Category = req.Category;
        resource.Tags = req.Tags;
        resource.Uri = req.Uri;
        resource.MimeType = req.MimeType;
        resource.HandlerRoute = req.HandlerRoute;
        resource.BumpVersion();

        await repository.SaveAsync(resource, cancellationToken);
        return resource.ToResponse();
    }
}
