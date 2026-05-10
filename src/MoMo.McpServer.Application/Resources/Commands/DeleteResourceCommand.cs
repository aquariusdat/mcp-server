using MoMo.McpServer.Application.Abstractions;

namespace MoMo.McpServer.Application.Resources.Commands;

public sealed record DeleteResourceCommand(Guid Id) : ICommand<bool>;
