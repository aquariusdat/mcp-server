using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Interfaces;
using MoMo.McpServer.Application.Mapping;
using MoMo.McpServer.Contract.Tools;
using MoMo.McpServer.Domain.Entities;

namespace MoMo.McpServer.Application.Tools.Commands;

public sealed class CreateToolCommandHandler(IToolRepository repository) : ICommandHandler<CreateToolCommand, ToolResponse>
{
    public async Task<ToolResponse> HandleAsync(CreateToolCommand command, CancellationToken cancellationToken = default)
    {
        var req = command.Request;

        var tool = new McpToolDefinition
        {
            Code = req.Code,
            Name = req.Name,
            Description = req.Description,
            Type = req.Type,
            Category = req.Category,
            Tags = req.Tags,
            Enabled = req.Enabled,
            InputSchema = req.InputSchema,
            OutputSchema = req.OutputSchema,
            HandlerRoute = req.HandlerRoute,
        };

        await repository.SaveAsync(tool, cancellationToken);
        return tool.ToResponse();
    }
}
