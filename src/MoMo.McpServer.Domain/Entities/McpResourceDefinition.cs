namespace MoMo.McpServer.Domain.Entities;

/// <summary>
/// Represents a stored MCP resource definition.
/// Resources are addressable data sources exposed to MCP clients via stable URIs.
/// </summary>
public sealed class McpResourceDefinition : BaseDefinition
{
    /// <summary>
    /// Stable URI identifying the resource, e.g. "momo://crm/customers".
    /// Must be unique across all resources.
    /// </summary>
    public string Uri { get; set; } = string.Empty;

    /// <summary>
    /// MIME type of the resource content, e.g. "application/json", "text/plain".
    /// </summary>
    public string MimeType { get; set; } = "application/json";

    /// <summary>
    /// Internal handler route key used to dispatch resource read requests.
    /// </summary>
    public string HandlerRoute { get; set; } = string.Empty;
}
