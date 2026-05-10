namespace MoMo.McpServer.Domain.Entities;

/// <summary>
/// Base entity shared by all MCP definition types (Tool, Resource, Prompt).
/// Contains all shared metadata fields required for management and runtime usage.
/// </summary>
public abstract class BaseDefinition
{
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Unique machine-readable key, e.g. "crm_get_customer".</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Human-readable display name.</summary>
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    /// <summary>Arbitrary type tag, e.g. "action", "lookup", "static".</summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>Logical category grouping, e.g. "crm", "finance".</summary>
    public string Category { get; set; } = string.Empty;

    public List<string> Tags { get; set; } = [];

    /// <summary>Whether this definition is exposed to MCP clients.</summary>
    public bool Enabled { get; set; } = true;

    public int Version { get; set; } = 1;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public void Enable()
    {
        Enabled = true;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Disable()
    {
        Enabled = false;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void BumpVersion()
    {
        Version++;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
