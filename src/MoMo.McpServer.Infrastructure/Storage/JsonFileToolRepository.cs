using System.Text.Json;
using Microsoft.Extensions.Options;
using MoMo.McpServer.Application.Interfaces;
using MoMo.McpServer.Domain.Entities;

namespace MoMo.McpServer.Infrastructure.Storage;

/// <summary>
/// File-based repository for MCP tool definitions.
/// Stores one JSON file per tool under {BasePath}/tools/{id}.json.
/// Replace this class with a DB implementation later without touching any Application code.
/// </summary>
public sealed class JsonFileToolRepository(IOptions<StorageOptions> options) : IToolRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };
    private readonly string _directory = Path.Combine(options.Value.BasePath, "tools");

    private void EnsureDirectoryExists() => Directory.CreateDirectory(_directory);

    private string FilePath(Guid id) => Path.Combine(_directory, $"{id}.json");

    public async Task<IReadOnlyList<McpToolDefinition>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        EnsureDirectoryExists();
        var files = Directory.GetFiles(_directory, "*.json");
        var results = new List<McpToolDefinition>(files.Length);

        foreach (var file in files)
        {
            var tool = await ReadFileAsync<McpToolDefinition>(file, cancellationToken);
            if (tool is not null) results.Add(tool);
        }

        return results;
    }

    public async Task<McpToolDefinition?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var path = FilePath(id);
        return File.Exists(path) ? await ReadFileAsync<McpToolDefinition>(path, cancellationToken) : null;
    }

    public async Task<McpToolDefinition?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var all = await GetAllAsync(cancellationToken);
        return all.FirstOrDefault(t => string.Equals(t.Code, code, StringComparison.OrdinalIgnoreCase));
    }

    public async Task SaveAsync(McpToolDefinition tool, CancellationToken cancellationToken = default)
    {
        EnsureDirectoryExists();
        var json = JsonSerializer.Serialize(tool, JsonOptions);
        await File.WriteAllTextAsync(FilePath(tool.Id), json, cancellationToken);
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
        catch
        {
            // Gracefully handle corrupt / empty files — return null
            return default;
        }
    }
}
