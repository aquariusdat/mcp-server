using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Resources.Commands;
using MoMo.McpServer.Application.Resources.Queries;
using MoMo.McpServer.Contract.Common;
using MoMo.McpServer.Contract.Resources;

namespace MoMo.McpServer.Api.Endpoints.Management;

/// <summary>
/// Management Layer: Admin CRUD endpoints for MCP resource definitions.
/// Routes: /admin/resources
/// </summary>
public static class ResourceEndpoints
{
    public static IEndpointRouteBuilder MapResourceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/admin/resources").WithTags("Admin - Resources");

        group.MapGet("/", async (IDispatcher dispatcher, CancellationToken ct) =>
        {
            var resources = await dispatcher.SendQueryAsync(new ListResourcesQuery(), ct);
            return Results.Ok(ApiResponse<IReadOnlyList<ResourceResponse>>.Ok(resources));
        })
        .WithName("ListResources")
        .WithSummary("List all resource definitions");

        group.MapGet("/{id:guid}", async (Guid id, IDispatcher dispatcher, CancellationToken ct) =>
        {
            var resource = await dispatcher.SendQueryAsync(new GetResourceByIdQuery(id), ct);
            return resource is null
                ? Results.NotFound(ApiResponse<ResourceResponse>.Fail($"Resource '{id}' not found."))
                : Results.Ok(ApiResponse<ResourceResponse>.Ok(resource));
        })
        .WithName("GetResourceById")
        .WithSummary("Get a resource definition by ID");

        group.MapPost("/", async (CreateResourceRequest request, IDispatcher dispatcher, CancellationToken ct) =>
        {
            var resource = await dispatcher.SendCommandAsync(new CreateResourceCommand(request), ct);
            return Results.Created($"/admin/resources/{resource.Id}", ApiResponse<ResourceResponse>.Ok(resource));
        })
        .WithName("CreateResource")
        .WithSummary("Create a new resource definition");

        group.MapPut("/{id:guid}", async (Guid id, UpdateResourceRequest request, IDispatcher dispatcher, CancellationToken ct) =>
        {
            var resource = await dispatcher.SendCommandAsync(new UpdateResourceCommand(id, request), ct);
            return resource is null
                ? Results.NotFound(ApiResponse<ResourceResponse>.Fail($"Resource '{id}' not found."))
                : Results.Ok(ApiResponse<ResourceResponse>.Ok(resource));
        })
        .WithName("UpdateResource")
        .WithSummary("Update an existing resource definition");

        group.MapDelete("/{id:guid}", async (Guid id, IDispatcher dispatcher, CancellationToken ct) =>
        {
            var deleted = await dispatcher.SendCommandAsync(new DeleteResourceCommand(id), ct);
            return deleted ? Results.Ok(ApiResponse.Ok()) : Results.NotFound(ApiResponse.Fail($"Resource '{id}' not found."));
        })
        .WithName("DeleteResource")
        .WithSummary("Delete a resource definition");

        group.MapPatch("/{id:guid}/enable", async (Guid id, IDispatcher dispatcher, CancellationToken ct) =>
        {
            var ok = await dispatcher.SendCommandAsync(new EnableResourceCommand(id), ct);
            return ok ? Results.Ok(ApiResponse.Ok()) : Results.NotFound(ApiResponse.Fail($"Resource '{id}' not found."));
        })
        .WithName("EnableResource")
        .WithSummary("Enable a resource definition");

        group.MapPatch("/{id:guid}/disable", async (Guid id, IDispatcher dispatcher, CancellationToken ct) =>
        {
            var ok = await dispatcher.SendCommandAsync(new DisableResourceCommand(id), ct);
            return ok ? Results.Ok(ApiResponse.Ok()) : Results.NotFound(ApiResponse.Fail($"Resource '{id}' not found."));
        })
        .WithName("DisableResource")
        .WithSummary("Disable a resource definition");

        return app;
    }
}
