using MoMo.McpServer.Application.Abstractions;

namespace MoMo.McpServer.Application.Tools.Commands;

public sealed record DeleteToolCommand(Guid Id) : ICommand<bool>;
