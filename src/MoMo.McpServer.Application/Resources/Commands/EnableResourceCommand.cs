using MoMo.McpServer.Application.Abstractions;

namespace MoMo.McpServer.Application.Resources.Commands;

public sealed record EnableResourceCommand(Guid Id) : ICommand<bool>;
