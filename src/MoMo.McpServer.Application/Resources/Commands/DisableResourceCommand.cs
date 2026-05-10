using MoMo.McpServer.Application.Abstractions;

namespace MoMo.McpServer.Application.Resources.Commands;

public sealed record DisableResourceCommand(Guid Id) : ICommand<bool>;
