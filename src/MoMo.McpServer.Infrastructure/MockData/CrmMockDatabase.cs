namespace MoMo.McpServer.Infrastructure.MockData;

/// <summary>
/// Singleton In-Memory store containing deterministic hard facts for the CRM Demo.
/// Allows MCP Tools to read consistent environment data.
/// </summary>
public sealed class CrmMockDatabase
{
    public object Conventions => new
    {
        BackendStack = ".NET 10, Minimal APIs, CQRS",
        FrontendStack = "ReactJS, TypeScript, NextJS",
        ArchitectureStyle = "Clean Architecture, Event-Driven",
        NamingConventions = "PascalCase for C#, camelCase for TS",
        JiraConventions = "Epic -> Task -> Sub-task. Prefix branches with IssueKey.",
        StoryPointRules = "1 SP = half day. Max 8 SP per task.",
        Labels = "backend, frontend, security, data-migration",
        DoD = "Unit tests pass, SonarQube green, PR approved by 2 reviewers"
    };

    public object ProjectStructure => new
    {
        Systems = new[] { "CRM Core", "Identity Provider", "Campaign Engine" },
        Modules = new[] { "Onboarding", "KYC", "AML", "Wallet", "Notification", "Loyalty" }
    };

    public object[] TeamMembers =>
    [
        new { Id = "u1", Name = "Hien Bui", Role = "Backend Engineer", Seniority = "Senior", PrimarySkills = new[] { ".NET", "Oracle" }, SecondarySkills = new[] { "RabbitMQ" }, Squad = "Core" },
        new { Id = "u2", Name = "Dat Hoang", Role = "Backend Engineer", Seniority = "Mid", PrimarySkills = new[] { ".NET", "Redis" }, SecondarySkills = new[] { "BigQuery" }, Squad = "Growth" },
        new { Id = "u3", Name = "Linh Nguyen", Role = "Frontend Engineer", Seniority = "Senior", PrimarySkills = new[] { "ReactJS" }, SecondarySkills = new[] { "Figma" }, Squad = "Core" },
        new { Id = "u4", Name = "Tuan Tran", Role = "QA", Seniority = "Mid", PrimarySkills = new[] { "Automation" }, SecondarySkills = new[] { "Performance" }, Squad = "Core" },
        new { Id = "u5", Name = "Mai Le", Role = "BA", Seniority = "Senior", PrimarySkills = new[] { "Requirements" }, SecondarySkills = new[] { "Jira" }, Squad = "Growth" }
    ];

    public object SkillMatrix => new
    {
        DotNet = new[] { "u1", "u2" },
        ReactJS = new[] { "u3" },
        Oracle = new[] { "u1" },
        Redis = new[] { "u2" },
        RabbitMQ = new[] { "u1" },
        AML = new[] { "u1" },
        KYC = new[] { "u2", "u5" }
    };

    public object[] TeamWorkload =>
    [
        new { Id = "u1", CurrentPoints = 12, Capacity = 16, Utilization = "75%" },
        new { Id = "u2", CurrentPoints = 16, Capacity = 16, Utilization = "100%" },
        new { Id = "u3", CurrentPoints = 5, Capacity = 16, Utilization = "31%" },
        new { Id = "u4", CurrentPoints = 8, Capacity = 16, Utilization = "50%" },
        new { Id = "u5", CurrentPoints = 10, Capacity = 16, Utilization = "62%" }
    ];

    public object ServiceOwnership => new
    {
        OnboardingApi = "u2",
        AmlService = "u1",
        KycService = "u2",
        NotificationService = "u1",
        CrmFrontend = "u3"
    };

    public object[] ExistingTickets =>
    [
        new { IssueKey = "CRM-1001", Summary = "Implement KYC Step 1", Assignee = "u2", Status = "Done", StoryPoints = 5 },
        new { IssueKey = "CRM-1002", Summary = "AML Background Check", Assignee = "u1", Status = "In Progress", StoryPoints = 8 },
        new { IssueKey = "CRM-1003", Summary = "Merchant UI Dashboard", Assignee = "u3", Status = "To Do", StoryPoints = 5 }
    ];

    public object[] InternalKnowledge =>
    [
        new { Title = "Onboarding Flow V2", Content = "Merchants must provide business license, owner ID, and bank details. Verified via KYC service." },
        new { Title = "Event-Driven Guidelines", Content = "Publish UserRegisteredEvent to RabbitMQ exchange 'crm.events' when onboarding succeeds." },
        new { Title = "AML Validation Rule", Content = "Any transaction over 100M VND triggers AML manual review." }
    ];

    // Mutable list to simulate Jira creation
    public List<object> CreatedTickets { get; } = new();
    
    // Auto-increment mock ID
    public int NextTicketId { get; set; } = 1021;
}
