namespace MoMo.McpServer.Infrastructure.Storage;

/// <summary>
/// Configuration options for local file-based storage.
/// Bind from appsettings.json under "Storage" section.
/// </summary>
public sealed class StorageOptions
{
    public const string SectionName = "Storage";

    /// <summary>
    /// Base directory for all data files.
    /// Defaults to {ContentRootPath}/data if not set.
    /// </summary>
    public string BasePath { get; set; } = string.Empty;
}
