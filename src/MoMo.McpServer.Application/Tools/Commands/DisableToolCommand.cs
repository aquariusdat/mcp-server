using MoMo.McpServer.Application.Abstractions;

namespace MoMo.McpServer.Application.Tools.Commands;

public sealed record DisableToolCommand(Guid Id) : ICommand<bool>;
