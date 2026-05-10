using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Contract.Tools;

namespace MoMo.McpServer.Application.Tools.Queries;

public sealed record GetToolByIdQuery(Guid Id) : IQuery<ToolResponse?>;
