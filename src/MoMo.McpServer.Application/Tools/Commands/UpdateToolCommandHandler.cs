using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Application.Interfaces;
using MoMo.McpServer.Application.Mapping;
using MoMo.McpServer.Contract.Tools;

namespace MoMo.McpServer.Application.Tools.Commands;

public sealed class UpdateToolCommandHandler(IToolRepository repository) : ICommandHandler<UpdateToolCommand, ToolResponse?>
{
    public async Task<ToolResponse?> HandleAsync(UpdateToolCommand command, CancellationToken cancellationToken = default)
    {
        var tool = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (tool is null) return null;

        var req = command.Request;
        tool.Name = req.Name;
        tool.Description = req.Description;
        tool.Type = req.Type;
        tool.Category = req.Category;
        tool.Tags = req.Tags;
        tool.InputSchema = req.InputSchema;
        tool.OutputSchema = req.OutputSchema;
        tool.HandlerRoute = req.HandlerRoute;
        tool.BumpVersion();

        await repository.SaveAsync(tool, cancellationToken);
        return tool.ToResponse();
    }
}
