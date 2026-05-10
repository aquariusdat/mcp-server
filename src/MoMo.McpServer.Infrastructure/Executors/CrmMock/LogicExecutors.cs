using System.Text.Json;
using System.Text.Json.Nodes;
using MoMo.McpServer.Application.Runtime;
using MoMo.McpServer.Infrastructure.MockData;

namespace MoMo.McpServer.Infrastructure.Executors.CrmMock;

public sealed class SuggestAssigneeExecutor : IToolExecutor
{
    public Task<ToolExecutionResult> ExecuteAsync(ToolExecutionContext context, CancellationToken cancellationToken = default)
    {
        var skill = context.Arguments.GetValueOrDefault("requiredSkill") ?? "unknown";
        
        // Mock logic: return a hardcoded suggestion based on the skill for deterministic demo
        var name = skill.ToLowerInvariant() switch
        {
            ".net" or "backend" or "api" => "Hien Bui (u1) or Dat Hoang (u2) - Utilization is 75-100%, consider workload.",
            "reactjs" or "frontend" or "ui" => "Linh Nguyen (u3) - Utilization is 31%, highly available.",
            "qa" or "testing" => "Tuan Tran (u4) - Utilization is 50%.",
            "ba" or "requirements" => "Mai Le (u5) - Utilization is 62%.",
            _ => "Unassigned - Please manually review the skill matrix."
        };

        var result = new { RequiredSkill = skill, SuggestedAssignee = name, Confidence = "High" };
        return Task.FromResult(ToolExecutionResult.Success(JsonSerializer.Serialize(result)));
    }
}

public sealed class DraftTaskBreakdownExecutor : IToolExecutor
{
    public Task<ToolExecutionResult> ExecuteAsync(ToolExecutionContext context, CancellationToken cancellationToken = default)
    {
        var feature = context.Arguments.GetValueOrDefault("featureDescription") ?? "Unknown Feature";
        
        var draft = new
        {
            Epic = $"Epic: {feature}",
            Tasks = new[]
            {
                new { Summary = "[BE] API Design and DB Schema", IssueType = "Task", ExpectedPoints = 3, RequiredSkill = ".NET" },
                new { Summary = "[FE] Build UI Components", IssueType = "Task", ExpectedPoints = 5, RequiredSkill = "ReactJS" },
                new { Summary = "[QA] Write Automation Tests", IssueType = "Task", ExpectedPoints = 2, RequiredSkill = "QA" }
            },
            Note = "This is a deterministic mock breakdown complying with MoMo CRM conventions."
        };

        return Task.FromResult(ToolExecutionResult.Success(JsonSerializer.Serialize(draft, new JsonSerializerOptions { WriteIndented = true })));
    }
}

public sealed class PreviewExecutionPlanExecutor : IToolExecutor
{
    public Task<ToolExecutionResult> ExecuteAsync(ToolExecutionContext context, CancellationToken cancellationToken = default)
    {
        var tasksRaw = context.Arguments.GetValueOrDefault("tasks");
        if (string.IsNullOrWhiteSpace(tasksRaw))
            return Task.FromResult(ToolExecutionResult.Error("Missing 'tasks' argument."));

        return Task.FromResult(ToolExecutionResult.Success($@"
=========================================
      JSM EXECUTION PLAN PREVIEW
=========================================
You are about to create the following tickets:
{tasksRaw}

Please review before calling bulk_create_tasks.
"));
    }
}

public sealed class ValidateJiraPayloadExecutor : IToolExecutor
{
    public Task<ToolExecutionResult> ExecuteAsync(ToolExecutionContext context, CancellationToken cancellationToken = default)
    {
        var payloadRaw = context.Arguments.GetValueOrDefault("payload");
        if (string.IsNullOrWhiteSpace(payloadRaw))
            return Task.FromResult(ToolExecutionResult.Error("Missing 'payload'."));

        try
        {
            var node = JsonNode.Parse(payloadRaw);
            if (node is null) return Task.FromResult(ToolExecutionResult.Error("Invalid JSON payload."));
            
            var summary = node["summary"]?.ToString();
            var issueType = node["issueType"]?.ToString();

            if (string.IsNullOrWhiteSpace(summary))
                return Task.FromResult(ToolExecutionResult.Error("Validation Failed: 'summary' is required."));
                
            var validTypes = new[] { "Epic", "Task", "Sub-task", "Story" };
            if (string.IsNullOrWhiteSpace(issueType) || !validTypes.Contains(issueType))
                return Task.FromResult(ToolExecutionResult.Error($"Validation Failed: 'issueType' must be one of {string.Join(", ", validTypes)}"));

            return Task.FromResult(ToolExecutionResult.Success("Payload is valid according to Jira conventions."));
        }
        catch (JsonException ex)
        {
            return Task.FromResult(ToolExecutionResult.Error($"JSON Parse Error: {ex.Message}"));
        }
    }
}

public sealed class BulkCreateTasksExecutor(CrmMockDatabase db) : IToolExecutor
{
    public Task<ToolExecutionResult> ExecuteAsync(ToolExecutionContext context, CancellationToken cancellationToken = default)
    {
        var tasksRaw = context.Arguments.GetValueOrDefault("tasks");
        if (string.IsNullOrWhiteSpace(tasksRaw))
            return Task.FromResult(ToolExecutionResult.Error("Missing 'tasks' argument."));

        try
        {
            var node = JsonNode.Parse(tasksRaw);
            if (node is not JsonArray array)
                return Task.FromResult(ToolExecutionResult.Error("'tasks' must be a JSON array."));

            var createdKeys = new List<string>();
            foreach (var item in array)
            {
                var key = $"CRM-{db.NextTicketId++}";
                createdKeys.Add(key);
                
                // Add to mock DB to simulate state change
                db.CreatedTickets.Add(new
                {
                    IssueKey = key,
                    Summary = item?["summary"]?.ToString() ?? "Untitled",
                    IssueType = item?["issueType"]?.ToString() ?? "Task",
                    CreatedAt = DateTimeOffset.UtcNow
                });
            }

            var result = new
            {
                Message = "Successfully simulated Jira ticket creation.",
                CreatedTickets = createdKeys,
                MockDatabaseCount = db.CreatedTickets.Count
            };

            return Task.FromResult(ToolExecutionResult.Success(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true })));
        }
        catch (Exception ex)
        {
            return Task.FromResult(ToolExecutionResult.Error($"Creation failed: {ex.Message}"));
        }
    }
}
