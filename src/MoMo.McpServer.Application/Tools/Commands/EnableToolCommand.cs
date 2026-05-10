using MoMo.McpServer.Application.Abstractions;

namespace MoMo.McpServer.Application.Tools.Commands;

public sealed record EnableToolCommand(Guid Id) : ICommand<bool>;
