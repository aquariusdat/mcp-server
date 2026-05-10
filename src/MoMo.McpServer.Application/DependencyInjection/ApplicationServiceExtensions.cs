using Microsoft.Extensions.DependencyInjection;
using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Dispatcher;
using MoMo.McpServer.Application.Prompts.Commands;
using MoMo.McpServer.Application.Prompts.Queries;
using MoMo.McpServer.Application.Resources.Commands;
using MoMo.McpServer.Application.Resources.Queries;
using MoMo.McpServer.Application.Tools.Commands;
using MoMo.McpServer.Application.Tools.Queries;
using MoMo.McpServer.Contract.Prompts;
using MoMo.McpServer.Contract.Resources;
using MoMo.McpServer.Contract.Tools;

namespace MoMo.McpServer.Application.DependencyInjection;

/// <summary>
/// Registers all Application layer services: dispatcher + all CQRS handlers.
/// Call this from the Api project startup.
/// </summary>
public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Dispatcher (in-house mediator)
        services.AddScoped<IDispatcher, Dispatcher.Dispatcher>();

        // ── Tool handlers ──────────────────────────────────────────────────
        services.AddScoped<ICommandHandler<CreateToolCommand, ToolResponse>, CreateToolCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateToolCommand, ToolResponse?>, UpdateToolCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteToolCommand, bool>, DeleteToolCommandHandler>();
        services.AddScoped<ICommandHandler<EnableToolCommand, bool>, EnableToolCommandHandler>();
        services.AddScoped<ICommandHandler<DisableToolCommand, bool>, DisableToolCommandHandler>();
        services.AddScoped<IQueryHandler<GetToolByIdQuery, ToolResponse?>, GetToolByIdQueryHandler>();
        services.AddScoped<IQueryHandler<ListToolsQuery, IReadOnlyList<ToolResponse>>, ListToolsQueryHandler>();

        // ── Resource handlers ──────────────────────────────────────────────
        services.AddScoped<ICommandHandler<CreateResourceCommand, ResourceResponse>, CreateResourceCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateResourceCommand, ResourceResponse?>, UpdateResourceCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteResourceCommand, bool>, DeleteResourceCommandHandler>();
        services.AddScoped<ICommandHandler<EnableResourceCommand, bool>, EnableResourceCommandHandler>();
        services.AddScoped<ICommandHandler<DisableResourceCommand, bool>, DisableResourceCommandHandler>();
        services.AddScoped<IQueryHandler<GetResourceByIdQuery, ResourceResponse?>, GetResourceByIdQueryHandler>();
        services.AddScoped<IQueryHandler<ListResourcesQuery, IReadOnlyList<ResourceResponse>>, ListResourcesQueryHandler>();

        // ── Prompt handlers ────────────────────────────────────────────────
        services.AddScoped<ICommandHandler<CreatePromptCommand, PromptResponse>, CreatePromptCommandHandler>();
        services.AddScoped<ICommandHandler<UpdatePromptCommand, PromptResponse?>, UpdatePromptCommandHandler>();
        services.AddScoped<ICommandHandler<DeletePromptCommand, bool>, DeletePromptCommandHandler>();
        services.AddScoped<ICommandHandler<EnablePromptCommand, bool>, EnablePromptCommandHandler>();
        services.AddScoped<ICommandHandler<DisablePromptCommand, bool>, DisablePromptCommandHandler>();
        services.AddScoped<IQueryHandler<GetPromptByIdQuery, PromptResponse?>, GetPromptByIdQueryHandler>();
        services.AddScoped<IQueryHandler<ListPromptsQuery, IReadOnlyList<PromptResponse>>, ListPromptsQueryHandler>();

        return services;
    }
}
