using System.Text.Json;
using Microsoft.Extensions.Options;
using MoMo.McpServer.Application.Interfaces;
using MoMo.McpServer.Domain.Entities;

namespace MoMo.McpServer.Infrastructure.Storage;

/// <summary>
/// File-based repository for MCP prompt definitions.
/// Stores one JSON file per prompt under {BasePath}/prompts/{id}.json.
/// </summary>
public sealed class JsonFilePromptRepository(IOptions<StorageOptions> options) : IPromptRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };
    private readonly string _directory = Path.Combine(options.Value.BasePath, "prompts");

    private void EnsureDirectoryExists() => Directory.CreateDirectory(_directory);
    private string FilePath(Guid id) => Path.Combine(_directory, $"{id}.json");

    public async Task<IReadOnlyList<McpPromptDefinition>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        EnsureDirectoryExists();
        var files = Directory.GetFiles(_directory, "*.json");
        var results = new List<McpPromptDefinition>(files.Length);
        foreach (var file in files)
        {
            var prompt = await ReadFileAsync<McpPromptDefinition>(file, cancellationToken);
            if (prompt is not null) results.Add(prompt);
        }
        return results;
    }

    public async Task<McpPromptDefinition?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var path = FilePath(id);
        return File.Exists(path) ? await ReadFileAsync<McpPromptDefinition>(path, cancellationToken) : null;
    }

    public async Task<McpPromptDefinition?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var all = await GetAllAsync(cancellationToken);
        return all.FirstOrDefault(p => string.Equals(p.Code, code, StringComparison.OrdinalIgnoreCase));
    }

    public async Task SaveAsync(McpPromptDefinition prompt, CancellationToken cancellationToken = default)
    {
        EnsureDirectoryExists();
        var json = JsonSerializer.Serialize(prompt, JsonOptions);
        await File.WriteAllTextAsync(FilePath(prompt.Id), json, cancellationToken);
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
