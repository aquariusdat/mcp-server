using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Contract.Prompts;

namespace MoMo.McpServer.Application.Prompts.Commands;

public sealed record UpdatePromptCommand(Guid Id, UpdatePromptRequest Request) : ICommand<PromptResponse?>;
