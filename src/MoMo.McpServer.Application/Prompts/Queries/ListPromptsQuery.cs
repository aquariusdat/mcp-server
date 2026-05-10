using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Contract.Prompts;

namespace MoMo.McpServer.Application.Prompts.Queries;

public sealed record ListPromptsQuery(bool? EnabledOnly = null) : IQuery<IReadOnlyList<PromptResponse>>;
