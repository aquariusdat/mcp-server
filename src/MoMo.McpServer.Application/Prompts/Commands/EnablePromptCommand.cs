using MoMo.McpServer.Application.Abstractions;

namespace MoMo.McpServer.Application.Prompts.Commands;

public sealed record EnablePromptCommand(Guid Id) : ICommand<bool>;
