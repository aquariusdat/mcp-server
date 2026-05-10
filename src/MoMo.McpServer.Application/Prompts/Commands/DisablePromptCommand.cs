using MoMo.McpServer.Application.Abstractions;

namespace MoMo.McpServer.Application.Prompts.Commands;

public sealed record DisablePromptCommand(Guid Id) : ICommand<bool>;
