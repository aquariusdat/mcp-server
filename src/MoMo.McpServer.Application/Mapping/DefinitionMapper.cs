using MoMo.McpServer.Contract.Tools;
using MoMo.McpServer.Contract.Resources;
using MoMo.McpServer.Contract.Prompts;
using MoMo.McpServer.Domain.Entities;

namespace MoMo.McpServer.Application.Mapping;

/// <summary>
/// Extension methods for mapping domain entities to contract response DTOs.
/// Kept in Application layer — bridges Domain ↔ Contract without coupling either direction.
/// </summary>
public static class DefinitionMapper
{
    public static ToolResponse ToResponse(this McpToolDefinition tool) => new()
    {
        Id = tool.Id,
        Code = tool.Code,
        Name = tool.Name,
        Description = tool.Description,
        Type = tool.Type,
        Category = tool.Category,
        Tags = tool.Tags,
        Enabled = tool.Enabled,
        Version = tool.Version,
        InputSchema = tool.InputSchema,
        OutputSchema = tool.OutputSchema,
        HandlerRoute = tool.HandlerRoute,
        CreatedAt = tool.CreatedAt,
        UpdatedAt = tool.UpdatedAt,
    };

    public static ResourceResponse ToResponse(this McpResourceDefinition resource) => new()
    {
        Id = resource.Id,
        Code = resource.Code,
        Name = resource.Name,
        Description = resource.Description,
        Type = resource.Type,
        Category = resource.Category,
        Tags = resource.Tags,
        Enabled = resource.Enabled,
        Version = resource.Version,
        Uri = resource.Uri,
        MimeType = resource.MimeType,
        HandlerRoute = resource.HandlerRoute,
        CreatedAt = resource.CreatedAt,
        UpdatedAt = resource.UpdatedAt,
    };

    public static PromptResponse ToResponse(this McpPromptDefinition prompt) => new()
    {
        Id = prompt.Id,
        Code = prompt.Code,
        Name = prompt.Name,
        Description = prompt.Description,
        Type = prompt.Type,
        Category = prompt.Category,
        Tags = prompt.Tags,
        Enabled = prompt.Enabled,
        Version = prompt.Version,
        Template = prompt.Template,
        Arguments = prompt.Arguments.Select(a => new PromptArgumentResponse
        {
            Name = a.Name,
            Description = a.Description,
            Required = a.Required,
        }).ToList(),
        CreatedAt = prompt.CreatedAt,
        UpdatedAt = prompt.UpdatedAt,
    };
}
