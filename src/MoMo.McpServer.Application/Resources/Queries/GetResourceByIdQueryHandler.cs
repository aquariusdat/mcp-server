using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Interfaces;
using MoMo.McpServer.Application.Mapping;
using MoMo.McpServer.Contract.Resources;

namespace MoMo.McpServer.Application.Resources.Queries;

public sealed class GetResourceByIdQueryHandler(IResourceRepository repository) : IQueryHandler<GetResourceByIdQuery, ResourceResponse?>
{
    public async Task<ResourceResponse?> HandleAsync(GetResourceByIdQuery query, CancellationToken cancellationToken = default)
    {
        var resource = await repository.GetByIdAsync(query.Id, cancellationToken);
        return resource?.ToResponse();
    }
}
