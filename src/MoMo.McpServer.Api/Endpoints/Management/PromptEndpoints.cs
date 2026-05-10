using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Prompts.Commands;
using MoMo.McpServer.Application.Prompts.Queries;
using MoMo.McpServer.Contract.Common;
using MoMo.McpServer.Contract.Prompts;

namespace MoMo.McpServer.Api.Endpoints.Management;

/// <summary>
/// Management Layer: Admin CRUD endpoints for MCP prompt definitions.
/// Routes: /admin/prompts
/// </summary>
public static class PromptEndpoints
{
    public static IEndpointRouteBuilder MapPromptEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/admin/prompts").WithTags("Admin - Prompts");

        group.MapGet("/", async (IDispatcher dispatcher, CancellationToken ct) =>
        {
            var prompts = await dispatcher.SendQueryAsync(new ListPromptsQuery(), ct);
            return Results.Ok(ApiResponse<IReadOnlyList<PromptResponse>>.Ok(prompts));
        })
        .WithName("ListPrompts")
        .WithSummary("List all prompt definitions");

        group.MapGet("/{id:guid}", async (Guid id, IDispatcher dispatcher, CancellationToken ct) =>
        {
            var prompt = await dispatcher.SendQueryAsync(new GetPromptByIdQuery(id), ct);
            return prompt is null
                ? Results.NotFound(ApiResponse<PromptResponse>.Fail($"Prompt '{id}' not found."))
                : Results.Ok(ApiResponse<PromptResponse>.Ok(prompt));
        })
        .WithName("GetPromptById")
        .WithSummary("Get a prompt definition by ID");

        group.MapPost("/", async (CreatePromptRequest request, IDispatcher dispatcher, CancellationToken ct) =>
        {
            var prompt = await dispatcher.SendCommandAsync(new CreatePromptCommand(request), ct);
            return Results.Created($"/admin/prompts/{prompt.Id}", ApiResponse<PromptResponse>.Ok(prompt));
        })
        .WithName("CreatePrompt")
        .WithSummary("Create a new prompt definition");

        group.MapPut("/{id:guid}", async (Guid id, UpdatePromptRequest request, IDispatcher dispatcher, CancellationToken ct) =>
        {
            var prompt = await dispatcher.SendCommandAsync(new UpdatePromptCommand(id, request), ct);
            return prompt is null
                ? Results.NotFound(ApiResponse<PromptResponse>.Fail($"Prompt '{id}' not found."))
                : Results.Ok(ApiResponse<PromptResponse>.Ok(prompt));
        })
        .WithName("UpdatePrompt")
        .WithSummary("Update an existing prompt definition");

        group.MapDelete("/{id:guid}", async (Guid id, IDispatcher dispatcher, CancellationToken ct) =>
        {
            var deleted = await dispatcher.SendCommandAsync(new DeletePromptCommand(id), ct);
            return deleted ? Results.Ok(ApiResponse.Ok()) : Results.NotFound(ApiResponse.Fail($"Prompt '{id}' not found."));
        })
        .WithName("DeletePrompt")
        .WithSummary("Delete a prompt definition");

        group.MapPatch("/{id:guid}/enable", async (Guid id, IDispatcher dispatcher, CancellationToken ct) =>
        {
            var ok = await dispatcher.SendCommandAsync(new EnablePromptCommand(id), ct);
            return ok ? Results.Ok(ApiResponse.Ok()) : Results.NotFound(ApiResponse.Fail($"Prompt '{id}' not found."));
        })
        .WithName("EnablePrompt")
        .WithSummary("Enable a prompt definition");

        group.MapPatch("/{id:guid}/disable", async (Guid id, IDispatcher dispatcher, CancellationToken ct) =>
        {
            var ok = await dispatcher.SendCommandAsync(new DisablePromptCommand(id), ct);
            return ok ? Results.Ok(ApiResponse.Ok()) : Results.NotFound(ApiResponse.Fail($"Prompt '{id}' not found."));
        })
        .WithName("DisablePrompt")
        .WithSummary("Disable a prompt definition");

        return app;
    }
}
