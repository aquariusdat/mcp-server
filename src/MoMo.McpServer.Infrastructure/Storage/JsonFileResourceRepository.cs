using System.Text.Json;
using Microsoft.Extensions.Options;
using MoMo.McpServer.Application.Interfaces;
using MoMo.McpServer.Domain.Entities;

namespace MoMo.McpServer.Infrastructure.Storage;

/// <summary>
/// File-based repository for MCP resource definitions.
/// Stores one JSON file per resource under {BasePath}/resources/{id}.json.
/// </summary>
public sealed class JsonFileResourceRepository(IOptions<StorageOptions> options) : IResourceRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };
    private readonly string _directory = Path.Combine(options.Value.BasePath, "resources");

    private void EnsureDirectoryExists() => Directory.CreateDirectory(_directory);
    private string FilePath(Guid id) => Path.Combine(_directory, $"{id}.json");

    public async Task<IReadOnlyList<McpResourceDefinition>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        EnsureDirectoryExists();
        var files = Directory.GetFiles(_directory, "*.json");
        var results = new List<McpResourceDefinition>(files.Length);
        foreach (var file in files)
        {
            var resource = await ReadFileAsync<McpResourceDefinition>(file, cancellationToken);
            if (resource is not null) results.Add(resource);
        }
        return results;
    }

    public async Task<McpResourceDefinition?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var path = FilePath(id);
        return File.Exists(path) ? await ReadFileAsync<McpResourceDefinition>(path, cancellationToken) : null;
    }

    public async Task<McpResourceDefinition?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var all = await GetAllAsync(cancellationToken);
        return all.FirstOrDefault(r => string.Equals(r.Code, code, StringComparison.OrdinalIgnoreCase));
    }

    public async Task SaveAsync(McpResourceDefinition resource, CancellationToken cancellationToken = default)
    {
        EnsureDirectoryExists();
        var json = JsonSerializer.Serialize(resource, JsonOptions);
        await File.WriteAllTextAsync(FilePath(resource.Id), json, cancellationToken);
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var path = FilePath(id);
        if (File.Exists(path)) File.Delete(path);
        return Task.CompletedTask;
    }

    private static async Task<T?> ReadFileAsync<T>(string path, CancellationToken cancellationToken)
    {
        try
        {
            var json = await File.ReadAllTextAsync(path, cancellationToken);
            return JsonSerializer.Deserialize<T>(json);
        }
        catch { return default; }
    }
}
