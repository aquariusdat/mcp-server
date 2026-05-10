using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoMo.McpServer.Application.Interfaces;
using MoMo.McpServer.Application.Runtime;
using MoMo.McpServer.Infrastructure.Executors.CrmMock;
using MoMo.McpServer.Infrastructure.MockData;
using MoMo.McpServer.Infrastructure.Runtime;
using MoMo.McpServer.Infrastructure.Storage;

namespace MoMo.McpServer.Infrastructure.DependencyInjection;

/// <summary>
/// Registers all Infrastructure layer services: repositories, storage configuration, and runtime stubs.
/// Call this from the Api project startup.
/// Accepts IConfiguration so the storage path resolution stays inside Infrastructure, not in Program.cs.
/// </summary>
public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration,
        string contentRootPath)
    {
        // ── Storage path resolution ────────────────────────────────────────────
        var configuredPath = configuration["Storage:BasePath"];
        var basePath = string.IsNullOrWhiteSpace(configuredPath)
            ? Path.Combine(contentRootPath, "data")
            : Path.GetFullPath(Path.Combine(contentRootPath, configuredPath));

        services.Configure<StorageOptions>(opts => opts.BasePath = basePath);

        // ── Repositories ────────────────────────────────────────────────────────
        services.AddScoped<IToolRepository, JsonFileToolRepository>();
        services.AddScoped<IResourceRepository, JsonFileResourceRepository>();
        services.AddScoped<IPromptRepository, JsonFilePromptRepository>();

        // ── CRM Mock Data & Executors (Demo Planning Phase) ────────────────────
        services.AddSingleton<CrmMockDatabase>(); // Singleton to keep state (CreatedTickets list)
        
        // Register Executors
        services.AddScoped<GetTeamConventionsExecutor>();
        services.AddScoped<GetProjectStructureExecutor>();
        services.AddScoped<GetTeamMembersExecutor>();
        services.AddScoped<GetSkillMatrixExecutor>();
        services.AddScoped<GetTeamWorkloadExecutor>();
        services.AddScoped<GetServiceOwnershipExecutor>();
        services.AddScoped<SearchTicketsExecutor>();
        services.AddScoped<SearchKnowledgeExecutor>();
        services.AddScoped<SuggestAssigneeExecutor>();
        services.AddScoped<DraftTaskBreakdownExecutor>();
        services.AddScoped<PreviewExecutionPlanExecutor>();
        services.AddScoped<ValidateJiraPayloadExecutor>();
        services.AddScoped<BulkCreateTasksExecutor>();

        // ── Execution Registry ──────────────────────────────────────────────────
        // Maps Tool HandlerRoute to the concrete IToolExecutor type
        services.AddSingleton<IToolRegistry>(sp =>
        {
            var map = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
            {
                { "momo.crm.get_conventions", typeof(GetTeamConventionsExecutor) },
                { "momo.crm.get_project_structure", typeof(GetProjectStructureExecutor) },
                { "momo.crm.get_team_members", typeof(GetTeamMembersExecutor) },
                { "momo.crm.get_skill_matrix", typeof(GetSkillMatrixExecutor) },
                { "momo.crm.get_team_workload", typeof(GetTeamWorkloadExecutor) },
                { "momo.crm.get_service_ownership", typeof(GetServiceOwnershipExecutor) },
                { "momo.crm.search_tickets", typeof(SearchTicketsExecutor) },
                { "momo.crm.search_knowledge", typeof(SearchKnowledgeExecutor) },
                { "momo.crm.suggest_assignee", typeof(SuggestAssigneeExecutor) },
                { "momo.crm.draft_task_breakdown", typeof(DraftTaskBreakdownExecutor) },
                { "momo.crm.preview_execution_plan", typeof(PreviewExecutionPlanExecutor) },
                { "momo.crm.validate_jira_payload", typeof(ValidateJiraPayloadExecutor) },
                { "momo.crm.bulk_create_tasks", typeof(BulkCreateTasksExecutor) }
            };
            
            var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<DictionaryToolRegistry>>();
            return new DictionaryToolRegistry(sp, map, logger);
        });

        return services;
    }
}

