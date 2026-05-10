using MoMo.McpServer.Application.Abstractions;
using MoMo.McpServer.Contract.Tools;

namespace MoMo.McpServer.Application.Tools.Queries;

/// <summary>
/// Lists all tools. Optionally filter by enabled state.
/// </summary>
public sealed record ListToolsQuery(bool? EnabledOnly = null) : IQuery<IReadOnlyList<ToolResponse>>;
