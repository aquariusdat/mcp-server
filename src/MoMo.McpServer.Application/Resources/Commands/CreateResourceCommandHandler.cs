using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Interfaces;
using MoMo.McpServer.Application.Mapping;
using MoMo.McpServer.Contract.Resources;
using MoMo.McpServer.Domain.Entities;

namespace MoMo.McpServer.Application.Resources.Commands;

public sealed class CreateResourceCommandHandler(IResourceRepository repository) : ICommandHandler<CreateResourceCommand, ResourceResponse>
{
    public async Task<ResourceResponse> HandleAsync(CreateResourceCommand command, CancellationToken cancellationToken = default)
    {
        var req = command.Request;
        var resource = new McpResourceDefinition
        {
            Code = req.Code,
            Name = req.Name,
            Description = req.Description,
            Type = req.Type,
            Category = req.Category,
            Tags = req.Tags,
            Enabled = req.Enabled,
            Uri = req.Uri,
            MimeType = req.MimeType,
            HandlerRoute = req.HandlerRoute,
        };

        await repository.SaveAsync(resource, cancellationToken);
        return resource.ToResponse();
    }
}
