namespace MoMo.McpServer.Domain.Entities;

/// <summary>
/// Base entity shared by all MCP definition types (Tool, Resource, Prompt).
/// Contains all shared metadata fields required for management and runtime usage.
/// </summary>
public abstract class BaseDefinition
{
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Stable machine-readable identity key. Never changes for a given definition.
    /// Convention: "{domain}.{action}" e.g. "crm.get_customer", "payments.get_transaction".
    /// MCP clients reference tools by Code (via HandlerRoute), not by Id.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Human-readable display name shown in MCP client UIs.</summary>
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    /// <summary>Arbitrary type tag for admin filtering, e.g. "action", "lookup", "static".</summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>Logical grouping for admin organisation, e.g. "crm", "finance", "payments".</summary>
    public string Category { get; set; } = string.Empty;

    public List<string> Tags { get; set; } = [];

    /// <summary>
    /// When false, this definition is hidden from MCP clients entirely.
    /// Managed via PATCH /admin/{type}/{id}/enable|disable.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Schema/content version. Increments on each meaningful update via BumpVersion().
    /// Semantics: Code = stable identity, Version = iteration of that identity.
    /// Future: MCP-visible tool identity may expose Code:Version (e.g. "crm.get_customer:v2")
    /// to allow clients to pin to a specific schema version.
    /// </summary>
    public int Version { get; set; } = 1;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>Enables this definition for MCP runtime discovery.</summary>
    public void Enable()
    {
        Enabled = true;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>Hides this definition from MCP runtime discovery.</summary>
    public void Disable()
    {
        Enabled = false;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Signals a meaningful content or schema change.
    /// Call on every Update command to track evolution over time.
    /// </summary>
    public void BumpVersion()
    {
        Version++;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
