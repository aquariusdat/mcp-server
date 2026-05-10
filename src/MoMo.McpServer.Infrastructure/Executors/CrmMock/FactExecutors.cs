using System.Text.Json;
using MoMo.McpServer.Application.Runtime;
using MoMo.McpServer.Infrastructure.MockData;

namespace MoMo.McpServer.Infrastructure.Executors.CrmMock;

public sealed class GetTeamConventionsExecutor(CrmMockDatabase db) : IToolExecutor
{
    public Task<ToolExecutionResult> ExecuteAsync(ToolExecutionContext context, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(db.Conventions, new JsonSerializerOptions { WriteIndented = true });
        return Task.FromResult(ToolExecutionResult.Success(json));
    }
}

public sealed class GetProjectStructureExecutor(CrmMockDatabase db) : IToolExecutor
{
    public Task<ToolExecutionResult> ExecuteAsync(ToolExecutionContext context, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(db.ProjectStructure, new JsonSerializerOptions { WriteIndented = true });
        return Task.FromResult(ToolExecutionResult.Success(json));
    }
}

public sealed class GetTeamMembersExecutor(CrmMockDatabase db) : IToolExecutor
{
    public Task<ToolExecutionResult> ExecuteAsync(ToolExecutionContext context, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(db.TeamMembers, new JsonSerializerOptions { WriteIndented = true });
        return Task.FromResult(ToolExecutionResult.Success(json));
    }
}

public sealed class GetSkillMatrixExecutor(CrmMockDatabase db) : IToolExecutor
{
    public Task<ToolExecutionResult> ExecuteAsync(ToolExecutionContext context, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(db.SkillMatrix, new JsonSerializerOptions { WriteIndented = true });
        return Task.FromResult(ToolExecutionResult.Success(json));
    }
}

public sealed class GetTeamWorkloadExecutor(CrmMockDatabase db) : IToolExecutor
{
    public Task<ToolExecutionResult> ExecuteAsync(ToolExecutionContext context, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(db.TeamWorkload, new JsonSerializerOptions { WriteIndented = true });
        return Task.FromResult(ToolExecutionResult.Success(json));
    }
}

public sealed class GetServiceOwnershipExecutor(CrmMockDatabase db) : IToolExecutor
{
    public Task<ToolExecutionResult> ExecuteAsync(ToolExecutionContext context, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(db.ServiceOwnership, new JsonSerializerOptions { WriteIndented = true });
        return Task.FromResult(ToolExecutionResult.Success(json));
    }
}

public sealed class SearchTicketsExecutor(CrmMockDatabase db) : IToolExecutor
{
    public Task<ToolExecutionResult> ExecuteAsync(ToolExecutionContext context, CancellationToken cancellationToken = default)
    {
        var query = context.Arguments.GetValueOrDefault("query")?.ToLowerInvariant();
        var tickets = string.IsNullOrWhiteSpace(query) 
            ? db.ExistingTickets 
            : db.ExistingTickets.Where(t => JsonSerializer.Serialize(t).ToLowerInvariant().Contains(query)).ToArray();
            
        var json = JsonSerializer.Serialize(tickets, new JsonSerializerOptions { WriteIndented = true });
        return Task.FromResult(ToolExecutionResult.Success(json));
    }
}

public sealed class SearchKnowledgeExecutor(CrmMockDatabase db) : IToolExecutor
{
    public Task<ToolExecutionResult> ExecuteAsync(ToolExecutionContext context, CancellationToken cancellationToken = default)
    {
        var query = context.Arguments.GetValueOrDefault("query")?.ToLowerInvariant();
        var docs = string.IsNullOrWhiteSpace(query) 
            ? db.InternalKnowledge 
            : db.InternalKnowledge.Where(d => JsonSerializer.Serialize(d).ToLowerInvariant().Contains(query)).ToArray();
            
        var json = JsonSerializer.Serialize(docs, new JsonSerializerOptions { WriteIndented = true });
        return Task.FromResult(ToolExecutionResult.Success(json));
    }
}
