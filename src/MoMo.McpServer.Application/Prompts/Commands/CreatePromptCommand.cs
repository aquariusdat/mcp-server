using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Contract.Prompts;

namespace MoMo.McpServer.Application.Prompts.Commands;

public sealed record CreatePromptCommand(CreatePromptRequest Request) : ICommand<PromptResponse>;
