using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Tools.Commands;
using MoMo.McpServer.Application.Tools.Queries;
using MoMo.McpServer.Contract.Common;
using MoMo.McpServer.Contract.Tools;

namespace MoMo.McpServer.Api.Endpoints.Management;

/// <summary>
/// Management Layer: Admin CRUD endpoints for MCP tool definitions.
/// Routes: /admin/tools
/// All business logic is delegated to the Application layer via IDispatcher.
/// </summary>
public static class ToolEndpoints
{
    public static IEndpointRouteBuilder MapToolEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/admin/tools").WithTags("Admin - Tools");

        // GET /admin/tools
        group.MapGet("/", async (IDispatcher dispatcher, CancellationToken ct) =>
        {
            var tools = await dispatcher.SendQueryAsync(new ListToolsQuery(), ct);
            return Results.Ok(ApiResponse<IReadOnlyList<ToolResponse>>.Ok(tools));
        })
        .WithName("ListTools")
        .WithSummary("List all tool definitions");

        // GET /admin/tools/{id}
        group.MapGet("/{id:guid}", async (Guid id, IDispatcher dispatcher, CancellationToken ct) =>
        {
            var tool = await dispatcher.SendQueryAsync(new GetToolByIdQuery(id), ct);
            return tool is null
                ? Results.NotFound(ApiResponse<ToolResponse>.Fail($"Tool '{id}' not found."))
                : Results.Ok(ApiResponse<ToolResponse>.Ok(tool));
        })
        .WithName("GetToolById")
        .WithSummary("Get a tool definition by ID");

        // POST /admin/tools
        group.MapPost("/", async (CreateToolRequest request, IDispatcher dispatcher, CancellationToken ct) =>
        {
            var tool = await dispatcher.SendCommandAsync(new CreateToolCommand(request), ct);
            return Results.Created($"/admin/tools/{tool.Id}", ApiResponse<ToolResponse>.Ok(tool));
        })
        .WithName("CreateTool")
        .WithSummary("Create a new tool definition");

        // PUT /admin/tools/{id}
        group.MapPut("/{id:guid}", async (Guid id, UpdateToolRequest request, IDispatcher dispatcher, CancellationToken ct) =>
        {
            var tool = await dispatcher.SendCommandAsync(new UpdateToolCommand(id, request), ct);
            return tool is null
                ? Results.NotFound(ApiResponse<ToolResponse>.Fail($"Tool '{id}' not found."))
                : Results.Ok(ApiResponse<ToolResponse>.Ok(tool));
        })
        .WithName("UpdateTool")
        .WithSummary("Update an existing tool definition");

        // DELETE /admin/tools/{id}
        group.MapDelete("/{id:guid}", async (Guid id, IDispatcher dispatcher, CancellationToken ct) =>
        {
            var deleted = await dispatcher.SendCommandAsync(new DeleteToolCommand(id), ct);
            return deleted
                ? Results.Ok(ApiResponse.Ok())
                : Results.NotFound(ApiResponse.Fail($"Tool '{id}' not found."));
        })
        .WithName("DeleteTool")
        .WithSummary("Delete a tool definition");

        // PATCH /admin/tools/{id}/enable
        group.MapPatch("/{id:guid}/enable", async (Guid id, IDispatcher dispatcher, CancellationToken ct) =>
        {
            var ok = await dispatcher.SendCommandAsync(new EnableToolCommand(id), ct);
            return ok ? Results.Ok(ApiResponse.Ok()) : Results.NotFound(ApiResponse.Fail($"Tool '{id}' not found."));
        })
        .WithName("EnableTool")
        .WithSummary("Enable a tool definition");

        // PATCH /admin/tools/{id}/disable
        group.MapPatch("/{id:guid}/disable", async (Guid id, IDispatcher dispatcher, CancellationToken ct) =>
        {
            var ok = await dispatcher.SendCommandAsync(new DisableToolCommand(id), ct);
            return ok ? Results.Ok(ApiResponse.Ok()) : Results.NotFound(ApiResponse.Fail($"Tool '{id}' not found."));
        })
        .WithName("DisableTool")
        .WithSummary("Disable a tool definition");

        return app;
    }
}
