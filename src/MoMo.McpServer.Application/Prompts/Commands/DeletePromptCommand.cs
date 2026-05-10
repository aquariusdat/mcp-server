using MoMo.McpServer.Application.Abstractions;

namespace MoMo.McpServer.Application.Prompts.Commands;

public sealed record DeletePromptCommand(Guid Id) : ICommand<bool>;
