using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Contract.Tools;

namespace MoMo.McpServer.Application.Tools.Commands;

public sealed record CreateToolCommand(CreateToolRequest Request) : ICommand<ToolResponse>;
